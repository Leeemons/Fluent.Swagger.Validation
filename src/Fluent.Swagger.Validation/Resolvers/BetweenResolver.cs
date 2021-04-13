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
        public Func<IRuleComponent, bool> MatchFunc => v => v.Validator is IBetweenValidator;

        public Task Resolve(
            OpenApiSchema schema,
            SchemaFilterContext context,
            IValidationRule validationRule,
            IRuleComponent ruleComponent,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            if (validationRule.HasConditions() || ruleComponent.HasConditions()) return Task.CompletedTask;

            var schemaProperty = schema.Properties[validationRule.GetPropertyKey()];

            var betweenValidator = (IBetweenValidator)ruleComponent.Validator;

            if (betweenValidator.From.GetType().IsValueType && betweenValidator.To.GetType().IsValueType)
            {
                schemaProperty.Maximum = betweenValidator.To.ToDecimal();
                schemaProperty.Minimum = betweenValidator.From.ToDecimal();
                if (betweenValidator.Name == "ExclusiveBetweenValidator")
                {
                    schemaProperty.ExclusiveMaximum = true;
                    schemaProperty.ExclusiveMinimum = true;
                }
            }

            return Task.CompletedTask;
        }
    }
}