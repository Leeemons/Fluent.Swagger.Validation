using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using System;
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
                ? char.ToLowerInvariant(text[0]) + text[1..]
                : isAllUpper ? text.ToLowerInvariant() : text;
        }

        public static string GetPropertyKey(this IValidationRule propertyRule)
        {
            return propertyRule.PropertyName.ToCamelCase();
        }

        public static bool HasConditions(this IValidationRule propertyRule)
        {
            return propertyRule.HasCondition || propertyRule.HasAsyncCondition;
        }

        public static bool HasConditions(this IRuleComponent ruleComponent)
        {
            return ruleComponent.HasCondition || ruleComponent.HasAsyncCondition;
        }

        public static bool IsAllUpper(this string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsUpper(text[i])) return false;
            }

            return true;
        }

        public static decimal ToDecimal(this object @object)
        {
            return Convert.ToDecimal(@object);
        }
    }
}