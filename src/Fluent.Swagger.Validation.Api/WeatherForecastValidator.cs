using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluent.Swagger.Validation.Api
{
    public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
    {
        public WeatherForecastValidator()
        {
            RuleFor(r => r.Date).NotNull();
            RuleFor(r => r.Summary).NotEmpty();
            RuleFor(r => r.Email).NotNull().EmailAddress();
            RuleFor(r => r.Telephone).NotNull();

            RuleFor(r => r.Address).NotNull().ChildRules(c =>
            {
                c.RuleFor(a => a.Street).NotNull();
            });            
        }
    }
}
