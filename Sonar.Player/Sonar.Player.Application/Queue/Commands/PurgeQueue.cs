using MediatR;

namespace Sonar.Player.Application.Queue.Commands;

public static class PurgeQueue
{
    public record Command() : IRequest<Response>;

    public record Response();

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}