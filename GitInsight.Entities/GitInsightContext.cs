using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GitInsight.Entities;

public class GitInsightContext : DbContext
{

    public DbSet<DBCommit> DBCommits => Set<DBCommit>();
    public GitInsightContext(DbContextOptions<GitInsightContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DBCommit>().HasMany(c => c.Frequencies).WithOne();

        modelBuilder.Entity<DBFrequency>().HasKey(f => new { f.DBCommitId, f.Date });
        modelBuilder.Entity<DBFrequency>().HasOne(f => f.DBCommit).WithMany(c => c.Frequencies);
        

    }
}
