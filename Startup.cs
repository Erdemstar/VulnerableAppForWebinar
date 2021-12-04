using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VulnerableAppForWebinar.Mapper;
using VulnerableAppForWebinar.Repository.Account;
using VulnerableAppForWebinar.Repository.Profile;
using VulnerableAppForWebinar.Utility.Database;
using VulnerableAppForWebinar.Utility.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulnerableAppForWebinar.Repository.Card;

namespace VulnerableAppForWebinar
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

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWTSettings:Issuer"],
                    ValidAudience = Configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSettings:Key"]))

                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VulnerableAppForWebinar",
                    Description = "This project created for API Security Webinar",
                    Contact = new OpenApiContact() { Name = "Erdemstar" , Email = "erdem.yildiz@windowslive.com"},
                });
            });

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mapperConfig.CreateMapper();

            services.Configure<JWTSettings>(Configuration.GetSection(nameof(JWTSettings)));
            services.AddSingleton<IJWTSettings>(sp => sp.GetRequiredService<IOptions<JWTSettings>>().Value);

            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions< DatabaseSettings>>().Value);
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));


            services.AddSingleton(mapper);
            services.AddSingleton<JWTAuthManager>();
            services.AddSingleton<AccountRepository>();
            services.AddSingleton<ProfileRepository>();
            services.AddSingleton<CardRepository>();

            services.AddControllers();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VulnerableAppForWebinar");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
