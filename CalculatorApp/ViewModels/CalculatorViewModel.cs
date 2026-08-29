using System.ComponentModel;
using System.Runtime.CompilerServices;
using CalculatorApp.Models;
using CalculatorApp.Services;

namespace CalculatorApp.ViewModels;
public class CalculatorViewModel : INotifyPropertyChanged
{
    private CalculatorEngine _engine;
    public CalculatorState State => _engine.State;

    public string DisplayText => State.DisplayText;
    public bool IsError => State.IsError;

    public event PropertyChangedEventHandler? PropertyChanged;

    public CalculatorViewModel(CalculatorEngine engine)
    {
        _engine = engine;
    }

    public void EnterDigit(int digit)
    {
        _engine.EnterDigit(digit);
        NotifyStateChanged();
    }

    public void EnterDecimal()
    {
        _engine.EnterDecimal();
        NotifyStateChanged();
    }

    public void ApplyOperation(CalculatorOperation operation)
    {
        _engine.ApplyOperation(operation);
        NotifyStateChanged();
    }

    public void Equals()
    {
        _engine.Equals();
        NotifyStateChanged();
    }

    public void Clear()
    {
        _engine.Clear();
        NotifyStateChanged();
    }

    public void ToggleSign()
    {
        _engine.ToggleSign();
        NotifyStateChanged();
    }

    public void Percentage()
    {
        _engine.Percentage();
        NotifyStateChanged();
    }

    public void Backspace()
    {
        _engine.Backspace();
        NotifyStateChanged();
    }
    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(DisplayText));
        OnPropertyChanged(nameof(IsError));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
