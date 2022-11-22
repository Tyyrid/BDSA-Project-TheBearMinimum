namespace GitInsight.Entities;

public class DBAnalysisRepository : IDBAnalysisRepository
{
    readonly GitInsightContext context;

    public DBAnalysisRepository(GitInsightContext context) {
        this.context = context;
    }


    public (Response, int) Create(DBAnalysisCreateDTO commit)
    {
        //If commidId exists in database do nothing and return last analyze
        if (context.DBAnalysis_s.Where(r => r.GitRepository == commit.GitRepository && r.LatestCommitId.Equals(commit.LatestCommitId) && r.Author == commit.Author).Any())
        {
            var existing = Find(commit.LatestCommitId, commit.GitRepository, commit.Author);
            return (Conflict, existing.Id);
        }
        //Create new DBCommit
        DBAnalysis c = new DBAnalysis(commit.LatestCommitId, commit.Author!, commit.GitRepository);
        //add to context and update database
        context.DBAnalyses.Add(c);
        context.SaveChanges();
        return (Created, c.Id);
    }

    public DBAnalysisDTO Find(string commitId, string gitRepository, string Author = "")
    {
        var commit = context.DBAnalysis_s.Where(r => r.GitRepository.Equals(gitRepository) && r.LatestCommitId.Equals(commitId) && r.Author.Equals(Author)).FirstOrDefault();
        if(commit is null) return null!;
        return new DBAnalysisDTO(commit.Id, commit.LatestCommitId, commit.Author, commit.GitRepository);
    }
    public DBAnalysisDTO Find(int DBAnalysisId)
    {
        var commit = context.DBAnalyses.Where(r => r.Id == DBAnalysisId).FirstOrDefault();
        if(commit is null) return null!;
        return new DBAnalysisDTO(commit.Id, commit.LatestCommitId, commit.Author, commit.GitRepository);
    }
    public IEnumerable<DBAnalysisDTO> FindAuthorAnalysis_s(string gitRepository)
    {
        var analysis_s = context.DBAnalysis_s.Where(c => c.GitRepository == gitRepository);
        foreach (var analysis in analysis_s)
        {
            if(analysis.Author == "") continue;
            yield return new DBAnalysisDTO(analysis.Id, analysis.LatestCommitId, analysis.Author, analysis.GitRepository);
        }

    }

    //måske sortere på analysisId?
    public IReadOnlyCollection<DBAnalysisDTO> Read()
    {
        var commits = from c in context.DBAnalyses
                      orderby c.LatestCommitId
                      select new DBAnalysisDTO(c.Id, c.LatestCommitId, c.Author!, c.GitRepository);
        return commits.ToArray();
    }

    public Response Update(DBAnalysisUpdateDTO analysis)
    {
        var entity = context.DBAnalysis_s.Where(r => r.Id == analysis.Id).FirstOrDefault();
        if (entity is null) return NotFound;
        else if ((entity.Author == "" || analysis.Author == "") && entity.Author != analysis.Author) return Conflict;
        else if (!context.DBAnalysis_s.Where(r => r.Id == analysis.Id && r.Author == analysis.Author).Any()) return BadRequest;

        entity.LatestCommitId = analysis.LatestCommitId;
        context.SaveChanges();
        return Updated;
    }

    public (Response, int?) UpdateOrCreate(DBAnalysisUpdateDTO analysis)
    {
        var response = Update(analysis);
        int? analysisID = null;

        if (response != Updated && !context.DBAnalysis_s.Where(r => r.Id == analysis.Id).Any())
        {
            (response, analysisID) = Create(new DBAnalysisCreateDTO(analysis.LatestCommitId, analysis.Author, analysis.GitRepository));
        }

        return (response, analysisID);
    }

    public Response Delete(int Id, bool force = false)
    {
        var entity = context.DBAnalysis_s.Where(a => a.Id == Id).FirstOrDefault();

        if (entity is null) return NotFound;

        if (force == false && context.DBFrequencies.Where(f => f.DBAnalysisId == Id).Any()) return Conflict;

        context.DBAnalysis_s.Remove(entity);
        context.SaveChanges();
        return Deleted;
    }

}