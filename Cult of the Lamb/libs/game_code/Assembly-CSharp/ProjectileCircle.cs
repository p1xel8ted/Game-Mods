// Decompiled with JetBrains decompiler
// Type: ProjectileCircle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using CotL.Projectiles;
using DG.Tweening;
using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectileCircle : ProjectileCircleBase
{
  [SerializeField]
  public float durationTillMaxRadius;
  [SerializeField]
  public Projectile[] projectiles;
  public Projectile projectile;

  public void OnDisable() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public override void Init(float radius)
  {
    this.projectile = this.GetComponent<Projectile>();
    float degree = 0.0f;
    float num = 360f / (float) this.projectiles.Length;
    for (int index = 0; index < this.projectiles.Length; ++index)
    {
      Vector2 endValue = Utils.DegreeToVector2(degree) * radius;
      this.projectiles[index].transform.localPosition = Vector3.zero;
      this.projectiles[index].transform.DOLocalMove((Vector3) endValue, this.durationTillMaxRadius);
      this.projectiles[index].team = this.projectile.health.team;
      this.projectiles[index].enabled = false;
      this.projectiles[index].Owner = this.projectile.Owner;
      degree += num;
    }
  }

  public override void InitDelayed(
    GameObject target,
    float radius,
    float shootDelay,
    System.Action onShoot = null)
  {
    this.projectile = this.GetComponent<Projectile>();
    float degree = 0.0f;
    float num = 360f / (float) this.projectiles.Length;
    foreach (Component projectile in this.projectiles)
      projectile.gameObject.SetActive(false);
    for (int index = 0; index < this.projectiles.Length; ++index)
    {
      Vector2 vector2 = Utils.DegreeToVector2(degree) * radius;
      this.projectiles[index].transform.localPosition = Vector3.zero;
      this.projectiles[index].transform.localPosition = (Vector3) vector2;
      this.projectiles[index].team = this.projectile.health.team;
      this.projectiles[index].enabled = false;
      this.projectiles[index].Owner = this.projectile.Owner;
      degree += num;
    }
    this.StartCoroutine((IEnumerator) this.EnableProjectiles(target, shootDelay, onShoot));
  }

  public IEnumerator EnableProjectiles(GameObject target, float delay, System.Action onShoot)
  {
    ProjectileCircle projectileCircle = this;
    projectileCircle.projectile.SpeedMultiplier = 0.0f;
    Projectile[] projectileArray = projectileCircle.projectiles;
    for (int index = 0; index < projectileArray.Length; ++index)
    {
      projectileArray[index].gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(0.02f);
    }
    projectileArray = (Projectile[]) null;
    yield return (object) new WaitForSeconds(0.25f + delay);
    foreach (Projectile projectile in projectileCircle.projectiles)
    {
      if ((bool) (UnityEngine.Object) projectile)
        projectile.enabled = true;
    }
    if ((bool) (UnityEngine.Object) target)
      projectileCircle.projectile.Angle = Utils.GetAngle(projectileCircle.transform.position, target.transform.position);
    System.Action action = onShoot;
    if (action != null)
      action();
    projectileCircle.projectile.SpeedMultiplier = 1f;
  }

  public void TargetMiddle(float speed, float lifetime, float acceleration)
  {
    foreach (Projectile projectile in this.projectiles)
    {
      projectile.SpeedMultiplier = speed;
      projectile.LifeTime = lifetime;
      projectile.Acceleration = acceleration;
      projectile.Angle = Utils.GetAngle(projectile.transform.position, this.transform.position);
      projectile.GetComponent<Rigidbody2D>().isKinematic = false;
      projectile.enabled = true;
    }
  }

  public void TargetMiddleInverse(float speed, float lifetime, float acceleration)
  {
    foreach (Projectile projectile in this.projectiles)
    {
      projectile.SpeedMultiplier = speed;
      projectile.LifeTime = lifetime;
      projectile.Acceleration = acceleration;
      projectile.Angle = Utils.GetAngle(this.transform.position, projectile.transform.position);
      projectile.GetComponent<Rigidbody2D>().isKinematic = false;
      projectile.enabled = true;
    }
  }
}
