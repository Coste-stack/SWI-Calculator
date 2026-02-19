using swi.Models.Operations;
using swi.Services.Operations.OperationStrategies;
namespace swi.Tests;

public class SubOperationStrategyTests
{
    private readonly Mock<ILogger<SubOperationStrategy>> _loggerMock;
    private readonly SubOperationStrategy _strategy;

    public SubOperationStrategyTests()
    {
        _loggerMock = new Mock<ILogger<SubOperationStrategy>>();
        _strategy = new SubOperationStrategy(_loggerMock.Object);
    }

    [Fact]
    public void Subtracts_MultipleOperands()
    {
        var op = new Operation(OperationType.Sub, new double[] { 10, 3, 2 });
        var result = _strategy.Execute(op);
        Assert.Equal(5, result);
    }

    [Fact]
    public void Throws_When_LessThanMinOperands()
    {
        var op = new Operation(OperationType.Sub, new double[] { 10 });
        Assert.Throws<ArgumentException>(() => _strategy.Execute(op));
    }

    [Fact]
    public void Result_Is_NaN_When_AnyOperandIsNaN()
    {
        var op = new Operation(OperationType.Sub, new double[] { double.NaN, 1 });
        var result = _strategy.Execute(op);
        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Handles_Infinity()
    {
        var op = new Operation(OperationType.Sub, new double[] { 1, double.PositiveInfinity });
        var result = _strategy.Execute(op);
        Assert.True(double.IsInfinity(result));
    }
}
