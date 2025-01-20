namespace K6.Hosting.Smtp4Dev;

internal static class Smtp4DevConstants
{
    internal const string Registry = "docker.io";
    internal const string Image = "rnwood/smtp4dev";
    internal const string Tag = "latest";

    internal const string SmtpEndpointName = "smtp";
    internal const int SmtpEndpointPort = 25;

    internal const string ImapEndpointName = "imap";
    internal const int ImapEndpointPort = 143;

    internal const string HttpEndpointName = "web";
    internal const int HttpEndpointPort = 80;
}