using Eiromplays.IdentityServer.Application.Common.Configurations.Database;
using Eiromplays.IdentityServer.Application.Common.Persistence;
using Eiromplays.IdentityServer.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.ConnectionString;

internal class ConnectionStringValidator : IConnectionStringValidator
{
    private readonly DatabaseConfiguration _databaseConfiguration;
    private readonly ILogger<ConnectionStringValidator> _logger;

    public ConnectionStringValidator(IOptions<DatabaseConfiguration> databaseConfiguration, ILogger<ConnectionStringValidator> logger)
    {
        _databaseConfiguration = databaseConfiguration.Value;
        _logger = logger;
    }

    public bool TryValidate(string connectionString, DatabaseProvider? dbProvider = null)
    {
        dbProvider ??= _databaseConfiguration.DatabaseProvider;

        try
        {
            switch (dbProvider)
            {
                case DatabaseProvider.PostgreSql:
                    _ = new NpgsqlConnectionStringBuilder(connectionString);
                    break;

                case DatabaseProvider.MySql:
                    _ = new MySqlConnectionStringBuilder(connectionString);
                    break;

                case DatabaseProvider.SqlServer:
                    _ = new SqlConnectionStringBuilder(connectionString);
                    break;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Connection String Validation Exception : {Message}", ex.Message);
            return false;
        }
    }
}