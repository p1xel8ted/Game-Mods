// Decompiled with JetBrains decompiler
// Type: MMRoomGeneration.GeneraterDecorations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
namespace MMRoomGeneration;

public class GeneraterDecorations : BaseMonoBehaviour
{
  public Color biomeColor;
  public SpriteShape SpriteShape;
  public SpriteShape SpriteShapeSecondary;
  public SpriteShape SpriteShapeBack;
  public Material SpriteShapeMaterial;
  public Material SpriteShapeBackMaterial;
  [Range(0.0f, 1f)]
  public float CritterThreshold = 0.25f;
  public float MaxRadiusFromMiddle = -1f;
  public GeneraterDecorations.ListOfDecorations Critters = new GeneraterDecorations.ListOfDecorations();
  public float NoiseScale = 100f;
  [Range(0.0f, 1f)]
  public float NoiseThreshold = 0.65f;
  public GeneraterDecorations.ListOfPerlinSpriteShape DecorationPerlinSpriteShapePrimary = new GeneraterDecorations.ListOfPerlinSpriteShape();
  public GeneraterDecorations.ListOfPerlinSpriteShape DecorationPerlinSpriteShapeSecondary = new GeneraterDecorations.ListOfPerlinSpriteShape();
  public GeneraterDecorations.ListOfDecorations DecorationPiece = new GeneraterDecorations.ListOfDecorations();
  public GeneraterDecorations.ListOfDecorations DecorationPiece2x2 = new GeneraterDecorations.ListOfDecorations();
  public GeneraterDecorations.ListOfDecorations DecorationPiece3x3 = new GeneraterDecorations.ListOfDecorations();
  public GeneraterDecorations.ListOfDecorations DecorationPiece3x3Tall = new GeneraterDecorations.ListOfDecorations();
  public GeneraterDecorations.ListOfDecorations DecorationPerlinNoiseOffPath = new GeneraterDecorations.ListOfDecorations();
  public GeneraterDecorations.ListOfDecorations DecorationPerlinNoiseOnPath = new GeneraterDecorations.ListOfDecorations();

  public static int GetRandomWeightedIndex(List<int> weights, double Random)
  {
    if (weights == null || weights.Count == 0)
      return -1;
    int num1 = 0;
    for (int index = 0; index < weights.Count; ++index)
    {
      if (weights[index] >= 0)
        num1 += weights[index];
    }
    float num2 = 0.0f;
    for (int index = 0; index < weights.Count; ++index)
    {
      if ((double) weights[index] > 0.0)
      {
        num2 += (float) weights[index] / (float) num1;
        if ((double) num2 >= Random)
          return index;
      }
    }
    return -1;
  }

  [Serializable]
  public class ListOfDecorations
  {
    public List<GeneraterDecorations.DecorationAndProbability> DecorationAndProabilies;
    private List<int> Weights;
    private int Index;

    public GeneraterDecorations.DecorationAndProbability GetRandomGameObject(double RandomSeed)
    {
      if (this.DecorationAndProabilies.Count <= 0)
        return (GeneraterDecorations.DecorationAndProbability) null;
      this.Weights = new List<int>();
      int index = -1;
      while (++index < this.DecorationAndProabilies.Count)
        this.Weights.Add(this.DecorationAndProabilies[index].Probability);
      this.Index = GeneraterDecorations.GetRandomWeightedIndex(this.Weights, RandomSeed);
      return this.DecorationAndProabilies[this.Index];
    }
  }

  [Serializable]
  public class ListOfPerlinSpriteShape
  {
    public List<GeneraterDecorations.DecorationPerlinSpriteShape> DecorationAndProabilies = new List<GeneraterDecorations.DecorationPerlinSpriteShape>();
  }

  [Serializable]
  public class DecorationPerlinSpriteShape
  {
    public string ObjectPath;
    [Range(0.0f, 1f)]
    public float PerlinThreshold = 0.6f;
    public float PerlinScale = 350f;
    public float PerlinOffset;
  }

  [Serializable]
  public class DecorationAndProbability
  {
    [Range(0.0f, 100f)]
    public int Probability = 50;
    public string ObjectPath;
    public Vector2 RandomOffset;
    public Vector2 ZOffset = Vector2.zero;

    public Vector3 GetRandomOffset()
    {
      return new Vector3(UnityEngine.Random.Range(-this.RandomOffset.x, this.RandomOffset.x), UnityEngine.Random.Range(-this.RandomOffset.y, this.RandomOffset.y), -UnityEngine.Random.Range(this.ZOffset.x, this.ZOffset.y));
    }
  }
}
