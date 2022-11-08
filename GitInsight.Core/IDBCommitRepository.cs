namespace GitInsight.Core;

public interface IDBCommitRepository
{
    (Response response, int latestCommitId) Create(DBCommitCreateDTO commit);
    DBCommitDTO Find(int commitId, string gitRepository);
    IReadOnlyCollection<DBCommitDTO> Read();
    
}