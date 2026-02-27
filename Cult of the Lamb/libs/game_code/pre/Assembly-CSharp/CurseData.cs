// Decompiled with JetBrains decompiler
// Type: CurseData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
