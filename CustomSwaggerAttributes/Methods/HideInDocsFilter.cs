using CustomSwaggerAttributes.AttributesModels;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Description;

namespace CustomSwaggerAttributes
{
    public class HideInDocsFilter : IDocumentFilter
    {

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach (var apiDescription in apiExplorer.ApiDescriptions)
            {
                if (!apiDescription.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<HideInDocsAttribute>().Any() && !apiDescription.ActionDescriptor.GetCustomAttributes<HideInDocsAttribute>().Any()) continue;
                var route = "/" + apiDescription.Route.RouteTemplate.TrimEnd('/');
                swaggerDoc.paths.Remove(route);
            }
        }


    }

}

