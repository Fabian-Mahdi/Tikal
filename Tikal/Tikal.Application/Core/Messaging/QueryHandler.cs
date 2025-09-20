using MediatR;

namespace Tikal.Application.Core.Messaging;

/// <summary>
///     The interface used for defining query handlers
/// </summary>
/// <typeparam name="TQuery">The type of the query handled by this query handler</typeparam>
/// <typeparam name="TResponse">The return type of the query handled by this query handler</typeparam>
public interface QueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : Query<TResponse>
{
}