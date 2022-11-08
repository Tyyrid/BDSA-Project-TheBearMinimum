namespace GitInsight.Test;
public class DBCommitRepositoryTest : IDisposable
{
    private readonly GitInsightContext context;
    private readonly DBCommitRepository repository;
    public DBCommitRepositoryTest(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);

        var _context = new GitInsightContext(builder.Options);
        _context.Database.EnsureCreated();
        
        var commit1 = new DBCommit(2, "Kristian", "userName/repositoryName");
        var commit2 = new DBCommit(5, "Jonas", "userName/repositoryName");
        _context.DBCommits.AddRange(commit1, commit2);

        _context.SaveChanges();

        context = _context;
        repository = new DBCommitRepository(context);
    }

    [Fact]
    public void Create_given_Commit_returns_Created_Commit_Id()
    {
        var (response, created) = repository.Create(new DBCommitCreateDTO(3, "Kristian", "userName/repositoryName"));
        
        response.Should().Be(Created);

        created.Should().Be(3);
    }

    [Fact]
    public void Create_given_existing_Commit_returns_Confilt_and_Commit_Id()
    {
        var (response, created) = repository.Create(new DBCommitCreateDTO(2, "Kristian", "userName/repositoryName"));
        
        response.Should().Be(Response.Conflict);

        created.Should().Be(2);
    }

    [Fact]
    public void Find_commit_1()
    {
        var commit = repository.Find(1);

        commit.Should().Be(new DBCommitDTO(1, 2, "Kristian", "userName/repositoryName"));
    }

    [Fact]
    public void Read_all_commits()
    {
        var result = repository.Read();
        var commits = result.ToArray(); 
        commits[0].Should().Be(new DBCommitDTO(1, 2, "Kristian", "userName/repositoryName"));
        commits[1].Should().Be(new DBCommitDTO(2, 5, "Jonas", "userName/repositoryName"));
    }

    

    public void Dispose()
    {
        context.Dispose();
    }
}