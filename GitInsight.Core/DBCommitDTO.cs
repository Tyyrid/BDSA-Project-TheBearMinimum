namespace GitInsight.Core;

public record DBCommitDTO(int Id, int LatestCommitId, string Author, string GitRepository);

public record DBCommitCreateDTO(int LatestCommitId, string Author, string GitRepository);

//public record DBCommitUpdateDTO(int Id, int CommitId, string Author, string GitRepository);
