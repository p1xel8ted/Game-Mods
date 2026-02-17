// Decompiled with JetBrains decompiler
// Type: CurseData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
