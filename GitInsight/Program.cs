using FluentArgs;


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
            Console.WriteLine(ex.Message);
            return Task.CompletedTask;
        }

        switch (mode)
        {
            case Mode.Author:
                AuthorFrequency(repo.Commits).ToList().ForEach(a =>
                {
                    Console.WriteLine(a.Author);
                    Print(a.Frequency);
                    Console.WriteLine();
                });
                break;
            case Mode.Frequency:
                Print(Frequency(repo.Commits));
                break;
        }

        return Task.CompletedTask;
    })
    .Parse(args);

IEnumerable<(string Author, IEnumerable<(int, DateTime)> Frequency)> AuthorFrequency(IEnumerable<Commit> commits)
    => commits.GroupBy(c => c.Author.Name).OrderBy(g => g.Key).Select(g => (g.Key, Frequency(g)));

IEnumerable<(int Count, DateTime When)> Frequency(IEnumerable<Commit> commits)
    => commits.GroupBy(c => c.Author.When.Date).Select(g => (g.Count(), g.Key));

void Print(IEnumerable<(int Count, DateTime When)> frequency)
    => frequency.OrderByDescending(f => f.When).ToList().ForEach(f => Console.WriteLine($"\t{f.Count} {f.When:yyyy-MM-dd}"));

enum Mode { Frequency, Author }
