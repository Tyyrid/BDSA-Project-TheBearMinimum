namespace GitInsight.Entities;

public class DBCommit
{
    public int Id { get; set; }
    //HEAD
    public int CommitID { get; set; }
    //hvis author er null, er det fordi at analyzen var i frequency mode
    public string? Author { get; set; }
    public string GitRepository { get; set; } = null!;
    public virtual ICollection<DBFrequency> Frequencies { get; set; }
}