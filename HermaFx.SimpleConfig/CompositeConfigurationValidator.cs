using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HermaFx.SimpleConfig
{
    internal class CompositeConfigurationValidator : ConfigurationValidatorBase
    {
        private readonly ValidationAttribute[] _validationAttributes;
        private readonly string _propertyName;

        public CompositeConfigurationValidator(ValidationAttribute[] validationAttributes, string propertyName)
        {
            _validationAttributes = validationAttributes;
            _propertyName = propertyName;
        }

        public override bool CanValidate(Type type)
        {
            return true;
        }

        public override void Validate(object value)
        {
            var validationErrors = (from validation in _validationAttributes
                                    where validation.IsValid(value) == false
                                    select validation.FormatErrorMessage(_propertyName)).ToList();

            if(validationErrors.Any())
            {
                var errorMsgs = new StringBuilder("Validation Errors:");
                var fullMsg = validationErrors.Aggregate(errorMsgs, (sb, cur) => sb.AppendLine(cur)).ToString();
                throw new ArgumentException(fullMsg);
            }
        }
    }
}
