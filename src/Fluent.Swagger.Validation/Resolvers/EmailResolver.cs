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
    public class EmailResolver : IResolver
    {
        public Func<IRuleComponent, bool> MatchFunc => v => v.Validator is IEmailValidator;

        public Task Resolve(
            OpenApiSchema schema,
            SchemaFilterContext context,
            IValidationRule validationRule,
            IRuleComponent ruleComponent,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            var schemaProperty = schema.Properties[validationRule.GetPropertyKey()];
            schemaProperty.Format = "email";

            return Task.CompletedTask;
        }
    }
}