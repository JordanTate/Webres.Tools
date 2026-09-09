using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Input;

namespace Deployment.UI.ViewModels;

public sealed class SettingsViewModel : INotifyPropertyChanged
{
    private readonly IDeploymentSettingsProvider _provider;
    private readonly DeploymentSettings _settings;
    public ObservableCollection<string> WebServers { get; set; } = [];
    private string _updateSource = string.Empty;
    public string UpdateSource
    {
        get => _updateSource;
        set
        {
            if (_updateSource == value)
                return;

            _updateSource = value;
            OnPropertyChanged();
        }
    }
    private string _websiteRoot = string.Empty;
    public string WebsiteRoot
    {
        get => _websiteRoot;
        set
        {
            if (_websiteRoot == value)
                return;

            _websiteRoot = value;
            OnPropertyChanged();
        }
    }
    private string _webServerInput = string.Empty;
    public string WebServerInput
    {
        get => _webServerInput;
        set
        {
            if (_webServerInput == value)
                return;

            _webServerInput = value;
            OnPropertyChanged();
        }
    }
    public ICommand AddWebServerCommand { get; }
    public ICommand RemoveWebServerCommand { get; }
    public ICommand SaveCommand { get; }
    public SettingsViewModel(
        IDeploymentSettingsProvider provider,
        DeploymentSettings settings)
    {
        _provider = provider;
        _settings = settings;

        AddWebServerCommand = new RelayCommand<object>(
            _ => AddWebServer());
        RemoveWebServerCommand = new RelayCommand<string>(RemoveWebServer);
        SaveCommand = new RelayCommand<object>(
            async _ => await SaveAsync());
    }
    public void AddWebServer()
    {
        if (string.IsNullOrWhiteSpace(WebServerInput))
            return;

        WebServers.Add(WebServerInput.Trim());
        WebServerInput = string.Empty;
    }
    public void RemoveWebServer(string server)
    {
        WebServers.Remove(server);
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(
       [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    #region Events for MainWindow
    public event Action? SaveRequested;
    public async Task LoadAsync()
    {
        DeploymentSettings settings =
            await _provider.GetAsync();

        UpdateSource = settings.UpdateSource;
        WebsiteRoot = settings.WebsiteRoot;

        WebServers.Clear();

        foreach (string server in settings.WebServers)
            WebServers.Add(server);
    }

    public async Task SaveAsync()
    {
        _settings.UpdateSource = UpdateSource;
        _settings.WebServers = [.. WebServers];
        _settings.WebsiteRoot = WebsiteRoot;

        await _provider.SaveAsync(_settings);

        SaveRequested?.Invoke();
    }
    #endregion
}
