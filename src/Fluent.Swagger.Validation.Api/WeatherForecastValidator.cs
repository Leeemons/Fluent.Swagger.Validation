using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Fluent.Swagger.Validation.Api
{
    public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
    {
        public WeatherForecastValidator(IHttpContextAccessor httpContextAccessor)
        {
            RuleFor(r => r.Date).NotNull();
            RuleFor(r => r.Summary).NotEmpty();
            RuleFor(r => r.Email).NotNull().EmailAddress();
            RuleFor(r => r.Telephone).NotNull();

            RuleFor(r => r.Address).NotNull().ChildRules(c =>
            {
                c.RuleFor(a => a.Street).NotNull();
                c.RuleFor(a => a.InnerAddress).ChildRules(r =>
                {
                    r.RuleFor(a => a.HS).NotNull();
                    r.RuleFor(a => a.TS).NotNull().When(_ => 5 < 0);
                });
            });

            RuleFor(r => r.MinLength).NotEmpty();
            RuleFor(r => r.Length).MaximumLength(10).MinimumLength(5);
            RuleFor(r => r.MaxLength).MaximumLength(10);
            RuleFor(r => r.Regular).Matches("^*$");
            RuleFor(r => r.Equal).Equal(10);
            RuleFor(r => r.GreaterThan).GreaterThan(20);
            RuleFor(r => r.GreaterThanOrEqual).GreaterThanOrEqualTo(30);
            RuleFor(r => r.LessThan).LessThan(40);
            RuleFor(r => r.LessThanOrEqual).LessThanOrEqualTo(50);
            RuleFor(r => r.ExclusiveBetween).ExclusiveBetween(100, 200);
            RuleFor(r => r.InclusiveBetween).InclusiveBetween(300, 400);
        }
    }
}
