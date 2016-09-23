using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rebus;

namespace HermaFx.Rebus
{
	/// <summary>
	/// Augmented IHandleMessages interface which allows auto-subscription 
	/// by using RebusExtensions.SubscribeFromAssembly(..)
	/// </summary>
	/// <typeparam name="TMessage">The type of the message.</typeparam>
	/// <seealso cref="Rebus.IHandleMessages{TMessage}" />
	public interface ISubscribeTo<in TMessage> : IHandleMessages<TMessage>
	{
	}
}
