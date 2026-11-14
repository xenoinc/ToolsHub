namespace Xeno.Dockzi.Services.Messaging;

using Lite.EventIpc;

/// <summary>Compatibility access for add-ins not yet using injected IEventAggregator.</summary>
public static class AppEvents
{
  public static IEventAggregator Instance { get; internal set; } = new EventAggregator();
}
