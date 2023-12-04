
using BussinessLogic.Data;
using BussinessLogic.Logic;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Dtos;
using WebApi.Middleware;

namespace WebApi;
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddScoped<ITokenService, TokenService>();

        var builder = services.AddIdentityCore<User>();

        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddEntityFrameworkStores<SecurityDbContext>();
        builder.AddSignInManager<SignInManager<User>>();

        services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

        services.AddAutoMapper(typeof(MappingProfiles));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddDbContext<MarketDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddDbContext<SecurityDbContext>(x =>
        {
            x.UseSqlServer(Configuration.GetConnectionString("IdentitySecurity"));
        });
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddControllers();

        services.AddCors( opt =>
        {
            opt.AddPolicy("CorsRule", rule =>
            {
                rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //if (env.IsDevelopment())
        //    app.UseDeveloperExceptionPage();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");

        app.UseRouting();
        app.UseCors("CorsRule");
        //Asegurrse de queste UseAuthentication
        app.UseAuthentication();
        //------------------------------------
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
