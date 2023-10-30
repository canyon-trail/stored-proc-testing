namespace DataLayer.Tests;

public sealed class UserAssessment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AssessmentId { get; set; }
    public DateTime LastTaken { get; set; }
    public bool Passed { get; set; }
}