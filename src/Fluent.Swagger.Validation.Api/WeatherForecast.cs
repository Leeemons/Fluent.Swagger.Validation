using System;

namespace Fluent.Swagger.Validation.Api
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)( TemperatureC / 0.5556 );

        public string Summary { get; set; }

        public string? Email { get; set; }

        public string Telephone { get; set; }

        public Address Address { get; set; }

        public int? MinLength { get; set; }

        public string? Length { get; set; }
        public string? MaxLength { get; set; }
        public string? Regular { get; set; }

        public int Equal { get; set; }
        public int GreaterThan { get; set; }
        public int GreaterThanOrEqual { get; set; }
        public int LessThan { get; set; }
        public int LessThanOrEqual { get; set; }
        public int ExclusiveBetween { get; set; }
        public int InclusiveBetween { get; set; }

    }

    public class Address
    {
        public string Street { get; set; }
        public string? HouseNumber { get; set; }

        public InnerAddress InnerAddress { get; set; }
    }

    public class InnerAddress
    {
        public string HS { get; set; }
        public string? TS { get; set; }
    }
}
