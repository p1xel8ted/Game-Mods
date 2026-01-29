// Decompiled with JetBrains decompiler
// Type: FrogBossTongueTip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FrogBossTongueTip : UnitObject
{
  public FrogBossTongue tongue;

  public override void Awake()
  {
    base.Awake();
    this.tongue = this.GetComponentInParent<FrogBossTongue>();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.tongue.DealDamageToBoss(Attacker, AttackLocation, AttackType, FromBehind);
  }
}
