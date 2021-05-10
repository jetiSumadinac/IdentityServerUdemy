using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;

namespace Movies.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies.API", Version = "v1" });
            });
            services.AddDbContext<MoviesAPIContext>(options =>
                    options.UseInMemoryDatabase("Movies"));
            //options.UseSqlServer(Configuration.GetConnectionString("MoviesAPIContext")));

            services.AddAuthentication(
                options =>
                {
                    //JwtBearer config
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5005";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //NameClaimType = JwtClaimTypes.GivenName,
                        //RoleClaimType = JwtClaimTypes.Role,
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        ValidTypes = new[] { "at+jwt" },
                        ValidateAudience = true
                    };
                    options.Audience = "apiResource";
                    options.MapInboundClaims = true;
                });

            /*
             Http request example
                POST: https://localhost:5005/connect/token
                grant_type:password
                scope:movieApi openid profile address email roles
                client_id:movies_external_client
                client_secret:secret
                username:bob
                password:bob
                claims:roles
             */

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id",  "movieClient", "movies_mvc_client", "movies_external_client"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
