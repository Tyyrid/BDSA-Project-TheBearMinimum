namespace GitInsight.Core;

public record DBCommitDTO(int Id, int CommitId, string Author, string GitRepository);

public record DBCommitCreateDTO(int CommitId, string Author, string GitRepository);

//public record DBCommitUpdateDTO(int Id, int CommitId, string Author, string GitRepository);
