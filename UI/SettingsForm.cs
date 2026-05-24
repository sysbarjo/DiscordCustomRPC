namespace DCRPC.UI;

public class SettingsForm : Form
{
    public AppConfig Config { get; private set; }

    private readonly TextBox clientIdBox;
    private readonly TextBox detailsBox;
    private readonly TextBox stateBox;
    private readonly TextBox largeImageKeyBox;
    private readonly TextBox largeImageTextBox;
    private readonly TextBox smallImageKeyBox;
    private readonly TextBox smallImageTextBox;
    private readonly TextBox buttonLabelBox;
    private readonly TextBox buttonUrlBox;

    private static TextBox MakeBox() => new TextBox { Dock = DockStyle.Fill };

    public SettingsForm(AppConfig config)
    {
        Config = config;
        Text = "DCRPC Settings";
        Width = 635;
        Height = 425;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        clientIdBox      = MakeBox();
        detailsBox       = MakeBox();
        stateBox         = MakeBox();
        largeImageKeyBox  = MakeBox();
        largeImageTextBox = MakeBox();
        smallImageKeyBox  = MakeBox();
        smallImageTextBox = MakeBox();
        buttonLabelBox   = MakeBox();
        buttonUrlBox     = MakeBox();

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 11,
            ColumnCount = 2,
            Padding = new Padding(25)
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        panel.Controls.Add(new Label { Text = "Client ID",        Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
        panel.Controls.Add(clientIdBox, 1, 0);
        panel.Controls.Add(new Label { Text = "Details",          Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
        panel.Controls.Add(detailsBox, 1, 1);
        panel.Controls.Add(new Label { Text = "State",            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
        panel.Controls.Add(stateBox, 1, 2);
        panel.Controls.Add(new Label { Text = "Large Image Key",  Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 3);
        panel.Controls.Add(largeImageKeyBox, 1, 3);
        panel.Controls.Add(new Label { Text = "Large Image Text", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 4);
        panel.Controls.Add(largeImageTextBox, 1, 4);
        panel.Controls.Add(new Label { Text = "Small Image Key",  Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 5);
        panel.Controls.Add(smallImageKeyBox, 1, 5);
        panel.Controls.Add(new Label { Text = "Small Image Text", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 6);
        panel.Controls.Add(smallImageTextBox, 1, 6);
        panel.Controls.Add(new Label { Text = "Button Label",     Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 7);
        panel.Controls.Add(buttonLabelBox, 1, 7);
        panel.Controls.Add(new Label { Text = "Button URL",       Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 8);
        panel.Controls.Add(buttonUrlBox, 1, 8);

        var saveButton = new Button { Text = "Save", Dock = DockStyle.Fill };
        saveButton.Click += Save;
        panel.Controls.Add(saveButton, 1, 10);

        Controls.Add(panel);
        LoadConfig();
    }

    private void LoadConfig()
    {
        clientIdBox.Text      = Config.ClientId;
        detailsBox.Text       = Config.Details;
        stateBox.Text         = Config.State;
        largeImageKeyBox.Text  = Config.LargeImageKey;
        largeImageTextBox.Text = Config.LargeImageText;
        smallImageKeyBox.Text  = Config.SmallImageKey;
        smallImageTextBox.Text = Config.SmallImageText;
        buttonLabelBox.Text   = Config.ButtonLabel;
        buttonUrlBox.Text     = Config.ButtonUrl;
    }

    private void Save(object? sender, EventArgs e)
    {
        Config.ClientId      = clientIdBox.Text.Trim();
        Config.Details       = detailsBox.Text.Trim();
        Config.State         = stateBox.Text.Trim();
        Config.LargeImageKey  = largeImageKeyBox.Text.Trim();
        Config.LargeImageText = largeImageTextBox.Text.Trim();
        Config.SmallImageKey  = smallImageKeyBox.Text.Trim();
        Config.SmallImageText = smallImageTextBox.Text.Trim();
        Config.ButtonLabel   = buttonLabelBox.Text.Trim();
        Config.ButtonUrl     = buttonUrlBox.Text.Trim();
        DialogResult = DialogResult.OK;
        Close();
    }
}