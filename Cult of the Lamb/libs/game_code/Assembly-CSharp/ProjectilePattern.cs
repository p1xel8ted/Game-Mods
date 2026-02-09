// Decompiled with JetBrains decompiler
// Type: ProjectilePattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ProjectilePattern : ProjectilePatternBase
{
  [Space]
  [SerializeField]
  public Projectile bulletPrefab;
  [SerializeField]
  public ProjectilePattern.BulletWave[] bulletWaves;
  [Space]
  [SerializeField]
  public float globalSpeed = -1f;
  [SerializeField]
  public float globalDelayBetweenGroups = -1f;
  [SerializeField]
  public float globalAcceleration = -1f;
  [Space]
  [SerializeField]
  public bool targetPlayer = true;
  [SerializeField]
  public bool recalculatePlayerEachWave;
  [SerializeField]
  public bool repositionEachWave = true;
  [SerializeField]
  public bool loop;
  [SerializeField]
  public float distance;
  [SerializeField]
  public bool debug;
  public Health health;
  public const int MAX_FRAMES_DELAY = 3;
  public int framesDelayed;
  public bool isBulletShotPlayedOnce;

  public ProjectilePattern.BulletWave[] Waves => this.bulletWaves;

  public bool Loop
  {
    get => this.loop;
    set => this.loop = value;
  }

  public Projectile BulletPrefab => this.bulletPrefab;

  public event ProjectilePattern.ProjectileWaveEvent OnProjectileWaveShot;

  public void Start()
  {
    this.health = this.GetComponentInParent<Health>();
    if ((bool) (UnityEngine.Object) this.health)
      this.health.OnDie += new Health.DieAction(this.Health_OnDie);
    ObjectPool.CreatePool<Projectile>(this.bulletPrefab, Mathf.Max(25, this.CountProjectiles(3f)));
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

  public void Shoot()
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(0.0f, (GameObject) null, (Transform) null, false));
  }

  public void Shoot(float delay, GameObject target, Transform parent)
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(delay, target, parent, false));
  }

  public void Shoot(float delay, Vector3 targetPosition, Transform parent)
  {
    this.StartCoroutine((IEnumerator) this.ShootIE(targetPosition, delay, parent));
  }

  public override IEnumerator ShootIE(
    float delay = 0.0f,
    GameObject target = null,
    Transform parent = null,
    bool allowSFXForEachShot = false)
  {
    ProjectilePattern projectilePattern = this;
    while (true)
    {
      target = (UnityEngine.Object) target == (UnityEngine.Object) null ? PlayerFarming.Instance.gameObject : target;
      float angle = Utils.GetAngle(projectilePattern.transform.position, target.transform.position);
      angle = projectilePattern.targetPlayer ? angle : Utils.GetAngle(Vector3.zero, projectilePattern.transform.right);
      float time = 0.0f;
      if ((double) delay != 0.0)
      {
        time = 0.0f;
        while ((double) (time += Time.deltaTime * projectilePattern.timeScale) < (double) delay)
          yield return (object) null;
      }
      int currentGroupID = 0;
      Vector3 startPosition = projectilePattern.transform.position - Vector3.forward * 0.2f;
      List<Projectile[]> projectiles = new List<Projectile[]>();
      bool isSpawnedAllAtOnce = true;
      projectilePattern.framesDelayed = 0;
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
          time = 0.0f;
          while ((double) (time += Time.deltaTime * projectilePattern.timeScale) < (double) projectilePattern.globalDelayBetweenGroups)
            yield return (object) null;
          projectilePattern.isBulletShotPlayedOnce = false;
        }
        if (projectilePattern.recalculatePlayerEachWave)
          angle = projectilePattern.targetPlayer ? Utils.GetAngle(projectilePattern.transform.position, PlayerFarming.Instance.transform.position) : angle;
        if (projectilePattern.repositionEachWave)
          startPosition = projectilePattern.transform.position - Vector3.forward * 0.2f;
        projectiles.Add(projectilePattern.SpawnBullets(projectilePattern.bulletWaves[i], angle, parent, startPosition, !isSpawnedAllAtOnce));
        if ((double) projectilePattern.globalDelayBetweenGroups == -1.0 && (double) projectilePattern.bulletWaves[i].FinishDelay != 0.0)
        {
          time = 0.0f;
          while ((double) (time += Time.deltaTime * projectilePattern.timeScale) < (double) projectilePattern.bulletWaves[i].FinishDelay)
            yield return (object) null;
          projectilePattern.isBulletShotPlayedOnce = false;
        }
        if (isSpawnedAllAtOnce && i != 0 && i % 2 == 0 && projectilePattern.framesDelayed <= 3)
        {
          yield return (object) null;
          ++projectilePattern.framesDelayed;
        }
      }
      projectilePattern.framesDelayed = 0;
      if (isSpawnedAllAtOnce)
        yield return (object) projectilePattern.AddSpeedToEachWave(projectiles);
      if (projectilePattern.loop)
      {
        yield return (object) new WaitForEndOfFrame();
        startPosition = new Vector3();
        projectiles = (List<Projectile[]>) null;
      }
      else
        break;
    }
  }

  public IEnumerator ShootIE(Vector3 targetPosition, float delay = 0.0f, Transform parent = null)
  {
    ProjectilePattern projectilePattern = this;
    float angle = Utils.GetAngle(projectilePattern.transform.position, targetPosition);
    angle = projectilePattern.targetPlayer ? angle : Utils.GetAngle(Vector3.zero, projectilePattern.transform.right);
    float time = 0.0f;
    if ((double) delay != 0.0)
    {
      time = 0.0f;
      while ((double) (time += Time.deltaTime * projectilePattern.timeScale) < (double) delay)
        yield return (object) null;
    }
    int currentGroupID = 0;
    Vector3 startPosition = projectilePattern.transform.position - Vector3.forward * 0.2f;
    List<Projectile[]> projectiles = new List<Projectile[]>();
    bool isSpawnedAllAtOnce = true;
    projectilePattern.framesDelayed = 0;
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
        time = 0.0f;
        while ((double) (time += Time.deltaTime * projectilePattern.timeScale) < (double) projectilePattern.globalDelayBetweenGroups)
          yield return (object) null;
        projectilePattern.isBulletShotPlayedOnce = false;
      }
      if (projectilePattern.recalculatePlayerEachWave)
        angle = projectilePattern.targetPlayer ? Utils.GetAngle(projectilePattern.transform.position, PlayerFarming.Instance.transform.position) : angle;
      if (projectilePattern.repositionEachWave)
        startPosition = projectilePattern.transform.position - Vector3.forward * 0.2f;
      projectiles.Add(projectilePattern.SpawnBullets(projectilePattern.bulletWaves[i], angle, parent, startPosition, !isSpawnedAllAtOnce));
      if ((double) projectilePattern.globalDelayBetweenGroups == -1.0 && (double) projectilePattern.bulletWaves[i].FinishDelay != 0.0)
      {
        time = 0.0f;
        while ((double) (time += Time.deltaTime * projectilePattern.timeScale) < (double) projectilePattern.bulletWaves[i].FinishDelay)
          yield return (object) null;
        projectilePattern.isBulletShotPlayedOnce = false;
      }
      if (isSpawnedAllAtOnce && i != 0 && i % 2 == 0 && projectilePattern.framesDelayed <= 3)
      {
        yield return (object) null;
        ++projectilePattern.framesDelayed;
      }
    }
    projectilePattern.framesDelayed = 0;
    if (isSpawnedAllAtOnce)
      yield return (object) projectilePattern.AddSpeedToEachWave(projectiles);
  }

  public IEnumerator AddSpeedToEachWave(List<Projectile[]> projectiles)
  {
    for (int index = 0; index < projectiles.Count; ++index)
      this.AddSpeed(this.bulletWaves[index], projectiles[index]);
    yield return (object) null;
  }

  public void AddSpeed(ProjectilePattern.BulletWave bulletWave, Projectile[] projectiles)
  {
    for (int index = 0; index < projectiles.Length; ++index)
    {
      projectiles[index].ArrowImage.gameObject.SetActive(true);
      projectiles[index].Speed = ((double) this.globalSpeed == -1.0 ? bulletWave.Speed : this.globalSpeed) + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      projectiles[index].Acceleration = ((double) this.globalAcceleration == -1.0 ? bulletWave.Acceleration : this.globalAcceleration) + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
      projectiles[index].Deceleration = bulletWave.Deceleration + UnityEngine.Random.Range(bulletWave.Randomness.x, bulletWave.Randomness.y);
    }
  }

  public Projectile[] SpawnBullets(
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
      component.SetOwner(this.health.gameObject);
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
        component.ArrowImage.gameObject.SetActive(false);
      }
      projectileArray[index] = component;
      this.SpawnedProjectile();
      num2 = Utils.Repeat(num2 + bulletWave.AngleBetweenBullets, 360f);
    }
    bulletWave.SpawnedProjectiles = projectileArray;
    ProjectilePattern.ProjectileWaveEvent projectileWaveShot = this.OnProjectileWaveShot;
    if (projectileWaveShot != null)
      projectileWaveShot(bulletWave);
    return projectileArray;
  }

  public int CountProjectiles(float maxCountTime = 0.0f)
  {
    int num1 = 0;
    float num2 = 0.0f;
    int num3 = 0;
    for (int index = 0; index < this.bulletWaves.Length; ++index)
    {
      num1 += this.bulletWaves[index].Bullets;
      if ((double) maxCountTime > 0.0)
      {
        num2 += this.bulletWaves[index].FinishDelay;
        if ((double) this.globalDelayBetweenGroups > 0.0 && this.bulletWaves[index].WaveGroupID != num3)
        {
          num3 = this.bulletWaves[index].WaveGroupID;
          num2 += this.globalDelayBetweenGroups;
        }
        if ((double) num2 >= (double) maxCountTime)
          break;
      }
    }
    return num1;
  }

  public void OnDrawGizmos()
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
      float num4 = Utils.GetAngle(Vector3.zero, this.transform.right) + (num2 - num3 / 2f + bulletWave.Offset);
      for (int index = 0; index < bulletWave.Bullets; ++index)
      {
        Utils.DrawCircleXY(Vector3.Lerp(this.transform.position, this.transform.position + (Vector3) new Vector2(Mathf.Cos(num4 * ((float) Math.PI / 180f)), Mathf.Sin(num4 * ((float) Math.PI / 180f))).normalized * 10f, distance), 0.15f, Color.green);
        num4 = Utils.Repeat(num4 + bulletWave.AngleBetweenBullets, 360f);
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
    public string AnimationToPlay;
    [HideInInspector]
    public Projectile[] SpawnedProjectiles;
  }

  public delegate void ProjectileWaveEvent(ProjectilePattern.BulletWave wave);
}
