using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;

namespace CustomTextures
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.gyk.customtextures";
        private const string PluginName = "Custom Textures";
        private const string PluginVer = "0.1.2";

        internal static ManualLogSource Log { get; private set; }
        internal static ConfigEntry<bool> Debug { get; private set; }
        internal static ConfigEntry<bool> DumpSprites { get; private set; }

        public static Dictionary<string, string> LoadedCustomTextures { get; set; } = new();
        private static readonly Dictionary<string, Texture2D> CachedTextureDict = new();
        private static readonly HashSet<string> DumpedSprites = [];

        private void Awake()
        {
            Log = Logger;
            Debug = Config.Bind("00. Advanced", "Debug Logging", false, "Enable or disable debug logging.");
            DumpSprites = Config.Bind("01. General", "Dump Sprites", false,
                new ConfigDescription("When enabled, exports all sprites encountered via SpriteAtlas as PNG files to a _dump folder inside the CustomTextures folder. Disable after collecting sprites."));
            LoadCustomTextures();
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        }

        private static void LoadCustomTextures()
        {
            LoadedCustomTextures.Clear();
            var path = Path.Combine(Paths.PluginPath, "CustomTextures");

            if (!Directory.Exists(path))
            {
                Log.LogInfo($"Creating directory {path}");
                Directory.CreateDirectory(path);
                return;
            }

            Log.LogInfo($"Loading textures from {path}");
            var dumpDir = Path.Combine(path, "_dump") + Path.DirectorySeparatorChar;
            foreach (var file in Directory.GetFiles(path, "*.png", SearchOption.AllDirectories))
            {
                if (file.StartsWith(dumpDir)) continue;
                var key = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                if (LoadedCustomTextures.ContainsKey(key))
                {
                    Log.LogWarning($"Duplicate texture name '{key}' — skipping {file}");
                    continue;
                }
                LoadedCustomTextures.Add(key, file);
                if (Debug.Value)
                {
                    Log.LogInfo($"Found texture: {key}");
                }
            }

            Log.LogInfo($"Loaded {LoadedCustomTextures.Count} custom textures");
        }

        internal static void DumpSprite(string spriteName, Sprite sprite)
        {
            if (!DumpedSprites.Add(spriteName)) return;

            try
            {
                var dumpDir = Path.Combine(Paths.PluginPath, "CustomTextures", "_dump");
                Directory.CreateDirectory(dumpDir);
                var filePath = Path.Combine(dumpDir, $"{spriteName}.png");

                var rect = sprite.textureRect;
                var width = (int)rect.width;
                var height = (int)rect.height;
                if (width <= 0 || height <= 0) return;

                var sourceTexture = sprite.texture;

                if (Debug.Value)
                {
                    Log.LogInfo($"Dump '{spriteName}': rect=({rect.x},{rect.y},{rect.width},{rect.height}), texture=({sourceTexture.width}x{sourceTexture.height}), packed={sprite.packed}");
                }

                // Atlas textures are typically not CPU-readable, so blit via RenderTexture
                var rt = RenderTexture.GetTemporary(sourceTexture.width, sourceTexture.height, 0, RenderTextureFormat.ARGB32);
                Graphics.Blit(sourceTexture, rt);

                var previous = RenderTexture.active;
                RenderTexture.active = rt;

                // Blit flips Y, so flip the read coordinate
                var flippedY = sourceTexture.height - rect.y - height;

                var readTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
                readTex.ReadPixels(new Rect(rect.x, flippedY, width, height), 0, 0);
                readTex.Apply();

                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(rt);

                File.WriteAllBytes(filePath, readTex.EncodeToPNG());
                Object.Destroy(readTex);

                if (Debug.Value)
                {
                    Log.LogInfo($"Dumped sprite: {spriteName}");
                }
            }
            catch (System.Exception ex)
            {
                Log.LogWarning($"Failed to dump sprite '{spriteName}': {ex.Message}");
            }
        }

        internal static Texture2D GetTexture(Texture2D spriteTexture, string path)
        {
            if (CachedTextureDict.TryGetValue(path, out var tex))
            {
                return tex;
            }
            tex = new Texture2D(2, 2);
            tex.LoadImage(File.ReadAllBytes(path));
            tex.filterMode = spriteTexture.filterMode;
            tex.wrapMode = spriteTexture.wrapMode;
            tex.wrapModeU = spriteTexture.wrapModeU;
            tex.wrapModeV = spriteTexture.wrapModeV;
            tex.wrapModeW = spriteTexture.wrapModeW;
            CachedTextureDict[path] = tex;
            return tex;
        }
    }
}