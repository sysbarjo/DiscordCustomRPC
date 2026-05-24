using DCRPC.Services;
using Microsoft.Win32;

namespace DCRPC;
public class PresenceContext : ApplicationContext
{
    private readonly NotifyIcon trayIcon;
    private readonly DiscordService discordService;
    private AppConfig config;
    private readonly System.Windows.Forms.Timer reconnectTimer;
    private ToolStripMenuItem startupMenuItem;

    private bool IsStartupEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        return key?.GetValue("DCRPC") != null;
    }

    private void SetStartup(bool enable)
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true)!;
        if (enable)
            key.SetValue("DCRPC", Application.ExecutablePath);
        else
            key.DeleteValue("DCRPC", false);
    }

    public PresenceContext()
    {
        config = ConfigService.Load();
        discordService = new DiscordService();
        trayIcon = new NotifyIcon
        {
            Icon = new Icon(typeof(PresenceContext).Assembly
                .GetManifestResourceStream("DCRPC.Assets.DCRPC.ico")!),
            Visible = true,
            Text = "DCRPC"
        };

        startupMenuItem = new ToolStripMenuItem("Launch at startup")
        {
            Checked = IsStartupEnabled()
        };
        startupMenuItem.Click += (_, _) =>
        {
            bool newValue = !startupMenuItem.Checked;
            SetStartup(newValue);
            startupMenuItem.Checked = newValue;
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("Edit Presence", null, OpenSettings);
        menu.Items.Add("Reload Presence", null, (_, _) =>
        {
            config = ConfigService.Load();
            discordService.Initialize(config);
        });
        menu.Items.Add("Open Config Folder", null, (_, _) => ConfigService.OpenConfigFolder());
        menu.Items.Add("Launch Discord", null, (_, _) =>
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "discord",
                    UseShellExecute = true
                });
            }
            catch { }
        });
        menu.Items.Add(startupMenuItem);
        menu.Items.Add("Quit", null, (_, _) => Exit());
        trayIcon.ContextMenuStrip = menu;

        Task.Run(async () =>
        {
            await Task.Delay(3000);
            discordService.Initialize(config);
        });

        reconnectTimer = new System.Windows.Forms.Timer();
        reconnectTimer.Interval = 10000;
        reconnectTimer.Tick += (_, _) =>
        {
            if (!discordService.IsInitialized)
            {
                config = ConfigService.Load();
                discordService.Initialize(config);
            }
        };
        reconnectTimer.Start();
    }

    private void OpenSettings(object? sender, EventArgs e)
    {
        using var form = new UI.SettingsForm(config);
        if (form.ShowDialog() == DialogResult.OK)
        {
            config = form.Config;
            ConfigService.Save(config);
            discordService.Initialize(config);
        }
    }

    private void Exit()
    {
        reconnectTimer.Stop();
        trayIcon.Visible = false;
        discordService.Dispose();
        trayIcon.Dispose();
        Application.Exit();
    }
}