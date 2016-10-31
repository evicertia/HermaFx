using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace HermaFx.MediatR
{
	public static class MediatorExtensions
	{
		#region ICommand Methods
		//
		// Summary:
		//     Invoke a command (with no response) against it predefined handler
		//
		// Parameters:
		//   request:
		//     Command object
		//
		public static void Invoke(this IMediator mediator, ICommand request)
		{
			mediator.Send<Unit>(request);
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
		public static TResponse Invoke<TResponse>(this IMediator mediator, ICommand<TResponse> request)
		{
			return mediator.Send<TResponse>(request);
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
		public static TResponse Execute<TResponse>(this IMediator mediator, IQuery<TResponse> request)
		{
			return mediator.Send<TResponse>(request);
		}
		#endregion
	}
}