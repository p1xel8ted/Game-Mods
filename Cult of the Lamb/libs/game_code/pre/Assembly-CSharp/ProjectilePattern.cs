// Decompiled with JetBrains decompiler
// Type: ProjectilePattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ProjectilePattern : ProjectilePatternBase
{
  [Space]
  [SerializeField]
  private Projectile bulletPrefab;
  [SerializeField]
  private ProjectilePattern.BulletWave[] bulletWaves;
  [Space]
  [SerializeField]
  private float globalSpeed = -1f;
  [SerializeField]
  private float globalDelayBetweenGroups = -1f;
  [SerializeField]
  private float globalAcceleration = -1f;
  [Space]
  [SerializeField]
  private bool targetPlayer = true;
  [SerializeField]
  private bool recalculatePlayerEachWave;
  [SerializeField]
  private bool repositionEachWave = true;
  [SerializeField]
  private float distance;
  [SerializeField]
  private bool debug;
  private Health health;
  private const int MAX_FRAMES_DELAY = 3;
  private int framesDelayed;
  private bool isBulletShotPlayedOnce;

  public ProjectilePattern.BulletWave[] Waves => this.bulletWaves;

  public Projectile BulletPrefab => this.bulletPrefab;

  private void Start()
  {
    this.health = this.GetComponentInParent<Health>();
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

  public void Shoot(float delay, GameObject target, Transform parent)
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(delay, target, parent));
  }

  public override IEnumerator ShootIE(float delay = 0.0f, GameObject target = null, Transform parent = null)
  {
    ProjectilePattern projectilePattern = this;
    target = (UnityEngine.Object) target == (UnityEngine.Object) null ? PlayerFarming.Instance.gameObject : target;
    float angle = Utils.GetAngle(projectilePattern.transform.position, target.transform.position);
    angle = projectilePattern.targetPlayer ? angle : Utils.GetAngle(Vector3.zero, projectilePattern.transform.right);
    if ((double) delay != 0.0)
      yield return (object) new WaitForSeconds(delay);
    int currentGroupID = 0;
    Vector3 startPosition = projectilePattern.transform.position - Vector3.forward * 0.2f;
    List<Projectile[]> projectiles = new List<Projectile[]>();
    bool isSpawnedAllAtOnce = true;
    for (int index = 0; index < projectilePattern.bulletWaves.Length; ++index)
    {
      if ((double) projectilePattern.bulletWaves[index].FinishDelay > 0.0)
        isSpawnedAllAtOnce = false;
    }
    if ((double) projectilePattern.globalDelayBetweenGroups != -1.0)
      isSpawnedAllAtOnce = false;
    projectilePattern.isBulletShotPlayedOnce = false;
    for (int i = 0; i < projectilePattern.bulletWaves.Length; ++i)
    {
      if (projectilePattern.bulletWaves[i].WaveGroupID != currentGroupID && (double) projectilePattern.globalDelayBetweenGroups != -1.0)
      {
        currentGroupID = projectilePattern.bulletWaves[i].WaveGroupID;
        yield return (object) new WaitForSeconds(projectilePattern.globalDelayBetweenGroups);
        projectilePattern.isBulletShotPlayedOnce = false;
      }
      if (projectilePattern.recalculatePlayerEachWave)
        angle = projectilePattern.targetPlayer ? Utils.GetAngle(projectilePattern.transform.position, PlayerFarming.Instance.transform.position) : angle;
      if (projectilePattern.repositionEachWave)
        startPosition = projectilePattern.transform.position - Vector3.forward * 0.2f;
      projectiles.Add(projectilePattern.SpawnBullets(projectilePattern.bulletWaves[i], angle, parent, startPosition, !isSpawnedAllAtOnce));
      if ((double) projectilePattern.globalDelayBetweenGroups == -1.0 && (double) projectilePattern.bulletWaves[i].FinishDelay != 0.0)
      {
        yield return (object) new WaitForSeconds(projectilePattern.bulletWaves[i].FinishDelay);
        projectilePattern.isBulletShotPlayedOnce = false;
      }
      if (isSpawnedAllAtOnce && i == Mathf.FloorToInt((float) (projectilePattern.bulletWaves.Length / 2)) - 1)
        yield return (object) null;
    }
    projectilePattern.framesDelayed = 0;
    if (isSpawnedAllAtOnce)
      yield return (object) projectilePattern.AddSpeedToEachWave(projectiles);
  }

  private IEnumerator AddSpeedToEachWave(List<Projectile[]> projectiles)
  {
    for (int i = 0; i < projectiles.Count; ++i)
    {
      for (int index = 0; index < projectiles[i].Length; ++index)
        projectiles[i][index].CircleCollider2D.enabled = true;
      if (this.framesDelayed <= 3)
      {
        yield return (object) null;
        ++this.framesDelayed;
      }
    }
    for (int index = 0; index < projectiles.Count; ++index)
      this.AddSpeed(this.bulletWaves[index], projectiles[index]);
  }

  private void AddSpeed(ProjectilePattern.BulletWave bulletWave, Projectile[] projectiles)
  {
    for (int index = 0; index < projectiles.Length; ++index)
    {
      projectiles[index].ArrowImage.gameObject.SetActive(true);
      projectiles[index].Speed = ((double) this.globalSpeed == -1.0 ? bulletWave.Speed : this.globalSpeed) + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      projectiles[index].Acceleration = ((double) this.globalAcceleration == -1.0 ? bulletWave.Acceleration : this.globalAcceleration) + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      projectiles[index].Deceleration = bulletWave.Deceleration + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
    }
  }

  private Projectile[] SpawnBullets(
    ProjectilePattern.BulletWave bulletWave,
    float playerAngle,
    Transform parent,
    Vector3 spawnPosition,
    bool addSpeedImmediately = true)
  {
    Projectile[] projectileArray = new Projectile[bulletWave.Bullets];
    float num1 = bulletWave.AngleBetweenBullets * (float) (bulletWave.Bullets - 1);
    float num2 = playerAngle - num1 / 2f + bulletWave.Offset;
    for (int index = 0; index < bulletWave.Bullets; ++index)
    {
      if (!this.isBulletShotPlayedOnce)
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_boss/patrol_boss_fire_projectile", this.gameObject);
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_acidslime", this.gameObject);
        this.isBulletShotPlayedOnce = true;
      }
      Projectile component = ObjectPool.Spawn<Projectile>(this.bulletPrefab, (UnityEngine.Object) parent != (UnityEngine.Object) null ? parent : this.transform.parent).GetComponent<Projectile>();
      component.transform.position = spawnPosition;
      component.Angle = num2;
      component.team = this.health.team;
      component.health = this.health;
      if (addSpeedImmediately)
      {
        component.Speed = ((double) this.globalSpeed == -1.0 ? bulletWave.Speed : this.globalSpeed) + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
        component.Acceleration = ((double) this.globalAcceleration == -1.0 ? bulletWave.Acceleration : this.globalAcceleration) + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
        component.Deceleration = bulletWave.Deceleration + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      }
      else
      {
        component.Speed = 0.0f;
        component.Acceleration = 0.0f;
        component.Deceleration = 0.0f;
        component.CircleCollider2D.enabled = false;
        component.ArrowImage.gameObject.SetActive(false);
      }
      projectileArray[index] = component;
      this.SpawnedProjectile();
      num2 = Mathf.Repeat(num2 + bulletWave.AngleBetweenBullets, 360f);
    }
    return projectileArray;
  }

  private void OnDrawGizmos()
  {
    if (this.bulletWaves == null || (double) this.distance <= 0.0 || !this.debug)
      return;
    float distance = this.distance;
    int num1 = 0;
    foreach (ProjectilePattern.BulletWave bulletWave in this.bulletWaves)
    {
      if (bulletWave.WaveGroupID != num1 && (double) this.globalDelayBetweenGroups != -1.0)
      {
        num1 = bulletWave.WaveGroupID;
        distance += this.globalDelayBetweenGroups / 3f;
      }
      float num2 = 0.0f;
      float num3 = bulletWave.AngleBetweenBullets * (float) (bulletWave.Bullets - 1);
      float num4 = Mathf.Repeat(Utils.GetAngle(Vector3.zero, this.transform.right) + (num2 - num3 / 2f + bulletWave.Offset), 360f);
      for (int index = 0; index < bulletWave.Bullets; ++index)
      {
        Utils.DrawCircleXY(Vector3.Lerp(this.transform.position, this.transform.position + (Vector3) new Vector2(Mathf.Cos(num4 * ((float) Math.PI / 180f)), Mathf.Sin(num4 * ((float) Math.PI / 180f))).normalized * 10f, distance), 0.15f, Color.green);
        num4 = Mathf.Repeat(num4 + bulletWave.AngleBetweenBullets, 360f);
      }
      distance += bulletWave.FinishDelay / 3f;
    }
  }

  [Serializable]
  public struct BulletWave
  {
    public int Bullets;
    public float Speed;
    public float Acceleration;
    public float Deceleration;
    public float Offset;
    public float AngleBetweenBullets;
    public float FinishDelay;
    public int WaveGroupID;
    public Vector2 Randomness;
  }
}
