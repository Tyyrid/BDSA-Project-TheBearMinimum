using Xunit;
using GitInsight;
using FluentAssertions;

namespace GitInsight.Test;

public class UnitTest1
{
    IRepository repository;
    public UnitTest1(){
        repository = Substitute.For<IRepository>();
    }

    [Fact]
    public void Test1()
    {

    }
    
    [Fact]
    public void Does_substitute_work()
    {
        Commit commit = Substitute.For<Commit>();
        commit.Author.Name.Returns("Kristian");
        repository.Head.Tip.Returns(commit);

        var testresult = repository.Head.Tip.Author.Name;
        testresult.Should().Be("Kristian");
    }

    [Fact]
    public void frequencyMode()
    {
        // Given
        Commit commit1 = Substitute.For<Commit>();
        commit1.Author.When.Date.Returns(new DateTime(2022, 10, 28));
        Commit commit2 = Substitute.For<Commit>();
        commit2.Author.When.Date.Returns(new DateTime(2022, 10, 28));
        Commit commit3 = Substitute.For<Commit>();
        commit3.Author.When.Date.Returns(new DateTime(2022, 10, 28));
        Commit commit4 = Substitute.For<Commit>();
        commit4.Author.When.Date.Returns(new DateTime(2022, 10, 28));

        Commit commit5 = Substitute.For<Commit>();  
        commit5.Author.When.Date.Returns(new DateTime(2022, 10, 29));
        Commit commit6 = Substitute.For<Commit>();
        commit5.Author.When.Date.Returns(new DateTime(2022, 10, 29));

        

        var commits = new List<Commit>{commit1, commit2, commit3, commit4, commit5, commit6};

        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        // When
        Program.frequencyMode(commits);
        var output = writer.GetStringBuilder().ToString().TrimEnd();

        // Then
        output.Should().Be("2 2022-10-27\n4 2022-10-28");
    }

    [Fact]
    public void authorMode()
    {
        // Given
        Commit commit1 = Substitute.For<Commit>();
        commit1.Author.When.Date.Returns(new DateTime(2022, 10, 28));
        commit1.Author.Name.Returns("Kristian");
        Commit commit2 = Substitute.For<Commit>();
        commit2.Author.When.Date.Returns(new DateTime(2022, 10, 28));
        commit2.Author.Name.Returns("Kristian");
        Commit commit3 = Substitute.For<Commit>();
        commit3.Author.When.Date.Returns(new DateTime(2022, 10, 28));
        commit3.Author.Name.Returns("Marcus");
        Commit commit4 = Substitute.For<Commit>();
        commit4.Author.When.Date.Returns(new DateTime(2022, 10, 28));
        commit4.Author.Name.Returns("Marcus");

        Commit commit5 = Substitute.For<Commit>();
        commit5.Author.When.Date.Returns(new DateTime(2022, 10, 29));
        commit5.Author.Name.Returns("Kristian");
        Commit commit6 = Substitute.For<Commit>();
        commit6.Author.When.Date.Returns(new DateTime(2022, 10, 29));
        commit6.Author.Name.Returns("Marcus");

        var commits = new List<Commit>{commit1, commit2, commit3, commit4, commit5, commit6};

        using var writer = new StringWriter();
        Console.SetOut(writer);
        
        // When
        Program.frequencyMode(commits);
        var output = writer.GetStringBuilder().ToString().TrimEnd();

        // Then
        output.Should().Be("Kristian\n1 2022-10-27\n2 2022-10-28\nMarcus\n1 2022-10-27\n2 2022-10-28");
    }
}