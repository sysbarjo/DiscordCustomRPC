using System.Text.Json;

namespace DCRPC.Services;

public static class ConfigService
{
    private static readonly string FolderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "DCRPC"
    );

    private static readonly string ConfigPath = Path.Combine(FolderPath, "config.json");

    public static AppConfig Load()
    {
        try
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            if (!File.Exists(ConfigPath))
            {
                var defaultConfig = new AppConfig();
                Save(defaultConfig);
                return defaultConfig;
            }

            string json = File.ReadAllText(ConfigPath);

            return JsonSerializer.Deserialize<AppConfig>(json)
                ?? new AppConfig();
        }
        catch
        {
            return new AppConfig();
        }
    }

    public static void Save(AppConfig config)
    {
        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        string json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(ConfigPath, json);
    }

    public static void OpenConfigFolder()
    {
        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        System.Diagnostics.Process.Start("explorer.exe", FolderPath);
    }
}