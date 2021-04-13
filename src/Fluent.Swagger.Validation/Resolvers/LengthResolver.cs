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
        public Func<IRuleComponent, bool> MatchFunc => v => v.Validator is ILengthValidator;

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

            var lengthValidator = (ILengthValidator)ruleComponent.Validator;

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