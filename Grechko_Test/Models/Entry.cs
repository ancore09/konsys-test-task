namespace Grechko_Test.Models;

public class Entry
{
    public EntryType Type { get; set; }
    public long Size { get; set; }
    public string Name { get; set; }
    public string FullPath { get; set; }
    public string MimeType { get; set; }
}