using MediatR;

namespace Tikal.Application.Abstractions.Messaging;

public interface Command<out TResponse> : IRequest<TResponse>
{
}