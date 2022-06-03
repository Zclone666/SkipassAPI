using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace CustomSwaggerAttributes.Methods
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            config
                 .EnableSwagger(c =>
                 {                   
                    c.DocumentFilter<HideInDocsFilter>();
                })
                .EnableSwaggerUi(c =>
                {

                });
        }
    }
}
