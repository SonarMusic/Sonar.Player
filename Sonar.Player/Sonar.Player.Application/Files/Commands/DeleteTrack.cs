using MediatR;

namespace Sonar.Player.Application.Files.Commands;

public static class DeleteTrack
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