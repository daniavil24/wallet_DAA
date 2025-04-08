
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WalletApp.Application.Interfaces;
using WalletApp.Application.Services;
using WalletApp.Infrastructure;

namespace WalletApp.API
{
    public static class ProgramServicesBuilder
    {
        public static void ConfigurarServicios(this WebApplicationBuilder builder)
        {
            ConfigurarServiciosFramework(builder);
            ConfigurarServiciosScoped(builder);
        }

        private static void ConfigurarServiciosScoped(WebApplicationBuilder builder)
        {
            //----------> Inyección dependencias.
            builder.Services.AddScoped<IWalletService, WalletService>();
        }

        private static void ConfigurarServiciosFramework(WebApplicationBuilder builder)
        {
            //-------------------------------- Configuración de JWT--------------------------------
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };

                //----->Mensajes para errores en autorización en los métodos.
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(@"{""error"":""Token inválido o expirado""}");
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(@"{""error"":""Se requiere un token válido""}");
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(@"{""error"":""No tiene permisos para acceder""}");
                    }
                };
            });

            //-------------------------------- Configuración de BD--------------------------------
            builder.Services.AddDbContext<WalletDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthorization();
        }
    }
}
