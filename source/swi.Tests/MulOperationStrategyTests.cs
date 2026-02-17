namespace swi.Tests;

public class MulOperationStrategyTests
{
    private readonly Mock<ILogger<MulOperationStrategy>> _loggerMock;
    private readonly MulOperationStrategy _strategy;

    public MulOperationStrategyTests()
    {
        _loggerMock = new Mock<ILogger<MulOperationStrategy>>();
        _strategy = new MulOperationStrategy(_loggerMock.Object);
    }

    [Fact]
    public void Multiplies_MultipleOperands()
    {
        var op = new Operation(OperationType.Mul, new double[] { 2, 3, 4 });
        var result = _strategy.Execute(op);
        Assert.Equal(24, result);
    }

    [Fact]
    public void Throws_When_LessThanMinOperands()
    {
        var op = new Operation(OperationType.Mul, new double[] { 2 });
        Assert.Throws<ArgumentException>(() => _strategy.Execute(op));
    }

    [Fact]
    public void Overflow_Produces_Infinity()
    {
        var op = new Operation(OperationType.Mul, new double[] { 1e308, 1e308 });
        var result = _strategy.Execute(op);
        Assert.True(double.IsInfinity(result));
    }

    [Fact]
    public void Handles_Decimal_Multiplication()
    {
        var op = new Operation(OperationType.Mul, new double[] { 0.1, 0.2 });
        var result = _strategy.Execute(op);
        Assert.Equal(0.020000000000000004, result, 15);
    }
}
