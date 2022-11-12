using System.Text.Encodings.Web;
using Grechko_Test.Utilities;
using Maroontress.Html;

namespace Grechko_Test;

internal static class Program
{
    private static NodeFactory _nodeOf;
    private static Tag _document;

    static Program()
    {
        _nodeOf = Nodes.NewFactory();
        _document = HtmlHelper.CreateHtml("SysCon Test Task");
    }

    static DirectoryInfo GetInputDirectory()
    {
        // /Users/ancored/RiderProjects/Grechko_Test
        Console.WriteLine("Enter full or relative path of desired directory:");
        var path = Console.ReadLine();
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists) throw new Exception("Directory does not exists");
        return directoryInfo;
    }
    
    static FileInfo GetOutputPath()
    {
        Console.WriteLine("Enter full or relative path of desired output file:");
        var path = Console.ReadLine();
        FileInfo fileInfo = new FileInfo(path);
        return fileInfo;
    }
    
    public static void Main(string[] args)
    {
        DirectoryInfo directoryInfo;
        
        try
        {
           directoryInfo = GetInputDirectory();
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid path");
            return;
        }

        var div = _nodeOf.Div.Add();

        var statistics = RecursiveProcessor.GetMimeTypeStatistics(directoryInfo.FullName);
        long total = statistics.Aggregate((pair, valuePair) =>
        {
            return new KeyValuePair<string, long[]>("", new[] {pair.Value[0] + valuePair.Value[0]});
        }).Value[0];
        
        var mimeUl = HtmlHelper.CreateUnorderedList();
        foreach (var (key, value) in statistics)
        {
            mimeUl = mimeUl.Add(HtmlHelper.GetStatisticsListElement(key, value[0], total, value[1]));
        }
        div = div.Add(HtmlHelper.CreateH3("Mime Statistics"), mimeUl)
            .AddClass("p-4");
        
        
        Dictionary<string, Entry> entries = new Dictionary<string, Entry>();
        Dictionary<string, List<string>> adjList = new Dictionary<string, List<string>>();
        RecursiveProcessor.Process(directoryInfo.FullName, entries, adjList);

        var treeUl = HtmlHelper.CreateUnorderedList();

        Dfs(entries[directoryInfo.FullName], ref treeUl);

        void Dfs(Entry entry, ref Tag list)
        {
            list = list.Add(HtmlHelper.GetFolderListItem(entry.Name, entry.Size));
            var subUl = HtmlHelper.CreateUnorderedList().AddClass("pl-4");
            
            foreach (var s in adjList[entry.FullPath])
            {
                if (entries[s].Type == EntryType.Folder)
                {
                    Dfs(entries[s], ref subUl);
                }
                else
                {
                    subUl = subUl.Add(HtmlHelper.GetFileListElement(entries[s].Name, entries[s].Size, entries[s].MimeType));
                }
            }

            list = list.Add(subUl);
        }

        div = div.Add(HtmlHelper.CreateH3("Folder Structure"), treeUl);
        _document = _document.Add(_nodeOf.Body.Add(div));

        using var sw = new StreamWriter(GetOutputPath().FullName);
        sw.Write(_document.ToString());
    }
}