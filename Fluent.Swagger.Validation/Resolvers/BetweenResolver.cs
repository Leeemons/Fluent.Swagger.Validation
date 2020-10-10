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
    public class BetweenResolver : IResolver
    {
        public Func<IPropertyValidator, bool> MatchFunc => v => v is IBetweenValidator;

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

            var betweenValidator = (IBetweenValidator)propertyValidator;

            if (betweenValidator.From.GetType().IsValueType && betweenValidator.To.GetType().IsValueType)
            {
                schemaProperty.Maximum = betweenValidator.To.ToDecimal();
                schemaProperty.Minimum = betweenValidator.From.ToDecimal();
                if (betweenValidator is ExclusiveBetweenValidator)
                {
                    schemaProperty.ExclusiveMaximum = true;
                    schemaProperty.ExclusiveMinimum = true;
                }
            }


            return Task.CompletedTask;
        }
    }
}
