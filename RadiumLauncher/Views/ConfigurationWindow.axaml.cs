using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using RadiumLauncher.Models;

namespace RadiumLauncher.Views;

public partial class ConfigurationWindow : Window
{
    private readonly string _configFolder = Path.Combine(AppConstants.AppDataDirectory, "Configuration");

    public ConfigurationWindow()
    {
        InitializeComponent();
        if (!Directory.Exists(_configFolder))
        {
            Directory.CreateDirectory(_configFolder);
        }

        string protonPathFile = Path.Combine(_configFolder, "protonpath.txt");
        string launchOptionsFile = Path.Combine(_configFolder, "launchoptions.txt");

        string currentProtonPath = File.Exists(protonPathFile) ? File.ReadAllText(protonPathFile) : string.Empty;
        string currentLaunchOptions =
            File.Exists(launchOptionsFile) ? File.ReadAllText(launchOptionsFile) : "%command%";

        Protonpathtb.Text = currentProtonPath;
        Launchoptstb.Text = currentLaunchOptions;
    }

    private void ProtonPath_Changed(object? sender, RoutedEventArgs e)
    {
        File.WriteAllText(Path.Combine(_configFolder, "protonpath.txt"), Protonpathtb.Text ?? string.Empty);
    }

    private void LaunchOptions_Changed(object? sender, RoutedEventArgs e)
    {
        File.WriteAllText(Path.Combine(_configFolder, "launchoptions.txt"), Launchoptstb.Text ?? "%command%");
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}