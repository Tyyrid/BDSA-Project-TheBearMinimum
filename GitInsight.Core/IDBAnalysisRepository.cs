namespace GitInsight.Core;

public interface IDBAnalysisRepository
{
    (Response, int) Create(DBAnalysisCreateDTO commit);
    DBAnalysisDTO Find(string commitId, string gitRepository, string Author = "");
    DBAnalysisDTO Find(int DBAnalysisId);
    IEnumerable<DBAnalysisDTO> FindAuthorAnalysis_s(string gitRepository);
    IReadOnlyCollection<DBAnalysisDTO> Read();
    Response Update(DBAnalysisUpdateDTO analysis);
    (Response, int?) UpdateOrCreate(DBAnalysisUpdateDTO analysis);
    Response Delete(int Id, bool force = false);
}