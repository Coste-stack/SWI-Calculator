# SWI Operations Application

It's a .NET 6 application that reads operations from a JSON file, executes them, and writes results to a TXT file. 
It supports basic arithmetic operations and handles input validation and errors.

## Requirements

- [.NET 6.0.428 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Supported Operations

- **Add** - addition of two numbers  
- **Sub** - subtraction of two numbers  
- **Mul** - multiplication of two numbers  
- **Sqrt** - square root of a number

Each operation currently works with one or two input values. It's extensible to more values in the future.

## Input JSON Structure

The application reads operations from an `input.json` file in the project directory (It copies it at build to project binaries).

Example:

```json
{
  "obj1": {
    "operator": "add",
    "value1": 2,
    "value2": 3
  },
  "obj2": {
    "operator": "sqrt",
    "value1": 16
  }
}
```

## Example Output

```txt
valid_add: 5
valid_mul: 12
value2_Infinity: Infinity
decimal_values: 0.30000000000000004
unknown_operator: Operator not supported
invalid_value1: Operand 1 is invalid
missing_value2: Operand 2 is missing
```

## Usage

There are three PowerShell scripts (in the project directory) containing `dotnet` commands for automation:

- `run.ps1` - runs the `swi` project  
- `test.ps1` - runs tests in the `swi.Tests` project  
- `publish.ps1` - creates project binaries in the parent directory (publish folder)
