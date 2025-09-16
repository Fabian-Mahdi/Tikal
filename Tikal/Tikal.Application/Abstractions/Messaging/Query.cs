using MediatR;

namespace Tikal.Application.Abstractions.Messaging;

public interface Query<out TResponse> : IRequest<TResponse>
{
}