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
    public class NotNullResolver : IResolver
    {
        public Func<IRuleComponent, bool> MatchFunc => v => v.Validator is INotNullValidator;

        public Task Resolve(
            OpenApiSchema schema,
            SchemaFilterContext context,
            IValidationRule validationRule,
            IRuleComponent ruleComponent,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            if (validationRule.HasConditions() || ruleComponent.HasConditions()) return Task.CompletedTask;

            if (schema.Required == null) schema.Required = new SortedSet<string>();
            if (!schema.Required.Contains(validationRule.GetPropertyKey()))
            {
                schema.Required.Add(validationRule.GetPropertyKey());
                schema.Properties[validationRule.GetPropertyKey()].Nullable = false;
            }

            return Task.CompletedTask;
        }
    }
}