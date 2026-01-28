// Decompiled with JetBrains decompiler
// Type: TrapLava
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class TrapLava : BaseMonoBehaviour
{
  [SerializeField]
  public AreaBurnTick areaBurn;
  [SerializeField]
  public ParticleSystem particleSystem;
  [SerializeField]
  public Transform graphic;
  [Space]
  [SerializeField]
  public float lifetime = 30f;
  [SerializeField]
  public float despawnDuration = 0.25f;
  [EventRef]
  public string LavaLoopSFX = "event:/dlc/dungeon06/trap/lava/puddle_loop";
  public EventInstance lavaLoopInstanceSFX;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public float spawnTimestamp = -1f;
  public float despawnTimestamp = -1f;
  public bool isDespawning;
  public bool hasDespawned;

  public float GetLifeTime => this.lifetime;

  public float GetDespawnDuration => this.despawnDuration;

  public static void CreateLava(
    GameObject prefab,
    Vector3 position,
    Transform parent,
    Health owner)
  {
    GameObject gameObject = ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
    gameObject.transform.position = position;
    AreaBurnTick component = gameObject.GetComponent<AreaBurnTick>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.Initialize();
      component.EnableDamage(1f, 1f, 2f);
      component.SetOwner(owner);
    }
    Vector3 localScale = gameObject.transform.localScale;
    gameObject.transform.localScale = Vector3.zero;
    gameObject.transform.DOScale(localScale, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
  }

  public static void CreateLava(
    GameObject prefab,
    Vector3 position,
    Transform parent,
    Health owner,
    float lifetime)
  {
    GameObject gameObject = ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
    gameObject.transform.position = position;
    gameObject.GetComponent<TrapLava>().lifetime = lifetime;
    AreaBurnTick component = gameObject.GetComponent<AreaBurnTick>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.Initialize();
      component.EnableDamage(1f, 1f, 2f);
      component.SetOwner(owner);
    }
    Vector3 localScale = gameObject.transform.localScale;
    gameObject.transform.localScale = Vector3.zero;
    gameObject.transform.DOScale(localScale, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
  }

  public static void CreateLava(Vector3 position, Transform parent, Health owner)
  {
    AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Misc/Trap Lava Big.prefab");
    loadOp.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      TrapLava.loadedAddressableAssets.Add(obj);
      GameObject gameObject = ObjectPool.Spawn(loadOp.Result, parent, position, Quaternion.identity);
      AreaBurnTick component = gameObject.GetComponent<AreaBurnTick>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.Initialize();
        component.EnableDamage(1f, 1f, 2f);
        component.SetOwner(owner);
      }
      Vector3 localScale = gameObject.transform.localScale;
      gameObject.transform.localScale = Vector3.zero;
      gameObject.transform.DOScale(localScale, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    });
  }

  public virtual void OnEnable()
  {
    this.hasDespawned = false;
    if ((bool) (UnityEngine.Object) GameManager.GetInstance() && (double) this.spawnTimestamp < 0.0)
      this.spawnTimestamp = GameManager.GetInstance().CurrentTime;
    if ((bool) (UnityEngine.Object) this.particleSystem)
      this.particleSystem.Play();
    if (!string.IsNullOrEmpty(this.LavaLoopSFX) && !AudioManager.Instance.IsEventInstancePlaying(this.lavaLoopInstanceSFX))
      this.lavaLoopInstanceSFX = AudioManager.Instance.CreateLoop(this.LavaLoopSFX, this.gameObject, true);
    BiomeGenerator.OnBiomeLeftRoom += new BiomeGenerator.BiomeAction(this.ClearOnRoomChange);
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.lavaLoopInstanceSFX);
    BiomeGenerator.OnBiomeLeftRoom -= new BiomeGenerator.BiomeAction(this.ClearOnRoomChange);
  }

  public void Update()
  {
    if ((double) this.lifetime != -1.0 && (double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) >= (double) this.lifetime && !this.isDespawning)
      this.DespawnLava();
    if (!this.isDespawning || (double) GameManager.GetInstance().TimeSince(this.despawnTimestamp) < (double) this.despawnDuration || this.hasDespawned)
      return;
    this.DisableLavaImmediate();
  }

  public virtual void ClearOnRoomChange() => this.DisableLavaImmediate();

  public void DisableLavaImmediate()
  {
    this.ResetObject();
    this.gameObject.Recycle();
    this.hasDespawned = true;
  }

  public void DespawnLava()
  {
    if ((bool) (UnityEngine.Object) this.particleSystem)
      this.particleSystem.Pause();
    this.isDespawning = true;
    AudioManager.Instance.StopLoop(this.lavaLoopInstanceSFX);
    this.despawnTimestamp = GameManager.GetInstance().CurrentTime;
    this.graphic.transform.localScale = Vector3.one;
    this.graphic.transform.DOScale(0.0f, this.despawnDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.ResetObject();
      this.gameObject.Recycle();
      this.hasDespawned = true;
    }));
  }

  public void ResetObject()
  {
    this.isDespawning = false;
    AudioManager.Instance.StopLoop(this.lavaLoopInstanceSFX);
    this.graphic.transform.localScale = Vector3.one;
    if ((bool) (UnityEngine.Object) this.particleSystem)
      this.particleSystem.Clear();
    DOTween.Kill((object) this.graphic);
    this.spawnTimestamp = -1f;
    this.despawnTimestamp = -1f;
  }

  [CompilerGenerated]
  public void \u003CDespawnLava\u003Eb__24_0()
  {
    this.ResetObject();
    this.gameObject.Recycle();
    this.hasDespawned = true;
  }
}
