using System.Runtime.CompilerServices;
using Grechko_Test.Models;
using Grechko_Test.Utilities;
using Maroontress.Html;

namespace Grechko_Test;

public class HtmlBuilder: IHtmlBuilder
{
    private NodeFactory _nodeOf;
    private Tag _document;
    private Tag _container;

    public HtmlBuilder()
    {
        _nodeOf = Nodes.NewFactory();
        _document = HtmlHelper.CreateHtml("KonSys Test Task");
        _container = _nodeOf.Div.Add();
    }

    public void AddMimeStatistics(DirectoryInfo directoryInfo)
    {
        var statistics = RecursiveProcessor.GetMimeTypeStatistics(directoryInfo.FullName);
        var totalSize = RecursiveProcessor.CalculateTotalSize(statistics);
        
        // list for displaying MimeType statistics
        var mimeUl = HtmlHelper.CreateUnorderedList();
        foreach (var (key, value) in statistics)
        {
            mimeUl = mimeUl.Add(HtmlHelper.GetStatisticsListElement(key, value[0], totalSize, value[1]));
        }
        _container = _container.Add(HtmlHelper.CreateH3("Mime Statistics"), mimeUl).AddClass("p-4");
    }

    public void AddFolderStructure(DirectoryInfo directoryInfo)
    {
        Dictionary<string, Entry> entries = new Dictionary<string, Entry>();
        Dictionary<string, List<string>> adjList = new Dictionary<string, List<string>>();
        RecursiveProcessor.Process(directoryInfo.FullName, entries, adjList);
        
        // list for displaying folder structure
        var treeUl = HtmlHelper.CreateUnorderedList();

        // recursively add file entry elements to treeUl
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

        // append treeUl to the container and to the html document
        _container = _container.Add(HtmlHelper.CreateH3("Folder Structure"), treeUl);
        _document = _document.Add(_nodeOf.Body.Add(_container));
    }

    public void Write(FileInfo fileInfo)
    {
        using var sw = new StreamWriter(fileInfo.FullName);
        sw.Write(_document.ToString());
    }
    
}