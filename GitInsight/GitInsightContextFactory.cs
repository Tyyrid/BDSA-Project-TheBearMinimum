using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GitInsight;

public class GitInsightContextFactory : IDesignTimeDbContextFactory<GitInsightContext>
{
    public GitInsightContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        //using var connection = new SqlConnection(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<GitInsightContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var options = optionsBuilder.Options;

        return new GitInsightContext(options);

    }
}