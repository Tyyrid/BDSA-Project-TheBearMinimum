namespace GitInsight.Core;

public record DBAnalysisDTO(int Id, int LatestCommitId, string Author, string GitRepository);

public record DBAnalysisCreateDTO(int LatestCommitId, string Author, string GitRepository);

