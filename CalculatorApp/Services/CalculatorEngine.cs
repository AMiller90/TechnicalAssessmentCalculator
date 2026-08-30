using CalculatorApp.Models;

namespace CalculatorApp.Services;

public class CalculatorEngine
{
    private readonly CalculatorState _state = new();

    public CalculatorState State => _state;

    public void EnterDigit(int digit)
    {
        if (digit is < 0 or > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(digit), "Digit must be between 0 and 9.");
        }

        if (_state.IsError)
        {
            Clear();
        }

        if (!_state.IsEnteringNumber)
        {
            _state.DisplayText = digit.ToString();
            _state.DisplayValue = digit;
            _state.IsEnteringNumber = true;
            return;
        }

        if (_state.DisplayText == "0")
        {
            _state.DisplayText = digit.ToString();
            _state.DisplayValue = digit;
            return;
        }

        var candidateText = _state.DisplayText + digit;

        if (!decimal.TryParse(candidateText, out var candidateValue))
        {
            return;
        }

        _state.DisplayText = candidateText;
        _state.DisplayValue = candidateValue;
    }

    public void EnterDecimal()
    {
        if (_state.IsError)
        {
            Clear();
        }

        if (!_state.IsEnteringNumber)
        {
            _state.DisplayText = "0.";
            _state.DisplayValue = 0;
            _state.IsEnteringNumber = true;
            return;
        }

        if (_state.DisplayText.Contains('.'))
        {
            return;
        }

        _state.DisplayText += ".";
    }

    public void ApplyOperation(CalculatorOperation operation)
    {
        if (_state.IsError)
        {
            return;
        }

        if (_state.PendingOperation.HasValue && _state.IsEnteringNumber)
        {
            if (!TryCalculatePendingOperation())
            {
                return;
            }
        }

        _state.StoredValue = _state.DisplayValue;
        _state.PendingOperation = operation;
        _state.IsEnteringNumber = false;
    }

    public void Equals()
    {
        if (_state.IsError)
        {
            return;
        }

        if (_state.PendingOperation.HasValue && _state.StoredValue.HasValue)
        {
            var lastOperand = _state.DisplayValue;

            if (!TryCalculatePendingOperation())
            {
                return;
            }

            _state.LastOperation = _state.PendingOperation;
            _state.LastOperand = lastOperand;

            _state.PendingOperation = null;
            _state.StoredValue = null;
            _state.IsEnteringNumber = false;

            return;
        }

        if (_state.LastOperation.HasValue && _state.LastOperand.HasValue)
        {
            var operation = _state.LastOperation.Value;
            var operand = _state.LastOperand.Value;

            var result = PerformCalculation(
                _state.DisplayValue,
                operand,
                operation);

            if (result is null)
            {
                return;
            }

            _state.DisplayValue = result.Value;
            _state.DisplayText = FormatDisplayValue(result.Value);
            _state.IsEnteringNumber = false;
        }
    }

    /// <summary>
    /// Clear state data
    /// </summary>
    public void Clear()
    {
        _state.DisplayValue = 0;
        _state.DisplayText = "0";
        _state.StoredValue = null;
        _state.PendingOperation = null;
        _state.LastOperation = null;
        _state.LastOperand = null;
        _state.IsEnteringNumber = false;
        _state.IsError = false;
    }

    public void ClearEntry()
    {
        if (_state.IsError)
        {
            Clear();
            return;
        }

        _state.DisplayValue = 0;
        _state.DisplayText = "0";
        _state.IsEnteringNumber = false;
    }


    public void ToggleSign()
    {
        if (_state.IsError)
        {
            return;
        }

        if (_state.DisplayValue == 0)
        {
            return;
        }

        _state.DisplayValue *= -1;

        if (_state.DisplayText.StartsWith('-'))
        {
            _state.DisplayText = _state.DisplayText[1..];
        }
        else
        {
            _state.DisplayText = "-" + _state.DisplayText;
        }
    }

    public void Percentage()
    {
        if (_state.IsError)
        {
            return;
        }

        if (!_state.PendingOperation.HasValue || !_state.StoredValue.HasValue)
        {
            _state.DisplayValue = 0;
            _state.DisplayText = "0";
            return;
        }

        var percentage = _state.DisplayValue / 100;

        if (_state.PendingOperation is CalculatorOperation.Add or CalculatorOperation.Subtract)
        {
            percentage = _state.StoredValue.Value * percentage;
        }

        _state.DisplayValue = percentage;
        _state.DisplayText = FormatDisplayValue(percentage);
    }

    public void Reciprocal()
    {
        if (_state.IsError)
        {
            return;
        }

        if (_state.DisplayValue == 0)
        {
            _state.IsError = true;
            _state.DisplayText = "Cannot divide by zero";
            return;
        }

        _state.DisplayValue = 1 / _state.DisplayValue;
        _state.DisplayText = FormatDisplayValue(_state.DisplayValue);
        _state.IsEnteringNumber = false;
    }

    public void Square()
    {
        if (_state.IsError)
        {
            return;
        }

        try
        {
            _state.DisplayValue *= _state.DisplayValue;
            _state.DisplayText = FormatDisplayValue(_state.DisplayValue);
            _state.IsEnteringNumber = false;
        }
        catch (OverflowException)
        {
            _state.IsError = true;
            _state.DisplayText = "Result too large";
        }
    }
    public void SquareRoot()
    {
        if (_state.IsError)
        {
            return;
        }

        if (_state.DisplayValue < 0)
        {
            _state.IsError = true;
            _state.DisplayText = "Invalid input";
            return;
        }

        _state.DisplayValue = (decimal)Math.Sqrt((double)_state.DisplayValue);
        _state.DisplayText = FormatDisplayValue(_state.DisplayValue);
        _state.IsEnteringNumber = false;
    }


    public void Backspace()
    {
        if (_state.IsError || !_state.IsEnteringNumber)
        {
            return;
        }

        if (_state.DisplayText.Length <= 1 ||
            (_state.DisplayText.Length == 2 && _state.DisplayText.StartsWith('-')))
        {
            _state.DisplayText = "0";
            _state.DisplayValue = 0;
            return;
        }

        _state.DisplayText = _state.DisplayText[..^1];

        if (_state.DisplayText == "-" ||
            string.IsNullOrEmpty(_state.DisplayText))
        {
            _state.DisplayText = "0";
            _state.DisplayValue = 0;
            return;
        }

        if (decimal.TryParse(_state.DisplayText, out var value))
        {
            _state.DisplayValue = value;
        }
    }


    private bool TryCalculatePendingOperation()
    {
        if (!_state.PendingOperation.HasValue ||
            !_state.StoredValue.HasValue)
        {
            return true;
        }

        var result = PerformCalculation(
            _state.StoredValue.Value,
            _state.DisplayValue,
            _state.PendingOperation.Value);

        if (result is null)
        {
            return false;
        }

        _state.DisplayValue = result.Value;
        _state.DisplayText = FormatDisplayValue(result.Value);

        return true;
    }

    private decimal? PerformCalculation(decimal left, decimal right, CalculatorOperation operation)
    {
        try
        {
            switch (operation)
            {
                case CalculatorOperation.Add:
                    return left + right;

                case CalculatorOperation.Subtract:
                    return left - right;

                case CalculatorOperation.Multiply:
                    return left * right;

                case CalculatorOperation.Divide:
                    if (right == 0)
                    {
                        _state.IsError = true;
                        _state.DisplayText = "Cannot divide by zero";
                        return null;
                    }

                    return left / right;

                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }
        catch (OverflowException)
        {
            _state.IsError = true;
            return null;
        }
    }

    private static string FormatDisplayValue(decimal value)
    {
        return value.ToString("G29");
    }

}