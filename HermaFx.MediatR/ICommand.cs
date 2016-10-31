using System;

using MediatR;

namespace HermaFx.MediatR
{
	public interface ICommand : IRequest
	{
	}

	public interface ICommand<out TResponse> : IRequest<TResponse>
	{
	}
}