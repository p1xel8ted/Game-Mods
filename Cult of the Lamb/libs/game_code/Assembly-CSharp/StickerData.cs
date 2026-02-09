// Decompiled with JetBrains decompiler
// Type: StickerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
