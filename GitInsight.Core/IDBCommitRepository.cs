namespace GitInsight.Core;

public interface IDBCommitRepository
{
    (Response Response, int commitId) Create(DBCommitCreateDTO commit);
    DBCommitDTO Find(int commitId);
    IReadOnlyCollection<DBCommitDTO> Read();
    
}