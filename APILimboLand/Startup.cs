using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMap;
using AutoMapper;
using DataBase.Models;
using EmailConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository.Repository;

namespace APILimboLand
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //AutoMapper
            services.AddAutoMapper(typeof(APIAutomapping).GetTypeInfo().Assembly);

            //dataServices
            services.AddDbContext<LIMBODBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));

            //Identity            
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 3,
                    RequireUppercase = false,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false
                };
            }).AddEntityFrameworkStores<LIMBODBContext>().AddDefaultTokenProviders();



            //Repository
            services.AddScoped<PublicacionesAPIRepo>();
            services.AddScoped<UsuarioAPIRepo>();
            services.AddScoped<ComentariosAPIRepo>();
            //services.AddScoped<AmigosRepo>();
            services.AddScoped<ImagenesAPIRepo>();


            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = String.Empty;

                });
            }
            else
            {
                app.UseHsts();
            }
           

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
