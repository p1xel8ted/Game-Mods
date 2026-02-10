using HarmonyLib;
using UnityEngine;
using UnityEngine.U2D;

namespace CustomTextures;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpriteAtlas), nameof(SpriteAtlas.GetSprites), typeof(Sprite[]))]
    private static void SpriteAtlas_GetSprites(SpriteAtlas __instance, Sprite[] sprites)
    {
        for (var i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];
            if (sprite == null) continue;

            var cleanName = sprite.name.Replace("(Clone)", "").Trim();
            var key = cleanName.ToLowerInvariant();

            if (Plugin.DumpSprites.Value)
            {
                Plugin.DumpSprite(__instance.name, cleanName, sprite);
            }

            if (!Plugin.LoadedCustomTextures.TryGetValue(key, out var texturePath)) continue;

            var tex = Plugin.GetTexture(sprite.texture, texturePath);
            var rect = sprite.rect;
            var normalizedPivot = new Vector2(sprite.pivot.x / rect.width, sprite.pivot.y / rect.height);

            var newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), normalizedPivot, sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, sprite.border, true);
            newSprite.name = sprite.name;

            sprites[i] = newSprite;
            Plugin.Log.LogInfo($"Replaced sprite: {__instance.name}/{cleanName}");
        }
    }
}