using MediatR;

namespace Tikal.Application.Abstractions.Messaging;

public interface QueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : Query<TResponse>
{
}