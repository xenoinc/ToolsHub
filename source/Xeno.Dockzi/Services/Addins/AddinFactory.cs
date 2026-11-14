namespace Xeno.Dockzi.Services.Addins;

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Mono.Addins;

/// <summary>DI activation boundary for Mono.Addins types. Owns the objects it creates.</summary>
public sealed class AddinFactory : IDisposable
{
  private readonly IServiceProvider _services;
  private readonly List<IDisposable> _owned = new();
  private bool _disposed;

  public AddinFactory(IServiceProvider services)
  {
    _services = services;
  }

  public object Create(TypeExtensionNode node) => Create(node.Type);

  public object Create(Type type)
  {
    ObjectDisposedException.ThrowIf(_disposed, this);
    var instance = ActivatorUtilities.CreateInstance(_services, type);
    if (instance is IDisposable disposable)
      _owned.Add(disposable);
    return instance;
  }

  public void Dispose()
  {
    if (_disposed)
      return;
    _disposed = true;
    for (int index = _owned.Count - 1; index >= 0; index--)
      _owned[index].Dispose();
    _owned.Clear();
  }
}
