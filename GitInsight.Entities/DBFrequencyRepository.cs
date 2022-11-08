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

    public DBFrequencyDTO Find(int frequencyId)
    {

        throw new NotImplementedException();
    }

    public IReadOnlyCollection<DBFrequencyDTO> Read()
    {
        throw new NotImplementedException();
    }
}