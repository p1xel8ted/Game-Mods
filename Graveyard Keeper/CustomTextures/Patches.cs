using System.IO;
using System.Linq;
using GYKHelper;
using HarmonyLib;
using UnityEngine;
using UnityEngine.U2D;

namespace CustomTextures;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpriteAtlas), nameof(SpriteAtlas.GetSprites), typeof(Sprite[]))]
    private static void SpriteAtlas_GetSprites(Sprite[] sprites)
    {
        for (var i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];
            if (sprite == null) continue;
            var text = sprite.name.Replace("(Clone)", "").ToLower();
            if (!Plugin.LoadedCustomTextures.TryGetValue(text, out var texture)) continue;
            var tex = Plugin.GetTexture(sprite.texture, texture); // returns Texture2D

            tex.Resize(sprite.texture.width, sprite.texture.height);
            tex.Apply();

            var newSprite = Sprite.Create(tex, sprite.rect, new Vector2(sprite.rect.width / 2f, sprite.rect.height / 2f), sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, sprite.border, true);
            newSprite.name = sprite.name;

            sprites[i] = newSprite; // Replace the reference in the array
            Plugin.Log.LogWarning($"Replaced sprite {sprite.name} with {newSprite.name}");
        }
    }


    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(EasySpritesCollection), nameof(EasySpritesCollection.OnAtlasLoaded))]
    // private static bool EasySpritesCollection_OnAtlasLoaded(Object atlas)
    // {
    //     var spriteAtlas = atlas as SpriteAtlas;
    //     if (spriteAtlas == null)
    //     {
    //         Debug.LogError("OnAtlasLoaded: atlas is null");
    //         return true;
    //     }
    //     Debug.Log("OnAtlasLoaded " + spriteAtlas.name + ", sprites: " + spriteAtlas.spriteCount.ToString(), spriteAtlas);
    //     var array = new Sprite[spriteAtlas.spriteCount];
    //     spriteAtlas.GetSprites(array);
    //     foreach (var sprite in array)
    //     {
    //         var text = sprite.name.Replace("(Clone)", "").ToLower();
    //         if (EasySpritesCollection.hash.ContainsKey(text))
    //         {
    //             Debug.LogWarning("Error adding sprite to a library - duplicate name: " + text);
    //         }
    //         else
    //         {
    //             EasySpritesCollection.hash.Add(text, sprite);
    //         }
    //     }
    //
    //     return false;
    // }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(EasySpritesCollection), nameof(EasySpritesCollection.CreateHash))]
    // private static void EasySpritesCollection_CreateHash()
    // {
    //     foreach (var sprite in EasySpritesCollection.hash)
    //     {
    //         Plugin.Log.LogWarning($"EasySpritesCollection hash: Key: {sprite.Key}, Name: {sprite.Value.name}, Name: {sprite.Value.texture.name}");   
    //     }
    //     
    //     foreach (var sprite in EasySpritesCollection.hash_sub)
    //     {
    //         Plugin.Log.LogWarning($"EasySpritesCollection hash_sub: Key: {sprite.Key}, Int: {sprite.Value}");   
    //     }
    // }
}