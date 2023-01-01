using System;

using Rebus.Bus;

using HermaFx;
using HermaFx.Logging;

namespace Rebus.Configuration
{
	public class MulticastOutgoingMessageLogger : OutgoingMessageLogger, IMulticastTransport
	{
		#region Private members

		private readonly IMulticastTransport _multicastTransport;

		#endregion

		#region IMulticastTransport properties

		public bool ManagesSubscriptions
		{
			get
			{
				return _multicastTransport.ManagesSubscriptions;
			}
		}

		#endregion

		#region IReceiveMessages properties

		public string InputQueue
		{
			get
			{
				return _multicastTransport.InputQueue;
			}
		}

		public string InputQueueAddress
		{
			get
			{
				return _multicastTransport.InputQueueAddress;
			}
		}

		#endregion

		#region .ctor

		public MulticastOutgoingMessageLogger(IMulticastTransport multicastTransport, LogLevel logLevel) :
			base(multicastTransport, logLevel)
		{
			Guard.IsNotNull(multicastTransport, nameof(multicastTransport));

			_multicastTransport = multicastTransport;
		}

		#endregion

		#region IMulticastTransport methods

		public void Subscribe(Type eventType, string inputQueueAddress)
		{
			_multicastTransport.Subscribe(eventType, inputQueueAddress);
		}

		public void Unsubscribe(Type messageType, string inputQueueAddress)
		{
			_multicastTransport.Unsubscribe(messageType, inputQueueAddress);
		}

		public string GetEventName(Type messageType)
		{
			return _multicastTransport.GetEventName(messageType);
		}

		#endregion

		#region IReceiveMessages methods

		public ReceivedTransportMessage ReceiveMessage(ITransactionContext context)
		{
			return _multicastTransport.ReceiveMessage(context);
		}

		#endregion
	}
}