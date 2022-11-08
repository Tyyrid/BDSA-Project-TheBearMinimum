namespace GitInsight.Entities;

public class DBFrequencyRepository : IDBFrequencyRepository
{
    readonly GitInsightContext context;

    public DBFrequencyRepository(GitInsightContext context)
    {
        this.context = context;
    }

    public (Response Response, int frequencyId) Create(DBFrequencyCreateDTO frequency)
    {
        if(context.DBFrequencies.Where(f => f.DBCommitId.Equals(frequency.DBCommitId)).Any()) 
        {
            //tjek for sidste dato
            /*if() 
            {

            }*/
        }

            DBFrequency f = new DBFrequency();
            f.Frequency = frequency.Frequency;
            f.Date = frequency.Date;
            f.DBCommitId = frequency.DBCommitId;
        


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