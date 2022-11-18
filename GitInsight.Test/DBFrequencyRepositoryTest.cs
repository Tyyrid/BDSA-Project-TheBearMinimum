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
    
    public DateTime parseStringToDateTime(string date)
    {
        return DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}