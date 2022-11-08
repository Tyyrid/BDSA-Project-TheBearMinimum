namespace GitInsight.Entities;

public class DBFrequencyRepository : IDBFrequencyRepository
{
    public (Response Response, int frequencyId) Create(DBFrequencyCreateDTO frequency)
    {
        throw new NotImplementedException();
    }

    public DBFrequencyDTO Find(int frequencyId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<DBFrequencyDTO> Read()
    {
        throw new NotImplementedException();
    }
}