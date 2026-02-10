// Decompiled with JetBrains decompiler
// Type: StickerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
