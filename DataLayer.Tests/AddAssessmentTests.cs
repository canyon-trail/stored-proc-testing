using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;

namespace DataLayer.Tests;

public sealed class AddAssessmentTests
{
    private readonly DatabaseTestFixture _fixture;

    public AddAssessmentTests()
    {
        _fixture = new DatabaseTestFixture();
    }

    [Fact]
    public async Task InsertsUserAssessmentResult()
    {
        var userId = await _fixture.InsertUserAndGetId("Bob", "Smith");
        var assessmentId = await _fixture.InsertAssessmentAndGetId("Sample Assessment");

        var connectionString = await _fixture.GetConnectionString();

        var testee = new DataAccessApi(connectionString);

        var taken = new DateTime(2023, 9, 7);
        await testee.AddAssessmentResult(userId, assessmentId, taken, true);

        var connection = new SqlConnection(connectionString);

        var result = (await connection.QueryAsync<UserAssessmentResult>("SELECT * FROM [UserAssessmentResult];"))
            .Single();

        result.Should().BeEquivalentTo(new
        {
            UserId = userId,
            AssessmentId = assessmentId,
            Taken = taken,
            Passed = true
        });
    }

    [Fact]
    public async Task InsertsUserAssessment()
    {
        var userId = await _fixture.InsertUserAndGetId("Bob", "Smith");
        var assessmentId = await _fixture.InsertAssessmentAndGetId("Sample Assessment");

        var connectionString = await _fixture.GetConnectionString();

        var testee = new DataAccessApi(connectionString);

        var taken = new DateTime(2023, 9, 7);
        await testee.AddAssessmentResult(userId, assessmentId, taken, true);

        var connection = new SqlConnection(connectionString);

        var result = (await connection.QueryAsync<UserAssessment>("SELECT * FROM [UserAssessment];"))
            .Single();

        result.Should().BeEquivalentTo(new
        {
            UserId = userId,
            AssessmentId = assessmentId,
            LastTaken = taken,
            Passed = true
        });
    }

    [Fact]
    public async Task UpdatesExistingUserAssessment()
    {
        var userId = await _fixture.InsertUserAndGetId("Bob", "Smith");
        var assessmentId = await _fixture.InsertAssessmentAndGetId("Sample Assessment");

        var connectionString = await _fixture.GetConnectionString();

        var testee = new DataAccessApi(connectionString);

        var taken = new DateTime(2023, 9, 7);
        await testee.AddAssessmentResult(userId, assessmentId, taken.AddDays(-1), true);
        await testee.AddAssessmentResult(userId, assessmentId, taken, false);

        var connection = new SqlConnection(connectionString);

        var result = (await connection.QueryAsync<UserAssessment>("SELECT * FROM [UserAssessment];"))
            .Single();

        result.Should().BeEquivalentTo(new
        {
            UserId = userId,
            AssessmentId = assessmentId,
            LastTaken = taken,
            Passed = false
        });
    }
}
