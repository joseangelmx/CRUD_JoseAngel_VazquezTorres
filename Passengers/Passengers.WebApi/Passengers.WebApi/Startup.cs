using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Passengers.ApplicationServices.Passengers;
using Passengers.Core.Passengers;
using Passengers.DataAccess;
using Passengers.DataAccess.Repositories;
using System;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Passengers.Shared.Config;
using Microsoft.AspNetCore.Identity;
using Passengers.WebApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Passengers.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("Default");
            string connectionStringLogs = Configuration.GetConnectionString("Logs");
            services.AddDbContext<PassengersContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            services.AddIdentity<IdentityUser, IdentityRole>(
                opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequireNonAlphanumeric = true;
                    opts.Password.RequiredLength = 7;
                    opts.Password.RequiredUniqueChars = 4;
                }).AddEntityFrameworkStores<PassengersContext>()
                .AddDefaultTokenProviders();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddTransient<IPassengersAppService, PassengersAppService>();
            services.AddTransient<IRepository<int, Passenger>, Repository<int, Passenger>>();
            services.Configure<JwtTokenValidationSettings>(Configuration.GetSection("JwtTokenValidationSettings"));
            services.AddTransient<IJwtIssuerOptions, JwtIssuerFactory>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Passengers Microservice API",
                    Description = "An ASP.NET Core Web Api for managing a passengers database",
                    Contact = new OpenApiContact
                    {
                        Name = "Contact the creator Jose Angel Vazquez",
                        Url = new Uri("https://github.com/joseangelmx")
                    }
                });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description="Jwt Authorization header using the bearer scheme  \r\n\r\n Enter your token in the text input before \r\n ",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                        new string[]{}
                }
                });
            });
            var tokenValidationSettings = services.BuildServiceProvider().GetService<IOptions<JwtTokenValidationSettings>>().Value;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidIssuer = tokenValidationSettings.ValidIssuer,
                    ValidAudience = tokenValidationSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidationSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PassengersContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.InitDb();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Passengers.WebApi v1"));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
