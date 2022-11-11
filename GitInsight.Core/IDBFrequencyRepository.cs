namespace GitInsight.Core;

public interface IDBFrequencyRepository
{
    (Response response, int analysisId) Create(DBFrequencyCreateDTO frequency);
    DBFrequencyDTO Find(int analysisId, DateTime Date);
    IEnumerable<DBFrequencyDTO> FindAll(int analysisId);
    IReadOnlyCollection<DBFrequencyDTO> Read();
    Response Update(DBFrequencyUpdateDTO frequency);
    Response Delete(int AnalysisId, DateTime Date, bool force = false);
}