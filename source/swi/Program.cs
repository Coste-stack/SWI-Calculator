public class Program
{
    public static async Task Main(string[] args)
    {
        // Get path
        var baseDir = AppContext.BaseDirectory;
        //var baseDir = "data";
        var inputPath = Path.Combine(baseDir, "input.json");
        var outputPath = Path.Combine(baseDir, "output.txt");

        // Execute processes
        try
        {
            var operations = await JsonHelper.ReadOperationsAsync(inputPath);
            foreach (var op in operations)
            {
                Console.WriteLine(op.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}