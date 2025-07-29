namespace ConsoleApp.Cli;

public class CliOptionsBinder(
    Option<string> process,
    Option<Guid> merge,
    Option<string> output,
    Option<ChunkStrategyType> strategy)
    : BinderBase<CliOptions>
{
    protected override CliOptions GetBoundValue(BindingContext bindingContext) =>
        new()
        {
            Process = bindingContext.ParseResult.GetValueForOption(process),
            Merge = bindingContext.ParseResult.GetValueForOption(merge),
            Output = bindingContext.ParseResult.GetValueForOption(output)!,
            Strategy = bindingContext.ParseResult.GetValueForOption(strategy)
        };
}