using FluentArgs;

//dotnet run --repo 'path'
//hvis path indeholder mellemrum, skal path skrives: "path"
// --mode author for author m½ode
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

        /*var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using var connection = new SqlConnection(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<GitInsightContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var options = optionsBuilder.Options;

        using var context = new GitInsightContext(options);*/
        var factory = new GitInsightContextFactory();
        var context = factory.CreateDbContext(args);


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
