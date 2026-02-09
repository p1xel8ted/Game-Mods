// Decompiled with JetBrains decompiler
// Type: CombatComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CombatComponent : WorldGameObjectComponent
{
  public CombatCollider[] combat_colliders;
  public Transform[] _collider_transforms;
  public Vector3[] _collider_local_poss;
  public List<long> _collided_ids = new List<long>();
  public int _colliders_count;
  public List<CombatCollider> active_combat_colliders = new List<CombatCollider>();

  public Transform[] collider_tf => this._collider_transforms;

  public bool is_started => this.started;

  public override void StartComponent()
  {
    base.StartComponent();
    this.combat_colliders = this.wgo.GetComponentsInChildren<CombatCollider>(true);
    this._colliders_count = this.combat_colliders.Length;
    if (this.combat_colliders != null && this._colliders_count > 0)
    {
      this._collider_transforms = new Transform[this._colliders_count];
      this._collider_local_poss = new Vector3[this._colliders_count];
      for (int index = 0; index < this.combat_colliders.Length; ++index)
      {
        this.combat_colliders[index].StartComponent(this);
        this.combat_colliders[index].enabled = true;
        this._collider_transforms[index] = this.combat_colliders[index].transform;
        this._collider_local_poss[index] = this._collider_transforms[index].localPosition;
      }
    }
    this.active_combat_colliders = new List<CombatCollider>();
  }

  public void WasHitBy(CombatComponent other, ObjectDefinition.DamageType damage_type)
  {
    if (!this.components.hp.enabled || this.wgo.is_dead)
      return;
    this.components.hp.DecHP(other.wgo.GetDamage(damage_type));
    Vector2 normalized = (other.wgo.pos - this.wgo.pos).normalized;
    float damage_direction = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
    if (this.components.character.enabled)
      this.components.character.OnWasDamaged(damage_direction);
    if (this.combat_colliders == null || (double) this.wgo.hp > 0.0)
      return;
    foreach (Component combatCollider in this.combat_colliders)
      combatCollider.gameObject.SetActive(false);
  }

  public void StartCombat()
  {
    if (this._collided_ids.Count <= 0)
      return;
    this._collided_ids.Clear();
  }

  public override bool HasUpdate() => true;

  public override void UpdateComponent(float delta_time)
  {
    for (int index = 0; index < this._colliders_count; ++index)
    {
      if ((bool) (Object) this._collider_transforms[index])
        this._collider_transforms[index].localPosition = this._collider_local_poss[index];
    }
  }

  public bool HitOther(
    CombatComponent other_combat_component,
    ObjectDefinition.DamageType damage_type)
  {
    if (!this.wgo.components.character.attack.performing_attack && this.active_combat_colliders.Count == 0 || this._collided_ids.Contains(other_combat_component.GetInstanceID()) || this.components.character.is_following_target && (Object) other_combat_component.tf != (Object) this.components.character.following_target)
      return false;
    this._collided_ids.Add(other_combat_component.GetInstanceID());
    other_combat_component.WasHitBy(this, damage_type);
    this.wgo.components.character.attack.SuccessAttack();
    return true;
  }

  public override int GetExecutionOrder() => 6;

  public override void UpdateEnableState(ObjectDefinition.ObjType obj_type)
  {
    this.enabled = obj_type == ObjectDefinition.ObjType.Mob || this.wgo.is_player;
  }
}
