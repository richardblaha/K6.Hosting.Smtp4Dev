using Aspire.Hosting.ApplicationModel;

namespace K6.Hosting.Smtp4Dev;

public class Smtp4DevResource : ContainerResource, IResourceWithConnectionString
{
    public Smtp4DevResource(string name) : base(name)
    {
        SmtpEndpoint = new(this, Smtp4DevConstants.SmtpEndpointName);
        ImapEndpoint = new(this, Smtp4DevConstants.ImapEndpointName);
        WebEndpoint = new(this, Smtp4DevConstants.HttpEndpointName);
    }

    public EndpointReference SmtpEndpoint { get; set; }
    public EndpointReference ImapEndpoint { get; set; }
    public EndpointReference WebEndpoint { get; set; }

    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create($"smtp://{SmtpEndpoint.Property(EndpointProperty.Host)}:{SmtpEndpoint.Property(EndpointProperty.Port)}");
}