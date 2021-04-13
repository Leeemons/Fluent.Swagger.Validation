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
    public class NotEmptyResolver : IResolver
    {
        public Func<IRuleComponent, bool> MatchFunc => v => v.Validator is INotEmptyValidator;

        public Task Resolve(OpenApiSchema schema,
            SchemaFilterContext context,
            IValidationRule validationRule,
            IRuleComponent ruleComponent,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            if (validationRule.HasConditions() || ruleComponent.HasConditions()) return Task.CompletedTask;

            var schemaProperty = schema.Properties[validationRule.GetPropertyKey()];
            schemaProperty.MinLength = 1;
            schemaProperty.Nullable = false;
            return Task.CompletedTask;
        }
    }
}