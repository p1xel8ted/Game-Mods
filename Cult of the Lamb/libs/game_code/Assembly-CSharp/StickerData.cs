// Decompiled with JetBrains decompiler
// Type: StickerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Sticker Data")]
public class StickerData : ScriptableObject
{
  public Sprite Sticker;
  public float MinScale = 0.5f;
  public float MaxScale = 2f;
  public bool UsePrefab;
  public StickerItem Prefab;
}
