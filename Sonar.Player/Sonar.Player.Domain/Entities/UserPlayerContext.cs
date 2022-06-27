using Sonar.Player.Domain.Models;

namespace Sonar.Player.Domain.Entities;

public class UserPlayerContext
{
    public Guid UserId { get; private init; }
    public TracksQueue Queue { get; private init; }
    
    private UserPlayerContext() { }
    public UserPlayerContext(Guid userId, TracksQueue queue)
    {
        UserId = userId;
        Queue = queue;
    }
}