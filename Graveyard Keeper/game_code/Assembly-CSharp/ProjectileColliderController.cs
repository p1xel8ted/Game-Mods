// Decompiled with JetBrains decompiler
// Type: ProjectileColliderController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Collider2D))]
public class ProjectileColliderController : MonoBehaviour
{
  public ProjectileObject father;
  public List<Collider2D> _skip_colliders = new List<Collider2D>();

  public void AddSkipColliders(List<Collider2D> colliders_for_skip)
  {
    if (colliders_for_skip == null || colliders_for_skip.Count == 0)
      return;
    if (this._skip_colliders == null)
      this._skip_colliders = new List<Collider2D>();
    this._skip_colliders.AddRange((IEnumerable<Collider2D>) colliders_for_skip);
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (this._skip_colliders.Contains(other))
      return;
    GameObject gameObject = other.gameObject;
    if (gameObject.layer != 0 && gameObject.layer != 9)
      return;
    WorldGameObject componentInParent = other.GetComponentInParent<WorldGameObject>();
    if ((Object) componentInParent != (Object) null)
    {
      if ((Object) this.father.father != (Object) null && componentInParent.unique_id == this.father.father.unique_id)
        this._skip_colliders.Add(other);
      else if (other.isTrigger)
        Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to trigger {componentInParent.name}[{componentInParent.obj_id}]. Doing nothing.", (Object) componentInParent);
      else if (!this.father.definition.can_damage_mobs && componentInParent.obj_def.IsMob())
        Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to mob {componentInParent.name}[{componentInParent.obj_id}]. Can not attack mobs => doing nothing", (Object) componentInParent);
      else if (componentInParent.components.combat.enabled)
      {
        Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to {componentInParent.name}[{componentInParent.obj_id}] combat component.", (Object) componentInParent);
        this.father.OnHitCombat(componentInParent.components.combat);
        this._skip_colliders.Add(other);
      }
      else if (componentInParent.components.hp.enabled)
      {
        Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to {componentInParent.name}[{componentInParent.obj_id}] hp component.", (Object) componentInParent);
        this.father.OnHitNonCombat(componentInParent);
      }
      else
      {
        Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to just WGO {componentInParent.name}[{componentInParent.obj_id}].", (Object) componentInParent);
        this.father.OnHitNonCombat(componentInParent);
      }
    }
    else
    {
      WorldSimpleObject context = other.GetComponent<WorldSimpleObject>();
      if ((Object) context == (Object) null)
        context = other.GetComponentInParent<WorldSimpleObject>();
      if ((Object) context != (Object) null)
      {
        if (other.isTrigger)
        {
          Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to trigger wso {context.name}.Doing nothing.", (Object) context);
        }
        else
        {
          Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to WSO. Destroying.", (Object) context);
          this.father.OnHitNonCombat((WorldGameObject) null);
        }
      }
      else
      {
        ProjectileColliderController component = other.GetComponent<ProjectileColliderController>();
        if ((Object) component != (Object) null && (Object) component.father != (Object) null && component.father.id == this.father.id)
        {
          this._skip_colliders.Add(other);
        }
        else
        {
          Debug.Log((object) $"Projectile {this.father.name}[{this.father.id}] is hit to unknown collider. Destroying.", (Object) other);
          this.father.OnHitNonCombat((WorldGameObject) null);
        }
      }
    }
  }
}
