using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.Settings
{
	public class GenericTypeDescriptorContext : ITypeDescriptorContext
	{
		#region .ctor
		public GenericTypeDescriptorContext(PropertyInfo property)
		{
			PropertyDescriptor = TypeDescriptor.GetProperties(property.DeclaringType)[property.Name];
		}
#if false
		public GenericTypeDescriptorContext(object instance, string propertyName)
		{
			Instance = instance;
			PropertyDescriptor = TypeDescriptor.GetProperties(instance)[propertyName];
		}

		public GenericTypeDescriptorContext(object instance, PropertyDescriptor propertyDescriptor)
		{
			Instance = instance;
			PropertyDescriptor = propertyDescriptor;
		}
#endif
		#endregion

		public PropertyDescriptor PropertyDescriptor
		{
			get; private set;
		}

		#region Not Implemented Functionality
		public object Instance
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IContainer Container
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public object GetService(Type serviceType)
		{
			throw new NotImplementedException();
		}

		public void OnComponentChanged()
		{
			throw new NotImplementedException();
		}

		public bool OnComponentChanging()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
