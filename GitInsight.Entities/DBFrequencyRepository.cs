namespace GitInsight.Entities;

public class DBFrequencyRepository : IDBFrequencyRepository
{
    readonly GitInsightContext context;

    public DBFrequencyRepository(GitInsightContext context)
    {
        this.context = context;
    }

    public (Response response, int analysisId) Create(DBFrequencyCreateDTO frequency)
    {
        if (context.DBFrequencies.Where(f => f.DBAnalysisId.Equals(frequency.DBAnalysisId)
            && f.Date.Equals(frequency.Date)).Any())
        {

            return (Conflict, frequency.DBAnalysisId);
        }

        DBFrequency f = new DBFrequency(frequency.DBAnalysisId, frequency.Date, frequency.Frequency);
        context.DBFrequencies.Add(f);
        context.SaveChanges();
        return(Created, f.DBAnalysisId);
    }

    public DBFrequencyDTO Find(int analysisId, DateTime Date)
    {
        var existing = context.DBFrequencies.Where(f => f.DBAnalysisId.Equals(analysisId)
                        && f.Date.Equals(Date)).FirstOrDefault();

        if (existing is null) return null!;
        return new DBFrequencyDTO(existing.DBAnalysisId, existing.Date, existing.Frequency);
    }

    public IReadOnlyCollection<DBFrequencyDTO> Read()
    {
        var allFrequencies = context.DBFrequencies.OrderBy(f => f.Date)
                                .Select(fre => new DBFrequencyDTO(
                                        fre.DBAnalysisId,
                                        fre.Date,
                                        fre.Frequency)).ToList();
        return allFrequencies.AsReadOnly();
    }
}