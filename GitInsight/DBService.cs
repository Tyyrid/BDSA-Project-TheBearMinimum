using GitInsight.Entities;

public class DBService : IDBService
{
    private DBAnalysisRepository dBAnalysisRepository { get; init; }
    private DBFrequencyRepository dBFrequencyRepository { get; init; }
    private Repository repo { get; init; }
    private string repoPath { get; init; }
    public DBService(Repository repo, string repoPath, DBAnalysisRepository dBAnalysisRepository, DBFrequencyRepository dBFrequencyRepository){
        

        
        this.dBAnalysisRepository = dBAnalysisRepository;
        this.dBFrequencyRepository = dBFrequencyRepository;

        
        this.repo = repo;
        this.repoPath = repoPath;
    }

    public IEnumerable<(string, IEnumerable<DBFrequencyDTO>)> GetAuthorAnalysis()
    {
        var dbanalysis = dBAnalysisRepository.Find(repo.Commits.First().Id.ToString(), repoPath, repo.Commits.First().Author.Name);
        
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
                //Console.WriteLine(dbanalysis.Author);
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