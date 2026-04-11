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
            if (!sprite) continue;

            var cleanName = sprite.name.Replace("(Clone)", "").Trim();
            var key = cleanName.ToLowerInvariant();

            if (Plugin.DumpSprites.Value)
            {
                Plugin.DumpSprite(cleanName, sprite);
            }

            if (!Plugin.LoadedCustomTextures.TryGetValue(key, out var texturePath)) continue;

            try
            {
                var tex = Plugin.GetTexture(sprite.texture, texturePath);
                var originalRect = sprite.rect;
                var ppu = sprite.pixelsPerUnit;
                var offset = sprite.packed ? sprite.textureRectOffset : Vector2.zero;

                // sprite.pivot is in pixels relative to sprite.rect (the logical rect, e.g. 96x96)
                // For tightly-packed sprites, the actual content is a smaller region within that rect,
                // offset by textureRectOffset from the bottom-left.
                //
                // If custom texture matches logical rect size (96x96): use pivot as-is
                // If custom texture matches trimmed content size (22x23): adjust pivot for the offset
                Vector2 normalizedPivot;
                if (Mathf.Abs(tex.width - originalRect.width) <= 1 && Mathf.Abs(tex.height - originalRect.height) <= 1)
                {
                    // Full-size replacement — pivot maps directly
                    normalizedPivot = new Vector2(sprite.pivot.x / originalRect.width, sprite.pivot.y / originalRect.height);
                }
                else
                {
                    // Trimmed-size replacement — pivot must account for the content offset
                    var pivotInContent = new Vector2(sprite.pivot.x - offset.x, sprite.pivot.y - offset.y);
                    normalizedPivot = new Vector2(pivotInContent.x / tex.width, pivotInContent.y / tex.height);
                }

                if (Plugin.Debug.Value)
                {
                    var tr = sprite.textureRect;
                    Plugin.Log.LogInfo($"Replace '{cleanName}': logicalRect=({originalRect.width}x{originalRect.height}), atlasRect=({tr.width:F0}x{tr.height:F0}), offset=({offset.x:F1},{offset.y:F1}), pivot=({sprite.pivot.x},{sprite.pivot.y}), normalizedPivot=({normalizedPivot.x:F3},{normalizedPivot.y:F3}), customTex=({tex.width}x{tex.height})");
                }

                var newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), normalizedPivot, ppu, 0, SpriteMeshType.FullRect, sprite.border, false);
                newSprite.name = sprite.name;

                sprites[i] = newSprite;
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"Failed to replace sprite '{cleanName}' from '{texturePath}': {ex.Message}");
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EasySpritesCollection), nameof(EasySpritesCollection.GetSprite), typeof(string), typeof(bool), typeof(string))]
    private static void EasySpritesCollection_GetSprite(string sprite_name, Sprite __result)
    {
        if (!Plugin.DumpSprites.Value) return;
        if (__result == null) return;
        if (string.IsNullOrEmpty(sprite_name)) return;

        var cleanName = sprite_name.Replace("(Clone)", "").Trim();
        Plugin.DumpSprite(cleanName, __result);
    }
}
