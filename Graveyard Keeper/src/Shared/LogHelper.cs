using BepInEx.Logging;

namespace Shared;

internal static class LogHelper
{
    internal static ManualLogSource Log;

    internal static void Info(string message) => Log?.LogInfo(message);
    internal static void Warning(string message) => Log?.LogWarning(message);
    internal static void Error(string message) => Log?.LogError(message);
}
