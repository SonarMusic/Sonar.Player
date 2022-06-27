using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sonar.Player.Data;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Tools;

public static class QueueExtensions
{
    public static async Task<UserPlayerContext> GetOrCreateContext(this PlayerDbContext dbContext, User user)
    {
        var context = await dbContext.Contexts.FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (context is not null) return context;
        var newContext = new UserPlayerContext(user.Id, new TracksQueue());
        await dbContext.Contexts.AddAsync(newContext);
        await dbContext.SaveChangesAsync();
        return newContext;

    }
}