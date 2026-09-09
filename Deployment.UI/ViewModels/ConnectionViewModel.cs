using Deployment.Core.Configuration;
using Deployment.Core.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Deployment.UI.ViewModels;

public sealed class ConnectionViewModel(
    DeploymentConfiguration configuration,
    IConnectionProvider provider,
    IConnectionValidator validator) : INotifyPropertyChanged
{
    public ConnectionInfo? Connection { get; private set; }
    private ConnectionValidationResult? _validation;
    public ConnectionValidationResult? Validation
    {
        get => _validation;
        private set
        {
            if (_validation == value)
                return;

            _validation = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasValidationResult));
            OnPropertyChanged(nameof(ValidationFailed));
            OnPropertyChanged(nameof(ValidationSucceeded));
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
    private bool _isValidating;
    public bool IsValidating
    {
        get => _isValidating;
        private set
        {
            if (_isValidating == value)
                return;

            _isValidating = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanValidate));
        }
    }

    public bool CanValidate =>
        !IsValidating;
    public bool HasValidationResult
        => Validation is not null;
    public bool ValidationSucceeded =>
        Validation?.Success == true;
    public bool ValidationFailed =>
        Validation is not null && !Validation.Success;
    public string ErrorMessage =>
        Validation is not null && !Validation.Success ? Validation.ErrorMessage! : string.Empty;

    public async Task LoadAsync()
    {
        Connection = provider.GetConnection(configuration.Website!);

        if (Connection is not null)
            configuration.Connection = Connection;
    }

    public async Task GetValidationResult()
    {
        if (IsValidating)
            return;

        try
        {
            IsValidating = true;
            Validation = null;
            Validation = await validator.ValidateAsync(
                configuration.Connection!,
                default);
        }
        finally
        {
            IsValidating = false;
        }
    }
    public void Reset()
    {
        Connection = null;
        Validation = null;
        IsValidating = false;
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
    public event Action? ContinueRequested;
    public void Continue()
    {
        if (Validation is null || !Validation.Success)
            return;

        ContinueRequested?.Invoke();
    }
    #endregion
}
