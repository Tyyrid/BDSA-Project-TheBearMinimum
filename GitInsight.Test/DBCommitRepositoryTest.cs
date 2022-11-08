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
        

        context = _context;
        repository = new DBCommitRepository(context);
    }

    [Fact]
    public void Create_given_Commit_returns_Created_Commit_Id()
    {
        var (response, created) = repository.Create(new DBCommitCreateDTO(1, "Kristian", "userName/repositoryName"));
        
        response.Should().Be(Created);

        created.Should().Be(1);
    }

    

    public void Dispose()
    {
        context.Dispose();
    }
}