namespace ConsoleApp.Cli;

public record CliOptions
{
    public string Process { get; init; }
    public Guid Merge { get; init; }
    public string Output { get; init; } = "output.dat";
    public ChunkStrategyType Strategy { get; init; } = ChunkStrategyType.Dynamic;
}