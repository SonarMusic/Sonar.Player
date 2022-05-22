using MediatR;

namespace Sonar.Player.Application.PlayerContext.Commands;

public static class SampleCommand
{
    public record Command() : IRequest<Response>;

    public record Response();

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
        }
    }
}