using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Karshare.API.Data;
using Karshare.API.Models;
//using Karshare.API.Repositories;
//using Karshare.API.Services;
using System.Text;
using System.Text.Json.Serialization;
using Karshare.API.Repositories;
using Karshare.API.Services.Interfaces;
using Karshare.API.Services;

namespace Karshare.API.Helpers
{
    public static class DependencyInjectionExtensions
    {
        public static void InjectDepencies(this WebApplicationBuilder builder)
        {

            builder.Services.AddControllers()
                            .AddJsonOptions(options => 
                                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.AddSwagger();

            builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

            builder.AddRepositories();

            builder.AddServices();

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            builder.AddAuthentication();
        }

        private static void AddSwagger(this WebApplicationBuilder builder)
        {

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Karshare.Api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your Bearer token in the format **Bearer {token}** to access this API."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        private static void AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRepository<User, Guid>, UserRepository>();
        }

        private static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<FirstRunService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }

        private static void AddAuthentication(this WebApplicationBuilder builder)
        {

            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings!.SecretKey);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
