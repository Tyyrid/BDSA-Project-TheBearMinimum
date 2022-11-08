namespace GitInsight.Core;

public interface IDBCommitRepository
{
    (Response Response, int commitId) Create(DBCommitCreateDTO commit);
    DBCommitDTO Read(int commitId);
    IReadOnlyCollection<DBCommitDTO> ReadAll();
    
}