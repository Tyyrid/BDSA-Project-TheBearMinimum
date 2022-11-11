using FluentArgs;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using GitInsight.Entities;

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

        using var context = new GitInsightContext(options);
        context.Database.EnsureCreated();
        
        var _DBAnalysisRepository = new DBAnalysisRepository(context);
        var _DBFrequencyRepository = new DBFrequencyRepository(context);

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
                var dbanalysis = _DBAnalysisRepository.Find(repo.Commits.First().Id.ToString(), repoInfo.ToString());
                if (dbanalysis is null)
                {
                    //saves commit data to database
                    var (response, dbanalysisid) =_DBAnalysisRepository.Create(new DBAnalysisCreateDTO(repo.Commits.First().Id.ToString(), "", repoInfo.ToString()));
                    dbanalysis = _DBAnalysisRepository.Find(dbanalysisid);
                    foreach(var commit in repo.Commits.Frequency()){
                        _DBFrequencyRepository.Create(new DBFrequencyCreateDTO(dbanalysisid, commit.When, commit.Count));
                    }

                    repo.Commits.Frequency().Print();    
                }else{
                    var something = context.DBFrequencies.Where(c => c.DBAnalysisId == dbanalysis.Id);
                    foreach (var f in something)
                    {
                        Console.WriteLine($"\t{f.Frequency} {f.Date:yyyy-MM-dd}");
                    }
                }

                break;
        }

        return Task.CompletedTask;
    })
    .Parse(args);

enum Mode { Frequency, Author }
