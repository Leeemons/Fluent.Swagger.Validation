using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fluent.Swagger.Validation.Resolvers
{
    public class LengthResolver : IResolver
    {
        public Func<IPropertyValidator, bool> MatchFunc => v => v is LengthValidator;

        public Task Resolve(
            OpenApiSchema schema,
            SchemaFilterContext context,
            PropertyRule propertyRule,
            IPropertyValidator propertyValidator,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            if (propertyRule.HasConditions() || propertyValidator.HasConditions()) return Task.CompletedTask;

            var schemaProperty = schema.Properties[propertyRule.GetPropertyKey()];

            var lengthValidator = (LengthValidator)propertyValidator;

            if (lengthValidator.Max > 0) schemaProperty.Maximum = lengthValidator.Max;
            if (lengthValidator.Min > 0)
            {
                schemaProperty.Minimum = lengthValidator.Min;
                schemaProperty.Nullable = false;
            }

            return Task.CompletedTask;
        }
    }
}
