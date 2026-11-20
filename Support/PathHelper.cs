namespace ShopAutomation.Support;

/// <summary>Stateless file-system helpers.</summary>
public static class PathHelper
{
    /// <summary>Project root (folder holding the .csproj) so report + screenshots land next to the source, not in bin/.</summary>
    public static string RootDir
    {
        get
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !dir.GetFiles("*.csproj").Any()) dir = dir.Parent;
            return dir?.FullName ?? AppContext.BaseDirectory;
        }
    }

    public static string Sanitize(string s) => string.Concat(s.Select(c => char.IsLetterOrDigit(c) ? c : '_')).Trim('_');
}
