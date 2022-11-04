namespace GitInsight.Entities;

public class DBFrequency
{
    //Date sammen med foreign key fra DBCommit giver en unik key
    public int Frequency { get; set; }
    public DateTime Date { get; set; }
    public int DBCommitId { get; set; }
    public DBCommit DBCommit { get; set; } = null!;
}