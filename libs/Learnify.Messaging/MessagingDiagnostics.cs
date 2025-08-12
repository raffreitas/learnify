using System.Diagnostics;

using OpenTelemetry.Context.Propagation;

namespace Learnify.Messaging;

public static class MessagingDiagnostics
{
    private static readonly string ActivitySourceName = $"{AppDomain.CurrentDomain.FriendlyName}.Messaging";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
    public static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
}