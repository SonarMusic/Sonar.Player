using Discord.Commands;
using Discord.WebSocket;

namespace Sonar.DiscordBot;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _provider;

    private const string Prefix = "--";
    
    public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider provider)
    {
        _commands = commands;
        _client = client;
        _provider = provider;
    }

    public async Task InstallCommandsAsync()
    {
        await _commands.AddModulesAsync(GetType().Assembly, _provider);
        _client.MessageReceived += HandleCommandAsync;
    }

    private async Task HandleCommandAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage userMessage)
        {
            return;
        }

        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;

        // Determine if the message is a command based on the prefix and make sure no bots trigger commands
        if (!(userMessage.HasStringPrefix(Prefix, ref argPos) || userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos))
         || userMessage.Author.IsBot)
            return;

        var context = new SocketCommandContext(_client, userMessage);
        await _commands.ExecuteAsync(context, argPos, _provider);
    }
}
