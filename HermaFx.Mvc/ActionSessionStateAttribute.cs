using System;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace HermaFx.Mvc
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class ActionSessionStateAttribute : Attribute
	{
		public SessionStateBehavior Behavior { get; private set; }

		public ActionSessionStateAttribute(SessionStateBehavior behavior)
		{
			this.Behavior = behavior;
		}
	}
}