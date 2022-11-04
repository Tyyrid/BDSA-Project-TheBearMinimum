namespace GitInsight.Core;

public interface IDBFrequencyRepository
{
    (Response Response, int frequencyId) Create(DBFrequencyCreateDTO frequency);
    DBFrequencyDTO Read(int frequencyId);
    IReadOnlyCollection<DBFrequencyDTO> Read();
    
}