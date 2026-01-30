// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuleTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

#nullable disable
namespace UnityEngine;

[CreateAssetMenu]
[Serializable]
public class RuleTile : TileBase
{
  public Sprite m_DefaultSprite;
  public Tile.ColliderType m_DefaultColliderType = Tile.ColliderType.Sprite;
  [HideInInspector]
  public List<RuleTile.TilingRule> m_TilingRules;

  public override void GetTileData(Vector3Int position, ITilemap tileMap, ref TileData tileData)
  {
    tileData.sprite = this.m_DefaultSprite;
    tileData.colliderType = this.m_DefaultColliderType;
    tileData.flags = TileFlags.LockTransform;
    tileData.transform = Matrix4x4.identity;
    foreach (RuleTile.TilingRule tilingRule in this.m_TilingRules)
    {
      Matrix4x4 transform = Matrix4x4.identity;
      if (this.RuleMatches(tilingRule, position, tileMap, ref transform))
      {
        switch (tilingRule.m_Output)
        {
          case RuleTile.TilingRule.OutputSprite.Single:
          case RuleTile.TilingRule.OutputSprite.Animation:
            tileData.sprite = tilingRule.m_Sprites[0];
            break;
          case RuleTile.TilingRule.OutputSprite.Random:
            int index = Mathf.Clamp(Mathf.FloorToInt(RuleTile.GetPerlinValue(position, tilingRule.m_PerlinScale, 100000f) * (float) tilingRule.m_Sprites.Length), 0, tilingRule.m_Sprites.Length - 1);
            tileData.sprite = tilingRule.m_Sprites[index];
            if (tilingRule.m_RandomTransform != RuleTile.TilingRule.Transform.Fixed)
            {
              transform = RuleTile.ApplyRandomTransform(tilingRule.m_RandomTransform, transform, tilingRule.m_PerlinScale, position);
              break;
            }
            break;
        }
        tileData.transform = transform;
        tileData.colliderType = tilingRule.m_ColliderType;
        break;
      }
    }
  }

  public static float GetPerlinValue(Vector3Int position, float scale, float offset)
  {
    return Mathf.PerlinNoise(((float) position.x + offset) * scale, ((float) position.y + offset) * scale);
  }

  public override bool GetTileAnimationData(
    Vector3Int position,
    ITilemap tilemap,
    ref TileAnimationData tileAnimationData)
  {
    foreach (RuleTile.TilingRule tilingRule in this.m_TilingRules)
    {
      Matrix4x4 identity = Matrix4x4.identity;
      if (this.RuleMatches(tilingRule, position, tilemap, ref identity) && tilingRule.m_Output == RuleTile.TilingRule.OutputSprite.Animation)
      {
        tileAnimationData.animatedSprites = tilingRule.m_Sprites;
        tileAnimationData.animationSpeed = tilingRule.m_AnimationSpeed;
        return true;
      }
    }
    return false;
  }

  public override void RefreshTile(Vector3Int location, ITilemap tileMap)
  {
    if (this.m_TilingRules != null && this.m_TilingRules.Count > 0)
    {
      for (int y = -1; y <= 1; ++y)
      {
        for (int x = -1; x <= 1; ++x)
          base.RefreshTile(location + new Vector3Int(x, y, 0), tileMap);
      }
    }
    else
      base.RefreshTile(location, tileMap);
  }

  public bool RuleMatches(
    RuleTile.TilingRule rule,
    Vector3Int position,
    ITilemap tilemap,
    ref Matrix4x4 transform)
  {
    for (int angle = 0; angle <= (rule.m_RuleTransform == RuleTile.TilingRule.Transform.Rotated ? 270 : 0); angle += 90)
    {
      if (this.RuleMatches(rule, position, tilemap, angle))
      {
        transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, (float) -angle), Vector3.one);
        return true;
      }
    }
    if (rule.m_RuleTransform == RuleTile.TilingRule.Transform.MirrorX && this.RuleMatches(rule, position, tilemap, true, false))
    {
      transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
      return true;
    }
    if (rule.m_RuleTransform != RuleTile.TilingRule.Transform.MirrorY || !this.RuleMatches(rule, position, tilemap, false, true))
      return false;
    transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
    return true;
  }

  public static Matrix4x4 ApplyRandomTransform(
    RuleTile.TilingRule.Transform type,
    Matrix4x4 original,
    float perlinScale,
    Vector3Int position)
  {
    float perlinValue = RuleTile.GetPerlinValue(position, perlinScale, 200000f);
    switch (type)
    {
      case RuleTile.TilingRule.Transform.Rotated:
        return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, (float) -(Mathf.Clamp(Mathf.FloorToInt(perlinValue * 4f), 0, 3) * 90)), Vector3.one);
      case RuleTile.TilingRule.Transform.MirrorX:
        return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((double) perlinValue < 0.5 ? 1f : -1f, 1f, 1f));
      case RuleTile.TilingRule.Transform.MirrorY:
        return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, (double) perlinValue < 0.5 ? 1f : -1f, 1f));
      default:
        return original;
    }
  }

  public bool RuleMatches(
    RuleTile.TilingRule rule,
    Vector3Int position,
    ITilemap tilemap,
    int angle)
  {
    for (int y = -1; y <= 1; ++y)
    {
      for (int x = -1; x <= 1; ++x)
      {
        if (x != 0 || y != 0)
        {
          Vector3Int original = new Vector3Int(x, y, 0);
          int indexOfOffset = this.GetIndexOfOffset(this.GetRotatedPos(original, angle));
          TileBase tile = tilemap.GetTile(position + original);
          if (rule.m_Neighbors[indexOfOffset] == RuleTile.TilingRule.Neighbor.This && (Object) tile != (Object) this || rule.m_Neighbors[indexOfOffset] == RuleTile.TilingRule.Neighbor.NotThis && (Object) tile == (Object) this)
            return false;
        }
      }
    }
    return true;
  }

  public bool RuleMatches(
    RuleTile.TilingRule rule,
    Vector3Int position,
    ITilemap tilemap,
    bool mirrorX,
    bool mirrorY)
  {
    for (int y = -1; y <= 1; ++y)
    {
      for (int x = -1; x <= 1; ++x)
      {
        if (x != 0 || y != 0)
        {
          Vector3Int original = new Vector3Int(x, y, 0);
          int indexOfOffset = this.GetIndexOfOffset(this.GetMirroredPos(original, mirrorX, mirrorY));
          TileBase tile = tilemap.GetTile(position + original);
          if (rule.m_Neighbors[indexOfOffset] == RuleTile.TilingRule.Neighbor.This && (Object) tile != (Object) this || rule.m_Neighbors[indexOfOffset] == RuleTile.TilingRule.Neighbor.NotThis && (Object) tile == (Object) this)
            return false;
        }
      }
    }
    return true;
  }

  public int GetIndexOfOffset(Vector3Int offset)
  {
    int indexOfOffset = offset.x + 1 + (-offset.y + 1) * 3;
    if (indexOfOffset >= 4)
      --indexOfOffset;
    return indexOfOffset;
  }

  public Vector3Int GetRotatedPos(Vector3Int original, int rotation)
  {
    switch (rotation)
    {
      case 0:
        return original;
      case 90:
        return new Vector3Int(-original.y, original.x, original.z);
      case 180:
        return new Vector3Int(-original.x, -original.y, original.z);
      case 270:
        return new Vector3Int(original.y, -original.x, original.z);
      default:
        return original;
    }
  }

  public Vector3Int GetMirroredPos(Vector3Int original, bool mirrorX, bool mirrorY)
  {
    return new Vector3Int(original.x * (mirrorX ? -1 : 1), original.y * (mirrorY ? -1 : 1), original.z);
  }

  [Serializable]
  public class TilingRule
  {
    public RuleTile.TilingRule.Neighbor[] m_Neighbors;
    public Sprite[] m_Sprites;
    public float m_AnimationSpeed;
    public float m_PerlinScale;
    public RuleTile.TilingRule.Transform m_RuleTransform;
    public RuleTile.TilingRule.OutputSprite m_Output;
    public Tile.ColliderType m_ColliderType;
    public RuleTile.TilingRule.Transform m_RandomTransform;

    public TilingRule()
    {
      this.m_Output = RuleTile.TilingRule.OutputSprite.Single;
      this.m_Neighbors = new RuleTile.TilingRule.Neighbor[8];
      this.m_Sprites = new Sprite[1];
      this.m_AnimationSpeed = 1f;
      this.m_PerlinScale = 0.5f;
      this.m_ColliderType = Tile.ColliderType.Sprite;
      for (int index = 0; index < this.m_Neighbors.Length; ++index)
        this.m_Neighbors[index] = RuleTile.TilingRule.Neighbor.DontCare;
    }

    public enum Transform
    {
      Fixed,
      Rotated,
      MirrorX,
      MirrorY,
    }

    public enum Neighbor
    {
      DontCare,
      This,
      NotThis,
    }

    public enum OutputSprite
    {
      Single,
      Random,
      Animation,
    }
  }
}
