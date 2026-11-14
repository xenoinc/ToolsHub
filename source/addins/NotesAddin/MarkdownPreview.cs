namespace Xeno.Dockzi.NotesAddin;

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

/// <summary>Renders CommonMark into native text; HTML and remote images never execute or load.</summary>
public static class MarkdownPreview
{
  private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder().DisableHtml().Build();

  public static FlowDocument Render(string text)
  {
    var document = new FlowDocument
    {
      FontFamily = new FontFamily("Segoe UI"),
      FontSize = 15,
      Foreground = new SolidColorBrush(Color.FromRgb(62, 53, 28)),
      PagePadding = new Thickness(22, 16, 22, 20),
    };

    AddBlocks(document.Blocks, Markdown.Parse(text, Pipeline));
    return document;
  }

  private static void AddBlocks(BlockCollection target, ContainerBlock source)
  {
    foreach (var block in source)
    {
      if (block is ListBlock list)
      {
        var rendered = new System.Windows.Documents.List
        {
          MarkerStyle = list.IsOrdered ? TextMarkerStyle.Decimal : TextMarkerStyle.Disc,
          Margin = new Thickness(0, 4, 0, 10),
          Padding = new Thickness(24, 0, 0, 0),
        };

        if (list.IsOrdered && int.TryParse(list.OrderedStart, out int start) && start > 0)
          rendered.StartIndex = start;

        foreach (ListItemBlock item in list)
        {
          var entry = new ListItem();
          AddBlocks(entry.Blocks, item);
          rendered.ListItems.Add(entry);
        }

        target.Add(rendered);
      }
      else if (block is QuoteBlock quote)
      {
        var section = new Section
        {
          BorderBrush = Brushes.DarkGoldenrod,
          BorderThickness = new Thickness(3, 0, 0, 0),
          Padding = new Thickness(12, 2, 0, 2),
          Margin = new Thickness(0, 8, 0, 12),
          FontStyle = FontStyles.Italic,
        };

        AddBlocks(section.Blocks, quote);
        target.Add(section);
      }
      else if (block is CodeBlock code)
      {
        target.Add(new Paragraph(new Run(code.Lines.ToString()))
        {
          FontFamily = new FontFamily("Consolas"),
          FontSize = 13,
          Background = new SolidColorBrush(Color.FromArgb(25, 120, 90, 0)),
          Padding = new Thickness(10),
          Margin = new Thickness(0, 6, 0, 12),
        });
      }
      else if (block is ThematicBreakBlock)
      {
        target.Add(new Paragraph { BorderBrush = Brushes.DarkGoldenrod, BorderThickness = new Thickness(0, 0, 0, 1), Margin = new Thickness(0, 8, 0, 12) });
      }
      else if (block is LeafBlock leaf)
      {
        var paragraph = new Paragraph { Margin = new Thickness(0, 0, 0, 10) };
        if (block is HeadingBlock heading)
        {
          paragraph.FontSize = heading.Level switch { 1 => 26, 2 => 22, 3 => 19, _ => 16 };
          paragraph.FontWeight = FontWeights.SemiBold;
          paragraph.Margin = new Thickness(0, 8, 0, 12);
        }

        if (leaf.Inline != null)
          AddInlines(paragraph.Inlines, leaf.Inline);

        target.Add(paragraph);
      }
    }
  }

  private static void AddInlines(InlineCollection target, ContainerInline source)
  {
    foreach (var inline in source)
    {
      switch (inline)
      {
        case LiteralInline literal:
          target.Add(new Run(literal.Content.ToString()));
          break;

        case CodeInline code:
          target.Add(new Run(code.Content) { FontFamily = new FontFamily("Consolas"), Background = new SolidColorBrush(Color.FromArgb(25, 120, 90, 0)) });
          break;

        case LineBreakInline:
          target.Add(new LineBreak());
          break;

        case EmphasisInline emphasis:
          Span span = emphasis.DelimiterCount >= 2 ? new Bold() : new Italic();
          AddInlines(span.Inlines, emphasis);
          target.Add(span);
          break;

        case LinkInline link:
          var label = new Span { Foreground = Brushes.SteelBlue, ToolTip = link.Url };
          AddInlines(label.Inlines, link);
          target.Add(label);
          break;

        case AutolinkInline link:
          target.Add(new Run(link.Url) { Foreground = Brushes.SteelBlue });
          break;

        case ContainerInline container:
          var nested = new Span();
          AddInlines(nested.Inlines, container);
          target.Add(nested);
          break;
      }
    }
  }
}
