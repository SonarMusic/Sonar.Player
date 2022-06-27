using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Sonar.DiscordBot;

var services = new ServiceCollection()
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton<CommandService>()
               .AddSingleton<CommandHandler>()
               .BuildServiceProvider();

var client = services.GetRequiredService<DiscordSocketClient>();

var handler = services.GetRequiredService<CommandHandler>();
await handler.InstallCommandsAsync();

client.Log += LogAsync;
client.Ready += () => ReadyAsync(client);

await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"));
await client.StartAsync();

await Task.Delay(Timeout.Infinite);


Task LogAsync(LogMessage log)
{
    Console.WriteLine(log.ToString());
    return Task.CompletedTask;
}

Task ReadyAsync(BaseSocketClient baseClient)
{
    Console.WriteLine($"{baseClient.CurrentUser} is connected!");
    return Task.CompletedTask;
}
