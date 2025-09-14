using MediatR;

namespace Shared.Abstractions.Messaging;

public interface Query<out TResponse> : IRequest<TResponse>
{
}