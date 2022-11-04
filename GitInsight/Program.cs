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
        Repository repo;
        try
        {
            repo = new Repository(repoInfo.ToString());
        }
        catch (RepositoryNotFoundException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return Task.CompletedTask;
        }

        switch (mode)
        {
            case Mode.Author:
                repo.Commits.AuthorFrequency().ToList().ForEach(a =>
                {
                    Console.WriteLine(a.Author);
                    a.Frequency.Print();
                    Console.WriteLine();
                });
                break;
            case Mode.Frequency:
                repo.Commits.Frequency().Print();
                break;
        }

        return Task.CompletedTask;
    })
    .Parse(args);

enum Mode { Frequency, Author }
