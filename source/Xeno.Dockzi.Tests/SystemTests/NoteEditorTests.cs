namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.NotesAddin;

[TestClass]
[DoNotParallelize]
[OSCondition(OperatingSystems.Windows)]
public class NoteEditorTests
{
  [TestMethod]
  [DataRow("- First", "- First\r\n- ")]
  [DataRow("* First", "* First\r\n* ")]
  [DataRow("+ First", "+ First\r\n+ ")]
  [DataRow("9. First", "9. First\r\n10. ")]
  [DataRow("12) First", "12) First\r\n13) ")]
  [DataRow("- Parent\r\n  - Child", "- Parent\r\n  - Child\r\n  - ")]
  [DataRow("1. Parent\n   2. Child", "1. Parent\n   2. Child\n   3. ")]
  [DataRow("```\r\n```\r\n- First", "```\r\n```\r\n- First\r\n- ")]
  public void EnterContinuesListMarkerNumberAndIndentation(string text, string expected)
  {
    WithEditor(text, editor =>
    {
      editor.CaretIndex = editor.Text.Length;
      Assert.IsTrue(Press(editor, Key.Return));
      Assert.AreEqual(expected, editor.Text);
      Assert.AreEqual(expected.Length, editor.CaretIndex);
      editor.Undo();
      Assert.AreEqual(text, editor.Text);
    });
  }

  [TestMethod]
  [DataRow("Plain text")]
  [DataRow("```markdown\n- Code")]
  [DataRow("~~~\n1. Code")]
  [DataRow("    - Indented code")]
  public void EnterLeavesOrdinaryTextAndCodeToNormalEditorBehavior(string text)
  {
    WithEditor(text, editor =>
    {
      editor.CaretIndex = editor.Text.Length;
      Assert.IsFalse(Press(editor, Key.Return));
      Assert.AreEqual(text, editor.Text);
    });
  }

  [TestMethod]
  [DataRow("- First\r\n- ", "- First\r\n")]
  [DataRow("1. First\r\n2. ", "1. First\r\n")]
  [DataRow("- Parent\n  * ", "- Parent\n")]
  public void EnterOnEmptyItemEndsList(string text, string expected)
  {
    WithEditor(text, editor =>
    {
      editor.CaretIndex = editor.Text.Length;
      Assert.IsTrue(Press(editor, Key.Return));
      Assert.AreEqual(expected, editor.Text);
    });
  }

  [TestMethod]
  public void EnterOnlyContinuesAtLogicalLineEndWithoutASelection()
  {
    WithEditor("- First\r\nNext line", editor =>
    {
      editor.CaretIndex = 3;
      Assert.IsFalse(Press(editor, Key.Return));

      editor.Select(3, 4);
      Assert.IsFalse(Press(editor, Key.Return));

      editor.Select(7, 0);
      Assert.IsTrue(Press(editor, Key.Return));
      Assert.AreEqual("- First\r\n- \r\nNext line", editor.Text);
    });
  }

  [TestMethod]
  public void TabInsertsTwoSpacesAtCaretOrReplacesSelectionAndCanBeUndone()
  {
    WithEditor("word", editor =>
    {
      editor.CaretIndex = 2;
      Assert.IsTrue(Press(editor, Key.Tab));
      Assert.AreEqual("wo  rd", editor.Text);
      Assert.AreEqual(4, editor.CaretIndex);

      editor.Undo();
      Assert.AreEqual("word", editor.Text);

      editor.Select(1, 2);
      Assert.IsTrue(Press(editor, Key.Tab));
      Assert.AreEqual("w  d", editor.Text);
      Assert.AreEqual(3, editor.CaretIndex);
    });
  }

  private static bool Press(TextBox editor, Key key)
  {
    var args = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(editor), Environment.TickCount, key)
    {
      RoutedEvent = Keyboard.PreviewKeyDownEvent,
    };
    editor.RaiseEvent(args);
    return args.Handled;
  }

  private static void WithEditor(string text, Action<TextBox> action)
  {
    Exception failure = null;
    var thread = new Thread(() =>
    {
      NoteWindow window = null;
      try
      {
        window = new NoteWindow(new Note(Guid.NewGuid(), text, default), source => new Note(Guid.NewGuid(), source, DateTime.UtcNow), true);
        window.Show();

        var editor = ((Grid)window.Content).Children.OfType<TextBox>().Single();
        action(editor);
      }
      catch (Exception ex)
      {
        failure = ex;
      }
      finally
      {
        window?.CloseWithoutSaving();
      }
    });

    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();

    if (failure != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
  }
}
