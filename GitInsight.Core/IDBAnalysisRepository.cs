namespace GitInsight.Core;

public interface IDBAnalysisRepository
{
    (Response response, int latestCommitId) Create(DBAnalysisCreateDTO commit);
    DBAnalysisDTO Find(int commitId, string gitRepository);
    IReadOnlyCollection<DBAnalysisDTO> Read();
    
}