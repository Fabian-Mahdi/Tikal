using FluentResults;
using MediatR;

namespace Shared.Abstractions.Messaging;

public interface Command : IRequest<Result>
{
}

public interface Command<TResponse> : IRequest<Result<TResponse>>
{
}