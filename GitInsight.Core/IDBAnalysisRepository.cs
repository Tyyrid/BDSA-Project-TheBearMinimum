namespace GitInsight.Core;

public interface IDBAnalysisRepository
{
    (Response response, int analysisID) Create(DBAnalysisCreateDTO commit);
    DBAnalysisDTO Find(string commitId, string gitRepository);
    IReadOnlyCollection<DBAnalysisDTO> Read();
    Response Update(DBAnalysisUpdateDTO analysis);
    Response Delete(int Id, bool force = false);
}