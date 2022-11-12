using Grechko_Test.Models;

namespace Grechko_Test.Utilities;

public static class RecursiveProcessor
{
    public static long Process(string targetDirectory, Dictionary<string, Entry> dictionary, Dictionary<string, List<string>> tree)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
        dictionary.TryAdd(dirInfo.FullName,
            new Entry()
            {
                FullPath = dirInfo.FullName, 
                Name = dirInfo.Name, 
                Size = 0, 
                Type = EntryType.Folder
            });
        tree.TryAdd(dirInfo.FullName, new List<string>());
        
        long size = 0;
        foreach (string fileName in Directory.GetFiles(targetDirectory))
        {
            FileInfo fileInfo = new FileInfo(fileName);
            size += fileInfo.Length;
            dictionary.TryAdd(fileInfo.FullName,
                new Entry()
                {
                    FullPath = fileInfo.FullName, 
                    Name = fileInfo.Name, 
                    Size = fileInfo.Length, 
                    Type = EntryType.File,
                    MimeType = MimeTypes.GetMimeType(fileInfo.Name)
                });
            tree[dirInfo.FullName].Add(fileInfo.FullName);
        }

        foreach (string subdirectory in Directory.GetDirectories(targetDirectory))
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
    
    private static long GetAllEntries(string targetDirectory, Dictionary<string, Entry> dictionary)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
        dictionary.TryAdd(dirInfo.FullName,
            new Entry()
            {
                FullPath = dirInfo.FullName, 
                Name = dirInfo.Name, 
                Size = 0, 
                Type = EntryType.Folder
            });
        
        long size = 0;
        foreach (string fileName in Directory.GetFiles(targetDirectory))
        {
            FileInfo fileInfo = new FileInfo(fileName);
            size += fileInfo.Length;
            dictionary.TryAdd(fileInfo.FullName,
                new Entry()
                {
                    FullPath = fileInfo.FullName, 
                    Name = fileInfo.Name, 
                    Size = fileInfo.Length, 
                    Type = EntryType.File
                });
        }

        foreach (string subdirectory in Directory.GetDirectories(targetDirectory))
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
        
        foreach (string fileName in Directory.GetFiles(targetDirectory))
        {
            FileInfo fileInfo = new FileInfo(fileName);
            tree[dirInfo.FullName].Add(fileInfo.FullName);
        }

        foreach (string subdirectory in Directory.GetDirectories(targetDirectory))
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
    
    public static long CalculateTotalSize(Dictionary<string, long[]> statistics) {
        long totalSize = statistics.Aggregate((pair, valuePair) =>
        {
            return new KeyValuePair<string, long[]>("", new[] {pair.Value[0] + valuePair.Value[0]});
        }).Value[0];
        return totalSize;
    }
    
    private static void GetMimeTypeStatistics(string targetDirectory, Dictionary<string, long[]> dict)
    {
        foreach (string fileName in Directory.GetFiles(targetDirectory))
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (!dict.TryAdd(MimeTypes.GetMimeType(fileInfo.Name), new long[] {1, fileInfo.Length}))
            {
                dict[MimeTypes.GetMimeType(fileInfo.Name)][0] += 1;
                dict[MimeTypes.GetMimeType(fileInfo.Name)][1] += fileInfo.Length;
            }
        }

        foreach (string subdirectory in Directory.GetDirectories(targetDirectory))
        {
            GetMimeTypeStatistics(subdirectory, dict);
        }
    }
}