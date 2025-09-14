using FluentResults;
using MediatR;

namespace Shared.Abstractions.Messaging;

public interface CommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : Command
{
}

public interface CommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : Command<TResponse>
{
}