using ConsoleApp.Cli;

var root = new RootCommand("Dosya Parçalama Aracı — Büyük dosyaları CLI ile işle ve birleştir");

var processOption = new Option<string>(
    name: "--process",
    description: "Parçalanacak dosya veya klasörün tam yolu");

var mergeOption = new Option<Guid>(
    name: "--merge",
    description: "Birleştirilecek dosyanın GUID formatındaki kimliği");

var outputOption = new Option<string>(
    name: "--output",
    getDefaultValue: () => "output.dat",
    description: "Birleştirme sonucu oluşturulacak dosyanın yolu (varsayılan: output.dat)");

var strategyOption = new Option<ChunkStrategyType>(
    name: "--strategy",
    getDefaultValue: () => ChunkStrategyType.Dynamic,
    description: "Parçalama stratejisi (Dynamic, Fixed, TargetCount)");

root.AddOption(processOption);
root.AddOption(mergeOption);
root.AddOption(outputOption);
root.AddOption(strategyOption);

root.SetHandler(
    async (CliOptions options, IConsole console) =>
    {
        var executor = new CliExecutor(options, console);
        await executor.ExecuteAsync();
    },
    new CliOptionsBinder(processOption, mergeOption, outputOption, strategyOption)
);

return await root.InvokeAsync(args);