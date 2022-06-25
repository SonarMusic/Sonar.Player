using Sonar.Player.Domain.Models;

namespace Sonar.Player.Domain.Entities;

public class UserPlayerContext
{
    public User User { get; private init; }
    public TracksQueue Queue { get; private init; }
    
    private UserPlayerContext() { }
    public UserPlayerContext(User user, TracksQueue queue)
    {
        User = user;
        Queue = queue;
    }
}