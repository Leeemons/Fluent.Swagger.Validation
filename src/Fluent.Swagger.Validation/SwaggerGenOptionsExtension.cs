using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluent.Swagger.Validation
{
    public static class SwaggerGenOptionsExtension
    {
        public static void AddFluentSwaggerFilters(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.SchemaFilter<FluentSwaggerValidationSchemeFilter>();
            swaggerGenOptions.OperationFilter<FluentSwaggerValidationOperationFilter>();
        }
    }
}
