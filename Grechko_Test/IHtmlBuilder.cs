namespace Grechko_Test;

public interface IHtmlBuilder
{
    public void AddMimeStatistics(DirectoryInfo directoryInfo);
    public void AddFolderStructure(DirectoryInfo directoryInfo);
    public void Write(FileInfo fileInfo);
}