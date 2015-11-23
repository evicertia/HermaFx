using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class ModelAwareValidationAttribute : ValidationAttribute
	{
		public ModelAwareValidationAttribute() { }

#if false // TODO: Bring client side stuff from foolproof. (pruiz)
		static ModelAwareValidationAttribute()
		{
			Register.All();
		}
#endif

		public override bool IsValid(object value)
		{
			throw new NotImplementedException();
		}

		public override string FormatErrorMessage(string name)
		{
			if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
				ErrorMessage = DefaultErrorMessage;

			return base.FormatErrorMessage(name);
		}

		public virtual string DefaultErrorMessage
		{
			get { return "{0} is invalid."; }
		}

		public virtual string ClientTypeName
		{
			get { return this.GetType().Name.Replace("Attribute", ""); }
		}

		protected virtual IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
		{
			return new KeyValuePair<string, object>[0];
		}

		public Dictionary<string, object> ClientValidationParameters
		{
			get { return GetClientValidationParameters().ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value); }
		}
	}
}