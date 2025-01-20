using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace K6.Hosting.Smtp4Dev;

public class ImapHealthCheck(
    string imapHost,
    int imapPort
) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(imapHost, imapPort, cancellationToken);

            return HealthCheckResult.Healthy("IMAP server is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Failed to connect to IMAP server.", ex);
        }
    }
}