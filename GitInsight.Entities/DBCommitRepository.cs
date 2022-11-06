namespace GitInsight.Entities;

public class DBCommitRepository : IDBCommitRepository
{
    readonly GitInsightContext context;

    public DBCommitRepository(GitInsightContext context) {
        this.context = context;
    }


    public (Response Response, int commitId) Create(DBCommitCreateDTO commit)
    {
        throw new NotImplementedException();
    }

    public DBCommitDTO Read(int commitId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<DBCommitDTO> Read()
    {
        throw new NotImplementedException();
    }
}