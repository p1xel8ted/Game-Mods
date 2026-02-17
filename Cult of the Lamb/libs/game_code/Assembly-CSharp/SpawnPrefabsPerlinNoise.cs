// Decompiled with JetBrains decompiler
// Type: SpawnPrefabsPerlinNoise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpawnPrefabsPerlinNoise : BaseMonoBehaviour
{
  public PolygonCollider2D Polygon;
  public bool IsometricPlacement = true;
  public float IsometricOffset;
  public float PerlinScale = 1f;
  public Vector2 PerlinOffset = Vector2.zero;
  public float Noise;
  public GameObject g;
  public Vector2 Spacing = new Vector2(2f, 2f);
  public List<SpawnPrefabsPerlinNoise.SpawnableItem> SpawnableItems = new List<SpawnPrefabsPerlinNoise.SpawnableItem>();

  public void Generate()
  {
  }

  public void ClearPrefabs()
  {
    int childCount = this.transform.childCount;
    while (--childCount > -1)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.transform.GetChild(childCount).gameObject);
  }

  [Serializable]
  public class SpawnableItem
  {
    public GameObject Prefab;
    public Vector2 PerlinRange;
    public Vector3 RandomOffset = Vector3.zero;
    public bool randomFlip;
  }
}
