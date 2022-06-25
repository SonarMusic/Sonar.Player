﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sonar.Player.Domain.Entities;
using Sonar.Player.Domain.Models;

namespace Sonar.Player.Application.Tools;

public static class QueueExtensions
{
    public static async Task<UserPlayerContext> GetOrCreateContext(this DbSet<UserPlayerContext> contexts, User user)
    {
        var context = contexts.FirstOrDefault(x => x.User.Id == user.Id);
        if (context is not null) return context;
        var newContext = new UserPlayerContext(user, new TracksQueue());
        await contexts.AddAsync(newContext);
        return newContext;

    }
}