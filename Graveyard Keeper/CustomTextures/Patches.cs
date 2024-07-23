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
}