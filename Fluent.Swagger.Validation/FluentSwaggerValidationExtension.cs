using System;
using System.Collections.Generic;
using System.IO;
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
    }
}
