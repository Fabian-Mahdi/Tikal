using MediatR;

namespace Tikal.Application.Abstractions.Messaging;

public interface CommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : Command<TResponse>
{
}