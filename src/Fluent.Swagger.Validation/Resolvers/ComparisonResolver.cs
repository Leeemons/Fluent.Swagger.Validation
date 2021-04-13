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
    public class ComparisonResolver : IResolver
    {
        public Func<IRuleComponent, bool> MatchFunc => v => v.Validator is IComparisonValidator;

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

            var comparisonValidator = (IComparisonValidator)ruleComponent.Validator;

            switch (comparisonValidator.Comparison)
            {
                case Comparison.Equal:
                    schemaProperty.Maximum = comparisonValidator.ValueToCompare.ToDecimal();
                    schemaProperty.Minimum = comparisonValidator.ValueToCompare.ToDecimal();
                    break;

                case Comparison.GreaterThan:
                    schemaProperty.Minimum = comparisonValidator.ValueToCompare.ToDecimal();
                    schemaProperty.ExclusiveMaximum = true;
                    break;

                case Comparison.GreaterThanOrEqual:
                    schemaProperty.Minimum = comparisonValidator.ValueToCompare.ToDecimal();
                    break;

                case Comparison.LessThan:
                    schemaProperty.Minimum = comparisonValidator.ValueToCompare.ToDecimal();
                    schemaProperty.ExclusiveMinimum = true;
                    break;

                case Comparison.LessThanOrEqual:
                    schemaProperty.Minimum = comparisonValidator.ValueToCompare.ToDecimal();
                    break;
            }

            return Task.CompletedTask;
        }
    }
}