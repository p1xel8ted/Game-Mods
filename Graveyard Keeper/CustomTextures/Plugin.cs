using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx.Logging;
using GYKHelper;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;


namespace CustomTextures
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.gyk.customtexutures";
        private const string PluginName = "Custom Textures";
        private const string PluginVer = "0.1.0";

        internal static ManualLogSource Log { get; private set; }

        public static Dictionary<string, string> LoadedCustomTextures { get; set; }= new Dictionary<string, string>();
        public static Dictionary<string, Texture2D> cachedTextureDict = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Sprite> cachedSprites = new Dictionary<string, Sprite>();

        private void Awake()
        {
            Log = Logger;
            LoadCustomTextures();
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        }
        
        private static void LoadCustomTextures()
        {
            LoadedCustomTextures.Clear();
            var path = Path.Combine(BepInEx.Paths.PluginPath, "CustomTextures");

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
            if (cachedTextureDict.TryGetValue(path, out var tex))
            {
                //Log.LogWarning($"found cached texture {path}, returning it: {tex.name}");
                return tex;
            }
            tex = new Texture2D(1, 1, spriteTexture.graphicsFormat, new TextureCreationFlags());
            tex.LoadImage(File.ReadAllBytes(path));
            tex.filterMode = spriteTexture.filterMode;
            tex.wrapMode = spriteTexture.wrapMode;
            tex.wrapModeU = spriteTexture.wrapModeU;
            tex.wrapModeV = spriteTexture.wrapModeV;
            tex.wrapModeW = spriteTexture.wrapModeW;
            cachedTextureDict[path] = tex;
            return tex;
        }


        // internal static Sprite TryGetReplacementSprite(Sprite oldSprite, string spriteName)
        // {
        //     if (oldSprite == null)
        //     {
        //         Log.LogWarning("oldSprite is null");
        //         return null;
        //     }
        //
        //     if (oldSprite.texture == null)
        //     {
        //         Log.LogWarning($"oldSprite.texture is null {oldSprite.name}");
        //         return oldSprite;
        //     }
        //
        //     Log.LogWarning($"TryGetReplacementSprite: FileName: {spriteName}, SpriteName: {oldSprite.name}, TextureName: {oldSprite.texture.name}");
        //     var textureName = oldSprite.texture.name;
        //
        //     if (cachedSprites.TryGetValue(spriteName, out var newSprite))
        //     {
        //         Log.LogWarning($"found cached sprite {oldSprite.name} {textureName}");
        //         return newSprite;
        //     }
        //
        //     if (LoadedCustomTextures.TryGetValue(spriteName, out var path))
        //     {
        //         Log.LogWarning($"replacing sprite {spriteName}");
        //         var newTex = GetTexture(path);
        //         newTex.name = textureName; // Assign the new texture the name of the old texture
        //
        //         // Ensure that the sprite dimensions do not exceed the texture dimensions
        //         var spriteRect = oldSprite.rect;
        //         float textureWidth = newTex.width;
        //         float textureHeight = newTex.height;
        //         var pivot = new Vector2(oldSprite.pivot.x / spriteRect.width, oldSprite.pivot.y / spriteRect.height);
        //         if (spriteRect.width > textureWidth || spriteRect.height > textureHeight)
        //         {
        //             Log.LogWarning($"Sprite dimensions ({spriteRect.width}x{spriteRect.height}) exceed texture dimensions ({textureWidth}x{textureHeight}). Adjusting sprite size.");
        //             spriteRect.size = new Vector2(textureWidth, textureHeight);
        //         }
        //         newSprite = Sprite.Create(newTex, spriteRect, pivot, oldSprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, oldSprite.border, true);
        //         newSprite.name = oldSprite.name;
        //         cachedSprites[spriteName] = newSprite; 
        //         return newSprite;
        //     }
        //
        //
        //     return oldSprite;
        // }
    }
}