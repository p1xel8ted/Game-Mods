namespace CultOfQoL;

public static class ReflectionHelper
{
    /// <summary>
    /// Gets the name of the class that called the method that called this method.
    /// </summary>
    /// <param name="framesToSkip">Number of frames to skip. Default is 2 (caller of caller).</param>
    /// <returns>The full name of the calling class, or "Unknown" if it cannot be determined.</returns>
    public static string GetCallingClassName(int framesToSkip = 2)
    {
        var stackTrace = new StackTrace();
        var frame = stackTrace.GetFrame(framesToSkip);
        if (frame == null) return "Unknown";
        var method = frame.GetMethod();
        if (method == null) return "Unknown";
        return method.DeclaringType != null ? method.DeclaringType.FullName : "Unknown";
    }
    
}