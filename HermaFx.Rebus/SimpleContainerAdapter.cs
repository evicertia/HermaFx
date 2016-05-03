using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rebus.Configuration;

using HermaFx;

namespace Rebus
{
	public class SimpleContainerAdapter : IContainerAdapter, IDisposable
	{
		private IBus _bus;
		private List<IHandleMessages> _handlers = new List<IHandleMessages>();

		public IBus Bus { get { return _bus; } }

		public void Attach(IHandleMessages handler)
		{
			_handlers.Add(handler);
		}

		#region IContainerAdapter Members

		public void SaveBusInstances(IBus bus)
		{
			if (!ReferenceEquals(null, _bus))
			{
				throw new InvalidOperationException(
						string.Format(
								"You can't call SaveBusInstances twice on the container adapter! Already have bus instance {0} when you tried to overwrite it with {1}",
								_bus, bus));
			}

			_bus = bus;
		}

		#endregion

		#region IActivateHandlers Members

		public IEnumerable<IHandleMessages> GetHandlerInstancesFor<T>()
		{
			return _handlers.Where(x => x is IHandleMessages<T>).ToArray();
		}

		public void Release(System.Collections.IEnumerable handlerInstances)
		{
			foreach (var handler in handlerInstances.OfType<IDisposable>())
			{
				_handlers.Remove(handler as IHandleMessages);
				handler.Dispose();
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_handlers.OfType<IDisposable>().ForEach(x => x.Dispose());
		}

		#endregion
	}
}
