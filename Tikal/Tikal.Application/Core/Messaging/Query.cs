using MediatR;

namespace Tikal.Application.Core.Messaging;

public interface Query<out TResponse> : IRequest<TResponse>
{
}