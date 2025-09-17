using MediatR;

namespace Tikal.Application.Core.Messaging;

public interface CommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : Command<TResponse>
{
}