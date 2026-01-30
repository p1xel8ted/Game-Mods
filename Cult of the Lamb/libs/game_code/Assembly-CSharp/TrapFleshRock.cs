// Decompiled with JetBrains decompiler
// Type: TrapFleshRock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TrapFleshRock : BaseMonoBehaviour
{
  [Header("Spawning Settings")]
  [Tooltip("Minimum time between rock spawns")]
  [SerializeField]
  public float spawnRateMin = 2f;
  [Tooltip("Maximum time between rock spawns")]
  [SerializeField]
  public float spawnRateMax = 4f;
  [Tooltip("Chance for a bomb rock to spawn")]
  [SerializeField]
  public float bombSpawnChance = 0.1f;
  [Tooltip("How long it takes for a rock to spawn after picking a position (Telegraphing)")]
  [SerializeField]
  public float spawnDelay;
  [Tooltip("How many rocks spawn each time")]
  [SerializeField]
  public int spawnAmount;
  [Tooltip("The initial amount of rocks that spawn around the trap")]
  [SerializeField]
  public int initialSpawnAmount;
  [Tooltip("The maximum amount of rocks that can be around the trap")]
  [SerializeField]
  public int maxRocks;
  [Header("Prefabs")]
  [Tooltip("The prefab of the rock")]
  [SerializeField]
  public GameObject rockPrefab;
  [SerializeField]
  public GameObject deathEffigyPrefab;
  [Header("Obstacle Check Settings")]
  [Tooltip("Radius used when checking if a point is clear")]
  [SerializeField]
  public float obstaclesRadiusCheck;
  [Tooltip("Layer mask used for checking obstacles")]
  [SerializeField]
  public LayerMask obstaclesLayermask;
  [Tooltip("Collider mask used for shaping the trap area")]
  [SerializeField]
  public Collider2D colliderMask;
  [SerializeField]
  public bool randomlyRotateCollider = true;
  [Header("Animations")]
  [SerializeField]
  public SkeletonAnimation spine;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string spawningAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string spawnedAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string idleAnimation;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [EventRef]
  public string GetHitSFX = "event:/dlc/dungeon06/trap/flesh_rock/main_gethit";
  [EventRef]
  public string DeathSFX = "event:/dlc/dungeon06/trap/flesh_rock/main_death";
  [EventRef]
  public string SpawnFleshBlocksSFX = "event:/dlc/dungeon06/trap/flesh_rock/block_spawn";
  public Dictionary<Vector2, GameObject> spawnedRocks = new Dictionary<Vector2, GameObject>();
  public HashSet<Vector2> reservedPositions = new HashSet<Vector2>();
  public Health trapHealth;

  public Vector2 TrapCenter
  {
    get
    {
      return new Vector2(Mathf.Floor(this.transform.position.x), Mathf.Floor(this.transform.position.y));
    }
  }

  public Health TrapHealth => this.trapHealth;

  public void Awake()
  {
    this.trapHealth = this.GetComponent<Health>();
    this.trapHealth.OnHit += new Health.HitAction(this.TrapHealth_OnHit);
  }

  public void TrapHealth_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!string.IsNullOrEmpty(this.GetHitSFX))
      AudioManager.Instance.PlayOneShot(this.GetHitSFX, this.gameObject);
    this.simpleSpineFlash.FlashFillRed();
  }

  public void Start()
  {
    if ((bool) (UnityEngine.Object) this.trapHealth)
      this.trapHealth.OnDie += new Health.DieAction(this.OnTrapDeath);
    if (this.spine.AnimationState != null)
      this.spine.AnimationState.SetAnimation(0, "idle", true);
    this.SpawnRocks(this.initialSpawnAmount);
    if (!this.randomlyRotateCollider)
      return;
    this.colliderMask.transform.rotation = Quaternion.Euler(0.0f, 0.0f, (float) UnityEngine.Random.Range(0, 360));
  }

  public void OnEnable() => this.StartCoroutine((IEnumerator) this.SpawnLoop());

  public void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.trapHealth)
      return;
    this.trapHealth.OnDie -= new Health.DieAction(this.OnTrapDeath);
    this.trapHealth.OnHit += new Health.HitAction(this.TrapHealth_OnHit);
  }

  public void OnTrapDeath(
    GameObject attacker,
    Vector3 attackLocation,
    Health victim,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathSFX))
      AudioManager.Instance.PlayOneShot(this.DeathSFX, this.gameObject);
    ObjectPool.Spawn(this.deathEffigyPrefab, this.transform.parent, this.transform.position, Quaternion.identity);
    foreach (GameObject gameObject in this.spawnedRocks.Values)
      gameObject.GetComponentInChildren<FleshRockChild>()?.SetRockActive(false, Vector2.Distance(this.TrapCenter, (Vector2) gameObject.transform.position) * 0.5f);
  }

  public void OnRockDie(
    GameObject attacker,
    Vector3 attackLocation,
    Health victim,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlags)
  {
    if ((UnityEngine.Object) victim == (UnityEngine.Object) null || (UnityEngine.Object) victim.gameObject == (UnityEngine.Object) null)
      return;
    GameObject gameObject = victim.gameObject;
    Vector2 key = new Vector2(Mathf.Floor(gameObject.transform.position.x), Mathf.Floor(gameObject.transform.position.y));
    if (!this.spawnedRocks.ContainsKey(key))
      return;
    victim.OnDie -= new Health.DieAction(this.OnRockDie);
    this.spawnedRocks.Remove(key);
  }

  public IEnumerator SpawnLoop()
  {
    while (true)
    {
      yield return (object) CoroutineStatics.WaitForScaledSeconds(UnityEngine.Random.Range(this.spawnRateMin, this.spawnRateMax), this.spine);
      this.SpawnRocks(this.spawnAmount);
    }
  }

  public void SpawnRocks(int amount)
  {
    if (CoopManager.Instance.IsSpawningPlayer || LetterBox.IsPlaying || MMConversation.isPlaying || MMTransition.IsPlaying)
      return;
    for (int index = 0; index < amount; ++index)
      this.StartCoroutine((IEnumerator) this.SpawnRockTelegraphed());
  }

  public IEnumerator SpawnRockTelegraphed()
  {
    TrapFleshRock trapFleshRock = this;
    if (trapFleshRock.spawnedRocks.Count + trapFleshRock.reservedPositions.Count < trapFleshRock.maxRocks)
    {
      yield return (object) null;
      Vector2 pos = trapFleshRock.PickPosition();
      if (!(pos == trapFleshRock.TrapCenter))
      {
        trapFleshRock.reservedPositions.Add(pos);
        trapFleshRock.StartCoroutine((IEnumerator) trapFleshRock.SpawnRumbleFX((Vector3) pos, trapFleshRock.spawnDelay));
        if (trapFleshRock.spine.AnimationState != null)
          trapFleshRock.spine.AnimationState.SetAnimation(0, trapFleshRock.spawningAnimation, false);
        if (trapFleshRock.CanSpawnAtPoint(pos) && !string.IsNullOrEmpty(trapFleshRock.SpawnFleshBlocksSFX))
        {
          AudioManager.Instance.PlayOneShot(trapFleshRock.SpawnFleshBlocksSFX, trapFleshRock.gameObject);
          GameObject gameObject = PlayerFarming.GetClosestPlayer(trapFleshRock.transform.position)?.gameObject;
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (double) Vector3.Distance(gameObject.transform.position, trapFleshRock.transform.position) < 3.0)
            MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, gameObject.GetComponent<PlayerFarming>(), coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
        }
        yield return (object) CoroutineStatics.WaitForScaledSeconds(trapFleshRock.spawnDelay, trapFleshRock.spine);
        while (CoopManager.Instance.IsSpawningPlayer || LetterBox.IsPlaying || MMConversation.isPlaying || MMTransition.IsPlaying)
          yield return (object) null;
        if (trapFleshRock.spine.AnimationState != null)
        {
          trapFleshRock.spine.AnimationState.SetAnimation(0, trapFleshRock.spawnedAnimation, false);
          trapFleshRock.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        }
        if (trapFleshRock.CanSpawnAtPoint(pos))
          trapFleshRock.SpawnRockAt(pos);
        else
          trapFleshRock.reservedPositions.Remove(pos);
      }
    }
  }

  public IEnumerator SpawnRumbleFX(Vector3 point, float duration)
  {
    float spawnRadius = 0.5f;
    float spawnEvery = 0.05f;
    float elapsed = 0.0f;
    while ((double) elapsed < (double) duration)
    {
      double deltaTime = (double) Time.deltaTime;
      SkeletonAnimation spine = this.spine;
      double num1 = spine != null ? (double) spine.timeScale : 1.0;
      float num2 = (float) (deltaTime * num1);
      elapsed += num2;
      if ((double) elapsed % (double) spawnEvery < (double) num2)
      {
        Vector3 worldPos = point + new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0.0f);
        BiomeConstants.Instance.EmitDustCloudParticles(worldPos, 5, 1.5f);
      }
      yield return (object) null;
    }
  }

  public void SpawnRockAt(Vector2 pos)
  {
    if (CoopManager.Instance.IsSpawningPlayer)
      return;
    this.reservedPositions.Remove(pos);
    this.BreakDecorationInRadius(pos);
    GameObject gameObject = ObjectPool.Spawn(this.rockPrefab, this.transform.parent, (Vector3) pos, Quaternion.identity);
    Health componentInChildren = gameObject.GetComponentInChildren<Health>();
    gameObject.GetComponentInChildren<FleshRockChild>().SetDropBombOnDeath((double) UnityEngine.Random.value < (double) this.bombSpawnChance);
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
      componentInChildren.OnDie += new Health.DieAction(this.OnRockDie);
    this.spawnedRocks.Add(pos, gameObject);
  }

  public bool CanSpawnAtPoint(Vector2 point)
  {
    bool flag1 = this.colliderMask.OverlapPoint(point);
    Debug.DrawLine((Vector3) point, (Vector3) (point + Vector2.up), Color.red, 1f);
    bool flag2 = Physics2D.OverlapCircleAll(point, this.obstaclesRadiusCheck, (int) this.obstaclesLayermask).Length == 0;
    return BiomeGenerator.PointWithinIsland((Vector3) point, out Vector3 _) & flag2 & flag1;
  }

  public void BreakDecorationInRadius(Vector2 point)
  {
    int mask = LayerMask.GetMask("Default");
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(point, 0.5f, mask))
    {
      if (collider2D.CompareTag("BreakableDecoration"))
        collider2D.GetComponent<Health>().Kill();
    }
  }

  public Vector2 PickPosition()
  {
    int num = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt((float) (this.spawnedRocks.Count + this.reservedPositions.Count + 1))));
    List<Vector2> source = new List<Vector2>();
    for (int x = -num; x <= num; ++x)
    {
      for (int y = -num; y <= num; ++y)
      {
        if ((x != 0 || y != 0) && x * x + y * y <= num * num)
          source.Add(this.TrapCenter + new Vector2((float) x, (float) y));
      }
    }
    foreach (Vector2 vector2 in (IEnumerable<Vector2>) source.OrderBy<Vector2, float>((Func<Vector2, float>) (c => Vector2.Distance(this.TrapCenter, c))))
    {
      if (!this.spawnedRocks.ContainsKey(vector2) && !this.reservedPositions.Contains(vector2) && this.CanSpawnAtPoint(vector2))
        return vector2;
    }
    return this.TrapCenter;
  }

  [CompilerGenerated]
  public float \u003CPickPosition\u003Eb__42_0(Vector2 c) => Vector2.Distance(this.TrapCenter, c);
}
