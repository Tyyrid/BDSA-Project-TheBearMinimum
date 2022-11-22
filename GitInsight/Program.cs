using FluentArgs;

//dotnet run --repo 'path'
//hvis path indeholder mellemrum, skal path skrives: "path"
// --mode author for author mode
// --mode frequency for frequency mode (default)

FluentArgsBuilder.New()
    .DefaultConfigs()
    .Parameter<DirectoryInfo>("-r", "--repo")
        .WithValidation(repo => repo.Exists, "Directory does not exist")
        .IsRequired()
    .Parameter<Mode>("-m", "--mode")
        .WithDescription("Which display mode to use.")
        .IsOptional()
    .Call(mode => repoInfo =>
    {
        var DBService = new DBService(repoInfo.ToString());

        switch (mode){
            case Mode.Author:
                var autherfrequiencies = DBService.GetAuthorAnalysis();//(repo, repoInfo.ToString(), dBAnalysisRepository, dBFrequencyRepository);
                
                Console.WriteLine(autherfrequiencies.Count());
                foreach (var author in autherfrequiencies)
                {
                    Console.WriteLine(author.Item1);
                    foreach (var f in author.Item2)
                    {
                        Console.WriteLine($"\t{f.Frequency} {f.Date:yyyy-MM-dd}");
                    }   
                } 
                break; 
            case Mode.Frequency:
                foreach (var f in DBService.GetFrequencyAnalysis())//(repo, repoInfo.ToString(), dBAnalysisRepository, dBFrequencyRepository))
                {
                    Console.WriteLine($"\t{f.Frequency} {f.Date:yyyy-MM-dd}");
                }
                break;
        }  

        return Task.CompletedTask;
    })
    .Parse(args);

enum Mode { Frequency, Author }
