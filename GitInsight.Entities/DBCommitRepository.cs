namespace GitInsight.Entities;

public class DBCommitRepository : IDBCommitRepository
{
    readonly GitInsightContext context;

    public DBCommitRepository(GitInsightContext context) {
        this.context = context;
    }


    public (Response response, int latestCommitId) Create(DBCommitCreateDTO commit)
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

    public DBCommitDTO Find(int commitId, string gitRepository)
    {
        var commit = context.DBAnalysis_s.Where(r => r.GitRepository.Equals(gitRepository) && r.LatestCommitId.Equals(commitId)).FirstOrDefault();
        if(commit is null) return null!;
        //if(commit.Author is null) commit.Author = "";
        return new DBCommitDTO(commit.Id, commit.LatestCommitId, commit.Author, commit.GitRepository);
    }

    public IReadOnlyCollection<DBCommitDTO> Read()
    {
        var commits = from c in context.DBAnalysis_s
                      orderby c.LatestCommitId
                      select new DBCommitDTO(c.Id, c.LatestCommitId, c.Author!, c.GitRepository);
        return commits.ToArray();
    }
}