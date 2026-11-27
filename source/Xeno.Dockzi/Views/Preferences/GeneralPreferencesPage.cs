namespace Xeno.Dockzi.Views.Preferences
{
  using System;
  using System.IO;
  using System.Security;
  using System.Windows.Forms;
  using Xeno.Dockzi.Services;

  /// <summary>Built-in settings, independent of add-ins.</summary>
  [System.Runtime.Versioning.SupportedOSPlatform("windows7.0")]
  public sealed class GeneralPreferencesPage : UserControl
  {
    private readonly IAutoStartService _autoStart;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Owned by the form control tree and disposed by Form.Dispose.")]
    private readonly CheckBox _enableAutoStart;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Owned and disposed by the form control tree.")]
    private readonly CheckBox _receivePreviewUpdates;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Owned by the form control tree and disposed by Form.Dispose.")]
    private readonly Label _status;

    private readonly UpdatePreferences _updatePreferences;

    private bool _originalAutoStart;
    private bool _originalPreviewUpdates;

    public GeneralPreferencesPage()
      : this(new AutoStartService(), new UpdatePreferences(Program.Settings ?? SettingsLoader.Load()))
    {
    }

    public GeneralPreferencesPage(IAutoStartService autoStart)
      : this(autoStart, null)
    {
    }

    public GeneralPreferencesPage(IAutoStartService autoStart, UpdatePreferences updatePreferences)
    {
      _autoStart = autoStart ?? throw new ArgumentNullException(nameof(autoStart));
      _updatePreferences = updatePreferences;
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

      _originalPreviewUpdates = _updatePreferences?.ReceivePreviewUpdates ?? false;
      _receivePreviewUpdates = new CheckBox
      {
        Name = "ReceivePreviewUpdates",
        Text = "Bleeding-Edge Preview Updates",
        AutoSize = true,
        Checked = _originalPreviewUpdates,
        Enabled = _updatePreferences != null,
        Margin = new Padding(0, 0, 0, 8),
      };

      var previewDescription = new Label
      {
        Text = "Include GitHub prereleases when checking for Dockzi updates.",
        AutoSize = true,
        MaximumSize = new System.Drawing.Size(380, 0),
        Margin = new Padding(0, 0, 0, 12),
      };

      _status = new Label
      {
        AutoSize = true,
        MaximumSize = new System.Drawing.Size(380, 0),
        ForeColor = System.Drawing.SystemColors.GrayText,
        Text = "Changes take effect when you click OK.",
      };

      layout.Controls.Add(_enableAutoStart);
      layout.Controls.Add(description);
      layout.Controls.Add(_receivePreviewUpdates);
      layout.Controls.Add(previewDescription);
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
      try
      {
        if (_enableAutoStart.Enabled && _enableAutoStart.Checked != _originalAutoStart)
          _autoStart.SetEnabled(_enableAutoStart.Checked);
        else if (_enableAutoStart.Enabled && _enableAutoStart.Checked)
          _autoStart.RepairRegistration();

        _originalAutoStart = _enableAutoStart.Checked;
      }
      catch (Exception ex) when (IsStartupAccessError(ex))
      {
        error = "Unable to update Windows startup settings: " + ex.Message;
        return false;
      }

      try
      {
        if (_updatePreferences != null && _receivePreviewUpdates.Checked != _originalPreviewUpdates)
        {
          _updatePreferences.SaveReceivePreviewUpdates(_receivePreviewUpdates.Checked);
          _originalPreviewUpdates = _receivePreviewUpdates.Checked;
        }

        return true;
      }
      catch (Exception ex) when (IsStartupAccessError(ex))
      {
        error = "Unable to save update settings: " + ex.Message;
        return false;
      }
    }

    private static bool IsStartupAccessError(Exception exception)
    {
      return exception is UnauthorizedAccessException || exception is SecurityException || exception is IOException;
    }
  }
}
