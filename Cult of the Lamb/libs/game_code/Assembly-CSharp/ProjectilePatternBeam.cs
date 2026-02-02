// Decompiled with JetBrains decompiler
// Type: ProjectilePatternBeam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectilePatternBeam : ProjectilePatternBase
{
  [Space]
  [SerializeField]
  public Projectile bulletPrefab;
  [SerializeField]
  public ProjectilePatternBeam.BulletWave[] bulletWaves;
  [Space]
  [SerializeField]
  public bool ContinuouslyTargetPlayer = true;
  [SerializeField]
  public float distance;
  [SerializeField]
  public bool debug;
  public Health health;
  public bool isBulletShotPlayedOnce;

  public ProjectilePatternBeam.BulletWave[] BulletWaves => this.bulletWaves;

  public Projectile BulletPrefab => this.bulletPrefab;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    if ((bool) (UnityEngine.Object) this.health)
      this.health.OnDie += new Health.DieAction(this.Health_OnDie);
    int initialPoolSize = 0;
    for (int index = 0; index < this.BulletWaves.Length; ++index)
      initialPoolSize += this.BulletWaves[index].Bullets;
    ObjectPool.CreatePool<Projectile>(this.bulletPrefab, initialPoolSize);
  }

  public void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.health)
      return;
    this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StopAllCoroutines();
  }

  public void Shoot(GameObject target = null)
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(0.0f, target, (Transform) null, false));
  }

  public void Shoot(float delay)
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(delay, (GameObject) null, (Transform) null, false));
  }

  public override IEnumerator ShootIE(
    float delay = 0.0f,
    GameObject target = null,
    Transform parent = null,
    bool allowSFXForEachShot = false)
  {
    ProjectilePatternBeam projectilePatternBeam = this;
    if ((double) delay != 0.0)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * projectilePatternBeam.timeScale) < (double) delay)
        yield return (object) null;
    }
    int count = 0;
    foreach (ProjectilePatternBeam.BulletWave bulletWave in projectilePatternBeam.bulletWaves)
      projectilePatternBeam.StartCoroutine((IEnumerator) projectilePatternBeam.SpawnBullets(bulletWave, target, parent, (System.Action) (() => ++count), allowSFXForEachShot));
    while (count < projectilePatternBeam.bulletWaves.Length)
      yield return (object) null;
  }

  public IEnumerator SpawnBullets(
    ProjectilePatternBeam.BulletWave bulletWave,
    GameObject target,
    Transform parent,
    System.Action callback,
    bool allowSFXForEachShot)
  {
    ProjectilePatternBeam projectilePatternBeam = this;
    if ((UnityEngine.Object) projectilePatternBeam.health == (UnityEngine.Object) null)
      projectilePatternBeam.health = projectilePatternBeam.GetComponent<Health>();
    target = (UnityEngine.Object) target == (UnityEngine.Object) null ? PlayerFarming.Instance.gameObject : target;
    float targetAngle = !bulletWave.TargetPlayer || !((UnityEngine.Object) target != (UnityEngine.Object) null) ? Utils.GetAngle(Vector3.zero, projectilePatternBeam.transform.right) : Utils.GetAngle(projectilePatternBeam.transform.position, target.transform.position);
    float pingPongTime = 0.0f;
    float offsetIncrement = 0.0f;
    for (int i = 0; i < bulletWave.Bullets; ++i)
    {
      if (!projectilePatternBeam.isBulletShotPlayedOnce | allowSFXForEachShot)
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_boss/patrol_boss_fire_projectile", projectilePatternBeam.gameObject);
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_acidslime", projectilePatternBeam.gameObject);
        projectilePatternBeam.isBulletShotPlayedOnce = true;
      }
      if (projectilePatternBeam.ContinuouslyTargetPlayer && bulletWave.TargetPlayer && (UnityEngine.Object) target != (UnityEngine.Object) null)
        targetAngle = Utils.GetAngle(projectilePatternBeam.transform.position, target.transform.position);
      else if (!bulletWave.TargetPlayer || (UnityEngine.Object) target == (UnityEngine.Object) null)
        targetAngle = Utils.GetAngle(Vector3.zero, projectilePatternBeam.transform.right);
      double num1 = (double) targetAngle + (double) bulletWave.Offset;
      float num2 = (float) (((double) Mathf.PingPong(pingPongTime, bulletWave.SinMinMax.y - bulletWave.SinMinMax.x) + (double) bulletWave.SinMinMax.x) * (bulletWave.InvertSin ? -1.0 : 1.0));
      double num3 = (double) offsetIncrement;
      float num4 = Utils.Repeat((float) (num1 + num3) + num2, 360f);
      Projectile component = ObjectPool.Spawn<Projectile>(projectilePatternBeam.bulletPrefab, (UnityEngine.Object) parent != (UnityEngine.Object) null ? parent : projectilePatternBeam.transform.parent, projectilePatternBeam.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)).GetComponent<Projectile>();
      component.transform.position = projectilePatternBeam.transform.position - Vector3.forward * 0.2f;
      component.Angle = num4;
      component.team = (UnityEngine.Object) projectilePatternBeam.health != (UnityEngine.Object) null ? projectilePatternBeam.health.team : Health.Team.Team2;
      component.Owner = projectilePatternBeam.health;
      component.Speed = bulletWave.Speed + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      component.Acceleration = bulletWave.Acceleration + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      component.Deceleration = bulletWave.Deceleration + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      projectilePatternBeam.SpawnedProjectile();
      offsetIncrement += bulletWave.OffsetIncrement;
      pingPongTime += bulletWave.SinAmountPerBullet;
      if ((double) bulletWave.DelayBetweenBullets != 0.0)
      {
        float time = 0.0f;
        while ((double) (time += Time.deltaTime * projectilePatternBeam.timeScale) < (double) bulletWave.DelayBetweenBullets)
          yield return (object) null;
      }
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void OnDrawGizmos()
  {
    if (this.bulletWaves == null || (double) this.distance <= 0.0 || !this.debug)
      return;
    foreach (ProjectilePatternBeam.BulletWave bulletWave in this.bulletWaves)
    {
      float distance = this.distance;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = (float) ((double) Utils.GetAngle(Vector3.zero, this.transform.right) + (double) num1 - (double) num2 / 2.0) + bulletWave.Offset;
      float t = 0.0f;
      for (int index = 0; index < bulletWave.Bullets; ++index)
      {
        float num4 = num3 + (float) (((double) Mathf.PingPong(t, bulletWave.SinMinMax.y - bulletWave.SinMinMax.x) + (double) bulletWave.SinMinMax.x) * (bulletWave.InvertSin ? -1.0 : 1.0));
        Utils.DrawCircleXY(Vector3.Lerp(this.transform.position, this.transform.position + (Vector3) new Vector2(Mathf.Cos(num4 * ((float) Math.PI / 180f)), Mathf.Sin(num4 * ((float) Math.PI / 180f))).normalized * 10f, distance), 0.15f, Color.green);
        num3 = Utils.Repeat(num3 + bulletWave.OffsetIncrement, 360f);
        distance += bulletWave.DelayBetweenBullets / 3f;
        t += bulletWave.SinAmountPerBullet;
      }
    }
  }

  [Serializable]
  public struct BulletWave
  {
    public int Bullets;
    public float Speed;
    public float Acceleration;
    public float Deceleration;
    public float BulletOffset;
    public float Offset;
    public float OffsetIncrement;
    public float DelayBetweenBullets;
    [Space]
    public Vector2 SinMinMax;
    public float SinAmountPerBullet;
    public bool InvertSin;
    [Space]
    public bool TargetPlayer;
    public Vector2 Randomness;
  }
}
