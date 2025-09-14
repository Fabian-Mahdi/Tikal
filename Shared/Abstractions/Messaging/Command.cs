using MediatR;

namespace Shared.Abstractions.Messaging;

public interface Command<out TResponse> : IRequest<TResponse>
{
}