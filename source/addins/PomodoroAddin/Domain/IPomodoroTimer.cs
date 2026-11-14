namespace Xeno.Dockzi.PomodoroAddin.Domain;

using System;

public interface IPomodoroTimer : IDisposable
{
  event EventHandler Tick;

  bool IsRunning { get; }

  void Start();

  void Stop();
}
