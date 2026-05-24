using DiscordRPC;
using DiscordRPC.Logging;

namespace DCRPC.Services;

public class DiscordService
{
    private DiscordRpcClient? client;

    // private DateTime? startTime;

    public bool IsInitialized => client is { IsInitialized: true };

    public void Initialize(AppConfig config)
    {
        try
        {
            Dispose();

            if (string.IsNullOrWhiteSpace(config.ClientId))
                return;

            client = new DiscordRpcClient(config.ClientId);

#if DEBUG
            client.Logger = new ConsoleLogger
            {
                Level = LogLevel.Warning
            };
#endif

            client.OnError += (_, e) =>
            {
                Console.WriteLine($"Discord Error: {e.Message}");
            };

            client.OnConnectionFailed += (_, e) =>
            {
                Console.WriteLine($"Connection failed: {e.FailedPipe}");
            };

            client.Initialize();

            // if (config.EnableTimestamp)
            //     startTime = DateTime.UtcNow;

            UpdatePresence(config);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public void UpdatePresence(AppConfig config)
    {
        if (client == null || !client.IsInitialized)
            return;

        try
        {
            var presence = new RichPresence
            {
                Details = config.Details,
                State = config.State,

                Assets = new Assets
                {
                    LargeImageKey = config.LargeImageKey,
                    LargeImageText = config.LargeImageText,
                    SmallImageKey = config.SmallImageKey,
                    SmallImageText = config.SmallImageText
                }
            };

            if (!string.IsNullOrWhiteSpace(config.ButtonLabel)
                && !string.IsNullOrWhiteSpace(config.ButtonUrl))
            {
                presence.Buttons = new[]
                {
                    new DiscordRPC.Button
                    {
                        Label = config.ButtonLabel,
                        Url = config.ButtonUrl
                    }
                };
            }

            client.SetPresence(presence);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Dispose()
    {
        try
        {
            client?.ClearPresence();
            client?.Dispose();
        }
        catch
        {
        }
    }
}