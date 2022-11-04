namespace GitInsight.Core;

//Date sammen med foreign key fra DBCommit giver en unik key
public record DBFrequencyDTO(int DBCommitId, DateTime Date, int Frequency);

public record DBFrequencyCreateDTO(int DBCommitId, DateTime Date, int Frequency);
