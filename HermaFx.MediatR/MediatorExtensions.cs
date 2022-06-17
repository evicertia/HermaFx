using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

using MediatR;

namespace HermaFx.MediatR
{
	public static class MediatorExtensions
	{
		#region ICommand Methods
		//
		// Summary:
		//     Invoke a command (with no response) against it's predefined handlers
		//
		// Parameters:
		//   request:
		//     Command object
		//
		public static void Invoke(this ISender mediator, ICommand request)
		{
			mediator.Send<Unit>(request);
		}

		//
		// Summary:
		//     Synchronously invoke a command (with no response) against it's predefined handlers
		//
		// Parameters:
		//   request:
		//     Command object
		//
		public static void InvokeSync(this ISender mediator, ICommand request)
		{
			mediator.Send<Unit>(request).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		//
		// Summary:
		//     Invoke a command against it predefined handler
		//
		// Parameters:
		//   request:
		//     Command object
		//
		// Type parameters:
		//   TResponse:
		//     Response type
		//
		// Returns:
		//     Response
		public static Task<TResponse> Invoke<TResponse>(this ISender mediator, ICommand<TResponse> request)
		{
			return mediator.Send<TResponse>(request);
		}

		//
		// Summary:
		//     Synchronously invoke a command (with no response) against it's predefined handlers
		//
		// Parameters:
		//   request:
		//     Command object
		//
		// Type parameters:
		//   TResponse:
		//     Response type
		//
		// Returns:
		//     Response
		public static TResponse InvokeSync<TResponse>(this ISender mediator, ICommand<TResponse> request)
		{
			return mediator.Send<TResponse>(request).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		#endregion

		#region IQuery Methods
		//
		// Summary:
		//     Executes a query and return it's results
		//
		// Parameters:
		//   request:
		//     Query object
		//
		// Type parameters:
		//   TResponse:
		//     Response type
		//
		// Returns:
		//     Response
		public static Task<TResponse> Execute<TResponse>(this ISender mediator, IQuery<TResponse> request)
		{
			return mediator.Send<TResponse>(request);
		}

		//
		// Summary:
		//     Executes a query and return it's results
		//
		// Parameters:
		//   request:
		//     Query object
		//
		// Type parameters:
		//   TResponse:
		//     Response type
		//
		// Returns:
		//     Response
		public static TResponse ExecuteSync<TResponse>(this ISender mediator, IQuery<TResponse> request)
		{
			return mediator.Send<TResponse>(request).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		#endregion

		#region IEvent Methods
		//
		// Summary:
		//     Publishes and event.
		//
		// Parameters:
		//   request:
		//     Query object
		//
		// Type parameters:
		//   TResponse:
		//     Response type
		//
		// Returns:
		//     Response
		public static Task Publish<TEvent>(this IPublisher mediator, TEvent @event)
			where TEvent : INotification
		{
			return mediator.Publish<TEvent>(@event);
		}

		//
		// Summary:
		//     Synchronously publishes and event.
		//
		// Parameters:
		//   request:
		//     Query object
		//
		// Type parameters:
		//   TResponse:
		//     Response type
		//
		// Returns:
		//     Response
		public static void ExecuteSync<TEvent>(this IPublisher mediator, TEvent request)
			where TEvent : INotification
		{
			mediator.Publish<TEvent>(request).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		#endregion
	}
}
