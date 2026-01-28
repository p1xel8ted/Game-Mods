// Decompiled with JetBrains decompiler
// Type: StickerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
