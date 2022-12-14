namespace GitInsight.Entities;

public class DBFrequencyRepository : IDBFrequencyRepository
{
    readonly GitInsightContext context;

    public DBFrequencyRepository(GitInsightContext context)
    {
        this.context = context;
    }

    public (Response, int) Create(DBFrequencyCreateDTO frequency)
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

    public Response Update(DBFrequencyUpdateDTO frequency)
    {
        var entity = context.DBFrequencies.Where(r => r.Date == frequency.Date && r.DBAnalysisId == frequency.DBAnalysisId).FirstOrDefault();

        if (entity is null) return NotFound;
        
        entity.Frequency = frequency.Frequency;

        context.SaveChanges();

        return Updated;
    }

    public (Response, int?) UpdateOrCreate(DBFrequencyUpdateDTO frequency)
    {
        var response = Update(frequency);
        int? DBAnalysisId = null;
        
        if (response != Updated)
        {
            (response, DBAnalysisId) = Create(new DBFrequencyCreateDTO(frequency.DBAnalysisId, frequency.Date, frequency.Frequency));
        }

        return (response, DBAnalysisId);
    }

    public Response Delete(int Id, DateTime Date)
    {
        var entity = context.DBFrequencies.Where(r => r.DBAnalysisId == Id && r.Date == Date).FirstOrDefault();

        if (entity is null) return NotFound;

        context.DBFrequencies.Remove(entity);
        context.SaveChanges();
        return Deleted;
    }

    public IEnumerable<DBFrequencyDTO> FindAll(int analysisId)
    {
        return context.DBFrequencies.Where(c => c.DBAnalysisId == analysisId)
            .Select(c => new DBFrequencyDTO(c.DBAnalysisId, c.Date, c.Frequency));
    }
}