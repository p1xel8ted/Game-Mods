// Decompiled with JetBrains decompiler
// Type: ProjectileCross
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectileCross : BaseMonoBehaviour
{
  [SerializeField]
  public float torque;
  [SerializeField]
  public Projectile[] projectiles;
  public Projectile projectile;
  public bool active;

  public Projectile Projectile => this.projectile;

  public void InitDelayed()
  {
    this.projectile = this.GetComponent<Projectile>();
    foreach (Component projectile in this.projectiles)
      projectile.gameObject.SetActive(false);
    for (int index = 0; index < this.projectiles.Length; ++index)
    {
      this.projectiles[index].health = this.projectile.health;
      this.projectiles[index].team = Health.Team.Team2;
    }
    this.StartCoroutine((IEnumerator) this.EnableProjectiles());
  }

  public void Update()
  {
    if (this.active && !PlayerRelic.TimeFrozen)
      this.transform.Rotate(new Vector3(0.0f, 0.0f, this.torque * Time.deltaTime));
    foreach (Projectile projectile in this.projectiles)
    {
      if ((bool) (Object) projectile)
        projectile.transform.eulerAngles = new Vector3(-60f, 0.0f, 0.0f);
    }
    if (!((Object) this.projectile != (Object) null) || !((Object) this.projectile.health == (Object) null))
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public IEnumerator EnableProjectiles()
  {
    yield return (object) new WaitForSeconds(0.1f);
    int counter = 0;
    for (int i = 0; i < this.projectiles.Length; ++i)
    {
      this.projectiles[i].gameObject.SetActive(true);
      this.projectiles[i].SpeedMultiplier = 0.0f;
      ++counter;
      if (counter >= 4)
      {
        counter = 0;
        yield return (object) new WaitForSeconds(0.08f);
      }
    }
    this.active = true;
  }

  public IEnumerator DisableProjectiles(bool force = false)
  {
    ProjectileCross projectileCross = this;
    if (projectileCross.active | force)
    {
      if (force)
        projectileCross.StopAllCoroutines();
      projectileCross.active = false;
      int counter = 0;
      for (int i = projectileCross.projectiles.Length - 1; i >= 0; --i)
      {
        if (projectileCross.projectiles != null && (Object) projectileCross.projectiles[i] != (Object) null)
        {
          projectileCross.projectiles[i].gameObject.SetActive(true);
          projectileCross.projectiles[i].DestroyProjectile(true);
        }
        ++counter;
        if (counter >= 4)
        {
          counter = 0;
          yield return (object) new WaitForSeconds(0.1f);
        }
      }
    }
  }
}
