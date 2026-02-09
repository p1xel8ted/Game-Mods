// Decompiled with JetBrains decompiler
// Type: ProjectileObjectPart
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ProjectileObjectPart : MonoBehaviour
{
  public const string PATH = "Projectiles/";
  public Collider2D attack_collider;
  public ProjectileColliderController collider_controller;
  public Animator animator;
  public System.Action on_start;
  public System.Action on_hit_combat;
  public System.Action on_hit_non_combat;
  public System.Action on_out_of_screen;
  public System.Action on_max_dist_reached;

  public void Init()
  {
    Collider2D[] componentsInChildren = this.GetComponentsInChildren<Collider2D>();
    if (componentsInChildren.Length == 0)
    {
      Debug.LogError((object) $"Not found any collider in projectile \"{this.name}\"!");
    }
    else
    {
      foreach (Collider2D collider2D in componentsInChildren)
      {
        if (collider2D.gameObject.layer == 0 && collider2D.isTrigger)
        {
          this.attack_collider = collider2D;
          break;
        }
      }
    }
    if ((UnityEngine.Object) this.attack_collider == (UnityEngine.Object) null)
      Debug.LogError((object) $"Not found attack collider for projectile \"{this.name}\"!");
    this.collider_controller = this.GetComponentInChildren<ProjectileColliderController>();
    if ((UnityEngine.Object) this.collider_controller == (UnityEngine.Object) null)
      Debug.LogError((object) $"Collider controller not found for projectile \"{this.name}\"!");
    this.animator = this.GetComponent<Animator>();
    if (!((UnityEngine.Object) this.animator == (UnityEngine.Object) null))
      return;
    Debug.LogError((object) $"Not found animator for projectile \"{this.name}\"!");
  }

  public static ProjectileObjectPart Load(string filename)
  {
    ProjectileObjectPart projectileObjectPart = Resources.Load<ProjectileObjectPart>("Projectiles/" + filename);
    if (!((UnityEngine.Object) projectileObjectPart == (UnityEngine.Object) null))
      return projectileObjectPart;
    Debug.LogError((object) ("Error loading: Projectiles/" + filename));
    return projectileObjectPart;
  }
}
