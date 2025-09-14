using MediatR;

namespace Shared.Abstractions.Messaging;

public interface QueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : Query<TResponse>
{
}