using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace K6.Hosting.Smtp4Dev;

public class SmtpHealthCheck(
    string smtpHost,
    int smtpPort
) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(smtpHost, smtpPort, cancellationToken);

            return HealthCheckResult.Healthy("SMTP server is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Failed to connect to SMTP server.", ex);
        }
    }
}