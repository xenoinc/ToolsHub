namespace Xeno.Dockzi.NotesAddin;

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

public sealed class NoteWindow : Window
{
  private readonly Func<bool> _confirmDelete;
  private readonly Action _delete;
  private readonly TextBox _editor;
  private readonly FlowDocumentScrollViewer _preview;
  private readonly Func<string, Note> _save;
  private readonly TextBlock _title;

  private bool _discardOnClose;
  private bool _hasSaved;
  private string _savedText;

  /// <summary>
  /// Initializes a new note window with editing, saving, preview, and optional deletion support.
  /// </summary>
  /// <remarks>If not provided, deletion confirmation uses a default warning dialog. When closing with unsaved
  /// changes, the window prompts to discard changes if saving fails.</remarks>
  /// <param name="note">The note to display and edit, including initial text and saved state.</param>
  /// <param name="save">A callback that persists note text and returns the saved note.</param>
  /// <param name="edit">true to open in edit mode with focus in the editor; false to open in preview mode.</param>
  /// <param name="delete">An optional callback that deletes the note.</param>
  /// <param name="confirmDelete">An optional callback that determines whether note deletion is confirmed.</param>
  public NoteWindow(
    Note note,
    Func<string, Note> save,
    bool edit = false,
    Action delete = null,
    Func<bool> confirmDelete = null)
  {
    _save = save;
    _delete = delete;

    _confirmDelete = confirmDelete ?? (
      () => MessageBox.Show(
        this,
        "Are you sure you want to delete note?",
        "Delete note",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning,
        MessageBoxResult.No) == MessageBoxResult.Yes);

    _savedText = note.Text;
    _hasSaved = note.SavedUtc != default;

    Width = 360;
    Height = 380;
    MinWidth = 240;
    MinHeight = 180;

    // Ensure we're using the NoteIcon.ico
    Icon = BitmapFrame.Create(new Uri("pack://application:,,,/NotesAddin;component/Resources/NoteIcon.ico", UriKind.Absolute));
    NoteTaskbarIdentity.Attach(this);

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
    header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(38) });
    header.ColumnDefinitions.Add(new ColumnDefinition());
    header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(38) });

    // Balance Save and a spacer against the menu and Close to keep the title centered.
    header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(38) });

    var noteIcon = new Image { Source = Icon, Width = 20, Height = 20, IsHitTestVisible = false, ToolTip = "Dockzi Notes" };
    Grid.SetColumn(noteIcon, 1);
    header.Children.Add(noteIcon);

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

    var moreButton = new Button
    {
      Content = "\uE712",
      FontFamily = new FontFamily("Segoe MDL2 Assets"),
      FontSize = 14,
      Background = Brushes.Transparent,
      BorderThickness = new Thickness(0),
      ToolTip = "Note options",
    };

    var deleteItem = new MenuItem
    {
      Header = "Delete",
      Icon = new TextBlock { Text = "\uE74D", FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 14 },
      IsEnabled = delete != null,
    };

    deleteItem.Click += (_, _) => DeleteNote();

    var menu = new ContextMenu { PlacementTarget = moreButton, Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom };
    menu.Items.Add(deleteItem);

    moreButton.ContextMenu = menu;
    System.Windows.Automation.AutomationProperties.SetName(moreButton, "Note options");
    Grid.SetColumn(moreButton, 3);

    moreButton.Click += (_, _) => menu.IsOpen = true;
    Closed += (_, _) => menu.IsOpen = false;

    _title = new TextBlock
    {
      VerticalAlignment = VerticalAlignment.Center,
      TextAlignment = TextAlignment.Center,
      TextTrimming = TextTrimming.CharacterEllipsis,
      FontWeight = FontWeights.SemiBold,
    };

    _title.MouseLeftButtonDown += (_, e) =>
    {
      if (e.ButtonState == MouseButtonState.Pressed)
        DragMove();
    };

    Grid.SetColumn(_title, 2);
    var closeButton = new Button
    {
      Content = "×",
      FontSize = 22,
      Background = Brushes.Transparent,
      BorderThickness = new Thickness(0),
      ToolTip = "Close note",
    };

    System.Windows.Automation.AutomationProperties.SetName(closeButton, "Close note");

    Grid.SetColumn(closeButton, 4);

    closeButton.Click += (_, _) => Close();

    header.Children.Add(saveButton);
    header.Children.Add(moreButton);
    header.Children.Add(_title);
    header.Children.Add(closeButton);

    layout.Children.Add(header);

    _editor = new TextBox
    {
      Text = note.Text,
      AcceptsReturn = true,
      AcceptsTab = false,
      TextWrapping = TextWrapping.Wrap,
      VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
      FontFamily = new FontFamily("Consolas"),
      FontSize = 14,
      Background = Brushes.Transparent,
      BorderThickness = new Thickness(0),
      Padding = new Thickness(20, 16, 20, 20),
    };

    NoteEditorBehavior.Attach(_editor);
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
      if (e.OriginalSource is DependencyObject source && FindScrollBar(source))
        return;

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
      Loaded += (_, _) =>
      {
        _editor.Focus();
        _editor.CaretIndex = _editor.Text.Length;
      };
    }
    else
    {
      ShowPreview();
    }
  }

  public bool IsEditing => _editor.Visibility == Visibility.Visible;

  public string NoteText
  {
    get => _editor.Text;
    set => _editor.Text = value;
  }

  public string SavedTitle => Note.GetTitle(_savedText);

  public void BeginEdit()
  {
    _preview.Visibility = Visibility.Collapsed;
    _editor.Visibility = Visibility.Visible;
    _editor.Focus();
  }

  public void CloseWithoutSaving()
  {
    _discardOnClose = true;
    Close();
  }

  public bool DeleteNote()
  {
    if (_delete == null || !_confirmDelete()) return false;
    try
    {
      _delete();
      if (IsVisible)
        CloseWithoutSaving();

      return true;
    }
    catch (Exception ex)
    when (ex is IOException || ex is UnauthorizedAccessException)
    {
      MessageBox.Show(this, ex.Message, "Unable to delete note", MessageBoxButton.OK, MessageBoxImage.Error);
      return false;
    }
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
    catch (Exception ex)
    when (ex is IOException || ex is UnauthorizedAccessException)
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
