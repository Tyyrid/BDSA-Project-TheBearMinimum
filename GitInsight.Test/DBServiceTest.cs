using System.Collections;

namespace GitInsight.Test;

public class DBServiceTest : IDisposable
{
    private readonly IEnumerable<Commit> _commits;
    private readonly Assembly _program;
    private readonly GitInsightContext context;
    private readonly DBAnalysisRepository Analysisrepository;
    private readonly DBFrequencyRepository frequencyRepository;
    private readonly IRepository gitrepo;
    private readonly DBService dBService;
    public DBServiceTest(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);

        var _context = new GitInsightContext(builder.Options);
        _context.Database.EnsureCreated();
        
        var analysis1 = new DBAnalysis("2", "Kristian", "userName/repositoryName");
        var analysis2 = new DBAnalysis("5", "Jonas", "userName/repositoryName");
        var analysis3 = new DBAnalysis("4", "", "userName/repositoryName");
                         
        var frequency1 = new DBFrequency(1, DBAnalysisRepositoryTest.parseStringToDateTime("5/1/2020 8:30:52 AM"), 5);
        var frequency2 = new DBFrequency(2, DBAnalysisRepositoryTest.parseStringToDateTime("10/22/2022 5:33:40 PM"), 3);
        _context.DBAnalysis_s.AddRange(analysis1, analysis2, analysis3);
        _context.DBFrequencies.AddRange(frequency1, frequency2);

        _context.SaveChanges();

        context = _context;
        Analysisrepository = new DBAnalysisRepository(context);
        frequencyRepository = new DBFrequencyRepository(context);
         _commits = MockCommits(new[] {
            ("Hannah", "2022-10-27"),
            ("Hannah", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Hannah", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Marcus", "2022-10-28"),
            ("Marcus", "2022-10-28"),
            ("Jonas", "2022-10-29"),
            ("Jonas", "2022-10-29"),
            ("Marcus", "2022-10-29"),
            ("Marcus", "2022-10-31"),
        });

        _program = Assembly.Load(nameof(GitInsight));
        var gitrepo = Substitute.For<IRepository>();
        gitrepo.Commits.Returns(_commits);
        this.gitrepo = gitrepo;
        var repoPath = "repo/path";
        var dbService = Substitute.For<DBService>();
        dbService.getRepositories(repoPath).Returns((repoPath, gitrepo, Analysisrepository, frequencyRepository));
        this.dBService = dbService;
    }
    
    private MockCommitLog MockCommits(IEnumerable<(string Author, string When)> mocks)
    {
        var commitlog = new MockCommitLog();
        foreach (var commit in mocks)
        {
            var mockCommit = Substitute.For<Commit>();
            mockCommit.Author.Returns(new Signature(commit.Author, "@", new DateTimeOffset(DateTime.Parse(commit.When, CultureInfo.InvariantCulture))));
            //var mockID = Substitute.For<ObjectId>();
            //mockID.ToString().Returns(new ObjectId(new byte[]{0,1}));
            mockCommit.Id.Returns(new ObjectId(new byte[]{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19}));
            commitlog.add(mockCommit);
        } 
        return commitlog;
    }

    [Fact]
    public void FrequencyMode()
    {
       var something = dBService.GetFrequencyAnalysis();
       something.Should().BeEquivalentTo(new DBFrequencyDTO[]{new DBFrequencyDTO(4, new DateTime(2022, 10, 27), 1), 
                new DBFrequencyDTO(4, new DateTime(2022, 10, 28), 9),
                new DBFrequencyDTO(4, new DateTime(2022, 10, 29), 3),
                new DBFrequencyDTO(4, new DateTime(2022, 10, 31), 1)}); 
    }

    [Fact]
    public void TestName()
    {
        // Given
    
        // When
    
        // Then
        true.Should().Be(true);
    }

    public void Dispose()
    {
         context.Dispose();
    }
}

public class MockCommitLog : IQueryableCommitLog
{
    public CommitSortStrategies SortedBy => throw new NotImplementedException();
    private IList<Commit> list;
    public MockCommitLog(){
        list = new List<Commit>();
    }

    public IEnumerator<Commit> GetEnumerator()
    {
        foreach(Commit item in list){
            yield return item;
        }
    }

    public ICommitLog QueryBy(CommitFilter filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<LogEntry> QueryBy(string path)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<LogEntry> QueryBy(string path, CommitFilter filter)
    {
        throw new NotImplementedException();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }

    public void add(Commit commit){
        list.Add(commit);
    }
    public Commit First(){
        return list[0];
    }
}