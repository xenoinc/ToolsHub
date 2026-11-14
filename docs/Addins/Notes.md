# Notes

Notes is a bundled Dockzi add-in. Build either `source/Dockzi.slnx` or `source/Dockzi.sln`, then enable **Notes** in the Add-Ins manager.

- Create a sticky note from **Notes > New note** in the system tray, or press **Ctrl+Alt+N** anywhere in Windows.
- Change or disable the global shortcut in **Preferences > Notes**. An optional Windows modifier is available. A conflicting shortcut reports an error; the tray command remains available.
- Write Markdown or plain text. Use `# Title here` for the centered window title. Notes without a level-one heading are called **Untitled note**.
- Click the rendered note (or press **F2**) to edit the source. Press **Ctrl+S** or click the small save icon to save and return to the formatted view.
- Drag the title to move the note, resize its edges, and use **×** to close. Closing also saves pending changes. If saving fails, the note stays open unless you explicitly discard it.
- The tray submenu shows the **10 most recently saved** notes, newest first. Saving an older note moves it to the top. Closing a note keeps its file.

Notes are UTF-8 `.md` files in the **Notes** directory beside the active `Dockzi.config`; the path is displayed in Preferences. All notes remain on disk, including those outside the recent ten. Open an older file in a text editor and save it to bring it back into the recent list. Back up this directory with your settings.

The preview uses [Markdig](https://github.com/xoofx/markdig) to parse CommonMark and renders native WPF text for headings, emphasis, lists, quotes, code, and separators. Links are displayed as text; remote images and embedded HTML are not loaded or executed. Markdown source is preserved when saving.
