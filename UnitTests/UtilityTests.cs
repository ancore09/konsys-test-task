using Grechko_Test;
using Grechko_Test.Utilities;

namespace UnitTests;


[TestClass]
public class UtilityTests
{
    [TestMethod]
    public void Html_BoilerPlate_Creation()
    {
        string title = "test-title";
        string? actual = HtmlHelper.CreateHtml(title).ToString();
        string expected =
            $"<html><head><title>{title}</title><script src=\"https://cdn.tailwindcss.com\"></script></head></html>";

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void File_List_Element_Creation()
    {
        string name = "test-name";
        long size = 12345678;
        string mime = "test/test";

        string? actual = HtmlHelper.GetFileListElement(name, size, mime).ToString();
        string expected =
            $"<li><div class=\"inline-block\"><p class=\"text-base font-normal\">{name}</p><div class=\"flex\"><p class=\"mr-2 text-sm font-light\">{size} bytes</p><p class=\"mx-2 text-sm font-light\">{mime}</p></div></div></li>";
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Folder_List_Element_Creation()
    {
        string name = "test-name";
        long size = 12345678;

        string? actual = HtmlHelper.GetFolderListItem(name, size).ToString();
        string expected = $"<li class=\"text-lg font-semibold\">{name} | {size} bytes</li>";
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Directory_Statistics()
    {
        string path = "./test";

        Dictionary<string, long[]> expected = new Dictionary<string, long[]>
        {
            {"application/octet-stream", new long[] {2, 12296}},
            {"text/yaml", new long[] {4, 1024}}
        };

        var actual = RecursiveProcessor.GetMimeTypeStatistics(path);

        foreach (var key in actual.Keys)
        {
            Assert.AreEqual(expected[key][0], actual[key][0]);
            Assert.AreEqual(expected[key][1], actual[key][1]);
        }
    }

    [TestMethod]
    public void Statistics_Element_Creation()
    {
        string mimeType = "test/test";
        long amount = 12;
        long total = 23;
        long totalSize = 123456;

        var actual = HtmlHelper.GetStatisticsListElement(mimeType, amount, total, totalSize).ToString();
        var expected = "<li>test/test: 12 | 52,17% | 10288 bytes</li>";

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Ul_Element_Creation()
    {
        var actual = HtmlHelper.CreateUnorderedList().ToString();
        var expected = "<ul class=\"list-none list-inside\"></ul>";

        Assert.AreEqual(expected, actual);
    }
    
    [TestMethod]
    public void H3_Element_Creation()
    {
        string text = "test";
        var actual = HtmlHelper.CreateH3(text).ToString();
        var expected = $"<h3 class=\"text-3xl font-semibold mt-4\">{text}</h3>";

        Assert.AreEqual(expected, actual);
    }
}