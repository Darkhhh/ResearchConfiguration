using ResearchConfiguration.Extensions;

namespace ResearchConfiguration.Configuration;

public static class Loader
{
    public static Reader[] Load(string configsDirectoryName, bool getFromProjectDirectory = true)
    {
        string dir;
        string[] configNames;
        if (getFromProjectDirectory)
        {
            var str = FileExtensions.GetProjectDirectory();
            dir = Path.Combine(str, configsDirectoryName);
            configNames = Directory.GetFiles(dir, "*.xml");
        }
        else
        {
            dir = FileExtensions.GetDirPath(FileExtensions.GetExeDirectory(), configsDirectoryName);
            configNames = Directory.GetFiles(dir, "*.xml");
        }

        var result = new Reader[configNames.Length];
        for (var i = 0; i < configNames.Length; i++)
        {
            result[i] = new Reader(configNames[i]);
        }

        return result;
    }

    public static Reader Load(string configName, string configsDirectoryName, bool getFromProjectDirectory = true)
    {
        string dir;
        string[] configNames;
        if (getFromProjectDirectory)
        {
            var str = FileExtensions.GetProjectDirectory();
            dir = Path.Combine(str, configsDirectoryName);
            configNames = Directory.GetFiles(dir, "*.xml");
        }
        else
        {
            dir = FileExtensions.GetDirPath(FileExtensions.GetExeDirectory(), configsDirectoryName);
            configNames = Directory.GetFiles(dir, "*.xml");
        }

        const string extension = ".xml";
        if (!configName.Contains(extension)) configName += extension;

        foreach (var path in configNames)
        {
            if (Path.GetFileName(path) == configName) return new Reader(path);
        }

        throw new Exception($"There is no file with name {configName}");
    }
}