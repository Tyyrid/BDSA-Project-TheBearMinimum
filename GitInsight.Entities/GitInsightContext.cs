using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GitInsight.Entities;

public class GitInsightContext : DbContext
{
    public DbSet<DBCommit> DBCommit { get; set; } = null!;

    public string DbPath { get; }

    public GitInsightContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DBCommit>().HasMany(c => c.Frequencies).WithOne();

        modelBuilder.Entity<DBFrequency>().HasKey(f => new { f.DBCommitId, f.Date });
        modelBuilder.Entity<DBFrequency>().HasOne(f => f.DBCommit).WithMany(c => c.Frequencies);

    }
}
