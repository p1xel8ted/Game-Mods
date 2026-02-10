// Decompiled with JetBrains decompiler
// Type: FleshEgg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FleshEgg : BaseMonoBehaviour
{
  [SerializeField]
  public float hatchTime = 5f;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public List<GameObject> hatchlings;
  public string GetHitSFX = "event:/dlc/dungeon06/trap/flesh_ball/egg_gethit";
  public string DeathSFX = "event:/dlc/dungeon06/trap/flesh_ball/egg_death";
  public Health health;
  public SimpleSpineFlash spineFlash;
  public bool timeFrozen;
  public Tweener _rattleTween;
  public static HashSet<GameObject> s_prewarmed = new HashSet<GameObject>();

  public IReadOnlyList<GameObject> Hatchlings => (IReadOnlyList<GameObject>) this.hatchlings;

  public void Awake()
  {
    this.health = this.GetComponent<Health>();
    this.spineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
    if (this.spine != null)
      return;
    this.spine = this.GetComponentInChildren<SkeletonAnimation>();
  }

  public void OnEnable()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void Start()
  {
    if (PlayerRelic.TimeFrozen)
    {
      this.timeFrozen = true;
      if ((Object) this.spine != (Object) null)
        this.spine.timeScale = 0.0001f;
    }
    this.StartCoroutine((IEnumerator) this.HatchingRoutine());
  }

  public void OnDisable()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void OnHit(
    GameObject attacker,
    Vector3 attacklocation,
    Health.AttackTypes attackType,
    bool fromBehind)
  {
    if (string.IsNullOrEmpty(this.GetHitSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.GetHitSFX);
  }

  public void OnDie(
    GameObject attacker,
    Vector3 attacklocation,
    Health victim,
    Health.AttackTypes attacktype,
    Health.AttackFlags attackflags)
  {
    if (string.IsNullOrEmpty(this.DeathSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.DeathSFX);
  }

  public IEnumerator HatchingRoutine()
  {
    float elapsed = 0.0f;
    float initialInterval = 0.8f;
    float finalInterval = 0.2f;
    float currentInterval;
    for (; (double) elapsed < (double) this.hatchTime; elapsed += currentInterval)
    {
      float t = elapsed / this.hatchTime;
      currentInterval = Mathf.Lerp(initialInterval, finalInterval, t);
      this.Rattle(Mathf.Lerp(0.1f, 0.5f, t));
      yield return (object) CoroutineStatics.WaitForScaledSeconds(currentInterval, this.spine);
    }
    this.Hatch();
  }

  public void Rattle(float strength)
  {
    if (this._rattleTween != null && this._rattleTween.IsActive())
      this._rattleTween.Kill();
    this._rattleTween = this.transform.DOPunchScale(Vector3.one * strength, strength, 1, 0.0f).SetRecyclable<Tweener>(true);
    if (!((Object) this.spineFlash != (Object) null))
      return;
    this.spineFlash.Flash(Color.white, 0.1f);
  }

  public void Hatch()
  {
    if (this.hatchlings == null || this.hatchlings.Count == 0)
      return;
    GameObject gameObject = ObjectPool.Spawn(this.hatchlings[Random.Range(0, this.hatchlings.Count)], this.transform.parent, this.transform.position, Quaternion.identity);
    gameObject.transform.DOScale(Vector3.zero, 0.5f).From<TweenerCore<Vector3, Vector3, VectorOptions>>().SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetRecyclable<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    Health component = gameObject.GetComponent<Health>();
    if ((Object) component != (Object) null)
      Interaction_Chest.Instance?.AddEnemy(component);
    if (!((Object) this.health != (Object) null))
      return;
    this.health.Kill();
  }

  public void Update()
  {
    if (PlayerRelic.TimeFrozen == this.timeFrozen)
      return;
    this.timeFrozen = PlayerRelic.TimeFrozen;
    if (!((Object) this.spine != (Object) null))
      return;
    this.spine.timeScale = this.timeFrozen ? 0.0001f : 1f;
  }

  public static void EnsurePrewarmed(List<GameObject> prefabs, int countPerPrefab)
  {
    if (prefabs == null || prefabs.Count == 0 || countPerPrefab <= 0)
      return;
    foreach (GameObject prefab in prefabs)
    {
      if (!((Object) prefab == (Object) null) && !FleshEgg.s_prewarmed.Contains(prefab))
      {
        for (int index = 0; index < countPerPrefab; ++index)
          ObjectPool.Spawn(prefab, (Transform) null, Vector3.zero, Quaternion.identity, false);
        FleshEgg.s_prewarmed.Add(prefab);
      }
    }
  }

  public static void Prewarm(IEnumerable<GameObject> prefabs, int countPerPrefab, Transform parent = null)
  {
    if (!(prefabs is List<GameObject> prefabs1))
      prefabs1 = new List<GameObject>(prefabs);
    FleshEgg.EnsurePrewarmed(prefabs1, countPerPrefab);
  }

  public static void CleanCache() => FleshEgg.s_prewarmed.Clear();
}
