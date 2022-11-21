using GitInsight.Entities;

public class DBService : IDBService
{   
    public static IEnumerable<(string, IEnumerable<DBFrequencyDTO>)> GetAuthorAnalysis(Repository repo, string repoPath, DBAnalysisRepository dBAnalysisRepository, DBFrequencyRepository dBFrequencyRepository)
    {
        var dbanalysis = dBAnalysisRepository.Find(repo.Commits.First().Id.ToString(), repoPath, repo.Commits.First().Author.Name);
        
        if (dbanalysis is null)
        {
            var authorgroups = repo.Commits.GroupBy(a => a.Author.Name);
            var authorList = new List<(string, IEnumerable<DBFrequencyDTO>)>();
            foreach (var authorgroup in authorgroups)
            {
                var leadAuthorComit = authorgroup.First();
                var (response, dbanalysisid) = dBAnalysisRepository.Create(new DBAnalysisCreateDTO(leadAuthorComit.Id.ToString(), leadAuthorComit.Author.Name, repoPath));
                dbanalysis = dBAnalysisRepository.Find(dbanalysisid);
                foreach(var commit in authorgroup.Frequency()){
                    dBFrequencyRepository.Create(new DBFrequencyCreateDTO(dbanalysisid, commit.When, commit.Count));
                }
                authorList.Add((dbanalysis.Author, dBFrequencyRepository.FindAll(dbanalysisid)));
            }
            return authorList;
            
        }else
        {
            var analysis_s = dBAnalysisRepository.FindAuthorAnalysis_s(repoPath);
            var authorList = new List<(string, IEnumerable<DBFrequencyDTO>)>();
            foreach (var analysis in analysis_s)
            {
                authorList.Add((analysis.Author, dBFrequencyRepository.FindAll(analysis.Id)));
            }
            return authorList;
        }
    }

    public static IEnumerable<DBFrequencyDTO> GetFrequencyAnalysis(Repository repo, string repoPath, DBAnalysisRepository dBAnalysisRepository, DBFrequencyRepository dBFrequencyRepository)
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