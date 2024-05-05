using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shared;

public static class FileLoader
{
    private static string GetResourcePath(Assembly assembly, string name)
    {
        var resourcePath = name;
        if (name.StartsWith(assembly.GetName().Name)) return resourcePath;
        resourcePath = assembly.GetManifestResourceNames()
            .FirstOrDefault(str => str.EndsWith(name));

        if (resourcePath == null)
            throw new InvalidOperationException("Resource not found: " + name);

        return resourcePath;
    }

    public static string LoadFile(Assembly assembly, string name)
    {
        using var stream = assembly.GetManifestResourceStream(GetResourcePath(assembly, name));
        if (stream == null)
            throw new InvalidOperationException("Resource stream not found for: " + name);

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static byte[] LoadFileBytes(Assembly assembly, string name)
    {
        using var stream = assembly.GetManifestResourceStream(GetResourcePath(assembly, name));
        if (stream == null)
            throw new InvalidOperationException("Resource stream not found for: " + name);

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}