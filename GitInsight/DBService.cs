using GitInsight.Entities;

public class DBService : IDBService
{
    private DBAnalysisRepository dBAnalysisRepository { get; init; }
    private DBFrequencyRepository dBFrequencyRepository { get; init; }
    private LibGit2Sharp.Repository repo { get; init; }
    private string repoPath { get; init; }
    public DBService(string repoPath){
        Repository repo;
        try
        {
            repo = new Repository(repoPath);
        }
        catch (RepositoryNotFoundException ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw new NotFoundException("Repository not found");
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
        
        dBAnalysisRepository = new DBAnalysisRepository(context);
        dBFrequencyRepository = new DBFrequencyRepository(context);
        this.repo = repo;
        this.repoPath = repoPath;
    }

    public IEnumerable<(string, IEnumerable<DBFrequencyDTO>)> GetAuthorAnalysis()
    {
        var dbanalysis = dBAnalysisRepository.Find(repo.Commits.First().Id.ToString(), repoPath);
                if (dbanalysis is null)
                {
                    var authorgroups = repo.Commits.GroupBy(a => a.Author.Name);
                    foreach (var authorgroup in authorgroups)
                    {
                        var leadAuthorComit = authorgroup.First();
                        var (response, dbanalysisid) = dBAnalysisRepository.Create(new DBAnalysisCreateDTO(leadAuthorComit.Id.ToString(), leadAuthorComit.Author.Name, repoPath));
                        dbanalysis = dBAnalysisRepository.Find(dbanalysisid);

                        foreach(var commit in authorgroup.Frequency()){
                            dBFrequencyRepository.Create(new DBFrequencyCreateDTO(dbanalysisid, commit.When, commit.Count));
                        }
                        
                        yield return (dbanalysis.Author, dBFrequencyRepository.FindAll(dbanalysisid));
                        
                    }
                }else
                {
                    var analysis_s = dBAnalysisRepository.FindAuthorAnalysis_s(repoPath);
                    foreach (var analysis in analysis_s)
                    {
                        yield return (analysis.Author, dBFrequencyRepository.FindAll(analysis.Id));
                    }
                }
    }

    public IEnumerable<DBFrequencyDTO> GetFrequencyAnalysis()
    {
        var dbanalysis = dBAnalysisRepository.Find(repo.Commits.First().Id.ToString(), repoPath);
        if (dbanalysis is null)
                {
                    //saves commit data to database
                    var (response, dbanalysisid) = dBAnalysisRepository.Create(new DBAnalysisCreateDTO(repo.Commits.First().Id.ToString(), "", repoPath));
                    dbanalysis = dBAnalysisRepository.Find(dbanalysisid);
                    foreach(var commit in repo.Commits.Frequency()){
                        dBFrequencyRepository.Create(new DBFrequencyCreateDTO(dbanalysisid, commit.When, commit.Count));
                    }
                    return dBFrequencyRepository.FindAll(dbanalysisid);
                }else{
                    return dBFrequencyRepository.FindAll(dbanalysis.Id);
                }
    }
}