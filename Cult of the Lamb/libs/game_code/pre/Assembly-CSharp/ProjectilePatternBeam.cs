// Decompiled with JetBrains decompiler
// Type: ProjectilePatternBeam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ProjectilePatternBeam : ProjectilePatternBase
{
  [Space]
  [SerializeField]
  private Projectile bulletPrefab;
  [SerializeField]
  private ProjectilePatternBeam.BulletWave[] bulletWaves;
  [SerializeField]
  private float distance;
  [SerializeField]
  private bool debug;
  private Health health;

  public ProjectilePatternBeam.BulletWave[] BulletWaves => this.bulletWaves;

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    if ((bool) (UnityEngine.Object) this.health)
      this.health.OnDie += new Health.DieAction(this.Health_OnDie);
    ObjectPool.CreatePool<Projectile>(this.bulletPrefab, 25);
  }

  private void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.health)
      return;
    this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
  }

  private void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StopAllCoroutines();
  }

  public void Shoot()
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(0.0f, (GameObject) null, (Transform) null));
  }

  public void Shoot(float delay)
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(delay, (GameObject) null, (Transform) null));
  }

  public override IEnumerator ShootIE(float delay = 0.0f, GameObject target = null, Transform parent = null)
  {
    ProjectilePatternBeam projectilePatternBeam = this;
    if ((double) delay != 0.0)
      yield return (object) new WaitForSeconds(delay);
    foreach (ProjectilePatternBeam.BulletWave bulletWave in projectilePatternBeam.bulletWaves)
      projectilePatternBeam.StartCoroutine((IEnumerator) projectilePatternBeam.SpawnBullets(bulletWave, target, parent));
  }

  private IEnumerator SpawnBullets(
    ProjectilePatternBeam.BulletWave bulletWave,
    GameObject target,
    Transform parent)
  {
    ProjectilePatternBeam projectilePatternBeam = this;
    if ((UnityEngine.Object) projectilePatternBeam.health == (UnityEngine.Object) null)
      projectilePatternBeam.health = projectilePatternBeam.GetComponent<Health>();
    target = (UnityEngine.Object) target == (UnityEngine.Object) null ? PlayerFarming.Instance.gameObject : target;
    float pingPongTime = 0.0f;
    float offsetIncrement = 0.0f;
    for (int i = 0; i < bulletWave.Bullets; ++i)
    {
      double num1 = (!bulletWave.TargetPlayer || !((UnityEngine.Object) target != (UnityEngine.Object) null) ? (double) Utils.GetAngle(Vector3.zero, projectilePatternBeam.transform.right) : (double) Utils.GetAngle(projectilePatternBeam.transform.position, target.transform.position)) + (double) bulletWave.Offset;
      float num2 = Mathf.PingPong(pingPongTime, bulletWave.SinMinMax.y - bulletWave.SinMinMax.x) + bulletWave.SinMinMax.x;
      double num3 = (double) offsetIncrement;
      float num4 = Mathf.Repeat((float) (num1 + num3) + num2, 360f);
      Projectile component = ObjectPool.Spawn<Projectile>(projectilePatternBeam.bulletPrefab, (UnityEngine.Object) parent != (UnityEngine.Object) null ? parent : projectilePatternBeam.transform.parent, projectilePatternBeam.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)).GetComponent<Projectile>();
      component.transform.position = projectilePatternBeam.transform.position - Vector3.forward * 0.2f;
      component.Angle = num4;
      component.team = (UnityEngine.Object) projectilePatternBeam.health != (UnityEngine.Object) null ? projectilePatternBeam.health.team : Health.Team.Team2;
      component.health = projectilePatternBeam.health;
      component.Speed = bulletWave.Speed + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      component.Acceleration = bulletWave.Acceleration + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      component.Deceleration = bulletWave.Deceleration + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      projectilePatternBeam.SpawnedProjectile();
      offsetIncrement += bulletWave.OffsetIncrement;
      pingPongTime += bulletWave.SinAmountPerBullet;
      if ((double) bulletWave.DelayBetweenBullets != 0.0)
        yield return (object) new WaitForSeconds(bulletWave.DelayBetweenBullets);
    }
  }

  private void OnDrawGizmos()
  {
    if (this.bulletWaves == null || (double) this.distance <= 0.0 || !this.debug)
      return;
    foreach (ProjectilePatternBeam.BulletWave bulletWave in this.bulletWaves)
    {
      float distance = this.distance;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = Mathf.Repeat((float) ((double) Utils.GetAngle(Vector3.zero, this.transform.right) + (double) num1 - (double) num2 / 2.0) + bulletWave.Offset, 360f);
      float t = 0.0f;
      for (int index = 0; index < bulletWave.Bullets; ++index)
      {
        float num4 = num3 + (Mathf.PingPong(t, bulletWave.SinMinMax.y - bulletWave.SinMinMax.x) + bulletWave.SinMinMax.x);
        Utils.DrawCircleXY(Vector3.Lerp(this.transform.position, this.transform.position + (Vector3) new Vector2(Mathf.Cos(num4 * ((float) Math.PI / 180f)), Mathf.Sin(num4 * ((float) Math.PI / 180f))).normalized * 10f, distance), 0.15f, Color.green);
        num3 = Mathf.Repeat(num3 + bulletWave.OffsetIncrement, 360f);
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
    public Vector2 SinMinMax;
    public float SinAmountPerBullet;
    public bool TargetPlayer;
    public Vector2 Randomness;
  }
}
