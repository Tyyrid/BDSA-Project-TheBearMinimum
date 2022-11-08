namespace GitInsight.Entities;

public class DBAnalysis
{
    public int Id { get; set; }
    //HEAD
    public int LatestCommitId { get; set; }
    //hvis author er "", er det fordi at analyzen var i frequency mode
    public string Author { get; set; } = null!;
    public string GitRepository { get; set; } = null!;
    public virtual ICollection<DBFrequency> Frequencies { get; set; } = new List<DBFrequency>();


    public DBAnalysis(){}
    public DBAnalysis(int _latestCommitID, string _Author, string _GitRepository){  
        LatestCommitId = _latestCommitID;
        Author = _Author;
        GitRepository = _GitRepository;
    }

}