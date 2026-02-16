using System.Text.Json;

namespace swi.Tests;

public class JsonHelperTests
{
    [Fact]
    public async Task ReadsOperationsFromFile()
    {
        // Arange
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
}