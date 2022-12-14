namespace GitInsight.Test;
public class DBFrequencyRepositoryTest : IDisposable
{
    private readonly GitInsightContext context;
    private readonly DBFrequencyRepository repository; 
    public DBFrequencyRepositoryTest(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<GitInsightContext>();
        builder.UseSqlite(connection);

        var _context = new GitInsightContext(builder.Options);
        _context.Database.EnsureCreated();
        
        var analysis1 = new DBAnalysis("2", "Kristian", "userName/repositoryName");
        var analysis2 = new DBAnalysis("5", "Jonas", "userName/repositoryName");
                         
        var frequency1 = new DBFrequency(1, parseStringToDateTime("5/1/2020 8:30:52 AM"), 5);
        var frequency2 = new DBFrequency(2, parseStringToDateTime("10/22/2022 5:33:40 PM"), 3);
        _context.DBAnalysis_s.AddRange(analysis1, analysis2);
        _context.DBFrequencies.AddRange(frequency1, frequency2);

        _context.SaveChanges();

        context = _context;
        repository = new DBFrequencyRepository(context);
    }



    [Fact]
    public void Create_Frequency_returns_conflict_and_analysisId()
    {

        var (response, id) = repository.Create(
                            new DBFrequencyCreateDTO(1, parseStringToDateTime("5/1/2020 8:30:52 AM"), 5));
        
        response.Should().Be(Response.Conflict);
        id.Should().Be(1);
    }

    [Fact]
    public void Create_Frequency_returns_created_and_analysisId()
    {
        var (response, id) = repository.Create(
                            new DBFrequencyCreateDTO(2, parseStringToDateTime("5/16/2021 8:30:52 AM"), 2));
        response.Should().Be(Response.Created);
        id.Should().Be(2);
    }

      [Fact]
    public void Find_frequency()
    {
        var frequency = repository.Find(1, parseStringToDateTime("5/1/2020 8:30:52 AM"));

        frequency.Should().Be(new DBFrequencyDTO(1, parseStringToDateTime("5/1/2020 8:30:52 AM"), 5));
    }

    [Fact]
    public void Read_all_frequencies()
    {
        var result = repository.Read();
        var frequencies = result.ToArray();
        frequencies[0].Should().Be(new DBFrequencyDTO(1, parseStringToDateTime("5/1/2020 8:30:52 AM"), 5));
        frequencies[1].Should().Be(new DBFrequencyDTO(2, parseStringToDateTime("10/22/2022 5:33:40 PM"), 3));
    }

    [Fact]
    public void Update_given_correct_DTO_updates_and_returns_Updated()
    {
        // Arrange
        var newFrequency = 10;
        var updateDTO = new DBFrequencyUpdateDTO(1, parseStringToDateTime("5/1/2020 8:30:52 AM"), newFrequency);

        // Act
        var response = repository.Update(updateDTO);
        var entity = repository.Find(1, parseStringToDateTime("5/1/2020 8:30:52 AM"));

        // Assert
        response.Should().Be(Updated);
        entity.Frequency.Should().Be(newFrequency);
    }

    [Fact]
    public void Update_given_NonExisting_DTO_returns_NotFound()
    {
        // Arrange
        var updateDTO = new DBFrequencyUpdateDTO(42, DateTime.Now, 3);

        // Act
        var response = repository.Update(updateDTO);

        // Assert
        response.Should().Be(NotFound);
    }

    [Fact]
    public void UpdateOrCreate_given_existing_DTO_updates_and_returns_updated_null()
    {
        // Arrange
        var newFrequency = 10;
        var updateDTO = new DBFrequencyUpdateDTO(1, parseStringToDateTime("5/1/2020 8:30:52 AM"), newFrequency);

        // Act
        (var response, int? id) = repository.UpdateOrCreate(updateDTO);
        var entity = repository.Find(1, parseStringToDateTime("5/1/2020 8:30:52 AM"));

        // Assert
        id.Should().BeNull();
        response.Should().Be(Updated);
        entity.Frequency.Should().Be(newFrequency);
    }

    [Fact]
    public void UpdateOrCreate_given_NonExisting_DTO_creates_returns_Created_ID()
    {
        // Arrange
        var DBAnalysisId = 1;
        var dateTime = DateTime.Now;
        var frequency = 10;
        var updateDTO = new DBFrequencyUpdateDTO(DBAnalysisId, dateTime, frequency);

        // Act
        (var response, int? id) = repository.UpdateOrCreate(updateDTO);
        var entity = repository.Find(DBAnalysisId, dateTime);

        // Assert
        id.Should().NotBeNull();
        entity.Should().NotBeNull();

        response.Should().Be(Created);
        id.Should().Be(DBAnalysisId);

        entity.DBAnalysisId.Should().Be(DBAnalysisId);
        entity.Date.Should().Be(dateTime);
        entity.Frequency.Should().Be(frequency);
    }

    [Fact]
    public void Delete_given_existing_id_deletes_returns_deleted()
    {
        // Arrange
        var id = 1;
        var dateTime = parseStringToDateTime("5/1/2020 8:30:52 AM");

        // Act
        var response = repository.Delete(id, dateTime);
        var entity = repository.Find(id, dateTime);

        // Assert
        response.Should().Be(Deleted);
        entity.Should().BeNull();
    }

    [Fact]
    public void Delete_given_nonExisting_id_returns_NotFound()
    {
        // Arrange
        var id = 42;
        var dateTime = DateTime.Now;

        // Act
        var response = repository.Delete(id, dateTime);

        // Assert
        response.Should().Be(NotFound);
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