namespace DataLayer.Tests;

public sealed class UserAssessmentResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AssessmentId { get; set; }
    public DateTime Taken { get; set; }
    public bool Passed { get; set; }
}
