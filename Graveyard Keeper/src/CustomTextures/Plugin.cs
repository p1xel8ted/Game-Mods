using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


namespace CustomTextures
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.gyk.customtexutures";
        private const string PluginName = "Custom Textures";
        private const string PluginVer = "0.1.1";

        internal static ManualLogSource Log { get; private set; }

        public static Dictionary<string, string> LoadedCustomTextures { get; set; }= new();
        public static Dictionary<string, Texture2D> CachedTextureDict = new();

        private void Awake()
        {
            Log = Logger;
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
                Log.LogWarning($"Creating directory {path}");
                Directory.CreateDirectory(path);
                return;
            }

            Log.LogWarning($"Loading textures from {path}");
            foreach (var file in Directory.GetFiles(path, "*.png", SearchOption.AllDirectories))
            {
                LoadedCustomTextures.Add(Path.GetFileNameWithoutExtension(file), file);
                Log.LogWarning($"Found texture {Path.GetFileNameWithoutExtension(file)}");
            }

            Log.LogWarning($"Loaded {LoadedCustomTextures.Count} textures");
        }

        internal static Texture2D GetTexture(Texture2D spriteTexture, string path)
        {
            if (CachedTextureDict.TryGetValue(path, out var tex))
            {
                Log.LogWarning($"found cached texture {path}, returning it: {tex.name}");
                return tex;
            }
            tex = new Texture2D(1, 1, spriteTexture.graphicsFormat, new TextureCreationFlags());
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