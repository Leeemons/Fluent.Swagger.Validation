using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public Func<IRuleComponent, bool> MatchFunc => v => v.Validator is IChildValidatorAdaptor;

        public Task Resolve(
            OpenApiSchema schema,
            SchemaFilterContext context,
            IValidationRule validationRule,
            IRuleComponent ruleComponent,
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers)
        {
            var childValidator = ruleComponent.Validator
                .GetType()
                .GetField("_validator", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(ruleComponent.Validator);

            if (childValidator is not IEnumerable<IValidationRule> validators)
            {
                logger.LogDebug($"Skipped '{childValidator}'");
                return Task.CompletedTask;
            }

            var innerSchema = validationRule is IIncludeRule ? schema : GetApiSchemeForProperty(context, validationRule);

            foreach (var rule in validators)
            {
                if (rule is not IValidationRule innerPropertyRule)
                {
                    logger.LogDebug($"Skipped '{rule}'");
                    continue;
                }
                foreach (var innerRuleComponent in rule.Components)
                {
                    foreach (var resolver in resolvers.Where(r => r.MatchFunc(innerRuleComponent)))
                    {
                        resolver.Resolve(innerSchema, context, innerPropertyRule, innerRuleComponent, validatorFactory, resolvers);
                    }
                }
            }

            if (validationRule is not IIncludeRule)
            {
                schema.Properties[validationRule.GetPropertyKey()] = innerSchema;
            }

            return Task.CompletedTask;
        }

        private OpenApiSchema GetApiSchemeForProperty(SchemaFilterContext context, IValidationRule validationRule)
        {
            var innerSchema = context.SchemaRepository.Schemas[validationRule.TypeToValidate.Name];
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