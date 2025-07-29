namespace FileChunkingConsoleApp.Tests;

public class CliOptionsValidatorTests
{
    private readonly CliOptionsValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Process_Is_Empty_And_Merge_Is_Empty()
    {
        var options = new CliOptions
        {
            Process = null,
            Merge = Guid.Empty
        };
        var validator = new CliOptionsValidator();
        var result = validator.TestValidate(options);
        result.ShouldHaveValidationErrorFor(x => x.Process);
    }


    [Fact]
    public void Should_Have_Error_When_Output_Is_Empty()
    {
        var model = new CliOptions { Output = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Output);
    }
}
