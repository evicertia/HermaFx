using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Castle.Components.DictionaryAdapter;

using PropertyDescriptor = Castle.Components.DictionaryAdapter.PropertyDescriptor;

namespace Castle.Components.DictionaryAdapter
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
	public class AppSettingsAttribute : KeyPrefixAttribute,
#if USE_DAVALIDATOR // FIXME: Validating thru DictionaryAdapter's own validation mechanism is not working.
		//IDictionaryInitializer,
		//IDictionaryValidator,
#endif
		IDictionaryPropertyGetter,
		IPropertyDescriptorInitializer
	{

		public AppSettingsAttribute(string keyPrefix)
			: base(keyPrefix)
		{
		}

#if USE_DAVALIDATOR
		public void Initialize(IDictionaryAdapter dictionaryAdapter, object[] behaviors)
		{
			dictionaryAdapter.CanValidate = true;
			dictionaryAdapter.AddValidator(this);
		}
#endif

		public void Initialize(PropertyDescriptor propertyDescriptor, object[] behaviors)
		{
			propertyDescriptor.Fetch = true;
		}

		private bool ValueIsNullOrDefault(PropertyDescriptor descriptor, object value)
		{
			return descriptor.PropertyType.IsByRef ?
				Activator.CreateInstance(descriptor.PropertyType) == value
				: value == null;
		}

		public object GetPropertyValue(IDictionaryAdapter dictionaryAdapter, string key, object storedValue,
				PropertyDescriptor descriptor, bool ifExists)
		{
			if (ValueIsNullOrDefault(descriptor, storedValue))
			{
				var defaultValue = descriptor.Annotations.OfType<DefaultValueAttribute>().SingleOrDefault();

				if (IsRequired(descriptor, ifExists))
				{
					throw new ValidationException(string.Format("No valid value for '{0}' found", key));
				}
				else if (defaultValue != null)
				{
					storedValue = defaultValue.Value;
				}
			}

#if !USE_DAVALIDATOR
			if (storedValue != null)
			{
				var propinfo = descriptor.Property;
				var context = new ValidationContext(storedValue)
				{
					DisplayName = propinfo.Name,
					MemberName = propinfo.Name
				};
				{
					var attrs = propinfo.GetCustomAttributes(true).OfType<ValidationAttribute>().ToArray();
					Validator.ValidateValue(storedValue, context, attrs);
				}
			}
#endif
			return storedValue;
		}

		private static bool IsRequired(PropertyDescriptor property, bool ifExists)
		{
			return property.Annotations.Any(x => x is RequiredAttribute) && ifExists == false;
		}

#if USE_DAVALIDATOR
		#region IDictionaryValidator

		// FIXME: Validation of Class-Level attributes missing.

		public bool IsValid(IDictionaryAdapter dictionaryAdapter)
		{
			foreach (var property in dictionaryAdapter.This.Properties)
			{
				if (Validate(dictionaryAdapter, property.Value) != String.Empty)
					return false;
			}

			return true;
		}

		public string Validate(IDictionaryAdapter dictionaryAdapter)
		{
			var results = new List<string>();

			foreach (var property in dictionaryAdapter.This.Properties)
			{
					var result = Validate(dictionaryAdapter, property.Value);
					if (result != null) results.Add(result);
			}

			return results.Count > 0 ? string.Join("", results) : String.Empty;
		}
		
		public string Validate(IDictionaryAdapter dictionaryAdapter, PropertyDescriptor property)
		{
			var key = dictionaryAdapter.GetKey(property.PropertyName);
			var value = dictionaryAdapter.GetProperty(property.PropertyName, true);
			var propinfo = property.Property;
			var context = new ValidationContext(value)
			{
					DisplayName = key,
					MemberName = propinfo.Name
			};

			var attrs = propinfo.GetCustomAttributes(true).OfType<ValidationAttribute>().ToArray();
			var results = new System.Collections.Generic.List<ValidationResult>();

			return Validator.TryValidateValue(value, context, results, attrs) ?
				String.Empty
				: string.Join(Environment.NewLine, results.Select(x => x.ErrorMessage));
		}

		public void Invalidate(IDictionaryAdapter dictionaryAdapter)
		{
		}
		#endregion
#endif
	}
}


