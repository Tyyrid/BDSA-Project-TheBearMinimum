namespace GitInsight.Test;
public class DBAnalysisRepositoryTest : IDisposable
{
    private readonly GitInsightContext context;
    private readonly DBAnalysisRepository repository;
    public DBAnalysisRepositoryTest(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);

        var _context = new GitInsightContext(builder.Options);
        _context.Database.EnsureCreated();
        
        var analysis1 = new DBAnalysis("2", "Kristian", "userName/repositoryName");
        var analysis2 = new DBAnalysis("5", "Jonas", "userName/repositoryName");
        var analysis3 = new DBAnalysis("4", "", "userName/repositoryName");
                         
        var frequency1 = new DBFrequency(1, parseStringToDateTime("5/1/2020 8:30:52 AM"), 5);
        var frequency2 = new DBFrequency(2, parseStringToDateTime("10/22/2022 5:33:40 PM"), 3);
        _context.DBAnalysis_s.AddRange(analysis1, analysis2);
        _context.DBFrequencies.AddRange(frequency1, frequency2);

        _context.SaveChanges();

        context = _context;
        repository = new DBAnalysisRepository(context);
    }

    [Fact]
    public void Create_given_Analysis_returns_Created_AnalysisId()
    {
        var (response, created) = repository.Create(new DBAnalysisCreateDTO("4", "Kristian", "userName/repositoryName"));
        
        response.Should().Be(Created);

        created.Should().Be(4);
    }

    [Fact]
    public void Create_given_existing_Analysis_returns_Conflict_and_AnalysisId()
    {
        var (response, created) = repository.Create(new DBAnalysisCreateDTO("2", "Kristian", "userName/repositoryName"));
        
        response.Should().Be(Response.Conflict);

        created.Should().Be(1);
    }

    [Fact]
    public void Find_analysis_with_commitId_2()
    {
        var commit = repository.Find("2", "userName/repositoryName");

        commit.Should().Be(new DBAnalysisDTO(1, "2", "Kristian", "userName/repositoryName"));
    }

    [Fact]
    public void Find_analysis_with_commitId_3_fails()
    {
        var commit = repository.Find("3", "userName/repositoryName");

        commit.Should().Be(null);
    }

    [Fact]
    public void Find_analysis_1_with_AnalysisID()
    {
        var analysis = repository.Find(1);

        analysis.Should().Be(new DBAnalysisDTO(1, "2", "Kristian", "userName/repositoryName"));
    }
    [Fact]
    public void Find_Analysis_5_with_AnalysisID_fails()
    {
        var analysis = repository.Find(5);

        analysis.Should().Be(null);
    }

    [Fact]
    public void Find_Analysis_s_with_same_repo()
    {
        var analysis_s = repository.FindAuthorAnalysis_s("userName/repositoryName");

        var analysis = analysis_s.ToArray(); 
        analysis[0].Should().Be(new DBAnalysisDTO(1, "2", "Kristian", "userName/repositoryName"));
        analysis[1].Should().Be(new DBAnalysisDTO(2, "5", "Jonas", "userName/repositoryName"));
    }

    [Fact]
    public void Find_Analysis_s_with_same_repo_returns_empty()
    {
        var analysis_s = repository.FindAuthorAnalysis_s("userName/repo");

        var analysis = analysis_s.ToArray();
        analysis.Length.Should().Be(0);
    }

    [Fact]
    public void Read_all_Analysis()
    {
        var result = repository.Read();
        var analysis = result.ToArray(); 
        analysis[0].Should().Be(new DBAnalysisDTO(1, "2", "Kristian", "userName/repositoryName"));
        analysis[1].Should().Be(new DBAnalysisDTO(2, "5", "Jonas", "userName/repositoryName"));
    }

    [Fact]
    public void Update_given_nonExisting_Analysis_returns_NotFound()
    {
        // Arrange
        var updateDTO = new DBAnalysisUpdateDTO(42, "Foo", "Bar", "LoremIpsum");

        // Act
        var response = repository.Update(updateDTO);

        // Assert
        response.Should().Be(NotFound);
    }

    [Fact]
    public void Update_given_wrong_analysisType_returns_Conflict()
    {
        // Analysis saved in database is in frequency mode, but this tries with author specified
        // Arrange
        var updateDTO = new DBAnalysisUpdateDTO(repository.Find("4", "userName/repositoryName", "").Id, "4", "Gert", "userName/repositoryName");

        // Act
        var response = repository.Update(updateDTO);

        // Assert
        response.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Update_given_new_author_returns_BadRequest()
    {
        // Analysis saved in database is in author mode, and this tries with a new author not previously in database
        // Arrange
        var updateDTO = new DBAnalysisUpdateDTO(repository.Find("2", "userName/repositoryName", "Kristian").Id, "2", "Gert", "userName/repositoryName");

        // Act
        var response = repository.Update(updateDTO);

        // Assert
        response.Should().Be(BadRequest);
    }

    [Fact]
    public void Update_updates_LatestCommitId_and_returns_Updated()
    {
        // Arrange
        var updateDTO = new DBAnalysisUpdateDTO(repository.Find("4", "userName/repositoryName", "").Id, "42", "", "userName/repositoryName");

        // Act
        var response = repository.Update(updateDTO);

        // Assert
        response.Should().Be(Updated);
    }

    [Fact]
    public void UpdateOrCreate_given_nonExisting_Analysis_returns_Created_analysisId()
    {
        // Arrange
        int analysisId = 42;
        var updateDTO = new DBAnalysisUpdateDTO(analysisId, "Foo", "Bar", "LoremIpsum");

        // Act
        (var response, var id) = repository.UpdateOrCreate(updateDTO);

        // Assert
        response.Should().Be(Created);
        id.Should().Be(analysisId);
    }

    [Fact]
    public void UpdateOrCreate_given_existing_analysis_returns_Updated_null()
    {
        // Arrange
        var updateDTO = new DBAnalysisUpdateDTO(repository.Find("4", "userName/repositoryName", "").Id, "42", "", "userName/repositoryName");

        // Act
        (var response, var id) = repository.UpdateOrCreate(updateDTO);

        // Assert
        response.Should().Be(Updated);
        id.Should().BeNull();
    }

    public DateTime parseStringToDateTime(string date)
    {
        return DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture);
    }
    

    public void Dispose()
    {
        context.Dispose();
    }
}