using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class BrandController : BaseApiController
    {
        private readonly IGenericRepository<Brand> _brandRepository;

        public BrandController(IGenericRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrandAll()
        {
            return Ok(await _brandRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }
    }
}
