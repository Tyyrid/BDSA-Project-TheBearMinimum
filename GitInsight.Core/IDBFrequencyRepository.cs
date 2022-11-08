namespace GitInsight.Core;

public interface IDBFrequencyRepository
{
    (Response Response, int frequencyId) Create(DBFrequencyCreateDTO frequency);
    DBFrequencyDTO Find(int frequencyId);
    IReadOnlyCollection<DBFrequencyDTO> Read();
    
}