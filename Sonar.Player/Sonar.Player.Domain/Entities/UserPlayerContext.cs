using Sonar.Player.Domain.Models;

namespace Sonar.Player.Domain.Entities;

public class UserPlayerContext
{
    public User User { get; }
    public TracksQueue Queue { get; }
    
    public UserPlayerContext(User user, TracksQueue queue)
    {
        User = user;
        Queue = queue;
    } 
}