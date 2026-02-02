// Decompiled with JetBrains decompiler
// Type: EnemyEgg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using Spine.Unity;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyEgg : UnitObject
{
  public List<SkeletonAnimation> Spine = new List<SkeletonAnimation>();
  public SimpleSpineFlash SimpleSpineFlash;
  public ParticleSystem hatchParticles;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string onWarningCrackPath = string.Empty;
  [EventRef]
  public string onHatchPath = string.Empty;
  public AssetReferenceGameObject EnemyPrefab;
  public int numHatchlings;
  public float hatchlingDistanceFromEgg;
  public float hatchTime = 5f;
  public int numWarningCracks = 5;
  public int warningCrackCounter;
  public GameManager gm;
  public float laidTimestamp;
  public bool isBroken;
  public static List<EnemyEgg> EnemyEggs = new List<EnemyEgg>();
  public AsyncOperationHandle<GameObject> loadedEnemyPrefabHandle;

  public override void Awake()
  {
    base.Awake();
    this.Preload();
  }

  public void Preload()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.EnemyPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedEnemyPrefabHandle = obj;
      obj.Result.CreatePool(this.numHatchlings, true);
    });
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.warningCrackCounter = 0;
    foreach (SkeletonAnimation skeletonAnimation in this.Spine)
    {
      if (skeletonAnimation.AnimationState != null)
      {
        skeletonAnimation.AnimationState.SetAnimation(0, "get-laid", false);
        skeletonAnimation.transform.DOComplete();
      }
    }
    this.health.OnDieCallback.AddListener(new UnityAction(this.BreakAnEgg));
  }

  public override void OnDisable()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    this.health.OnDieCallback.RemoveListener(new UnityAction(this.BreakAnEgg));
    base.OnDisable();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!this.loadedEnemyPrefabHandle.IsValid())
      return;
    Addressables.Release<GameObject>(this.loadedEnemyPrefabHandle);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    foreach (SkeletonAnimation skeletonAnimation in this.Spine)
    {
      if (skeletonAnimation.AnimationState != null)
      {
        skeletonAnimation.AnimationState.SetAnimation(0, "squash", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "idle", false, 0.0f);
      }
    }
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void Update()
  {
    if (this.isBroken)
      return;
    if (PlayerRelic.TimeFrozen)
    {
      this.laidTimestamp += Time.deltaTime;
    }
    else
    {
      base.Update();
      if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
      {
        this.gm = GameManager.GetInstance();
        if (!((UnityEngine.Object) this.gm != (UnityEngine.Object) null))
          return;
        this.laidTimestamp = this.gm.CurrentTime;
        this.laidTimestamp += UnityEngine.Random.Range(0.0f, 0.5f);
      }
      if ((double) this.gm.TimeSince(this.laidTimestamp) >= (double) this.hatchTime)
      {
        this.Hatch();
      }
      else
      {
        if ((double) this.warningCrackCounter >= (double) this.gm.TimeSince(this.laidTimestamp) / (double) this.hatchTime * (double) this.numWarningCracks)
          return;
        this.WarningCrack();
        ++this.warningCrackCounter;
      }
    }
  }

  public void WarningCrack()
  {
    foreach (SkeletonAnimation skeletonAnimation in this.Spine)
    {
      if (skeletonAnimation.AnimationState != null)
      {
        skeletonAnimation.AnimationState.SetAnimation(0, "squash", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "idle", false, 0.0f);
        switch (this.warningCrackCounter)
        {
          case 0:
            skeletonAnimation.skeleton.SetSkin("egg_0");
            continue;
          case 1:
            skeletonAnimation.skeleton.SetSkin("egg_1");
            continue;
          case 2:
            skeletonAnimation.skeleton.SetSkin("egg_2");
            continue;
          case 3:
            skeletonAnimation.skeleton.SetSkin("egg_3");
            continue;
          default:
            continue;
        }
      }
    }
    if (string.IsNullOrEmpty(this.onWarningCrackPath))
      return;
    AudioManager.Instance.PlayOneShot(this.onWarningCrackPath, this.transform.position);
  }

  public void Hatch()
  {
    this.BreakAnEgg();
    for (int index = 0; index < this.numHatchlings; ++index)
    {
      Vector3 vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle.normalized * this.hatchlingDistanceFromEgg);
      if (!(bool) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) vector3.normalized, this.hatchlingDistanceFromEgg, (int) this.layerToCheck))
      {
        GameObject gameObject = ObjectPool.Spawn(this.loadedEnemyPrefabHandle.Result, this.transform.parent, this.transform.position + vector3, Quaternion.identity).gameObject;
        gameObject.transform.DOComplete();
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        UnitObject component = gameObject.GetComponent<UnitObject>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.RemoveModifier();
      }
    }
  }

  public void BreakAnEgg()
  {
    this.isBroken = true;
    if ((UnityEngine.Object) this.hatchParticles != (UnityEngine.Object) null)
      this.hatchParticles.Play();
    foreach (SkeletonAnimation skeletonAnimation in this.Spine)
    {
      if (skeletonAnimation.AnimationState != null)
      {
        skeletonAnimation.AnimationState.SetAnimation(0, "squash", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "idle", false, 0.0f);
        skeletonAnimation.skeleton.SetSkin("egg_4");
      }
    }
    if (!string.IsNullOrEmpty(this.onHatchPath))
      AudioManager.Instance.PlayOneShot(this.onHatchPath, this.transform.position);
    ShowHPBar component1 = this.GetComponent<ShowHPBar>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.DestroyHPBar();
    this.health.ClearElectrified();
    this.health.ClearPoison();
    this.health.ClearIce();
    this.health.ClearCharm();
    this.health.enabled = false;
    this.enabled = false;
    Collider2D component2 = this.GetComponent<Collider2D>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component2.enabled = false;
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__18_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedEnemyPrefabHandle = obj;
    obj.Result.CreatePool(this.numHatchlings, true);
  }
}
