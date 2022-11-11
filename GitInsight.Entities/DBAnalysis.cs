namespace GitInsight.Entities;

public class DBAnalysis
{
    public int Id { get; set; }
    //HEAD
    public string LatestCommitId { get; set; }
    //hvis author er "", er det fordi at analyzen var i frequency mode
    public string Author { get; set; } = null!;
    public string GitRepository { get; set; } = null!;
    public virtual ICollection<DBFrequency> Frequencies { get; set; } = new List<DBFrequency>();

    public DBAnalysis(string LatestCommitId, string Author, string GitRepository){  
        this.LatestCommitId = LatestCommitId;
        this.Author = Author;
        this.GitRepository = GitRepository;
    }

}