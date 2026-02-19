using swi.Models.Operations;
using swi.Services.Operations;
using swi.Services.Operations.OperationStrategies;
using System.Text.Json;

namespace swi.Tests;

public class OperationFactoryTests
{
    private readonly OperationFactory _factory;

    public OperationFactoryTests()
    {
        var strategies = new IOperationStrategy[]
        {
            new AddOperationStrategy(Mock.Of<ILogger<AddOperationStrategy>>()),
            new SubOperationStrategy(Mock.Of<ILogger<SubOperationStrategy>>()),
            new MulOperationStrategy(Mock.Of<ILogger<MulOperationStrategy>>()),
            new SqrtOperationStrategy(Mock.Of<ILogger<SqrtOperationStrategy>>())
        };
        _factory = new OperationFactory(strategies);
    }

    private static JsonElement? E(string json)
    {
        if (json == null) return null;
        return JsonDocument.Parse(json).RootElement;
    }

    [Fact]
    public void Creates_Add_For_ValidAdd()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = E("2"), Value2 = E("3") };
        var op = _factory.Create(dto);
        Assert.Equal(OperationType.Add, op.OperatorType);
        Assert.Equal(2, op.Operands.Count);
        Assert.Equal(2, op.Operands[0]);
        Assert.Equal(3, op.Operands[1]);
    }

    [Fact]
    public void Parses_Operator_CaseInsensitive()
    {
        var dto = new OperationDto { Operator = E("\"AdD\""), Value1 = E("10"), Value2 = E("5") };
        var op = _factory.Create(dto);
        Assert.Equal(OperationType.Add, op.OperatorType);
    }

    [Fact]
    public void Throws_When_OperatorMissing()
    {
        var dto = new OperationDto { Operator = null, Value1 = E("1"), Value2 = E("2") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_When_OperatorIsNullToken()
    {
        var dto = new OperationDto { Operator = E("null"), Value1 = E("1"), Value2 = E("2") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_When_OperatorIsNumeric()
    {
        var dto = new OperationDto { Operator = E("123"), Value1 = E("2"), Value2 = E("8") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_When_OperatorUnsupported()
    {
        var dto = new OperationDto { Operator = E("\"pow\""), Value1 = E("2"), Value2 = E("8") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_On_Invalid_Value1_String()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = E("\"abc\""), Value2 = E("8") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_On_Invalid_Value2_String()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = E("1"), Value2 = E("\"xyz\"") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_When_Value1_Missing_For_Binary()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = null, Value2 = E("5") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_When_Value2_Missing_For_Binary()
    {
        var dto = new OperationDto { Operator = E("\"mul\""), Value1 = E("2"), Value2 = null };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_On_NonNumeric_Boolean()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = E("true"), Value2 = E("2") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_On_NonNumeric_Array()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = E("[1]"), Value2 = E("2") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Throws_On_NonNumeric_Object()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = E("{ \"x\": 1 }"), Value2 = E("2") };
        Assert.Throws<InvalidOperationException>(() => _factory.Create(dto));
    }

    [Fact]
    public void Parses_ScientificNotation_Strings()
    {
        var dto = new OperationDto { Operator = E("\"add\""), Value1 = E("\"1e3\""), Value2 = E("\"2.5e2\"") };
        var op = _factory.Create(dto);
        Assert.Equal(1250, op.Operands[0] + op.Operands[1]);
    }

    [Fact]
    public void Parses_NaN_And_Infinity_Strings()
    {
        var dto1 = new OperationDto { Operator = E("\"add\""), Value1 = E("\"NaN\""), Value2 = E("1") };
        var op1 = _factory.Create(dto1);
        Assert.True(double.IsNaN(op1.Operands[0]));

        var dto2 = new OperationDto { Operator = E("\"add\""), Value1 = E("1"), Value2 = E("\"Infinity\"") };
        var op2 = _factory.Create(dto2);
        Assert.True(double.IsInfinity(op2.Operands[1]));
    }

    [Fact]
    public void Handles_Sqrt_With_SingleOperand_When_SecondMissing()
    {
        var dto = new OperationDto { Operator = E("\"sqrt\""), Value1 = E("16"), Value2 = null };
        var op = _factory.Create(dto);
        Assert.Equal(OperationType.Sqrt, op.OperatorType);
        Assert.Single(op.Operands);
    }
}
