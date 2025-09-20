using MediatR;

namespace Tikal.Application.Core.Messaging;

/// <summary>
///     The interface used for defining command handlers
/// </summary>
/// <typeparam name="TCommand">The type of the command handled by this command handler</typeparam>
/// <typeparam name="TResponse">The return type of the command handled by this command handler</typeparam>
public interface CommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : Command<TResponse>
{
}