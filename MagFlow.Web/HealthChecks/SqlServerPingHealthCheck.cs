using MagFlow.Shared.Constants;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MagFlow.Web.HealthChecks
{
    public sealed class SqlServerPingHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        public SqlServerPingHealthCheck(IConfiguration configuration) =>
            _connectionString = configuration.GetConnectionString(DatabaseConstants.COREDB) ?? "";

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);
                return HealthCheckResult.Healthy("SQL OK");
            }
            catch(Exception ex)
            {
                return HealthCheckResult.Unhealthy("SQL down", ex);
            }
        }
    }
}
