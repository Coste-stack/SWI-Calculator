using System.Text.Json;

public static class JsonHelper
{
    public static async Task<Dictionary<string, Operation>> ReadOperationsAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Input file not found: {path}");
        
        // Read operations
        using FileStream jsonStream = File.OpenRead(path);
        var operations = await JsonSerializer.DeserializeAsync<Dictionary<string, Operation>>(jsonStream);
        
        if (operations == null) 
            throw new InvalidDataException($"Failed to deserialize JSON from {path}");

        return operations;
    }
}