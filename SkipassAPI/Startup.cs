using CustomSwaggerAttributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkipassAPI
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
            services.AddSwaggerGen();
            services.AddSwaggerGen(options => { options.CustomSchemaIds(type => type.ToString()); options.EnableAnnotations();});
          //  services.AddSwaggerGen(c => { c.DocumentFilter<HideInDocsFilter>(); });
            var directory = Directory.GetCurrentDirectory();
            if (!string.IsNullOrEmpty(directory))
            {
                var descriptions = Directory.EnumerateFiles(directory, "*.xml", SearchOption.AllDirectories).ToList();
                if (descriptions != null) { foreach (var item in descriptions) { services.AddSwaggerGen(options => { options.IncludeXmlComments(item); }); } }
            }


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //     app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction => { setupAction.RoutePrefix = String.Empty; setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiPass API v1.2"); });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
