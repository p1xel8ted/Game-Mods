// Decompiled with JetBrains decompiler
// Type: ProjectileCross
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectileCross : BaseMonoBehaviour
{
  [SerializeField]
  private float torque;
  [SerializeField]
  private Projectile[] projectiles;
  private Projectile projectile;
  private bool active;

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

  private void Update()
  {
    if (this.active)
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

  private IEnumerator EnableProjectiles()
  {
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

  public IEnumerator DisableProjectiles()
  {
    if (this.active)
    {
      this.active = false;
      int counter = 0;
      for (int i = this.projectiles.Length - 1; i >= 0; --i)
      {
        if (this.projectiles != null && (Object) this.projectiles[i] != (Object) null)
          this.projectiles[i].DestroyProjectile(true);
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
