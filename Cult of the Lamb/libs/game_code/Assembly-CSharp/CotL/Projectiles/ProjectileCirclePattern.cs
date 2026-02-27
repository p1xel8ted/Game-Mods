// Decompiled with JetBrains decompiler
// Type: CotL.Projectiles.ProjectileCirclePattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;

#nullable disable
namespace CotL.Projectiles;

public class ProjectileCirclePattern : ProjectileCircleBase
{
  [SerializeField]
  public float durationTillMaxRadius;
  [SerializeField]
  public Projectile projectilePrefab;
  [SerializeField]
  public int baseProjectilesCount = 15;
  public Projectile[] projectiles;
  public Projectile projectile;

  public Projectile ProjectilePrefab => this.projectilePrefab;

  public int BaseProjectilesCount => this.baseProjectilesCount;

  public void Awake()
  {
    this.projectile = this.GetComponent<Projectile>();
    this.projectiles = new Projectile[this.baseProjectilesCount];
    this.InitializeProjectiles();
  }

  public void InitializeProjectiles()
  {
    if (ObjectPool.CountPooled<Projectile>(this.projectilePrefab) != 0)
      return;
    ObjectPool.CreatePool<Projectile>(this.projectilePrefab, this.baseProjectilesCount);
  }

  public override void Init(float radius)
  {
    float degree = 0.0f;
    float num = 360f / (float) this.baseProjectilesCount;
    for (int index = 0; index < this.baseProjectilesCount; ++index)
    {
      Projectile projectile = ObjectPool.Spawn<Projectile>(this.projectilePrefab, this.transform);
      Vector2 endValue = Utils.DegreeToVector2(degree) * radius;
      projectile.transform.localPosition = Vector3.zero;
      projectile.transform.DOLocalMove((Vector3) endValue, this.durationTillMaxRadius);
      projectile.team = Health.Team.Team2;
      projectile.Owner = this.projectile.health;
      projectile.enabled = false;
      this.projectiles[index] = projectile;
      degree += num;
    }
  }

  public override void InitDelayed(
    GameObject target,
    float radius,
    float shootDelay,
    System.Action onShoot = null)
  {
    float degree = 0.0f;
    float num = 360f / (float) this.baseProjectilesCount;
    for (int index = 0; index < this.baseProjectilesCount; ++index)
    {
      Projectile projectile = ObjectPool.Spawn<Projectile>(this.projectilePrefab, this.transform);
      Vector2 vector2 = Utils.DegreeToVector2(degree) * radius;
      projectile.transform.localPosition = Vector3.zero;
      projectile.transform.localPosition = (Vector3) vector2;
      projectile.team = Health.Team.Team2;
      projectile.Owner = this.projectile.health;
      projectile.enabled = false;
      projectile.gameObject.SetActive(false);
      this.projectiles[index] = projectile;
      degree += num;
    }
    this.StartCoroutine(this.EnableProjectiles(target, shootDelay, onShoot));
  }

  public IEnumerator EnableProjectiles(GameObject target, float delay, System.Action onShoot)
  {
    ProjectileCirclePattern projectileCirclePattern = this;
    projectileCirclePattern.projectile.SpeedMultiplier = 0.0f;
    Projectile[] projectileArray = projectileCirclePattern.projectiles;
    for (int index = 0; index < projectileArray.Length; ++index)
    {
      projectileArray[index].gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(0.02f);
    }
    projectileArray = (Projectile[]) null;
    yield return (object) new WaitForSeconds(0.25f + delay);
    foreach (Projectile projectile in projectileCirclePattern.projectiles)
    {
      if ((bool) (UnityEngine.Object) projectile)
        projectile.enabled = true;
    }
    if ((bool) (UnityEngine.Object) target)
      projectileCirclePattern.projectile.Angle = Utils.GetAngle(projectileCirclePattern.transform.position, target.transform.position);
    System.Action action = onShoot;
    if (action != null)
      action();
    projectileCirclePattern.projectile.SpeedMultiplier = 1f;
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
