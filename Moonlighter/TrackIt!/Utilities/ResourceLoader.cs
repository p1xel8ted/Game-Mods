using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TrackIt;

public static class ResourceLoader
{
    public static Sprite LoadSpriteFromEmbeddedResourceByName(string fileName)
    {
        var resourceName = FindResourceNameByFileName(fileName);
        if (string.IsNullOrEmpty(resourceName))
        {
            Plugin.LOG.LogError($"Failed to find the embedded resource: {fileName}");
            return null;
        }

        var imageData = GetEmbeddedResourceBytes(resourceName);
        if (imageData == null)
        {
            Plugin.LOG.LogError("Failed to load the embedded resource.");
            return null;
        }

        var texture = new Texture2D(8, 8);
        if (texture.LoadImage(imageData))
        {
            texture.filterMode = FilterMode.Point;
            var sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);
            return sprite;
        }
        
        Plugin.LOG.LogError("Failed to create texture from embedded resource.");
        return null;
    }
    
    private static byte[] GetEmbeddedResourceBytes(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            Plugin.LOG.LogError($"Resource not found: {resourceName}");
            return null;
        }

        var buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        return buffer;
    }

    private static string FindResourceNameByFileName(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var allResourceNames = assembly.GetManifestResourceNames();
        return allResourceNames.FirstOrDefault(name => name.EndsWith(fileName));
    }
}