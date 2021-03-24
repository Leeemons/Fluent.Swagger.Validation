using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluent.Swagger.Validation.Resolvers
{
    public class ChildResolver : IResolver
    {
        private readonly ILogger<ChildResolver> logger;

        public ChildResolver(ILogger<ChildResolver> logger)
        {
            this.logger = logger;
        }

        public Func<IPropertyValidator, bool> MatchFunc => v => v is IChildValidatorAdaptor;

        public Task Resolve(
            OpenApiSchema schema,
            SchemaFilterContext context,
            PropertyRule propertyRule,
            IPropertyValidator propertyValidator,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            var propertyValidationContext = new PropertyValidatorContext(new ValidationContext<object>(new { }), null, string.Empty, null);

            var childValidatorAdaptor = (IChildValidatorAdaptor)propertyValidator;

            var childValidator = (IValidator)childValidatorAdaptor
                .GetType()
                .GetMethod(nameof(ChildValidatorAdaptor<object, object>.GetValidator))
                .Invoke(childValidatorAdaptor, new[] { propertyValidationContext });

            if (childValidator is not IEnumerable<IValidationRule> validators)
            {
                logger.LogDebug($"Skipped '{childValidator}'");
                return Task.CompletedTask;
            }

            var innerSchema = propertyRule is IIncludeRule ? schema : GetApiSchemeForProperty(context, propertyRule);

            foreach (var rule in validators)
            {
                if (rule is not PropertyRule innerPropertyRule)
                {
                    logger.LogDebug($"Skipped '{rule}'");
                    continue;
                }
                foreach (var innerPropertyValidator in rule.Validators)
                {
                    foreach (var resolver in resolvers.Where(r => r.MatchFunc(innerPropertyValidator)))
                    {
                        resolver.Resolve(innerSchema, context, innerPropertyRule, innerPropertyValidator, validatorFactory, resolvers);
                    }
                }
            }

            if (propertyRule is not IIncludeRule)
            {
                schema.Properties[propertyRule.GetPropertyKey()] = innerSchema;
            }

            return Task.CompletedTask;
        }

        private OpenApiSchema GetApiSchemeForProperty(SchemaFilterContext context, PropertyRule propertyRule)
        {
            var innerSchema = context.SchemaRepository.Schemas[propertyRule.TypeToValidate.Name];
            OpenApiSchema newInnerScheme = GetNewInnerScheme(innerSchema);

            foreach (var property in innerSchema.Properties)
            {
                if (!newInnerScheme.Properties.ContainsKey(property.Key))
                {
                    newInnerScheme.Properties[property.Key] = GetNewInnerScheme(innerSchema.Properties[property.Key]);
                }
            }

            return newInnerScheme;
        }

        private static OpenApiSchema GetNewInnerScheme(OpenApiSchema innerSchema)
        {
            return innerSchema.DeepCopy();
        }
    }
}