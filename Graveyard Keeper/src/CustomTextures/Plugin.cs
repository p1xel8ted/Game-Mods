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
        internal static ConfigEntry<bool> DumpSprites { get; private set; }

        public static Dictionary<string, string> LoadedCustomTextures { get; set; } = new();
        public static Dictionary<string, Texture2D> CachedTextureDict = new();
        internal static readonly HashSet<string> DumpedSprites = new();

        private void Awake()
        {
            Log = Logger;
            DumpSprites = Config.Bind("01. General", "Dump Sprites", false,
                new ConfigDescription("When enabled, exports all sprites encountered via SpriteAtlas as PNG files to a _dump folder inside the CustomTextures folder. Disable after collecting sprites."));
            LoadCustomTextures();
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            GYKHelper.StartupLogger.PrintModLoaded(PluginName, Log);
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
            foreach (var file in Directory.GetFiles(path, "*.png", SearchOption.AllDirectories))
            {
                var key = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                if (LoadedCustomTextures.ContainsKey(key))
                {
                    Log.LogWarning($"Duplicate texture name '{key}' — skipping {file}");
                    continue;
                }
                LoadedCustomTextures.Add(key, file);
                Log.LogInfo($"Found texture: {key}");
            }

            Log.LogInfo($"Loaded {LoadedCustomTextures.Count} custom textures");
        }

        internal static void DumpSprite(string atlasName, string spriteName, Sprite sprite)
        {
            var dumpKey = $"{atlasName}/{spriteName}";
            if (!DumpedSprites.Add(dumpKey)) return;

            try
            {
                var dumpDir = Path.Combine(Paths.PluginPath, "CustomTextures", "_dump", atlasName);
                Directory.CreateDirectory(dumpDir);
                var filePath = Path.Combine(dumpDir, $"{spriteName}.png");

                var rect = sprite.rect;
                var width = (int)rect.width;
                var height = (int)rect.height;
                if (width <= 0 || height <= 0) return;

                var sourceTexture = sprite.texture;

                // Atlas textures are typically not CPU-readable, so blit via RenderTexture
                var rt = RenderTexture.GetTemporary(sourceTexture.width, sourceTexture.height, 0, RenderTextureFormat.ARGB32);
                Graphics.Blit(sourceTexture, rt);

                var previous = RenderTexture.active;
                RenderTexture.active = rt;

                var readTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
                readTex.ReadPixels(new Rect(rect.x, rect.y, width, height), 0, 0);
                readTex.Apply();

                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(rt);

                File.WriteAllBytes(filePath, readTex.EncodeToPNG());
                Object.Destroy(readTex);
            }
            catch (System.Exception ex)
            {
                Log.LogWarning($"Failed to dump sprite {dumpKey}: {ex.Message}");
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