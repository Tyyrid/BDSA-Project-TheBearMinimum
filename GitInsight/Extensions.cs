namespace GitInsight;

public static class Extenstions
{
    public static IEnumerable<(string Author, IEnumerable<(int, DateTime)> Frequency)> AuthorFrequency(this IEnumerable<Commit> commits)
        => commits.GroupBy(c => c.Author.Name).OrderBy(g => g.Key).Select(g => (g.Key, Frequency(g)));

    public static IEnumerable<(int Count, DateTime When)> Frequency(this IEnumerable<Commit> commits)
        => commits.GroupBy(c => c.Author.When.Date).Select(g => (g.Count(), g.Key));

    public static void Print(this IEnumerable<(int Count, DateTime When)> frequency)
        => frequency.OrderByDescending(f => f.When).ToList().ForEach(f => Console.WriteLine($"\t{f.Count} {f.When:yyyy-MM-dd}"));
}
