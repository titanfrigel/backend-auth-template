using FluentValidation;
using ISO3166;

namespace BackendAuthTemplate.Application.Common.Validation
{
    public static class CountryCodeValidator
    {
        public static IRuleBuilderOptions<T, string> CountryCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(IsValidCountry)
                .WithMessage("'{PropertyName}' is not a valid country code.");
        }

        private static bool IsValidCountry(string? countryCode)
        {
            if (countryCode == null)
            {
                return true;
            }

            return Country.List.Select(c => c.ThreeLetterCode).Contains(countryCode);
        }
    }
}