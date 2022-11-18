


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

        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using var connection = new SqlConnection(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<GitInsightContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var options = optionsBuilder.Options;

        using var context = new GitInsightContext(options); //using ???
        context.Database.EnsureCreated();
        Console.WriteLine(context.DBAnalysis_s.FirstOrDefault());

        DBService dBService = new DBService(repo, repoInfo.ToString(), new DBAnalysisRepository(context), new DBFrequencyRepository(context));


        
        switch (mode){
            
            case Mode.Author:
                
                var autherfrequiencies = dBService.GetAuthorAnalysis();
                
                Console.WriteLine(autherfrequiencies.Count());
                try{
                foreach (var author in autherfrequiencies)
                {
                    Console.WriteLine(author.Item1);
                    foreach (var f in author.Item2)
                    {
                        Console.WriteLine($"\t{f.Frequency} {f.Date:yyyy-MM-dd}");
                    }   
                } 
                }catch(System.ObjectDisposedException e){
                    Console.WriteLine("got ja");
                }
            
                break; 
            case Mode.Frequency:
                foreach (var f in dBService.GetFrequencyAnalysis())
                {
                    Console.WriteLine($"\t{f.Frequency} {f.Date:yyyy-MM-dd}");
                }
                break;
        } 

        return Task.CompletedTask;
    })
    .Parse(args);

enum Mode { Frequency, Author }
