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
    public interface IResolver
    {
        Func<IPropertyValidator, bool> MatchFunc { get; }

        Task Resolve(
            OpenApiSchema schema, 
            SchemaFilterContext context, 
            PropertyRule propertyRule, 
            IPropertyValidator propertyValidator, 
            IValidatorFactory validatorFactory,
            IEnumerable<IResolver> resolvers);
    }
}
