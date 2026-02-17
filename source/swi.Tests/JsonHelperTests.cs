using System.Text.Json;

namespace swi.Tests;

public class JsonHelperTests
{
    [Fact]
    public async Task ReadsOperationsFromFile()
    {
        // Arrange
        var logger = new Mock<ILogger<JsonHelper>>();
        var jsonHelper = new JsonHelper(logger.Object);
        
        string tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, @"{
            ""obj1"": { ""operator"": ""add"", ""value1"": 2, ""value2"": 3 }
        }");

        // Act
        var operations = await jsonHelper.ReadOperationsAsync(tempFile);

        // Assert
        Assert.Single(operations);
        Assert.Equal("add", operations["obj1"].Operator);

        File.Delete(tempFile);
    }

    [Fact]
    public async Task ThrowsFileNotFoundException()
    {
        // Arrange
        var logger = new Mock<ILogger<JsonHelper>>();
        var jsonHelper = new JsonHelper(logger.Object);
        string missingFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".json");

        // Act
        var readOperations = () => jsonHelper.ReadOperationsAsync(missingFile);
        
        // Assert
        await Assert.ThrowsAsync<FileNotFoundException>(readOperations);
    }

    [Fact]
    public async Task ThrowsInvalidJsonException()
    {
        // Arrange
        var logger = new Mock<ILogger<JsonHelper>>();
        var jsonHelper = new JsonHelper(logger.Object);
 
        string tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, @"{ invalid json }");

        // Act
        var readOperations = () => jsonHelper.ReadOperationsAsync(tempFile);

        // Assert
        await Assert.ThrowsAsync<JsonException>(readOperations);

        File.Delete(tempFile);
    }

    [Fact]
    public async Task HandlesInvalidAndMissingFields()
    {
        // Arrange
        var logger = new Mock<ILogger<JsonHelper>>();
        var jsonHelper = new JsonHelper(logger.Object);

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
        var operations = await jsonHelper.ReadOperationsAsync(tempFile);

        // Assert
        // Should load all keys
        Assert.Equal(5, operations.Count);

        // valid_add
        Assert.Equal("add", operations["valid_add"].Operator);
        Assert.Equal(2, operations["valid_add"].Value1);
        Assert.Equal(3, operations["valid_add"].Value2);

        // invalid_operator
        Assert.Equal("2", operations["invalid_operator"].Operator);
        Assert.Equal(2, operations["invalid_operator"].Value1);
        Assert.Equal(8, operations["invalid_operator"].Value2);
        Assert.Null(operations["invalid_operator"].OperatorType);

        // invalid_value1
        Assert.Equal("add", operations["invalid_value1"].Operator);
        Assert.Null(operations["invalid_value1"].Value1);
        Assert.Equal(8, operations["invalid_value1"].Value2);

        // missing_value2
        Assert.Equal("sub", operations["missing_value2"].Operator);
        Assert.Equal(5, operations["missing_value2"].Value1);
        Assert.Null(operations["missing_value2"].Value2);

        // sqrt_negative
        Assert.Equal("sqrt", operations["sqrt_negative"].Operator);
        Assert.Equal(-4, operations["sqrt_negative"].Value1);
        Assert.Null(operations["sqrt_negative"].Value2);
        
        File.Delete(tempFile);
    }
}