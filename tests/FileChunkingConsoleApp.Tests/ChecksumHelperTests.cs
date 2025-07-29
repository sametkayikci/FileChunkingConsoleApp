namespace FileChunkingConsoleApp.Tests;

public class ChecksumHelperTests
{
    [Fact]
    public void CalculateSha256_ShouldReturnCorrectHash()
    {
        var input = Encoding.UTF8.GetBytes("test");
        var result = ChecksumHelper.CalculateSha256(input);
        Assert.NotNull(result);
        Assert.Equal(64, result.Length); // SHA-256 in hex is 64 characters
    }
}
