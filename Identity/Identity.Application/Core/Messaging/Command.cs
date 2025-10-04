using MediatR;

namespace Identity.Application.Core.Messaging;

/// <summary>
///     The interface used for defining commands
/// </summary>
/// <typeparam name="TResponse">The response type of the command</typeparam>
public interface Command<out TResponse> : IRequest<TResponse>
{
}