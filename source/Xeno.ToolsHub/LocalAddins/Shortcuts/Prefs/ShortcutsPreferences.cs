/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    ShortcutsPreferences.cs
 * Description:
 *  Shortcuts preference window
 */

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  using System;
  using System.Windows.Forms;
  using Newtonsoft.Json.Linq;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  public partial class ShortcutsPreferences : Form, IPreferencePageForm
  {
    private bool _isModified = false;

    private ShortcutsManager _shortcuts = new ShortcutsManager();

    public ShortcutsPreferences()
    {
      InitializeComponent();
    }

    public bool IsModified
    {
      get { return _isModified; }

      set
      {
        if (_isModified != value)
        {
          _isModified = value;
          LblModified.Visible = value;
        }
      }
    }

    public bool OnSave()
    {
      string errMsg = string.Empty;
      string json = TxtRawFile.Text;

      IsModified = false;

      if (_shortcuts.ValidateJson(json, out errMsg))
      {
        _shortcuts.ShortcutItems = Newtonsoft.Json.JsonConvert.DeserializeObject<ShortcutItems>(json);
        _shortcuts.SaveShortcuts();
        _shortcuts.Refresh();

        return true;
      }
      else
      {
        Log.Error("Your JSON formatting is bad, and you should feel bad too!" + Environment.NewLine + errMsg);
        return false;
      }
    }

    private void LoadShortcutsFile()
    {
      TxtRawFile.Text = _shortcuts.ToString();

      RefreshTreeView();
    }

    private void ShortcutsPreferences_Load(object sender, EventArgs e)
    {
      LoadShortcutsFile();
      IsModified = false;
    }

    private void TxtRawFile_TextChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    #region JSON Viewer

    private void RefreshTreeView()
    {
      // http://huseyint.com/2013/12/Have-your-JSON-TreeView-and-unit-test-it-too/
      // https://github.com/huseyint/JsonTreeView/blob/master/JsonTreeView/JsonTreeViewLoader.cs
      // https://stackoverflow.com/questions/39673815/how-to-recursively-populate-a-treeview-with-json-data

      // TEST:
      string json = _shortcuts.ToString();
      string test = "{ 'foo': 'bar', 'baz': [ 42, 'quux' ] }";
      LoadJsonToTreeView(treeShortcuts, json);

      // Fails beause we start with a "[ {"
      // LoadJsonToTreeView(treeShortcuts, _shortcuts.ToString());
      treeShortcuts.ExpandAll();

      ////string data = _shortcuts.ToString();
      ////using (var jsonReader = new JsonTextReader(data))
      ////{
      ////  var root = JToken.Load(jsonReader);
      ////  DispayTreeView(root, )
      ////}
    }

    private void LoadJsonToTreeView(TreeView treeView, string json)
    {
      if (string.IsNullOrWhiteSpace(json))
      {
        return;
      }

      var @object = JObject.Parse(json);
      AddObjectNodes(@object, "Shortcuts", treeView.Nodes);
    }

    private void AddObjectNodes(JObject @object, string name, TreeNodeCollection parent)
    {
      var node = new TreeNode(name);
      parent.Add(node);

      foreach (var property in @object.Properties())
      {
        AddTokenNodes(property.Value, property.Name, node.Nodes);
      }
    }

    private void AddArrayNodes(JArray array, string name, TreeNodeCollection parent)
    {
      var node = new TreeNode(name);
      parent.Add(node);

      for (var i = 0; i < array.Count; i++)
      {
        AddTokenNodes(array[i], string.Format("[{0}]", i), node.Nodes);
      }
    }

    private void AddTokenNodes(JToken token, string name, TreeNodeCollection parent)
    {
      if (token is JValue)
      {
        parent.Add(new TreeNode(string.Format("{0}: {1}", name, ((JValue)token).Value)));
      }
      else if (token is JArray)
      {
        AddArrayNodes((JArray)token, name, parent);
      }
      else if (token is JObject)
      {
        AddObjectNodes((JObject)token, name, parent);
      }
    }

    #endregion JSON Viewer
  }
}
