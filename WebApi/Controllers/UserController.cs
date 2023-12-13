using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IGenericSecurityRepository<User> _securityRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, ITokenService tokenService, IMapper mapper, IPasswordHasher<User> passwordHasher, IGenericSecurityRepository<User> securityRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _securityRepository = securityRepository;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Token = _tokenService.CreateToken(user, roles),
                Name = user.Name,
                Lastname = user.Lastname,
                Image = user.Image,
                Admin = roles.Contains("ADMIN")
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Username,
                Name = registerDto.Name,
                Lastname = registerDto.Lastname
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Token = _tokenService.CreateToken(user, null),
                Email = user.Email,
                Username = user.UserName,
                Admin = false
            };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<UserDto>> Update(string id, RegisterDto registerDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new CodeErrorResponse(404, "Usuario no encontrado"));
            }

            user.Name = registerDto.Name;
            user.Lastname = registerDto.Lastname;
            user.Image = registerDto.Image;

            if (!string.IsNullOrEmpty(registerDto.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);
            }
            
            
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo actualizar el usuario"));
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.UserName,
                Token = _tokenService?.CreateToken(user, roles),
                Image = user.Image,
                Admin = roles.Contains("ADMIN")
            };

        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("pagination")]
        public async Task<ActionResult<Pagination<UserDto>>> GetUsers([FromQuery] UserSpecificationParams userparams)
        {
            var spec = new UserSpecification(userparams);
            var users = await _securityRepository.GetAllWithSpec(spec);
            var specCount = new UserForCountingSpecification(userparams);
            var totalUsers = await _securityRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalUsers) / userparams.PageSize);
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserDto>>(users);

            return Ok(
                new Pagination<UserDto>
                {
                    Count = totalUsers,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = userparams.PageIndex,
                    PageSize = userparams.PageSize
                }
                );
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("role/{id}")]
        public async Task<ActionResult<UserDto>> UpdateRole(string id, RoleDto roleParam)
        {
            var role = _roleManager.FindByNameAsync(roleParam.Name);

            if(role == null)
            {
                return NotFound(new CodeErrorResponse(404, "El rol no existe"));
            }

            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));
            }

            var userDto = _mapper.Map<User, UserDto>(user);

            if (roleParam.Status)
            {
                var result = await _userManager.AddToRoleAsync(user, roleParam.Name);
                if (result.Succeeded)
                {
                    userDto.Admin = true;
                }

                if (result.Errors.Any())
                {
                    if(result.Errors.Where(x => x.Code == "UserAlreadyInRole").Any())
                    {
                        userDto.Admin = true;
                    }
                }
            }
            else
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleParam.Name);
                if (result.Succeeded)
                {
                    userDto.Admin = false;
                }
            }

            
            if (userDto.Admin)
            {
                var roles = new List<string>();
                roles.Add("ADMIN");
                userDto.Token = _tokenService.CreateToken(user, roles);
            }
            else
            {
                userDto.Token = _tokenService.CreateToken(user, null);
            }

            
            return userDto;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("account/{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.UserName,
                Image = user.Image,
                Admin = roles.Contains("ADMIN")
            };

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var user = await _userManager.SearchUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email,
                Username= user.UserName,
                Image = user.Image,
                Token = _tokenService.CreateToken(user, roles),
                Admin = roles.Contains("ADMIN")
            };

        }

        [HttpGet("emailValid")]
        public async Task<ActionResult<bool>> ValidateEmail([FromQuery]string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return false;

            return true;
        }

        [Authorize]
        [HttpGet("direction")]
        public async Task<ActionResult<DirectionDto>> GetDirection()
        {
            var user = await _userManager.SearchUserWithDirectionAsync(HttpContext.User);

            return _mapper.Map<Direction,DirectionDto>(user.Direction);
        }

        [Authorize]
        [HttpPut("direction")]
        public async Task<ActionResult<DirectionDto>> UpdateDirection(DirectionDto direction)
        {
            var user = await _userManager.SearchUserWithDirectionAsync(HttpContext.User);

            user.Direction = _mapper.Map<DirectionDto, Direction>(direction);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Direction, DirectionDto>(user.Direction));

            return BadRequest("No se pudo actualizar la direccion del usuario");
        }

    }
}
