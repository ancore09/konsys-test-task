using System.Text.Encodings.Web;
using Grechko_Test.Models;
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
        _document = HtmlHelper.CreateHtml("KonSys Test Task");
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
        DirectoryInfo inputDirectoryInfo = GetInputDirectory();
        FileInfo outputFileInfo = GetOutputPath();

        IHtmlBuilder builder = new HtmlBuilder();
        builder.AddMimeStatistics(inputDirectoryInfo);
        builder.AddFolderStructure(inputDirectoryInfo);
        builder.Write(outputFileInfo);
    }
}