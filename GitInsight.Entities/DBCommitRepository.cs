namespace GitInsight.Entities;

public class DBCommitRepository : IDBCommitRepository
{
    readonly GitInsightContext context;

    public DBCommitRepository(GitInsightContext context) {
        this.context = context;
    }


    public (Response Response, int commitId) Create(DBCommitCreateDTO commit)
    {
        //If commidId exists in database do nothing and return last analyze
        if (context.DBCommits.Where(c => c.CommitId.Equals(commit.CommitId)).Any())
        {
            if(commit.Author != null && context.DBCommits.Where(a => a.Author!.Equals(commit.Author)).Any()) {
                return (Response.Conflict, commit.CommitId);
            }
        }
        //Create new DBCommit
        DBCommit c = new DBCommit();
        c.CommitId = commit.CommitId;
        c.Author = commit.Author;
        c.GitRepository = commit.GitRepository;
        //add to context and update database
        context.DBCommits.Add(c);
        context.SaveChanges();
        return (Response.Created, c.CommitId);
    }

    public DBCommitDTO Find(int commitId)
    {
        var commit = context.DBCommits.Find(commitId);
        if(commit is null) return null!;
        return new DBCommitDTO(commit.Id, commit.CommitId, commit.Author!, commit.GitRepository);
    }

    public IReadOnlyCollection<DBCommitDTO> Read()
    {
        var commits = from c in context.DBCommits
                      orderby c.CommitId
                      select new DBCommitDTO(c.Id, c.CommitId, c.Author!, c.GitRepository);
        return commits.ToArray();
    }
}