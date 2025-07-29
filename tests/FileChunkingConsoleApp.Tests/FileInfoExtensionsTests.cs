namespace FileChunkingConsoleApp.Tests;

public class FileInfoExtensionsTests
{
    [Fact]
    public void NameHash_ShouldReturnGuid()
    {
        var fileName = Path.GetTempFileName();
        var file = new FileInfo(fileName);
        var result = file.NameHash();
        Assert.IsType<System.Guid>(result);
    }
}
