// Decompiled with JetBrains decompiler
// Type: KnockableMortarBombKingOfLambs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections;
using UnityEngine;

#nullable disable
public class KnockableMortarBombKingOfLambs : KnockableMortarBomb
{
  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!this.canBeHit || this.isKnocked)
      return;
    Health component = Attacker.GetComponent<Health>();
    if ((Object) component != (Object) null && component.team != Health.Team.PlayerTeam)
      return;
    this.canHurtOwner = true;
    this.StartCoroutine((IEnumerator) this.EnableTriggerCollider(0.0f));
    this.Target.transform.localScale = Vector3.zero;
    this.TargetWarning.transform.localScale = Vector3.zero;
    this.BombVisual.transform.position = new Vector3(this.BombVisual.transform.position.x, this.BombVisual.transform.position.y, 0.0f);
    this.isHit = true;
    this.StopAllCoroutines();
    this.KnockTowardsEnemy((Object) Attacker != (Object) null ? Attacker.transform.position : (Vector3) Random.insideUnitCircle, AttackType);
    if (string.IsNullOrEmpty(this.BombWindUpSFX))
      return;
    AudioManager.Instance.StopOneShotInstanceEarly(this.windUpInstanceSFX, STOP_MODE.IMMEDIATE);
  }
}
