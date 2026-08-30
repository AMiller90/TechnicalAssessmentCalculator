Technical Assessment — Calculator

A desktop calculator application developed as part of a Software Developer technical assessment.

The application is implemented in C# using .NET 9, .NET MAUI, and Blazor Hybrid with an MVVM architecture. The calculator provides functionality similar to Windows Calculator Standard mode while demonstrating separation between the UI, application logic, and calculator state.

Technology Stack
C#
.NET 9
.NET MAUI
Blazor Hybrid
Razor / HTML / CSS
MVVM
JavaScript interop for keyboard input
Git / GitHub
Why .NET MAUI + Blazor Hybrid?

The assessment requires WPF or a newer UI technology and provides bonus consideration for cross-platform UI.

.NET MAUI was selected because it provides a native application model with cross-platform support. Blazor Hybrid allows the UI to be implemented using Razor, HTML, and CSS while running inside a native MAUI BlazorWebView.

The technology choice also aligns closely with technologies identified in the target role, including C#, WPF, and MAUI/Blazor.

The application is not a traditional web application. It runs as a native .NET MAUI application with the Blazor UI hosted inside the application.

Architecture

The application follows a simple MVVM-oriented architecture:
```text
┌────────────────────────────┐
│         Blazor UI          │
│        Home.razor          │
│           View             │
└──────────────┬─────────────┘
               │
               ▼
┌────────────────────────────┐
│   CalculatorViewModel      │
│         ViewModel          │
│                            │
│ UI-facing actions          │
│ Exposes calculator state   │
└──────────────┬─────────────┘
               │
               ▼
┌────────────────────────────┐
│     CalculatorEngine       │
│     Application Logic      │
│                            │
│ Calculator behavior        │
│ State transitions          │
│ Arithmetic                 │
│ Error handling             │
└──────────────┬─────────────┘
               │
               ▼
┌────────────────────────────┐
│      CalculatorState       │
│           Model            │
│                            │
│ Calculator data and state  │
└────────────────────────────┘
```

The calculator engine has no dependency on MAUI, Blazor, or UI components.

This keeps calculator behavior separate from presentation and makes the core calculation logic easier to understand and maintain.

Keyboard Input

Mouse and keyboard input use the same ViewModel actions.

Keyboard events are captured by JavaScript inside the Blazor WebView and forwarded to the Razor component through JavaScript interop.
```text
Keyboard
    │
    ▼
JavaScript keydown handler
    │
    ▼
Blazor JS interop
    │
    ▼
Home.razor
    │
    ▼
CalculatorViewModel
    │
    ▼
CalculatorEngine
```
```text
Supported Keyboard Input
Key	Action
0-9	Enter digit
.	Decimal point
+	Addition
-	Subtraction
*	Multiplication
/	Division
Enter / =	Equals
Escape	Clear
Backspace	Backspace
%	Percentage
F9	Toggle sign
Q / q	Square
R / r	Reciprocal
@	Square root
Delete	Clear Entry
```

Keyboard input is intentionally translated into the same ViewModel operations used by mouse input rather than implementing separate calculator logic.

Calculator Functionality
Required Calculator Functions

The application supports the core assessment requirements:

Number input 0-9
Decimal point
Addition
Subtraction
Multiplication
Division
Equals
Clear
Mouse input
Keyboard input
Additional Standard Calculator Functions

The implementation also includes:
```text
Backspace
Clear Entry (CE)
Toggle sign (±)
Percentage (%)
Reciprocal (1/x)
Square (x²)
Square root (√x)
Sequential/chained calculations
Repeated equals
Decimal input handling
Leading-zero handling
Division-by-zero protection
Invalid square-root handling
Arithmetic overflow handling
Dynamic display sizing
```
The additional functions were implemented to provide a more complete Standard Calculator experience without attempting to reproduce the entire Windows Calculator application.

Calculator Behavior
```text
The calculator uses sequential calculator behavior rather than an expression parser.

For example:

12 + 5 - 3 =


is evaluated sequentially:

12 + 5 = 17
17 - 3 = 14

Repeated Equals

The calculator also supports repeated equals:

12 + 5 = 17
=        22
=        27
```

The implementation maintains the previous operation and operand to support this behavior.

Percentage Behavior

Percentage behavior was specifically tested against Windows Standard Calculator, including:
```text
100 + 10 % = 110
100 - 10 % = 90
100 × 10 % = 10
100 ÷ 10 % = 1000
```
Error Handling

Calculator errors are represented through an explicit calculator error state rather than allowing arithmetic exceptions to reach the UI.

Division by Zero
10 ÷ 0 =


Displays:

Cannot divide by zero

Invalid Square Root
-25 → √x


Displays:

Invalid input

Arithmetic Overflow

An operation exceeding the supported decimal range displays:

Result too large


The calculator remains running and can recover when new input is entered.

Numeric Representation

Normal calculator arithmetic uses C# decimal.

The application intentionally maintains a distinction between the numeric value and its display/input representation.

For example:

DisplayValue = 0
DisplayText  = "0."


This allows the calculator to correctly represent an in-progress decimal entry.

Maintaining the display text separately also allows the UI and calculator logic to distinguish between a numeric value such as 0 and an input state such as 0..

User Interface

The calculator uses a compact desktop-oriented interface with:

Dark calculator styling
Right-aligned display
Four-column keypad
Distinct number, function, and operator buttons
Highlighted equals button
Hover and pressed states
Rounded controls
Dynamic display sizing
Native application window sizing
```text
Keypad Layout
%     CE    C     ←
1/x   x²    √x    ÷
7     8     9     ×
4     5     6     −
1     2     3     +
±     0     .     =
```

The styling is inspired by modern desktop calculator interfaces without directly reproducing Microsoft's branding or assets.

Testing

Testing was primarily performed manually through the running application.

The development process was:
```text
Implement feature
       │
       ▼
Build application
       │
       ▼
Test through the UI
       │
       ▼
Compare behavior with Windows Standard Calculator where appropriate
       │
       ▼
Refine implementation
       │
       ▼
Commit changes
       │
       ▼
Push to feature branch
       │
       ▼
Merge completed work into main
```
Testing Coverage

Testing covered:

Basic arithmetic
Decimal input
Leading zeros
Backspace
Clear and Clear Entry
Sign toggle
Percentage behavior
Reciprocal
Square
Square root
Repeated equals
Chained calculations
Keyboard input
Mouse input
Division by zero
Invalid square roots
Arithmetic overflow
Error recovery
Long display values

A dedicated automated test project was intentionally not added because the assessment's primary goal was the calculator implementation and UI. Manual testing was used extensively during development, with Windows Standard Calculator used as a behavioral reference for relevant functionality.
```text
Project Structure
TechnicalAssessmentCalculator/
│
├── CalculatorApp/
│   │
│   ├── Components/
│   │   └── Pages/
│   │       ├── Home.razor
│   │       └── Home.razor.css
│   │
│   ├── Models/
│   │   ├── CalculatorOperation.cs
│   │   └── CalculatorState.cs
│   │
│   ├── Services/
│   │   └── CalculatorEngine.cs
│   │
│   ├── ViewModels/
│   │   └── CalculatorViewModel.cs
│   │
│   ├── Platforms/
│   ├── Resources/
│   │
│   ├── wwwroot/
│   │   └── keyboard.js
│   │
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── MainPage.xaml
│   ├── MainPage.xaml.cs
│   ├── MauiProgram.cs
│   └── CalculatorApp.csproj
│
└── README.md
```
Git Development

Calculator development was initially completed on the:

feature/calculator


branch.

Development was intentionally kept separate from main until the implementation was complete.

The project was developed through multiple meaningful commits rather than a single final commit. The commit history reflects the progression from the generated MAUI application through the calculator architecture, functionality, keyboard support, behavioral refinements, error handling, and UI improvements.

The completed feature branch was subsequently merged into main.

Scope

The goal of this project is to demonstrate software development ability through a focused calculator implementation.

The following Windows Calculator features are intentionally outside the scope of this assessment:

Scientific mode
Programmer mode
Graphing
Date calculations
Unit conversion
History
Memory functions
Keep on top
Compact/mini mode
Scientific notation
Full dynamic text measurement and scaling behavior

These features were excluded intentionally to keep the implementation focused on the assessment requirements rather than attempting to reproduce the complete Windows Calculator application.

Build and Run
Requirements
.NET 9 SDK
.NET MAUI workload
Supported development environment for the desired MAUI target

The project was developed and tested using:

.NET SDK 9.0.317

Build

From the repository root:

dotnet build

Run

The application can be launched using the normal .NET MAUI development workflow for the target platform.

Assessment Focus

This project is intended to demonstrate:

C# development
.NET MAUI application development
Blazor Hybrid UI development
MVVM architecture
Separation of UI and application logic
Dependency injection
Keyboard and mouse interaction
State management
Error handling
Cross-platform UI technology
Git-based incremental development
Manual testing and behavioral verification
Practical scope management
