using Deployment.Core.Configuration;
using Deployment.Core.Deployment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Deployment.UI.ViewModels;

public sealed class DeploymentViewModel : INotifyPropertyChanged
{
    private readonly DeploymentConfiguration _configuration;
    private readonly IDeploymentService _deploymentService;
    private readonly IDeploymentProgress _logger;
    private DeploymentResult? _result;
    public DeploymentResult? Result
    {
        get => _result;
        private set
        {
            if (_result == value)
                return;

            _result = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsSuccess));
            OnPropertyChanged(nameof(IsFailure));
        }
    }
    public ObservableCollection<string> LogMessages { get; } = [];
    private string _logText = string.Empty;
    public string LogText
    {
        get => _logText;
        private set
        {
            if (_logText == value)
                return;

            _logText = value;
            OnPropertyChanged();
        }
    }
    private int _progress;
    public int Progress
    {
        get => _progress;
        private set
        {
            if (_progress == value)
                return;

            _progress = value;
            OnPropertyChanged();
        }
    }
    private string _status = "Deploying...";
    public string Status
    {
        get => _status;
        private set
        {
            if (_status == value)
                return;

            _status = value;
            OnPropertyChanged();
        }
    }
    private bool _isDeploying;
    public bool IsDeploying
    {
        get => _isDeploying;
        private set
        {
            if (_isDeploying == value)
                return;

            _isDeploying = value;
            OnPropertyChanged();
        }
    }
    private bool _isComplete;
    public bool IsComplete
    {
        get => _isComplete;
        private set
        {
            if (_isComplete == value)
                return;

            _isComplete = value;
            OnPropertyChanged();
        }
    }
    public bool IsSuccess =>
        Result?.Success == true;
    public bool IsFailure =>
        Result is not null && !Result.Success;


    public DeploymentViewModel(
        DeploymentConfiguration configuration,
        IDeploymentService deploymentService,
        IDeploymentProgress progress)
    {
        _configuration = configuration;
        _deploymentService = deploymentService;
        _logger = progress;

        _logger.ProgressReported += OnProgressReported;
    }

    private void OnProgressReported(
        string message,
        int percentage)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            LogMessages.Add(message);
            AddLogMessage(message);
            Progress = percentage;
        });
    }

    private void AddLogMessage(string message)
    {
        LogText += message + Environment.NewLine;
    }

    public void Reset()
    {
        Result = null;
        LogMessages.Clear();
        LogText = string.Empty;
        Progress = 0;
        Status = "Deploying...";
        IsDeploying = false;
        IsComplete = false;
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
    public event Action? NewDeploymentRequested;
    public event Action? CloseRequested;
    public async Task StartAsync()
    {
        IsDeploying = true;

        if (_configuration.Mode == DeploymentMode.Preview)
            AddLogMessage("Running in 'Preview' mode; no changes will be made...");

        try
        {
            DeploymentResult result
                = await Task.Run(
                    () => _deploymentService.DeployAsync(_configuration));

            LogMessages.Add("");

            if (result.Success)
            {
                Status = "Deployment completed successfully";
                LogMessages.Add(Status);
            }
            else
            {
                Status = "Deployment failed.";
                LogMessages.Add($"Deployment failed: {result.ErrorMessage}");
                AddLogMessage($"Deployment failed: {result.ErrorMessage}");
            }

            if (_configuration.Mode == DeploymentMode.Preview)
                AddLogMessage("Running in 'Preview' mode; no changes were made.");


            IsComplete = true;
            Result = result;
        }
        finally
        {
            IsDeploying = false;
        }
    }
    public void StartNewDeployment()
    {
        NewDeploymentRequested?.Invoke();
    }
    public void Finish()
    {
        CloseRequested?.Invoke();
    }
    #endregion

}
