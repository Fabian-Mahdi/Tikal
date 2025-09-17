using MediatR;

namespace Tikal.Application.Core.Messaging;

public interface Command<out TResponse> : IRequest<TResponse>
{
}