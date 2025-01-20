using System;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

using Microsoft.Extensions.DependencyInjection;

namespace K6.Hosting.Smtp4Dev;

public static class Smtp4DevBuilderExtensions
{
    public static IResourceBuilder<Smtp4DevResource> AddSmtp4Dev(this IDistributedApplicationBuilder builder, [ResourceName] string name)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);

        var resource = new Smtp4DevResource(name);

        string connectionString;
        builder.Eventing.Subscribe<ConnectionStringAvailableEvent>(resource, async (@event, cancellationToken) =>
        {
            connectionString = await resource.ConnectionStringExpression.GetValueAsync(cancellationToken).ConfigureAwait(false);
            if (connectionString == null)
            {
                throw new DistributedApplicationException(
                    $"ConnectionStringAvailableEvent was published for the '{resource.Name}' resource but the connection string was null.");
            }
        });

        builder.Services
            .AddHealthChecks()
            .AddCheck("SMTP Health Check", new SmtpHealthCheck(resource.SmtpEndpoint.Host, resource.SmtpEndpoint.Port))
            .AddCheck("IMAP Health Check", new ImapHealthCheck(resource.ImapEndpoint.Host, resource.ImapEndpoint.Port))
            .AddCheck("HTTP Health Check", new HttpHealthCheck(resource.WebEndpoint.Host, resource.WebEndpoint.Port));

        return builder
            .AddResource(resource)
            .WithImageRegistry(Smtp4DevConstants.Registry)
            .WithImage(Smtp4DevConstants.Image, Smtp4DevConstants.Tag)
            .WithEndpoint(targetPort: Smtp4DevConstants.SmtpEndpointPort, name: Smtp4DevConstants.SmtpEndpointName)
            .WithEndpoint(targetPort: Smtp4DevConstants.ImapEndpointPort, name: Smtp4DevConstants.ImapEndpointName)
            .WithEndpoint(targetPort: Smtp4DevConstants.HttpEndpointPort, name: Smtp4DevConstants.HttpEndpointName);
    }
}