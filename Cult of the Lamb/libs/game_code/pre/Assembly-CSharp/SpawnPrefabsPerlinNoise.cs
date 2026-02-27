// Decompiled with JetBrains decompiler
// Type: SpawnPrefabsPerlinNoise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpawnPrefabsPerlinNoise : BaseMonoBehaviour
{
  private PolygonCollider2D Polygon;
  public bool IsometricPlacement = true;
  private float IsometricOffset;
  public float PerlinScale = 1f;
  public Vector2 PerlinOffset = Vector2.zero;
  private float Noise;
  private GameObject g;
  public Vector2 Spacing = new Vector2(2f, 2f);
  public List<SpawnPrefabsPerlinNoise.SpawnableItem> SpawnableItems = new List<SpawnPrefabsPerlinNoise.SpawnableItem>();

  private void Generate()
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
