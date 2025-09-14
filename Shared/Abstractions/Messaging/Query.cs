using FluentResults;
using MediatR;

namespace Shared.Abstractions.Messaging;

public interface Query<TResponse> : IRequest<Result<TResponse>>
{
}