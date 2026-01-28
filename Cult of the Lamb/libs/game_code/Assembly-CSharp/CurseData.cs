// Decompiled with JetBrains decompiler
// Type: CurseData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Curse Data")]
public class CurseData : EquipmentData
{
  public GameObject Prefab;
  public GameObject SecondaryPrefab;
  public string PerformActionAnimation;
  public string PerformActionAnimationLoop;
  public bool CanAim = true;
  public float Damage = 2f;
  public float Delay = 0.5f;
  public float CastingDuration = 0.1f;
  [Range(0.0f, 1f)]
  public float Chance = 1f;
}
