// Decompiled with JetBrains decompiler
// Type: FrogBossTongueTip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FrogBossTongueTip : UnitObject
{
  private FrogBossTongue tongue;

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
