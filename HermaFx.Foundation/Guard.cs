using System;

namespace HermaFx
{
	/// <summary>
	/// Provides utility methods to guard parameter and local variables.
	/// </summary>
	public static partial class Guard
	{
		/// <summary>
		/// Throws an exception of type <typeparamref name="TException"/> with the specified message
		/// when the assertion statement is true.
		/// </summary>
		/// <typeparam name="TException">The type of exception to throw.</typeparam>
		/// <param name="assertion">The assertion to evaluate. If true then the <typeparamref name="TException"/> exception is thrown.</param>
		/// <param name="message">string. The exception message to throw.</param>
		public static void Against<TException>(bool assertion, string message, params object[] args) where TException : Exception
		{
			if (assertion)
			{
				message = args != null ? string.Format(message, args) : message;
				throw (TException)Activator.CreateInstance(typeof(TException), message);
			}
		}

		/// <summary>
		/// Throws an exception of type <typeparamref name="TException"/> with the specified message
		/// when the assertion
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="assertion"></param>
		/// <param name="message"></param>
		public static void Against<TException>(Func<bool> assertion, string message, params object[] args) where TException : Exception
		{
			//Execute the lambda and if it evaluates to true then throw the exception.
			if (assertion())
			{
				message = args != null ? string.Format(message, args) : message;
				throw (TException)Activator.CreateInstance(typeof(TException), message);
			}
		}

		/// <summary>
		/// Throws a <see cref="InvalidOperationException"/> when the specified object
		/// instance does not inherit from <typeparamref name="TBase"/> type.
		/// </summary>
		/// <typeparam name="TBase">The base type to check for.</typeparam>
		/// <param name="instance">The object to check if it inherits from <typeparamref name="TBase"/> type.</param>
		/// <param name="message">string. The exception message to throw.</param>
		public static void InheritsFrom<TBase>(object instance, string message, params object[] args) where TBase : Type
		{
			if (instance != null)
				InheritsFrom<TBase>(instance.GetType(), message, args);
		}

		/// <summary>
		/// Throws a <see cref="InvalidOperationException"/> when the specified type does not
		/// inherit from the <typeparamref name="TBase"/> type.
		/// </summary>
		/// <typeparam name="TBase">The base type to check for.</typeparam>
		/// <param name="type">The <see cref="Type"/> to check if it inherits from <typeparamref name="TBase"/> type.</param>
		/// <param name="message">string. The exception message to throw.</param>
		public static void InheritsFrom<TBase>(Type type, string message, params object[] args)
		{
			if (type.BaseType != typeof(TBase))
			{
				message = args != null ? string.Format(message, args) : message;
				throw new InvalidOperationException(message);
			}
		}

		/// <summary>
		/// Throws a <see cref="InvalidOperationException"/> when the specified object
		/// instance does not implement the <typeparamref name="TInterface"/> interface.
		/// </summary>
		/// <typeparam name="TInterface">The interface type the object instance should implement.</typeparam>
		/// <param name="instance">The object insance to check if it implements the <typeparamref name="TInterface"/> interface</param>
		/// <param name="message">string. The exception message to throw.</param>
		public static void Implements<TInterface>(object instance, string message, params object[] args)
		{
			if (instance != null)
				Implements<TInterface>(instance.GetType(), message, args);
		}

		/// <summary>
		/// Throws an <see cref="InvalidOperationException"/> when the specified type does not
		/// implement the <typeparamref name="TInterface"/> interface.
		/// </summary>
		/// <typeparam name="TInterface">The interface type that the <paramref name="type"/> should implement.</typeparam>
		/// <param name="type">The <see cref="Type"/> to check if it implements from <typeparamref name="TInterface"/> interface.</param>
		/// <param name="message">string. The exception message to throw.</param>
		public static void Implements<TInterface>(Type type, string message, params object[] args)
		{
			if (!typeof(TInterface).IsAssignableFrom(type))
			{
				message = args != null ? string.Format(message, args) : message;
				throw new InvalidOperationException(message);
			}
		}

		/// <summary>
		/// Throws an <see cref="InvalidOperationException"/> when the specified object instance is
		/// not of the specified type.
		/// </summary>
		/// <typeparam name="TType">The Type that the <paramref name="instance"/> is expected to be.</typeparam>
		/// <param name="instance">The object instance whose type is checked.</param>
		/// <param name="message">The message of the <see cref="InvalidOperationException"/> exception.</param>
		public static void TypeOf<TType> (object instance, string message, params object[] args)
		{
			if (!(instance is TType))
			{
				message = args != null ? string.Format(message, args) : message;
				throw new InvalidOperationException(message);
			}
		}

		/// <summary>
		/// Throws an exception if an instance of an object is not equal to another object instance.
		/// </summary>
		/// <typeparam name="TException">The type of exception to throw when the guard check evaluates false.</typeparam>
		/// <param name="compare">The comparison object.</param>
		/// <param name="instance">The object instance to compare with.</param>
		/// <param name="message">string. The message of the exception.</param>
		public static void IsEqual<TException>(object compare, object instance, string message, params object[] args) where TException : Exception
		{
			if (object.Equals(compare, instance))
			{
				message = args != null ? string.Format(message, args) : message;
				throw (TException)Activator.CreateInstance(typeof(TException), message);
			}
		}

		/// <summary>
		/// Throws an exception if instance is null.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="message">The message.</param>
		public static void IsNotNull(object instance, string message, params object[] args)
		{
			if (instance == null)
			{
				message = args != null ? string.Format(message, args) : message;
				throw new ArgumentNullException(message);
			}
		}

		/// <summary>
		/// Throws an exception if instance is null or empty.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="message">The message.</param>
		public static void IsNotNullNorEmpty(string instance, string message, params object[] args)
		{
			if (string.IsNullOrEmpty(instance))
			{
				message = args != null ? string.Format(message, args) : message;
				throw new ArgumentNullException(message);
			}
		}

		/// <summary>
		/// Throws an exception if instance is null or witespace.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="message">The message.</param>
		public static void IsNotNullNorWhitespace(string instance, string message, params object[] args)
		{
			if (string.IsNullOrWhiteSpace(instance))
			{
				message = args != null ? string.Format(message, args) : message;
				throw new ArgumentNullException(message);
			}
		}

		/// <summary>
		/// Throws an exception if instance is default.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="message">The message.</param>
		public static void IsNotDefault<TInstance>(TInstance instance, string message, params object[] args)
			where TInstance : struct
		{
			if (object.Equals(instance, default(TInstance)))
			{
				message = args != null ? string.Format(message, args) : message;
				throw new ArgumentNullException(message);
			}
		}

		/// <summary>
		/// Throws an exception if instance is null or default.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="message">The message.</param>
		public static void IsNotNullNorDefault<TInstance>(TInstance instance, string message, params object[] args)
		{
			if (instance == null || object.Equals(instance, default(TInstance)))
			{
				message = args != null ? string.Format(message, args) : message;
				throw new ArgumentNullException(message);
			}
		}

	}
}
