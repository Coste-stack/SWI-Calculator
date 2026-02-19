using swi.Utils;
using swi.Services.Operations.OperationStrategies;
using swi.Services.Operations;
using swi.Models.Operations;
using System.Text.Json;

namespace swi.Tests;

public class JsonHelperTests
{
    private readonly JsonHelper _jsonHelper;
    private readonly Mock<ILogger<JsonHelper>> _logger;

    public JsonHelperTests()
    {
        // Arrange
        _logger = new Mock<ILogger<JsonHelper>>();
        
        var strategies = new IOperationStrategy[]
        {
            new AddOperationStrategy(Mock.Of<ILogger<AddOperationStrategy>>()),
            new SubOperationStrategy(Mock.Of<ILogger<SubOperationStrategy>>()),
            new MulOperationStrategy(Mock.Of<ILogger<MulOperationStrategy>>()),
            new SqrtOperationStrategy(Mock.Of<ILogger<SqrtOperationStrategy>>())
        };
        var factory = new OperationFactory(strategies);
        _jsonHelper = new JsonHelper(_logger.Object, factory);
    }

    [Fact]
    public async Task ReadsOperationsFromFile()
    {  
        string tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, @"{
            ""obj1"": { ""operator"": ""add"", ""value1"": 2, ""value2"": 3 }
        }");

        // Act
        var operations = await _jsonHelper.ReadOperationsAsync(tempFile);

        // Assert
        Assert.Single(operations.Valid);
        Assert.Empty(operations.Failed);
        Assert.Equal(OperationType.Add, operations.Valid["obj1"].OperatorType);
        Assert.Equal(2, operations.Valid["obj1"].Operands[0]);
        Assert.Equal(3, operations.Valid["obj1"].Operands[1]);

        File.Delete(tempFile);
    }

    [Fact]
    public async Task ThrowsFileNotFoundException()
    {
        string missingFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".json");

        // Act
        var readOperations = () => _jsonHelper.ReadOperationsAsync(missingFile);
        
        // Assert
        await Assert.ThrowsAsync<FileNotFoundException>(readOperations);
    }

    [Fact]
    public async Task ThrowsInvalidJsonException()
    {
        string tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, @"{ invalid json }");

        // Act
        var readOperations = () => _jsonHelper.ReadOperationsAsync(tempFile);

        // Assert
        await Assert.ThrowsAsync<JsonException>(readOperations);

        File.Delete(tempFile);
    }

    [Fact]
    public async Task HandlesInvalidAndMissingFields()
    {
        string tempFile = Path.GetTempFileName();
        string json = @"
        {
            ""valid_add"": { ""operator"": ""add"", ""value1"": 2, ""value2"": 3 },
            ""invalid_operator"": { ""operator"": 2, ""value1"": 2, ""value2"": 8 },
            ""invalid_value1"": { ""operator"": ""add"", ""value1"": ""aaa"", ""value2"": 8 },
            ""missing_value2"": { ""operator"": ""sub"", ""value1"": 5 },
            ""sqrt_negative"": { ""operator"": ""sqrt"", ""value1"": -4 }
        }";
        await File.WriteAllTextAsync(tempFile, json);

        // Act
        var operations = await _jsonHelper.ReadOperationsAsync(tempFile);

        // Assert
        Assert.Equal(2, operations.Valid.Count);
        Assert.Equal(3, operations.Failed.Count);

        File.Delete(tempFile);
    }
}