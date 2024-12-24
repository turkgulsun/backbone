using System.Data;
using Core.Application.Services;
using Npgsql;

namespace Core.Infrastructure.Services.Impl;

public class PostgreDbConnectionFactory(string connectionString, string dbName) : IDbConnectionFactory, IDisposable
{
    private IDbConnection _connection;

    public string GetDbName() => dbName;

    public void Dispose()
    {
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Dispose();
        }
    }

    public IDbConnection GetOpenConnection()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection = new NpgsqlConnection(connectionString);

            _connection.Open();
        }

        return _connection;
    }
}