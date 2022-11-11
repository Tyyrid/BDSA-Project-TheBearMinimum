namespace GitInsight.Entities;

public class DBAnalysisRepository : IDBAnalysisRepository
{
    readonly GitInsightContext context;

    public DBAnalysisRepository(GitInsightContext context) {
        this.context = context;
    }


    public (Response response, int latestCommitId) Create(DBAnalysisCreateDTO commit)
    {
        //If commidId exists in database do nothing and return last analyze
        if (context.DBAnalysis_s.Where(c => c.LatestCommitId.Equals(commit.LatestCommitId)).Any())
        {
            if(context.DBAnalysis_s.Where(a => a.Author!.Equals(commit.Author)).Any())
            {
                var existing = Find(commit.LatestCommitId, commit.GitRepository);
                return (Conflict, existing.Id);
            }
            
        }
        //Create new DBCommit
        DBAnalysis c = new DBAnalysis(commit.LatestCommitId, commit.Author!, commit.GitRepository);
        //add to context and update database
        context.DBAnalysis_s.Add(c);
        context.SaveChanges();
        return (Created, c.Id);
    }

    public DBAnalysisDTO Find(int commitId, string gitRepository)
    {
        var commit = context.DBAnalysis_s.Where(r => r.GitRepository.Equals(gitRepository) && r.LatestCommitId.Equals(commitId)).FirstOrDefault();
        if(commit is null) return null!;
        return new DBAnalysisDTO(commit.Id, commit.LatestCommitId, commit.Author, commit.GitRepository);
    }
    public DBAnalysisDTO Find(int DBAnalysisId)
    {
        var commit = context.DBAnalysis_s.Where(r => r.Id == DBAnalysisId).FirstOrDefault();
        if(commit is null) return null!;
        return new DBAnalysisDTO(commit.Id, commit.LatestCommitId, commit.Author, commit.GitRepository);
    }

    //måske sortere på analysisId?
    public IReadOnlyCollection<DBAnalysisDTO> Read()
    {
        var commits = from c in context.DBAnalysis_s
                      orderby c.LatestCommitId
                      select new DBAnalysisDTO(c.Id, c.LatestCommitId, c.Author!, c.GitRepository);
        return commits.ToArray();
    }
}