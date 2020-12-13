using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    internal class CompositeConfigurationValidator : ConfigurationValidatorBase
    {
        private readonly ValidationAttribute[] _validationAttributes;
		private readonly MemberInfo _member;

		public CompositeConfigurationValidator(ValidationAttribute[] validationAttributes, MemberInfo member)
        {
            _validationAttributes = validationAttributes;
			_member = member;
        }

		private void ValidateFallback(object value)
		{
			var errors = (from validation in _validationAttributes
				where validation.IsValid(value) == false
			    	select validation.FormatErrorMessage(_member.Name)).ToList();

			if (errors.Any())
			{
				var errorMsgs = new StringBuilder("Validation Errors:");
				var fullMsg = errors.Aggregate(errorMsgs, (sb, cur) => sb.AppendLine(cur)).ToString();
				throw new ArgumentException(fullMsg);
			}
		}

        public override bool CanValidate(Type type)
        {
            return true;
        }

        public override void Validate(object value)
        {
			var assembly = typeof(System.ComponentModel.DataAnnotations.RequiredAttribute).Assembly;
			var vtype = assembly.GetType("System.ComponentModel.DataAnnotations.Validator");

			if (vtype == null)
			{
				ValidateFallback(value);
				return;
			}

			try
			{
				Console.WriteLine(" ==> TYPE: {0}", value?.GetType());

				var ctype = assembly.GetType("System.ComponentModel.DataAnnotations.ValidationContext");
				var context = Activator.CreateInstance(ctype, new object[] { value });
				var validator = vtype.GetMethod("ValidateProperty",
					BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
					null,
					CallingConventions.Any,
					new Type[] { typeof(object), ctype },
					null
				);

				//var otypeprop = ctype.GetProperty("ObjectType");
				//otypeprop.SetValue(context, _member.DeclaringType, null);
				var mnameprop = ctype.GetProperty("MemberName");
				mnameprop.SetValue(context, _member.Name, null);

				validator.Invoke(null, new object[] { value, context });
			}
			catch (Exception ex)
			{
				Console.WriteLine(" ==> EXCEPTION: {0}\n{1}\n <== EXCEPTION", ex, ex.StackTrace);
				throw;
			}
        }
    }
}
