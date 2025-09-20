using MediatR;

namespace Tikal.Application.Core.Messaging;

/// <summary>
///     The interface used for defining queries
/// </summary>
/// <typeparam name="TResponse">The return type of the query</typeparam>
public interface Query<out TResponse> : IRequest<TResponse>
{
}