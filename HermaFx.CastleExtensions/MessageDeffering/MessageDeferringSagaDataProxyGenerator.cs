using System;
using Rebus;
using Castle.DynamicProxy;

namespace HermaFx.Rebus
{
	public class MessageDeferringSagaDataProxyGenerator
	{
		private readonly ProxyGenerator _generator = new ProxyGenerator();

		public T CreateProxy<T>()
			where T : ISagaData
		{
			return (T)_generator.CreateClassProxy(typeof(T), new[] { typeof(MessageDeferringSagaPersister.ILockedSagaData) });
		}
	}
}
