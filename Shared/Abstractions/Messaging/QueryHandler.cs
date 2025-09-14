using FluentResults;
using MediatR;

namespace Shared.Abstractions.Messaging;

public interface QueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : Query<TResponse>
{
}