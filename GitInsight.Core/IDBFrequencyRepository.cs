namespace GitInsight.Core;

public interface IDBFrequencyRepository
{
    (Response response, int analysisId) Create(DBFrequencyCreateDTO frequency);
    DBFrequencyDTO Find(int analysisId, DateTime Date);
    IReadOnlyCollection<DBFrequencyDTO> Read();
    
}