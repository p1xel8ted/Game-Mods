// Decompiled with JetBrains decompiler
// Type: EnemyDual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class EnemyDual : UnitObject
{
  [SerializeField]
  public EnemyDual partner;
  [SerializeField]
  public float attackTogetherCooldown;
  [SerializeField]
  public float syncMaxTime;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float syncMaxHealth;
  public bool hasToPhase;
  public float attackTogetherTimeStamp;
  public EnemyDual.PhaseAction OnPhaseStart;

  public EnemyDual Partner => this.partner;

  public bool PartnerAlive
  {
    get => (Object) this.partner != (Object) null && (double) this.partner.health.HP > 0.0;
  }

  public bool IsAttackTogetherReady
  {
    get
    {
      if ((double) GameManager.GetInstance().TimeSince(this.attackTogetherTimeStamp) < (double) this.attackTogetherCooldown)
        return false;
      return (double) this.health.HP / (double) this.health.totalHP < (double) this.syncMaxHealth || (double) this.partner.health.HP / (double) this.partner.health.totalHP < (double) this.partner.syncMaxHealth;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.partner.health.OnDie += new Health.DieAction(this.OnPartnerDeath);
  }

  public virtual void OnPartnerDeath(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!((Object) this.partner != (Object) null))
      return;
    this.partner.health.OnDie -= new Health.DieAction(this.OnPartnerDeath);
  }

  public delegate void PhaseAction();
}
