using System.Diagnostics;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Sonar.Player.Application.Tools;
using Sonar.Player.Data;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserTracksManagement.ApiClient;

namespace Sonar.DiscordBot;

public class VoiceModule : ModuleBase<SocketCommandContext>
{
    // private readonly IUserApiClient _userApiClient;
    // private readonly IUserTracksApiClient _userTracksApiClient;
    // private readonly ITrackPathBuilder _pathBuilder;
    // private readonly PlayerDbContext _dbContext;
    //
    // public VoiceModule(
    //     IUserApiClient userApiClient,
    //     IUserTracksApiClient userTracksApiClient,
    //     ITrackPathBuilder pathBuilder,
    //     PlayerDbContext dbContext)
    // {
    //     _userApiClient = userApiClient;
    //     _userTracksApiClient = userTracksApiClient;
    //     _pathBuilder = pathBuilder;
    //     _dbContext = dbContext;
    // }

    [Command("join", RunMode = RunMode.Async)]
    public async Task JoinChannel(params string[] parameters)
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
            if (botVoiceChannel.Id == userVoiceChannel.Id)
            {
                await Context.Channel.SendMessageAsync($"Bot is already in requested channel");
                return;
            }
    
            await Context.Channel.SendMessageAsync($"Bot is busy in channel: {botVoiceChannel.Name}");
            return;
        }
    
        await userVoiceChannel.ConnectAsync(true);
    }

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

    // [Command("play", RunMode = RunMode.Async)]
    // public async Task PlayMusic(string name)
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
    //         await Context.Channel.SendMessageAsync($"Bot is busy in channel: {botVoiceChannel.Name}");
    //         return;
    //     }
    //
    //     var userToken = await _userApiClient.LoginByDiscordBotAsync(
    //         Environment.GetEnvironmentVariable("SONAR_REG_TOKEN"),
    //         Context.User.Id.ToString(),
    //         default);
    //
    //     var tracks = await _userTracksApiClient.All2Async(userToken);
    //     var requiredTrack = tracks.FirstOrDefault(t => t.Name.Contains(name));
    //
    //     if (requiredTrack is null)
    //     {
    //         await ReplyAsync("Track not found");
    //         return;
    //     }
    //
    //     var trackName = _dbContext.Tracks.FirstOrDefault(t => t.Id == requiredTrack.Id);
    //
    //     var path = Path.Combine(_pathBuilder.GetTrackFolderPath(requiredTrack.Id), trackName.FileName);
    //
    //     var audio = await userVoiceChannel.ConnectAsync(true);
    //     await SendAsync(audio, path);
    //     await userVoiceChannel.DisconnectAsync();
    // }
    
    [Command("sample", RunMode = RunMode.Async)]
    public async Task SampleMusic(params string[] parameters)
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
            if (botVoiceChannel.Id != userVoiceChannel.Id)
            {
                await Context.Channel.SendMessageAsync($"Bot is busy in channel: {botVoiceChannel.Name}");
                return;
            }

            await userVoiceChannel.DisconnectAsync();
        }
    
        var audio = await userVoiceChannel.ConnectAsync(true);
        await SendAsync(audio, @"C:\Users\mdpol\Downloads\битбокс батл с кактусом.mp4");
        await userVoiceChannel.DisconnectAsync();
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

    private async Task SendAsync(IAudioClient client, string path)
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
