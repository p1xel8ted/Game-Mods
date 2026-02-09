// Decompiled with JetBrains decompiler
// Type: FrogBossTongueTip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
