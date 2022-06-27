using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

namespace Sonar.DiscordBot;

public class VoiceModule : ModuleBase<SocketCommandContext>
{
    // [Command("join", RunMode = RunMode.Async)]
    // public async Task JoinChannel(params string[] parameters)
    // {
    //     var userVoiceChannel = (Context.User as IGuildUser)?.VoiceChannel;
    //     if (userVoiceChannel is null)
    //     {
    //         await Context.Channel.SendMessageAsync("User must be in a voice channel");
    //         return;
    //     }
    //
    //     var botVoiceChannel = await GetBotVoiceChannel();
    //     if (botVoiceChannel is not null)
    //     {
    //         if (botVoiceChannel.Id == userVoiceChannel.Id)
    //         {
    //             await Context.Channel.SendMessageAsync($"Bot is already in requested channel");
    //             return;
    //         }
    //
    //         await Context.Channel.SendMessageAsync($"Bot is busy in channel: {botVoiceChannel.Name}");
    //         return;
    //     }
    //
    //     await userVoiceChannel.ConnectAsync(true);
    // }

    [Command("leave", RunMode = RunMode.Async)]
    public async Task LeaveChannel(params string[] parameters)
    {
        var userVoiceChannel = (Context.User as IGuildUser)?.VoiceChannel;
        if (userVoiceChannel is null)
        {
            await Context.Channel.SendMessageAsync("User must be in a voice channel");
            return;
        }

        var botVoiceChannel = await GetBotVoiceChannel();
        if (botVoiceChannel is null)
        {
            await Context.Channel.SendMessageAsync("Are you crazy or what? I'm already disconnected");
            return;
        }

        await userVoiceChannel.DisconnectAsync();
    }

    [Command("play", RunMode = RunMode.Async)]
    public async Task PlayMusic(params string[] parameters)
    {
        var userVoiceChannel = (Context.User as IGuildUser)?.VoiceChannel;
        if (userVoiceChannel is null)
        {
            await Context.Channel.SendMessageAsync("User must be in a voice channel");
            return;
        }

        var botVoiceChannel = await GetBotVoiceChannel();
        if (botVoiceChannel is not null)
        {
            await Context.Channel.SendMessageAsync($"Bot is busy in channel: {botVoiceChannel.Name}");
            return;
        }

        var audio = await userVoiceChannel.ConnectAsync(true);
        await SendAsync(audio);
        await userVoiceChannel.DisconnectAsync();
    }

    [Command("whois")]
    public async Task GetUserEmail(SocketUser user = null)
    {
        
        var userInfo = user ?? Context.Client.CurrentUser;

        var client = new HttpClient();

        using var request = new HttpRequestMessage(HttpMethod.Get, $"https://discord.com/api/users/{userInfo.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"));

        var response = await client.SendAsync(request);
        
        await ReplyAsync($"{response.StatusCode} {response.Content.ToString()}");
    }

    private async Task<IVoiceChannel> GetBotVoiceChannel()
    {
        var clientUser = await Context.Channel.GetUserAsync(Context.Client.CurrentUser.Id);
        if (clientUser is IGuildUser bot)
        {
            return bot.VoiceChannel;
        }

        Console.WriteLine($"Unable to find bot in server: {Context.Guild.Name}");
        return null;
    }

    private Process CreateStream(string path)
    {
        return Process.Start(
            new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel info -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
    }

    private async Task SendAsync(
        IAudioClient client,
        string path = @"C:\Users\mdpol\Downloads\битбокс батл с кактусом.mp4")
    {
        using Process ffmpeg = CreateStream(path);
        await using Stream output = ffmpeg.StandardOutput.BaseStream;
        await using AudioOutStream discord = client.CreatePCMStream(AudioApplication.Mixed);
        try
        {
            await output.CopyToAsync(discord);
        }
        finally
        {
            await discord.FlushAsync();
        }
    }
}
