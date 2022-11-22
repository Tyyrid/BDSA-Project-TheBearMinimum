using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GitInsight.Entities;

public class GitInsightContext : DbContext
{
    public DbSet<DBAnalysis> DBAnalysis_s => Set<DBAnalysis>();
    public DbSet<DBFrequency> DBFrequencies => Set<DBFrequency>();
    
    public GitInsightContext(DbContextOptions<GitInsightContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DBAnalysis>().HasMany(c => c.Frequencies).WithOne();

        modelBuilder.Entity<DBFrequency>().HasKey(f => new { f.DBAnalysisId, f.Date });
        modelBuilder.Entity<DBFrequency>().HasOne(f => f.DBAnalysis).WithMany(c => c.Frequencies);
        

    }
}
