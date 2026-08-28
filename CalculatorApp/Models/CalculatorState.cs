namespace CalculatorApp.Models;

public class CalculatorState
{
    /// <summary>
    /// The number currently being displayed.
    /// </summary>
    public decimal DisplayValue { get; set; }
    public string DisplayText { get; set; } = "0";

    /// <summary>
    /// The value we've saved when an operation is selected.
    /// </summary>
    public decimal? StoredValue { get; set; }
    /// <summary>
    /// The operation waiting to be performed.
    /// </summary>
    public CalculatorOperation? PendingOperation { get; set; }
    public CalculatorOperation? LastOperation { get; set; }
    public decimal? LastOperand { get; set; }
    public bool IsEnteringNumber { get; set; }
    /// <summary>
    /// Allows the engine to enter a controlled error state.
    /// </summary>
    public bool IsError { get; set; }
}
