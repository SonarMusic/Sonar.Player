using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Tools;

public static class QueueExtensions
{
    public static TracksQueue GetOrCreateContext(this DbSet<UserPlayerContext> contexts, User user)
    {
        var context = contexts.FirstOrDefault(x => x.User.Id == user.Id);
        return context is null ? new TracksQueue() : context.Queue;
    }
}