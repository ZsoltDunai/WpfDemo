using System.IO;

namespace WpfDemo.E2ETests.Infrastructure;

public sealed class AppExecutablePathResolver : IAppExecutablePathResolver
{
    private const string AppProjectName = "WpfDemo.App";

    public string Resolve()
    {
        var baseDirectory = AppContext.BaseDirectory;

        var candidates = new[]
        {
            Path.Combine(baseDirectory, $"{AppProjectName}.exe"),
            Path.Combine(baseDirectory, "..", "..", "..", "..", "src", AppProjectName, "bin", "Debug", "net10.0-windows", $"{AppProjectName}.exe"),
            Path.Combine(baseDirectory, "..", "..", "..", "..", "src", AppProjectName, "bin", "Release", "net10.0-windows", $"{AppProjectName}.exe"),
        };

        foreach (var candidate in candidates)
        {
            var fullPath = Path.GetFullPath(candidate);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        throw new FileNotFoundException(
            $"Could not find {AppProjectName}.exe. Build the solution before running E2E tests.");
    }
}
