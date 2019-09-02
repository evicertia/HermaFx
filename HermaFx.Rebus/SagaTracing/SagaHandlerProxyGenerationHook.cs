using System;
using System.Reflection;

using Rebus;

using Castle.DynamicProxy;

namespace HermaFx.Rebus
{
	/// <summary>
	/// An IProxyGenerationHook which filters out from proxying methods
	/// those not strictly related to handling incomming messages.
	/// </summary>
	public class SagaHandlerProxyGenerationHook : IProxyGenerationHook
	{
		public void MethodsInspected()
		{
			// do nothing.
		}

		public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
		{
			// We may want to log a warning if/when called for a non-saga type?
		}

		public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
		{
			return methodInfo.Name == nameof(IHandleMessages<object>.Handle)
				&& methodInfo.IsPublic && !methodInfo.IsStatic && (methodInfo.DeclaringType.IsInterface || !methodInfo.IsAbstract)
				&& methodInfo.ReturnType == typeof(void)
				&& methodInfo.GetParameters()?.Length == 1;
		}
	}
}
