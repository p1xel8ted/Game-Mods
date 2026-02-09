// Decompiled with JetBrains decompiler
// Type: CurvedAttack
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CurvedAttack : BaseCharacterAttack
{
  public CurvedAttack.CurvedAttackType[] types;
  public float max_delay_for_subtype;
  [NonSerialized]
  public int _last_subtype;
  public ProjectileEmitter _last_enabled_emitter;

  public override bool Perform(
    WorldGameObject target,
    int type,
    BaseCharacterAttack.AttackResult on_performed = null,
    bool anim_based_timing = true)
  {
    if (!this.CommonPerformStuff(type, on_performed))
      return false;
    this.components.character.CurveMoveTo(target, this.GetCurve(type), this.GetDist(type), based_on_anim_timing: this.IsAnimBasedTiming(type));
    return true;
  }

  public override bool Perform(
    Direction dir,
    int type,
    BaseCharacterAttack.AttackResult on_performed = null,
    bool anim_based_timing = true)
  {
    if (!this.CommonPerformStuff(type, on_performed))
      return false;
    this.components.character.CurveMove(dir == Direction.IgnoreDirection ? this.components.character.direction : dir.ToVec(), this.GetCurve(type), this.GetDist(type), based_on_anim_timing: this.IsAnimBasedTiming(type));
    return true;
  }

  public bool CommonPerformStuff(int type, BaseCharacterAttack.AttackResult on_performed = null)
  {
    if (!this.performing && !this.IsAnimBasedTiming(type))
      this.components.timer.Play(this.GetTime(type));
    if (this.types != null && this.types.Length > type && type >= 0 && (UnityEngine.Object) this.types[type].enable_projectile_emitter != (UnityEngine.Object) null && this.types[type].enable_projectile_emitter.is_paused && this.types[type].enable_projectile_emitter.has_period)
    {
      this._last_enabled_emitter = this.types[type].enable_projectile_emitter;
      this._last_enabled_emitter.is_paused = false;
    }
    return this.Perform(type, on_performed, this.IsAnimBasedTiming(type));
  }

  public override void OnFirstAttackStepDone() => this.components.character.StopMovement();

  public override void InterruptAttack()
  {
    base.InterruptAttack();
    this.components.timer.ClearTimer();
    this.components.character.StopMovement();
    if (!((UnityEngine.Object) this._last_enabled_emitter != (UnityEngine.Object) null))
      return;
    this._last_enabled_emitter.is_paused = true;
    this._last_enabled_emitter = (ProjectileEmitter) null;
  }

  public override void Stop(bool call_callback)
  {
    base.Stop(call_callback);
    if (!((UnityEngine.Object) this._last_enabled_emitter != (UnityEngine.Object) null))
      return;
    this._last_enabled_emitter.is_paused = true;
    this._last_enabled_emitter = (ProjectileEmitter) null;
  }

  public override void UpdateAttackState(bool is_attacking, int type)
  {
    base.UpdateAttackState(is_attacking, type);
    if (!is_attacking)
    {
      this.components.animator.SetInteger("attack_type", 0);
    }
    else
    {
      if ((double) Time.time - (double) this._stopped_time > (double) this.max_delay_for_subtype)
        this._last_subtype = 0;
      else if (type > this.types.Length)
        this._last_subtype = 0;
      else if (++this._last_subtype >= this.types[type].sub_types)
        this._last_subtype = 0;
      this.components.animator.SetInteger("attack_type", this._last_subtype);
      Debug.Log((object) "ATTACK STATE!!!!");
    }
  }

  public AnimationCurve GetCurve(int type)
  {
    return type >= this.types.Length || (UnityEngine.Object) this.types[type].curve == (UnityEngine.Object) null ? this.wgo.GetWOP().GetCurve("attack") : this.types[type].curve.curve;
  }

  public float GetDist(int type) => type < this.types.Length ? this.types[type].dist : 0.0f;

  public float GetTime(int type) => type < this.types.Length ? this.types[type].time : 0.0f;

  public bool IsAnimBasedTiming(int type) => this.GetTime(type).EqualsTo(0.0f);

  [Serializable]
  public struct CurvedAttackType
  {
    public float dist;
    public float time;
    public MovementCurve curve;
    public int sub_types;
    public ProjectileEmitter enable_projectile_emitter;
  }
}
