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
    public class ComparisonResolver : IResolver
    {
        public Func<IPropertyValidator, bool> MatchFunc => v => v is IComparisonValidator;

        public Task Resolve(OpenApiSchema schema, SchemaFilterContext context, PropertyRule propertyRule, IPropertyValidator propertyValidator, IValidatorFactory validatorFactory, IEnumerable<IResolver> resolvers)
        {
            if (propertyRule.HasConditions() || propertyValidator.HasConditions()) return Task.CompletedTask;

            var schemaProperty = schema.Properties[propertyRule.GetPropertyKey()];

            var comparisonValidator = (IComparisonValidator)propertyValidator;

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
                    schemaProperty.Maximum = comparisonValidator.ValueToCompare.ToDecimal();
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
