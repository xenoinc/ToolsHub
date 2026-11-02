namespace Xeno.ToolsHub.Views.Preferences
{
  using System;
  using System.IO;
  using System.Security;
  using System.Windows.Forms;
  using Xeno.ToolsHub.Services;

  /// <summary>Built-in settings, independent of add-ins.</summary>
  [System.Runtime.Versioning.SupportedOSPlatform("windows7.0")]
  public sealed class GeneralPreferencesPage : UserControl
  {
    private readonly IAutoStartService _autoStart;
    private readonly CheckBox _enableAutoStart;
    private readonly Label _status;
    private bool _originalAutoStart;

    public GeneralPreferencesPage()
      : this(new AutoStartService())
    {
    }

    public GeneralPreferencesPage(IAutoStartService autoStart)
    {
      _autoStart = autoStart ?? throw new ArgumentNullException(nameof(autoStart));
      AutoScaleMode = AutoScaleMode.Font;
      AutoScroll = true;
      Padding = new Padding(12);

      var layout = new FlowLayoutPanel
      {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true,
      };

      _enableAutoStart = new CheckBox
      {
        Name = "EnableAutoStart",
        Text = "Enable Auto-Start With OS",
        AutoSize = true,
        Margin = new Padding(0, 0, 0, 8),
      };

      var description = new Label
      {
        Text = "Launch automatically when you sign in to Windows.",
        AutoSize = true,
        MaximumSize = new System.Drawing.Size(380, 0),
        Margin = new Padding(0, 0, 0, 12),
      };

      _status = new Label
      {
        AutoSize = true,
        MaximumSize = new System.Drawing.Size(380, 0),
        ForeColor = System.Drawing.SystemColors.GrayText,
        Text = "Changes take effect when you click OK. Applies to your Windows account.",
      };

      layout.Controls.Add(_enableAutoStart);
      layout.Controls.Add(description);
      layout.Controls.Add(_status);
      Controls.Add(layout);

      try
      {
        _originalAutoStart = _autoStart.IsEnabled;
        _enableAutoStart.Checked = _originalAutoStart;
      }
      catch (Exception ex) when (IsStartupAccessError(ex))
      {
        _enableAutoStart.Enabled = false;
        _status.Text = "Unable to read Windows startup settings: " + ex.Message;
      }
    }

    public bool TrySave(out string error)
    {
      error = null;
      if (!_enableAutoStart.Enabled || _enableAutoStart.Checked == _originalAutoStart)
        return true;

      try
      {
        _autoStart.SetEnabled(_enableAutoStart.Checked);
        _originalAutoStart = _enableAutoStart.Checked;
        return true;
      }
      catch (Exception ex) when (IsStartupAccessError(ex))
      {
        error = "Unable to update Windows startup settings: " + ex.Message;
        return false;
      }
    }

    private static bool IsStartupAccessError(Exception exception)
    {
      return exception is UnauthorizedAccessException || exception is SecurityException || exception is IOException;
    }
  }
}
