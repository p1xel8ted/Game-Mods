using UnityEngine;

namespace Shared;

//credits to Morthy for the general code below
public static class SpriteUtil
{
    private static Texture2D CreateTexture(byte[] data)
    {
        var texture = new Texture2D(1, 1);
        texture.LoadImage(data);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }

    public static Sprite CreateSprite(byte[] textureData, string name, Vector2? pivot = null, SpriteMeshType meshType = SpriteMeshType.Tight, float pixelsPerUnit = 24f)
    {
        var texture = CreateTexture(textureData);
        texture.name = name;

        pivot ??= new Vector2(0.5f, 0.5f);

        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot.Value, pixelsPerUnit, 0, meshType);
        sprite.name = name;
        return sprite;
    }
}