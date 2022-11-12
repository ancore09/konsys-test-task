namespace Grechko_Test.Utilities;

public static class RecursiveProcessor
{
    public static long Process(string targetDirectory, Dictionary<string, Entry> dictionary, Dictionary<string, List<string>> tree)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
        dictionary.TryAdd(dirInfo.FullName,
            new Entry()
            {
                FullPath = dirInfo.FullName, Name = dirInfo.Name, Size = 0, Type = EntryType.Folder
            });
        tree.TryAdd(dirInfo.FullName, new List<string>());
        
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        long size = 0;
        foreach (string fileName in fileEntries)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            size += fileInfo.Length;
            dictionary.TryAdd(fileInfo.FullName,
                new Entry()
                {
                    FullPath = fileInfo.FullName, Name = fileInfo.Name, Size = fileInfo.Length, Type = EntryType.File,
                    MimeType = MimeTypes.GetMimeType(fileInfo.Name)
                });
            tree[dirInfo.FullName].Add(fileInfo.FullName);
        }

        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            tree[dirInfo.FullName].Add(subdirectory);
            var subSize = Process(subdirectory, dictionary, tree);
            size += subSize;
        }

        dictionary[dirInfo.FullName].Size = size;
        return size;
    }

    public static Dictionary<string, Entry> GetAllEntries(string targetDirectory)
    {
        var dict = new Dictionary<string, Entry>();
        GetAllEntries(targetDirectory, dict);
        return dict;
    }
    
    public static long GetAllEntries(string targetDirectory, Dictionary<string, Entry> dictionary)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
        dictionary.TryAdd(dirInfo.FullName,
            new Entry()
            {
                FullPath = dirInfo.FullName, Name = dirInfo.Name, Size = 0, Type = EntryType.Folder
            });
        
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        long size = 0;
        foreach (string fileName in fileEntries)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            size += fileInfo.Length;
            dictionary.TryAdd(fileInfo.FullName,
                new Entry()
                {
                    FullPath = fileInfo.FullName, Name = fileInfo.Name, Size = fileInfo.Length, Type = EntryType.File
                });
        }

        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            var subSize = GetAllEntries(subdirectory, dictionary);
            size += subSize;
        }

        dictionary[dirInfo.FullName].Size = size;
        return size;
    }

    public static Dictionary<string, List<string>> GetDirectoriesRelations(string targetDirectory)
    {
        var dict = new Dictionary<string, List<string>>();
        GetDirectoriesRelations(targetDirectory, dict);
        return dict;
    }

    private static void GetDirectoriesRelations(string targetDirectory, Dictionary<string, List<string>> tree)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
        tree.TryAdd(dirInfo.FullName, new List<string>());
        
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            tree[dirInfo.FullName].Add(fileInfo.FullName);
        }

        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            tree[dirInfo.FullName].Add(subdirectory);
            GetDirectoriesRelations(subdirectory, tree);
        }
    }

    public static Dictionary<string, long[]> GetMimeTypeStatistics(string targetDirectory)
    {
        var statistics = new Dictionary<string, long[]>();
        GetMimeTypeStatistics(targetDirectory, statistics);
        return statistics;
    }
    
    private static void GetMimeTypeStatistics(string targetDirectory, Dictionary<string, long[]> dict)
    {
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (!dict.TryAdd(MimeTypes.GetMimeType(fileInfo.Name), new long[] {1, fileInfo.Length}))
            {
                dict[MimeTypes.GetMimeType(fileInfo.Name)][0] += 1;
                dict[MimeTypes.GetMimeType(fileInfo.Name)][1] += fileInfo.Length;
            }
        }

        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            GetMimeTypeStatistics(subdirectory, dict);
        }
    }
}