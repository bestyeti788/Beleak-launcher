using Avalonia.Controls;
using Avalonia.Interactivity;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace minecraft_launcher
{
    public partial class MainWindow : Window
    {
        private bool _isLaunching;
        private string _playerName = "gamer123";
        private string _selectedVersion = "1.20.4";
        private int _ramMb = 4096;

        private readonly string[] _versions =
        {
            "1.20.4",
            "1.20.1",
            "1.19.4",
            "1.18.2"
        };

        public MainWindow()
        {
            InitializeComponent();

            VersionComboBox.ItemsSource = _versions;
            VersionComboBox.SelectedIndex = 0;

            PseudoTextBox.Text = _playerName;
            RamTextBox.Text = _ramMb.ToString();

            RefreshUi();
            SetStatus("Prêt.");
        }

        private void RefreshUi()
        {
            AccountNameText.Text = _playerName;
            ActivePseudoText.Text = _playerName;

            if (VersionComboBox.SelectedItem is string version && !string.IsNullOrWhiteSpace(version))
                _selectedVersion = version;

            ActiveVersionText.Text = _selectedVersion;
            ActiveRamText.Text = $"{_ramMb} MB";
        }

        private void SetStatus(string message)
        {
            StatusText.Text = message;
        }

        private string GetVersionSafe()
        {
            if (VersionComboBox.SelectedItem is string version && !string.IsNullOrWhiteSpace(version))
                return version;

            return _selectedVersion;
        }

        private int GetRamSafe()
        {
            if (int.TryParse(RamTextBox.Text?.Trim(), out var ram) && ram >= 1024)
                return ram;

            return _ramMb;
        }

        private async void PlayButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_isLaunching)
                return;

            _isLaunching = true;

            try
            {
                _selectedVersion = GetVersionSafe();
                _ramMb = GetRamSafe();
                RefreshUi();

                ProgressBar.Value = 0;
                SetStatus("Vérification des fichiers...");

                var path = new MinecraftPath();
                Directory.CreateDirectory(path.BasePath);

                var launcher = new MinecraftLauncher(path);

                launcher.ByteProgressChanged += (s, args) =>
                {
                    if (args.TotalBytes <= 0)
                        return;

                    var percent = (double)args.ProgressedBytes / args.TotalBytes * 100.0;

                    Dispatcher.UIThread.Post(() =>
                    {
                        ProgressBar.Value = Math.Clamp(percent, 0, 100);
                        SetStatus($"Téléchargement... {percent:0}%");
                    });
                };

                await launcher.InstallAsync(_selectedVersion);

                SetStatus("Préparation du lancement...");

                var option = new MLaunchOption
                {
                    MaximumRamMb = _ramMb,
                    Session = MSession.CreateOfflineSession(_playerName),
                };

                var process = await launcher.BuildProcessAsync(_selectedVersion, option);
                var wrapper = new ProcessWrapper(process);
                wrapper.StartWithEvents();

                SetStatus("Minecraft lancé.");
                Close();
            }
            catch (Exception ex)
            {
                SetStatus($"Erreur : {ex.Message}");
            }
            finally
            {
                _isLaunching = false;
                ProgressBar.Value = 0;
            }
        }

        private void OpenFolderButton_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var path = new MinecraftPath();
                Directory.CreateDirectory(path.BasePath);

                Process.Start(new ProcessStartInfo
                {
                    FileName = path.BasePath,
                    UseShellExecute = true
                });

                SetStatus("Dossier Minecraft ouvert.");
            }
            catch (Exception ex)
            {
                SetStatus($"Erreur : {ex.Message}");
            }
        }

        private void SettingsButton_Click(object? sender, RoutedEventArgs e)
        {
            PseudoTextBox.Text = _playerName;
            RamTextBox.Text = _ramMb.ToString();
            SettingsOverlay.IsVisible = true;
            SetStatus("Paramètres ouverts.");
        }

        private void CloseSettingsButton_Click(object? sender, RoutedEventArgs e)
        {
            SettingsOverlay.IsVisible = false;
            SetStatus("Prêt.");
        }

        private void SaveSettingsButton_Click(object? sender, RoutedEventArgs e)
        {
            var pseudo = PseudoTextBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(pseudo))
            {
                SetStatus("Pseudo invalide.");
                return;
            }

            _playerName = pseudo;
            _ramMb = GetRamSafe();

            if (_ramMb < 1024)
                _ramMb = 4096;

            RefreshUi();
            SettingsOverlay.IsVisible = false;
            SetStatus($"Paramètres enregistrés : {_playerName} • {_ramMb} MB");
        }

        private void QuitButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}