namespace GitInsight.Entities;

public class DBFrequency
{
    //Date sammen med foreign key fra DBCommit giver en unik key

    //skal referere til primary key hos DBAnalys
    public int DBAnalysisId { get; set; }
    public DateTime Date { get; set; }
    public int Frequency { get; set; }
    public DBAnalysis DBAnalysis { get; set; } = null!;

    public DBFrequency(int DBAnalysisId, DateTime Date, int Frequency)
    {
        this.DBAnalysisId = DBAnalysisId;
        this.Date = Date;
        this.Frequency = Frequency;
    }
}