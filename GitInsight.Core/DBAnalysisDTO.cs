namespace GitInsight.Core;

public record DBAnalysisDTO(int Id, string LatestCommitId, string Author, string GitRepository);

public record DBAnalysisCreateDTO(string LatestCommitId, string Author, string GitRepository);

public record DBAnalysisUpdateDTO(int Id, string LatestCommitId, string Author, string GitRepository);