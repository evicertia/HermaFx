using System;

using MediatR;

namespace HermaFx.MediatR
{
	public interface IQuery : IRequest
	{
	}

	public interface IQuery<out TResponse> : IRequest<TResponse>
	{
	}
}