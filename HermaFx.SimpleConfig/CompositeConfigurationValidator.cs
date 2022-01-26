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
            var context = new ValidationContext(value) { MemberName = _propertyName };
            var errors = _validationAttributes
                .Select(x => x.GetValidationResult(value, context))
                .Where(x => x != ValidationResult.Success)
                .ToArray();

            if(errors.Any())
            {
                var errorMsgs = new StringBuilder("Validation Errors:");
                var fullMsg = errors.Select(x => x.ErrorMessage).Aggregate(errorMsgs, (sb, cur) => sb.AppendLine(cur)).ToString();
                throw new ArgumentException(fullMsg);
            }
        }
    }
}
