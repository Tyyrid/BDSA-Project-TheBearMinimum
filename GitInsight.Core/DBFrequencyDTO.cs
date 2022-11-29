namespace GitInsight.Core;

//Date sammen med foreign key fra DBAnalysis_s giver en unik key
public record DBFrequencyDTO(int DBAnalysisId, DateTime Date, int Frequency);

public record DBFrequencyCreateDTO(int DBAnalysisId, DateTime Date, int Frequency);

public record DBFrequencyUpdateDTO(int DBAnalysisId, DateTime Date, int Frequency);
