using System.Globalization;
using System.Reflection;

namespace GitInsight.Test;

public class GitInsightTest
{
    private readonly IEnumerable<Commit> _commits;
    private readonly Assembly _program;

    public GitInsightTest()
    {
        _commits = MockCommits(new[] {
            ("Hannah", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Hannah", "2022-10-28"),
            ("Hannah", "2022-10-27"),
            ("Jonas", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Jonas", "2022-10-28"),
            ("Jonas", "2022-10-29"),
            ("Jonas", "2022-10-29"),
            ("Marcus", "2022-10-28"),
            ("Marcus", "2022-10-28"),
            ("Marcus", "2022-10-31"),
            ("Marcus", "2022-10-29"),
        });

        _program = Assembly.Load(nameof(GitInsight));
    }

    private IEnumerable<Commit> MockCommits(IEnumerable<(string Author, string When)> mocks)
    {
        foreach (var commit in mocks)
        {
            var mockCommit = Substitute.For<Commit>();
            mockCommit.Author.Returns(new Signature(commit.Author, "@", new DateTimeOffset(DateTime.Parse(commit.When, CultureInfo.InvariantCulture))));
            yield return mockCommit;
        }
    }

    [Fact]
    public void Program_DisplaysError_WhenDirectoryIsMissing()
    {
        // Given
        using var writer = new StringWriter();
        Console.SetError(writer);

        // When
        _program.EntryPoint?.Invoke(null, new[] { new[] { "--repo", "missing/directory" } });
        var output = writer.GetStringBuilder().ToString().TrimEnd();

        // Then
        output.Should().Contain("Directory does not exist");
    }

    [Fact]
    public void Program_DisplaysError_WhenDirectoryIsNotGit()
    {
        // Given
        using var writer = new StringWriter();
        Console.SetError(writer);

        // When
        // Only works when '..' is not a git directory
        _program.EntryPoint?.Invoke(null, new[] { new[] { "--repo", ".." } });
        var output = writer.GetStringBuilder().ToString().TrimEnd();

        // Then
        output.Should().Be("Path '..' doesn't point at a valid Git repository or workdir.");
    }

    [Fact]
    public void AuthorMode()
    {
        // When
        var freq = _commits.AuthorFrequency();

        // Then
        freq.Should().BeEquivalentTo(new[] {
            ("Hannah", new[] {
                (2, DateTime.Parse("2022-10-28")),
                (1, DateTime.Parse("2022-10-27")),
             }),
            ("Jonas", new[] {
                (2, DateTime.Parse("2022-10-29")),
                (5, DateTime.Parse("2022-10-28")),
             }),
            ("Marcus", new[] {
                (1, DateTime.Parse("2022-10-31")),
                (1, DateTime.Parse("2022-10-29")),
                (2, DateTime.Parse("2022-10-28")),
             }),
        });
    }

    [Fact]
    public void FrequencyMode()
    {
        // When
        var freq = _commits.Frequency();

        // Then
        freq.Should().BeEquivalentTo(new[] {
            (1, DateTime.Parse("2022-10-31")),
            (3, DateTime.Parse("2022-10-29")),
            (9, DateTime.Parse("2022-10-28")),
            (1, DateTime.Parse("2022-10-27")),
        });
    }
}
