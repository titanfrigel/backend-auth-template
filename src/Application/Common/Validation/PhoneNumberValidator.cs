using FluentValidation;
using PhoneNumbers;

namespace BackendAuthTemplate.Application.Common.Validation
{
    public static class PhoneNumberValidator
    {
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(IsValidPhoneNumber)
                .WithMessage("'{PropertyName}' is not a valid phone number.");
        }

        private static bool IsValidPhoneNumber(string? phoneNumber)
        {
            if (phoneNumber == null)
            {
                return true;
            }

            try
            {
                PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

                PhoneNumber parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, null);

                return phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
    }
}