namespace Xeno.Dockzi.NotesAddin;

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

public sealed class NoteWindow : Window
{
  private readonly TextBox _editor;
  private readonly FlowDocumentScrollViewer _preview;
  private readonly Func<string, Note> _save;
  private readonly TextBlock _title;
  private bool _hasSaved;
  private bool _discardOnClose;
  private string _savedText;

  public NoteWindow(Note note, Func<string, Note> save, bool edit = false)
  {
    _save = save;
    _savedText = note.Text;
    _hasSaved = note.SavedUtc != default;
    Width = 360;
    Height = 380;
    MinWidth = 240;
    MinHeight = 180;
    WindowStartupLocation = WindowStartupLocation.CenterScreen;
    WindowStyle = WindowStyle.None;
    ResizeMode = ResizeMode.CanResizeWithGrip;
    Background = new SolidColorBrush(Color.FromRgb(255, 246, 174));
    Foreground = new SolidColorBrush(Color.FromRgb(62, 53, 28));

    WindowChrome.SetWindowChrome(this, new WindowChrome { CaptionHeight = 0, ResizeBorderThickness = new Thickness(6), CornerRadius = new CornerRadius(0) });

    var layout = new Grid();
    layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(38) });
    layout.RowDefinitions.Add(new RowDefinition());

    var header = new Grid { Background = new SolidColorBrush(Color.FromRgb(247, 227, 125)) };
    header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(38) });
    header.ColumnDefinitions.Add(new ColumnDefinition());
    header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(38) });

    var saveButton = new Button
    {
      Content = "\uE74E",
      FontFamily = new FontFamily("Segoe MDL2 Assets"),
      FontSize = 14,
      Background = Brushes.Transparent,
      BorderThickness = new Thickness(0),
      ToolTip = "Save (Ctrl+S)",
    };

    System.Windows.Automation.AutomationProperties.SetName(saveButton, "Save note");

    saveButton.Click += (_, _) => SaveNote();

    _title = new TextBlock { VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis, FontWeight = FontWeights.SemiBold };
    _title.MouseLeftButtonDown += (_, e) => { if (e.ButtonState == MouseButtonState.Pressed) DragMove(); };

    Grid.SetColumn(_title, 1);
    var closeButton = new Button { Content = "×", FontSize = 22, Background = Brushes.Transparent, BorderThickness = new Thickness(0), ToolTip = "Close note" };
    System.Windows.Automation.AutomationProperties.SetName(closeButton, "Close note");

    Grid.SetColumn(closeButton, 2);

    closeButton.Click += (_, _) => Close();

    header.Children.Add(saveButton);
    header.Children.Add(_title);
    header.Children.Add(closeButton);

    layout.Children.Add(header);

    _editor = new TextBox
    {
      Text = note.Text,
      AcceptsReturn = true,
      AcceptsTab = true,
      TextWrapping = TextWrapping.Wrap,
      VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
      FontFamily = new FontFamily("Consolas"),
      FontSize = 14,
      Background = Brushes.Transparent,
      BorderThickness = new Thickness(0),
      Padding = new Thickness(20, 16, 20, 20),
    };

    _editor.TextChanged += (_, _) => UpdateTitle();
    _preview = new FlowDocumentScrollViewer
    {
      IsToolBarVisible = false,
      VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
      Background = Brushes.Transparent,
      Focusable = true,
    };

    _preview.PreviewMouseLeftButtonDown += (_, e) =>
    {
      // Leave the scrollbar available for scrolling longer notes.
      if (e.OriginalSource is DependencyObject source && FindScrollBar(source)) return;
      BeginEdit();
      e.Handled = true;
    };

    Grid.SetRow(_editor, 1);
    Grid.SetRow(_preview, 1);
    layout.Children.Add(_preview);
    layout.Children.Add(_editor);
    Content = layout;

    PreviewKeyDown += (_, e) =>
    {
      if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
      {
        SaveNote();
        e.Handled = true;
      }
      else if (e.Key == Key.F2 && _preview.Visibility == Visibility.Visible)
      {
        BeginEdit();
        e.Handled = true;
      }
      else if (e.Key == Key.Escape)
      {
        e.Handled = true;
        if (e.IsRepeat)
          return;

        if (IsEditing)
        {
          _editor.Text = _savedText;
          _editor.IsUndoEnabled = false;
          _editor.IsUndoEnabled = true;
          UpdateTitle();
          ShowPreview();
        }
        else
        {
          // Escape dismisses a new, discarded draft without creating a saved note.
          _discardOnClose = true;
          Close();
        }
      }
    };

    Closing += (_, e) =>
    {
      if (!_discardOnClose && (_editor.Text != _savedText || !_hasSaved) && !SaveNote())
      {
        e.Cancel = MessageBox.Show(this, "The note could not be saved. Close and discard these changes?", "Unsaved note", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes;
      }
    };

    UpdateTitle();

    if (edit)
    {
      _preview.Visibility = Visibility.Collapsed;
      Loaded += (_, _) => { _editor.Focus(); _editor.CaretIndex = _editor.Text.Length; };
    }
    else
      ShowPreview();
  }

  public bool IsEditing => _editor.Visibility == Visibility.Visible;

  public string NoteText
  {
    get => _editor.Text;
    set => _editor.Text = value;
  }

  public void BeginEdit()
  {
    _preview.Visibility = Visibility.Collapsed;
    _editor.Visibility = Visibility.Visible;
    _editor.Focus();
  }

  public bool SaveNote()
  {
    try
    {
      var saved = _save(_editor.Text);
      _savedText = saved.Text;
      _hasSaved = true;
      UpdateTitle();
      ShowPreview();
      return true;
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
      MessageBox.Show(this, ex.Message, "Unable to save note", MessageBoxButton.OK, MessageBoxImage.Error);
      return false;
    }
  }

  private static bool FindScrollBar(DependencyObject source)
  {
    while (source != null)
    {
      if (source is System.Windows.Controls.Primitives.ScrollBar)
        return true;

      source = source is Visual ? VisualTreeHelper.GetParent(source) : LogicalTreeHelper.GetParent(source);
    }

    return false;
  }

  private void ShowPreview()
  {
    _preview.Document = MarkdownPreview.Render(_editor.Text);
    _editor.Visibility = Visibility.Collapsed;
    _preview.Visibility = Visibility.Visible;
    _preview.Focus();
  }

  private void UpdateTitle()
  {
    Title = Note.GetTitle(_editor.Text);
    _title.Text = Title + (_editor.Text != _savedText || !_hasSaved ? " *" : string.Empty);
  }
}
