namespace GitSharp.Test.Helpers;

public class DirectoryHelpers
{
    private readonly Guid _guid = Guid.NewGuid();
    
    public string TestDirectory => Path.Combine(Directory.GetCurrentDirectory(), "TestDirectory", _guid.ToString());
    
    public void SetupTestDirectory()
    {
        if (Directory.Exists(TestDirectory))
        {
            Directory.Delete(TestDirectory, true);
        }

        Directory.CreateDirectory(TestDirectory);
    }

    public async Task<string> SetupTestDirectoryWithTestFileAsync()
    {
        SetupTestDirectory();
        
        var filePath = Path.Combine(TestDirectory, "test.txt");
        await File.WriteAllTextAsync(filePath, "Hello, World!");

        return filePath;
    }
}