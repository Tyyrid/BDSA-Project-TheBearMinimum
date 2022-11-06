namespace GitInsight.Entities;

public class DBCommitRepository : IDBCommitRepository
{
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