// Decompiled with JetBrains decompiler
// Type: CombatCollider
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class CombatCollider : MonoBehaviour
{
  public const int MAX_SKIP = 20;
  public List<Collider2D> _skip_colliders = new List<Collider2D>();
  public List<Collider2D> _hit_colliders = new List<Collider2D>();
  public CombatComponent combat_component;
  public ObjectDefinition.DamageType damage;
  public bool rotate_collider;
  public bool do_not_round_rotation;
  public long _cc_id;
  public Transform _collider_tf;
  public Vector3 _collider_rotation;
  public bool _started;

  public void StartComponent(CombatComponent cc)
  {
    this.combat_component = cc;
    this._cc_id = cc.GetInstanceID();
    this._collider_tf = this.transform;
    this._started = true;
  }

  public void OnTriggerEnter2D(Collider2D other) => this.OnCollision(other);

  public void OnTriggerExit2D(Collider2D other) => this.OnCollision(other);

  public void OnCollision(Collider2D other)
  {
    if (this.combat_component == null || this._skip_colliders.Contains(other) || this._hit_colliders.Contains(other))
      return;
    if ((UnityEngine.Object) other.GetComponent<CombatCollider>() != (UnityEngine.Object) null)
    {
      this._skip_colliders.Add(other);
    }
    else
    {
      WorldGameObject componentInParent = other.GetComponentInParent<WorldGameObject>();
      if ((UnityEngine.Object) componentInParent == (UnityEngine.Object) null)
        return;
      CombatComponent combat = componentInParent.components.combat;
      if (combat == null || this._cc_id == combat.GetInstanceID() || combat.wgo.is_player == this.combat_component.wgo.is_player)
      {
        this._skip_colliders.Add(other);
        if (this._skip_colliders.Count <= 20)
          return;
        this._skip_colliders.RemoveAt(0);
      }
      else
      {
        if (!this.combat_component.HitOther(combat, this.damage) || this._hit_colliders.Contains(other))
          return;
        this._hit_colliders.Add(other);
      }
    }
  }

  public void RotateCollider(float dir_angle)
  {
    this._collider_rotation.z = !this.do_not_round_rotation ? (float) (Mathf.RoundToInt((float) (((double) dir_angle + 90.0) / 90.0)) * 90) : (float) ((int) dir_angle + 90);
    this._collider_tf.eulerAngles = this._collider_rotation;
  }

  public void OnEnable()
  {
    if (!this._started)
      return;
    if (this._hit_colliders.Count > 0)
      this._hit_colliders.Clear();
    if (this.combat_component.active_combat_colliders.Count == 0)
      this.combat_component.StartCombat();
    this.combat_component.active_combat_colliders.Add(this);
  }

  public void OnDisable()
  {
    if (!this._started || !this.combat_component.active_combat_colliders.Contains(this))
      return;
    this.combat_component.active_combat_colliders.Remove(this);
  }
}
