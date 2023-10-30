using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DataLayer;

public sealed class DataAccessApi
{
    private readonly string _connectionString;

    public DataAccessApi(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddAssessmentResult(int userId, int assessmentId, DateTime taken, bool passed)
    {
        await using var connection = new SqlConnection(_connectionString);

        var parameters = new
        {
            userId,
            assessmentId,
            taken,
            passed,
        };

        await connection.ExecuteAsync("AddAssessment", parameters, commandType: CommandType.StoredProcedure);
    }
}
