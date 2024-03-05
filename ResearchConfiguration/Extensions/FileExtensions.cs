using System.Reflection;

namespace ResearchConfiguration.Extensions;

public static class FileExtensions
{
    public static string GetExeDirectory()
    {
        var codeBase = Assembly.GetExecutingAssembly().Location;
        var path = Path.GetDirectoryName(codeBase);
        if (path is null) throw new Exception("Can not get exe path");
        return path;
    }

    public static string GetDirPath(string exeDirectoryPath, string directoryName)
    {
        var dir = Path.Combine(exeDirectoryPath, directoryName);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        return dir;
    }

    public static string GetProjectDirectory()
    {
        const string msg = "Project directory is not reachable. Perhaps program is released.";
        var exeDirectory = GetExeDirectory();
        var level1Dir = Directory.GetParent(exeDirectory) ?? throw new NullReferenceException(msg);
        var level2Dir = level1Dir.Parent ?? throw new NullReferenceException(msg);
        var level3Dir = level2Dir.Parent ?? throw new NullReferenceException(msg);
        return level3Dir.FullName;
    }
}