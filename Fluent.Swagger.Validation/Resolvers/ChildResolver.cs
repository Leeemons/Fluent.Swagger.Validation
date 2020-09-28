using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var childValidatorAdapter = (IChildValidatorAdaptor)propertyValidator;

            var childValidator = (IValidator)childValidatorAdapter
                .GetType()
                .GetMethod(nameof(ChildValidatorAdaptor<object, object>.GetValidator))
                .Invoke(childValidatorAdapter, new[] { propertyValidationContext });

            if (childValidator is not IEnumerable<IValidationRule> validators)
            {
                logger.LogDebug($"Skipped '{childValidator}'");
                return Task.CompletedTask;
            }

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
                        var innerSchema = context.SchemaRepository.Schemas[propertyRule.PropertyName].DeepCopy();
                        resolver.Resolve(innerSchema, context, innerPropertyRule, innerPropertyValidator, validatorFactory, resolvers);
                        schema.Properties[propertyRule.PropertyName.ToLower()] = innerSchema;
                    }
                }
            }
            

            return Task.CompletedTask;
        }
    }
}
