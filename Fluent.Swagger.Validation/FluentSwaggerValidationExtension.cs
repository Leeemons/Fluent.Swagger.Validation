using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace Fluent.Swagger.Validation
{
    public static class FluentSwaggerValidationExtension
    {
        public static T DeepCopy<T>(this T other)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(other);

            return JsonSerializer.Deserialize<T>(bytes);
        }

        public static string ToCamelCase(this string text)
        {
            var isAllUpper = text.IsAllUpper();

            return !string.IsNullOrWhiteSpace(text) && text.Length >= 1 && !isAllUpper
                ? char.ToLowerInvariant(text[0]) + text.Substring(1) 
                : isAllUpper ? text.ToLowerInvariant() : text;
        }

        public static string GetPropertyKey(this PropertyRule propertyRule)
        {
            return propertyRule.PropertyName.ToCamelCase();
        }

        public static bool HasConditions(this PropertyRule propertyRule)
        {
            return propertyRule.Condition != null || propertyRule.AsyncCondition != null;
        }

        public static bool HasConditions(this IPropertyValidator propertyValidator)
        {
            return propertyValidator.Options?.Condition != null || propertyValidator.Options?.AsyncCondition != null;
        }

        public static bool IsAllUpper(this string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsUpper(text[i])) return false;
            }

            return true;
        }
    }
}
