using GitInsight.Entities;

public interface IDBService
{
    public IEnumerable<(string, IEnumerable<DBFrequencyDTO>)> GetAuthorAnalysis();
    public IEnumerable<DBFrequencyDTO> GetFrequencyAnalysis();
}