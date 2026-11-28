# Notes

Notes is a bundled Dockzi add-in. Build either `source/Dockzi.slnx` or `source/Dockzi.sln`, then enable **Notes** in the Add-Ins manager.

Addins metadata is declared in `source/addins/NotesAddin/AddinAssembly.cs`.

- Create a sticky note from **Notes > New note** in the system tray, or press **Ctrl+Alt+N** anywhere in Windows.
- New notes start with numbered titles such as **Note 1**, **Note 2**, and **Note 3**. The next number follows the highest numbered title among saved notes and open drafts.
- Change or disable the global shortcut in **Preferences > Notes**. An optional Windows modifier is available. A conflicting shortcut reports an error; the tray command remains available.
- Write Markdown or plain text. Use `# Title here` for the centered window title. Notes without a level-one heading are called **Untitled note**.
- Click the rendered note (or press **F2**) to edit the source. Press **Ctrl+S** or click the small save icon to save and return to the formatted view.
- In the editor, **Tab** inserts two spaces. **Enter** at the end of a list item continues `-`, `*`, or `+` bullets, or increments numbered markers such as `1.` and `1)`, preserving indentation. Enter on an empty item ends the list. Code blocks and typing in the middle of a line keep normal Enter behavior.
- Press **Esc** while editing to discard unsaved changes and return to the formatted view. Press **Esc** again from the formatted view to close. Dismissing a new, discarded draft this way does not save it.
- Drag the title to move the note, resize its edges, and use **×** to close. Closing also saves pending changes. If saving fails, the note stays open unless you explicitly discard it.
- Notes use the embedded `NoteIcon.ico` and share their own Windows taskbar group, separate from Dockzi's application icon.
- The tray submenu shows the **10 most recently saved** notes, newest first. Saving an older note moves it to the top. Closing a note keeps its file.
- **Preferences > Notes** lists all saved notes with their title, saved date, and a trash icon. Double-click a note to open it. To delete a note, click its trash icon in Preferences or choose **… > Delete** beside Close in the note window. Confirm **Are you sure you want to delete note?** to proceed; **No** is selected by default. Deleting also closes the note and discards any pending edits.

Notes are UTF-8 `.md` files in the **Notes** directory beside the active `Dockzi.config`; the path is displayed in Preferences. All notes remain on disk until deleted, including those outside the recent ten. Back up this directory with your settings.

Saved files begin with settings separated from the Markdown body by a blank line:

```text
!SaveDate: 2026-11-14_13:30:00
!NoteColor: Yellow
!Reminder:
!ReminderSnooze: 5min

# Title here

Note content
```

`SaveDate` uses UTC and the 24-hour format `yyyy-MM-dd_HH:mm:ss`. Preferences displays that date in local time. An empty `Reminder` means unset; a reminder value uses the same date format. Color and reminder settings, including custom headers, are preserved when editing the note. These headers are file metadata and are hidden in both the editor and preview. Reminder scheduling and color controls are not currently provided. Older files without headers remain readable and acquire default settings on their next save.

The preview uses [Markdig](https://github.com/xoofx/markdig) to parse CommonMark and renders native WPF text for headings, emphasis, lists, quotes, code, and separators. Links are displayed as text; remote images and embedded HTML are not loaded or executed. Markdown source is preserved when saving.
