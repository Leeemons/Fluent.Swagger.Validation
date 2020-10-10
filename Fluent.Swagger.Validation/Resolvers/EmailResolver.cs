using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fluent.Swagger.Validation.Resolvers
{
    public class EmailResolver : IResolver
    {
        public Func<IPropertyValidator, bool> MatchFunc => v => v is AspNetCoreCompatibleEmailValidator || v is EmailValidator;

        public Task Resolve(OpenApiSchema schema, SchemaFilterContext context, PropertyRule propertyRule, IPropertyValidator propertyValidator, IValidatorFactory validatorFactory, IEnumerable<IResolver> resolvers)
        {
            var schemaProperty = schema.Properties[propertyRule.GetPropertyKey()];
            schemaProperty.Format = "email";

            return Task.CompletedTask;
        }
    }
}
