using swi.Models.Operations;
using swi.Services.Operations.OperationStrategies;
namespace swi.Tests;

public class SqrtOperationStrategyTests
{
    private readonly Mock<ILogger<SqrtOperationStrategy>> _loggerMock;
    private readonly SqrtOperationStrategy _strategy;

    public SqrtOperationStrategyTests()
    {
        _loggerMock = new Mock<ILogger<SqrtOperationStrategy>>();
        _strategy = new SqrtOperationStrategy(_loggerMock.Object);
    }

    [Fact]
    public void Computes_SquareRoot_For_ValidOperand()
    {
        var op = new Operation(OperationType.Sqrt, new double[] { 16 });
        var result = _strategy.Execute(op);
        Assert.Equal(4, result);
    }

    [Fact]
    public void Throws_For_NegativeOperand()
    {
        var op = new Operation(OperationType.Sqrt, new double[] { -4 });
        Assert.Throws<ArgumentException>(() => _strategy.Execute(op));
    }

    [Fact]
    public void Throws_When_TooManyOperands()
    {
        var op = new Operation(OperationType.Sqrt, new double[] { 16, 4 });
        Assert.Throws<ArgumentException>(() => _strategy.Execute(op));
    }

    [Fact]
    public void Returns_NaN_For_NaNOperand()
    {
        var op = new Operation(OperationType.Sqrt, new double[] { double.NaN });
        var result = _strategy.Execute(op);
        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Returns_Infinity_For_InfiniteOperand()
    {
        var op = new Operation(OperationType.Sqrt, new double[] { double.PositiveInfinity });
        var result = _strategy.Execute(op);
        Assert.True(double.IsInfinity(result));
    }
}
