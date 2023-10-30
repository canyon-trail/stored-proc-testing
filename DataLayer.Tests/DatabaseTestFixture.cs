using System.Data;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace DataLayer.Tests;

public sealed class DatabaseTestFixture : IAsyncDisposable
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .Build();

    private const string MainDatabaseName = "main";

    private readonly Task _databaseCreated;

    public DatabaseTestFixture()
    {
        _databaseCreated = CreateDatabaseAndSchema();
    }

    public async Task<string> GetConnectionString()
    {
        await _databaseCreated;

        var rawConnectionString = _container.GetConnectionString();

        var builder = new SqlConnectionStringBuilder(rawConnectionString)
        {
            InitialCatalog = MainDatabaseName
        };

        return builder.ConnectionString;
    }

    public async Task<int> InsertUserAndGetId(string firstName, string lastName)
    {
        await using var connection = new SqlConnection(await GetConnectionString());

        var arguments = new {firstName, lastName};

        var id = await connection.ExecuteScalarAsync<int>(
            @"
                INSERT INTO [User] (firstName, lastName)
                OUTPUT Inserted.id
                VALUES (@firstName, @lastName);",
            arguments
        );

        return id;
    }

    public async Task<int> InsertAssessmentAndGetId(string name)
    {
        await using var connection = new SqlConnection(await GetConnectionString());

        var arguments = new { name };

        var id = await connection.ExecuteScalarAsync<int>(
            @"
                INSERT INTO [Assessment] (name)
                OUTPUT Inserted.id
                VALUES (@name);",
            arguments
        );

        return id;
    }

    public ValueTask DisposeAsync()
    {
        return _container.DisposeAsync();
    }

    private async Task CreateDatabaseAndSchema()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();

        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync($"create database [{MainDatabaseName}];", commandType: CommandType.Text);
        await connection.OpenAsync();
        await connection.ChangeDatabaseAsync(MainDatabaseName);
        foreach (var tblSql in GetEmbeddedSql(x => x.Contains(".Tables.")))
        {
            await connection.ExecuteAsync(tblSql);
        }

        foreach (var procSql in GetEmbeddedSql(x => x.Contains(".Procs.")))
        {
            await connection.ExecuteAsync(procSql);
        }
    }

    private string[] GetEmbeddedSql(Func<string, bool> predicate)
    {
        var assembly = typeof(DataAccessApi).Assembly;
        var names = assembly
            .GetManifestResourceNames()
            .Where(x => x.EndsWith(".sql"))
            .Where(predicate)
            .OrderBy(x => x)
            .ToArray()
            ;

        var sqlFilesContents = names
            .Select(x => assembly.GetManifestResourceStream(x)!)
            .Select(x => new StreamReader(x, Encoding.UTF8))
            .Select(x => x.ReadToEnd())
            .ToArray();

        return sqlFilesContents;
    }
}
