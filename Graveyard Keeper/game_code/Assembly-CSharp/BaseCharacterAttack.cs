// Decompiled with JetBrains decompiler
// Type: BaseCharacterAttack
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BaseCharacterAttack : WorldObjectPartComponent
{
  public const string ATTACK_TYPE = "attack_type";
  public const string ATTACK_TYPE_F = "attack_type_f";
  [NonSerialized]
  public bool performing;
  [NonSerialized]
  public bool anim_based_timing;
  [NonSerialized]
  public bool successed;
  [NonSerialized]
  public bool using_item;
  [NonSerialized]
  public BaseCharacterAttack.AttackResult on_performed;
  [NonSerialized]
  public float _stopped_time;
  [NonSerialized]
  public int _callback_setting_frame;
  [NonSerialized]
  public int cur_attack_type = -1;
  public bool _collider_cached;
  public Transform _collider_tf;

  public bool performing_attack => this.performing;

  public override void StartComponent()
  {
    base.StartComponent();
    this.InitAnimator();
  }

  public override void UpdateComponent(float delta_time)
  {
    CombatComponent combat = this.components.combat;
    if (!combat.is_started)
      return;
    float dirAngle = this.components.character.dir_angle;
    foreach (CombatCollider combatCollider in combat.combat_colliders)
    {
      if (combatCollider.rotate_collider)
        combatCollider.RotateCollider(dirAngle);
    }
  }

  public void AttackAnimationEnded() => this.Stop(true);

  public void OnItemLoop(ItemDefinition.ItemType item_type, bool flag)
  {
    this.components.animated_behaviour.on_item_loop -= new AnimatedBehaviour.OnItemAnimationEvent(this.OnItemLoop);
    if (this.wgo.is_player && item_type != ItemDefinition.ItemType.Sword)
      return;
    this.Stop(true);
  }

  public void OnLoop()
  {
    if (this.anim_based_timing)
      this.components.animated_behaviour.on_loop -= new GJCommons.VoidDelegate(this.OnLoop);
    else
      this.components.timer.on_loop -= new GJCommons.VoidDelegate(this.OnLoop);
    this.Stop(true);
  }

  public void ClearAllLoopCallbacks()
  {
    if (this.using_item)
      this.components.animated_behaviour.on_item_loop -= new AnimatedBehaviour.OnItemAnimationEvent(this.OnItemLoop);
    else if (this.anim_based_timing)
      this.components.animated_behaviour.on_loop -= new GJCommons.VoidDelegate(this.OnLoop);
    else
      this.components.timer.on_loop -= new GJCommons.VoidDelegate(this.OnLoop);
  }

  public void InitAnimator() => this.ForceRecache();

  public virtual bool Perform(
    Direction dir,
    int type,
    BaseCharacterAttack.AttackResult on_performed = null,
    bool anim_based_timing = true)
  {
    return this.Perform(type, on_performed, anim_based_timing);
  }

  public virtual bool Perform(
    WorldGameObject target,
    int type,
    BaseCharacterAttack.AttackResult on_performed = null,
    bool anim_based_timing = true)
  {
    return this.Perform(type, on_performed, anim_based_timing);
  }

  public virtual bool Perform(
    int type,
    BaseCharacterAttack.AttackResult on_performed = null,
    bool anim_based_timing = true)
  {
    if (this.performing)
      return false;
    this.components.combat.StartCombat();
    this.UpdateAttackState(true, type);
    this.performing = true;
    this.successed = false;
    this.on_performed = on_performed;
    this._callback_setting_frame = Time.frameCount;
    this.anim_based_timing = anim_based_timing;
    this.using_item = anim_based_timing && this.wgo.GetEquippedWeaponType() != 0;
    if (anim_based_timing)
    {
      if (this.wgo.GetEquippedWeaponType() == ItemDefinition.ItemType.None)
        this.components.animated_behaviour.on_loop += new GJCommons.VoidDelegate(this.OnLoop);
      else
        this.components.animated_behaviour.on_item_loop += new AnimatedBehaviour.OnItemAnimationEvent(this.OnItemLoop);
    }
    else
      this.components.timer.on_loop += new GJCommons.VoidDelegate(this.OnLoop);
    Item equippedWeapon = this.wgo.is_player ? this.wgo.GetEquippedWeapon() : (Item) null;
    if (equippedWeapon != null)
      this.wgo.components.character.player.TrySpendEnergy(equippedWeapon.definition.params_on_use.Get("energy") * -1f);
    return true;
  }

  public virtual void Stop(bool call_callback)
  {
    if (!this.performing)
      return;
    this._stopped_time = Time.time;
    this.performing = false;
    this.UpdateAttackState(false, 0);
    this.anim_based_timing = false;
    if (call_callback && this.on_performed != null)
      this.on_performed(this.successed);
    if (this._callback_setting_frame != Time.frameCount)
      this.on_performed = (BaseCharacterAttack.AttackResult) null;
    this.successed = false;
  }

  public virtual void InterruptAttack()
  {
    this.ClearAllLoopCallbacks();
    this.Stop(false);
  }

  public virtual void UpdateAttackState(bool is_attacking, int type)
  {
    bool flag = this.wgo.is_player || this.wgo.GetEquippedWeaponType() == ItemDefinition.ItemType.Sword;
    this.cur_attack_type = type;
    this.components.animator.SetFloat("attack_type_f", (float) type);
    BaseCharacterComponent character = this.components.character;
    if (is_attacking)
    {
      if (flag)
        character.SetAnimationState(CharAnimState.Tool, this.wgo.GetEquippedWeaponType());
      else
        character.SetAnimationState(CharAnimState.Attack);
    }
    else if (flag && this.components.character.anim_state == CharAnimState.Tool || character.anim_state == CharAnimState.Attack)
      character.SetAnimationState(CharAnimState.Idle);
    if (is_attacking)
      return;
    this.components.animator.SetInteger("attack_type", 0);
  }

  public virtual void OnFirstAttackStepDone()
  {
  }

  public virtual void SuccessAttack() => this.successed = true;

  public delegate void AttackResult(bool success);
}
