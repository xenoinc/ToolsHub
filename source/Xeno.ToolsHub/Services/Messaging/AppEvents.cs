namespace Xeno.ToolsHub.Services.Messaging;

using Lite.EventIpc;

/// <summary>The shared in-process event aggregator used by the host and its add-ins.</summary>
public static class AppEvents
{
  public static IEventAggregator Instance { get; } = new EventAggregator();
}
