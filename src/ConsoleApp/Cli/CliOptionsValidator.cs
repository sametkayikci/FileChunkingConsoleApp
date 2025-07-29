namespace ConsoleApp.Cli;

public sealed class CliOptionsValidator : AbstractValidator<CliOptions>
{
    public CliOptionsValidator()
    {
        RuleFor(x => x)
            .Custom((options, context) =>
            {
                if (!string.IsNullOrWhiteSpace(options.Process) || options.Merge != Guid.Empty) return;
                context.AddFailure(nameof(CliOptions.Process),
                    "'--process' parametresi zorunludur çünkü '--merge' parametresi verilmedi.");
                context.AddFailure(nameof(CliOptions.Merge),
                    "'--merge' parametresi zorunludur çünkü '--process' parametresi verilmedi.");
            });

        RuleFor(x => x.Process)
            .Must(path => string.IsNullOrWhiteSpace(path) || File.Exists(path) || Directory.Exists(path))
            .WithMessage("'--process' ile belirtilen dosya veya klasör mevcut değil.");

        RuleFor(x => x.Output)
            .NotEmpty()
            .WithMessage("'--output' parametresi boş bırakılamaz.");
    }
}