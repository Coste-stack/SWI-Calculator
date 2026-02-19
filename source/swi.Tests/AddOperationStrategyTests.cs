using swi.Models.Operations;
using swi.Services.Operations.OperationStrategies;
namespace swi.Tests;

public class AddOperationStrategyTests
{
    private readonly Mock<ILogger<AddOperationStrategy>> _loggerMock;
    private readonly AddOperationStrategy _strategy;

    public AddOperationStrategyTests()
    {
        _loggerMock = new Mock<ILogger<AddOperationStrategy>>();
        _strategy = new AddOperationStrategy(_loggerMock.Object);
    }

    [Fact]
    public void Sums_MultipleOperands()
    {
        var op = new Operation(OperationType.Add, new double[] { 1, 2, 3 });
        var result = _strategy.Execute(op);
        Assert.Equal(6, result);
    }

    [Fact]
    public void Throws_When_LessThanMinOperands()
    {
        var op = new Operation(OperationType.Add, new double[] { 1 });
        Assert.Throws<ArgumentException>(() => _strategy.Execute(op));
    }

    [Fact]
    public void Handles_Decimal_Precision()
    {
        var op = new Operation(OperationType.Add, new double[] { 0.1, 0.2 });
        var result = _strategy.Execute(op);
        Assert.Equal(0.30000000000000004, result, 15);
    }

    [Fact]
    public void Handles_ScientificNotation()
    {
        var op = new Operation(OperationType.Add, new double[] { 1e3, 2.5e2 });
        var result = _strategy.Execute(op);
        Assert.Equal(1250, result);
    }

    [Fact]
    public void Returns_NaN_When_AnyOperandIsNaN()
    {
        var op = new Operation(OperationType.Add, new double[] { double.NaN, 1 });
        var result = _strategy.Execute(op);
        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Returns_Infinity_When_Overflow()
    {
        var op = new Operation(OperationType.Add, new double[] { 1e308, 1e308 });
        var result = _strategy.Execute(op);
        Assert.True(double.IsInfinity(result));
    }
}
