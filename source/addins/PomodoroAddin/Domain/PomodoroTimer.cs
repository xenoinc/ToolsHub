namespace PomodoroAddin.Domain;

using System;

public sealed class PomodoroTimer : IPomodoroTimer
{
  private readonly System.Timers.Timer _timer = new(60000);

  public PomodoroTimer() => _timer.Elapsed += OnElapsed;

  public event EventHandler Tick;

  public bool IsRunning => _timer.Enabled;

  public void Start() => _timer.Start();

  public void Stop() => _timer.Stop();

  public void Dispose()
  {
    _timer.Stop();
    _timer.Elapsed -= OnElapsed;
    _timer.Dispose();
  }

  private void OnElapsed(object sender, System.Timers.ElapsedEventArgs args) => Tick?.Invoke(this, EventArgs.Empty);
}
