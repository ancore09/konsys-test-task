using Maroontress.Html;

namespace Grechko_Test.Utilities;

public static class HtmlHelper
{
    private static NodeFactory _nodeOf = Nodes.NewFactory();

    public static Tag CreateHtml(string title)
    {
        return _nodeOf.Html.Add(
            _nodeOf.Head.Add(
                _nodeOf.Title.Add(title),
                _nodeOf.Script.AddAttributes(("src", "https://cdn.tailwindcss.com"))));
    }

    public static Tag GetFolderListItem(string name, long size)
    {
        return _nodeOf.Li.Add($"{name} | {size} bytes")
            .AddClass("text-lg")
            .AddClass("font-semibold");
    }
    
    public static Tag GetFileListElement(string name, long size, string mimeType)
    {
       return  _nodeOf.Li.Add(
            _nodeOf.Div.Add(
                _nodeOf.P.Add($"{name}").AddClass("text-base").AddClass("font-normal"),
                _nodeOf.Div.Add(
                    _nodeOf.P.Add($"{size} bytes").AddClass("mr-2").AddClass("text-sm").AddClass("font-light"),
                    _nodeOf.P.Add($"{mimeType}").AddClass("mx-2").AddClass("text-sm").AddClass("font-light")
                ).AddClass("flex")
            ).AddClass("inline-block")
        );
    }

    public static Tag GetStatisticsListElement(string mimeType, long amount, long total, long totalSize)
    {
        return _nodeOf.Li.Add($"{mimeType}: {amount} | " +
                       $"{((double) amount / total * 100):#.##}% | " +
                       $"{((double) totalSize / amount):#.##} bytes");
    }

    public static Tag CreateUnorderedList()
    {
        return _nodeOf.Ul.Add()
            .AddClass("list-none")
            .AddClass("list-inside");
    }

    public static Tag CreateH3(string text)
    {
        return _nodeOf.H3.Add(text)
            .AddClass("text-3xl")
            .AddClass("font-semibold")
            .AddClass("mt-4");
    }
}