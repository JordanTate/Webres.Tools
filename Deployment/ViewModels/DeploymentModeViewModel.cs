using Deployment.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Deployment.ViewModels;

public class DeploymentModeViewModel
{
    private DeploymentMode _selectedMode = DeploymentMode.Preview;
    public DeploymentMode SelectedMode 
    { 
        get => _selectedMode; 
        set
        {
            if (_selectedMode == value)
                return;

            _selectedMode = value;

            
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
