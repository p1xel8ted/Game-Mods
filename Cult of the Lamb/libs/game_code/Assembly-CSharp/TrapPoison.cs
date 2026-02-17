// Decompiled with JetBrains decompiler
// Type: TrapPoison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class TrapPoison : BaseMonoBehaviour
{
  public Collider2D collider;
  public Transform graphic;
  [SerializeField]
  public ParticleSystem particleSystem;
  public float lifetime;
  public float spawnDuration = 0.25f;
  public float despawnDuration = 0.25f;
  [Space]
  [SerializeField]
  public Sprite[] sprites;
  public Health.Team team;
  public float spawnTimestamp;
  public bool isSpawning;
  public bool isDespawning;
  public static GameObject trapPoisonGO;
  public static GameObject trapPoisonSmallGO;
  public List<Health> victims = new List<Health>();
  public static List<TrapPoison> ActivePoison = new List<TrapPoison>();
  public bool useMeshGoop;
  [SerializeField]
  public Mesh[] goopMeshes;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public bool HasDespawned;
  public string PuddleLoopSFX = "event:/dlc/dungeon06/trap/lava/puddle_loop";
  public EventInstance puddleLoopInstanceSFX;

  public static void Preload(int count, bool smallversion = false)
  {
    if (!smallversion)
    {
      if ((UnityEngine.Object) TrapPoison.trapPoisonGO == (UnityEngine.Object) null)
      {
        AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Misc/Trap Poison.prefab");
        loadOp.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          TrapPoison.loadedAddressableAssets.Add(obj);
          TrapPoison.trapPoisonGO = loadOp.Result;
          ObjectPool.CreatePool(TrapPoison.trapPoisonGO, count, true);
        });
      }
      else
        ObjectPool.CreatePool(TrapPoison.trapPoisonGO, count, true);
    }
    else if ((UnityEngine.Object) TrapPoison.trapPoisonSmallGO == (UnityEngine.Object) null)
    {
      AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Misc/Trap Poison Small.prefab");
      loadOp.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        TrapPoison.loadedAddressableAssets.Add(obj);
        TrapPoison.trapPoisonSmallGO = loadOp.Result;
        ObjectPool.CreatePool(TrapPoison.trapPoisonSmallGO, count, true);
      });
    }
    else
      ObjectPool.CreatePool(TrapPoison.trapPoisonSmallGO, count, true);
  }

  public static void CreatePoison(
    Vector3 position,
    int amount,
    float radius,
    Transform parent,
    bool smallversion = false)
  {
    if (!smallversion)
    {
      if ((UnityEngine.Object) TrapPoison.trapPoisonGO == (UnityEngine.Object) null)
      {
        AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Misc/Trap Poison.prefab");
        loadOp.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          TrapPoison.loadedAddressableAssets.Add(obj);
          TrapPoison.trapPoisonGO = loadOp.Result;
          for (int index = 0; index < amount; ++index)
          {
            Vector3 position1 = (position + (Vector3) UnityEngine.Random.insideUnitCircle * radius) with
            {
              z = 0.0f
            };
            ObjectPool.Spawn(TrapPoison.trapPoisonGO, parent, position1, Quaternion.identity).transform.position = position1;
          }
        });
      }
      else
      {
        for (int index = 0; index < amount; ++index)
        {
          Vector3 position2 = (position + (Vector3) UnityEngine.Random.insideUnitCircle * radius) with
          {
            z = 0.0f
          };
          ObjectPool.Spawn(TrapPoison.trapPoisonGO, parent, position2, Quaternion.identity).transform.position = position2;
        }
      }
    }
    else if ((UnityEngine.Object) TrapPoison.trapPoisonSmallGO == (UnityEngine.Object) null)
    {
      AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Misc/Trap Poison Small.prefab");
      loadOp.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        TrapPoison.loadedAddressableAssets.Add(obj);
        TrapPoison.trapPoisonSmallGO = loadOp.Result;
        for (int index = 0; index < amount; ++index)
        {
          Vector3 position3 = (position + (Vector3) UnityEngine.Random.insideUnitCircle * radius) with
          {
            z = 0.0f
          };
          GameObject gameObject = ObjectPool.Spawn(TrapPoison.trapPoisonSmallGO, parent, position3, Quaternion.identity);
          gameObject.transform.position = position3;
          gameObject.SetActive(false);
        }
      });
    }
    else
    {
      for (int index = 0; index < amount; ++index)
      {
        Vector3 position4 = (position + (Vector3) UnityEngine.Random.insideUnitCircle * radius) with
        {
          z = 0.0f
        };
        ObjectPool.Spawn(TrapPoison.trapPoisonSmallGO, parent, position4, Quaternion.identity).transform.position = position4;
      }
    }
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.puddleLoopInstanceSFX, STOP_MODE.IMMEDIATE);
  }

  public void OnDestroy()
  {
    if (TrapPoison.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in TrapPoison.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      TrapPoison.loadedAddressableAssets.Clear();
    }
    BiomeGenerator.OnBiomeLeftRoom -= new BiomeGenerator.BiomeAction(this.ClearOnRoomChange);
  }

  public void Awake()
  {
    BiomeGenerator.OnBiomeLeftRoom += new BiomeGenerator.BiomeAction(this.ClearOnRoomChange);
  }

  public void OnEnable()
  {
    this.HasDespawned = false;
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime;
    if ((bool) (UnityEngine.Object) this.particleSystem)
      this.particleSystem.Play();
    if (string.IsNullOrEmpty(this.PuddleLoopSFX) || AudioManager.Instance.IsEventInstancePlaying(this.puddleLoopInstanceSFX))
      return;
    this.puddleLoopInstanceSFX = AudioManager.Instance.CreateLoop(this.PuddleLoopSFX, this.gameObject, true);
  }

  public void Update()
  {
    if (this.isSpawning)
    {
      if ((double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) < (double) this.spawnDuration)
        return;
      this.isSpawning = false;
      this.collider.enabled = true;
    }
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (LetterBox.IsPlaying || PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.CustomAnimation))
      this.DespawnPoison();
    if (!this.isDespawning && (double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) >= (double) this.spawnDuration + (double) this.lifetime)
      this.DespawnPoison();
    if (!this.isDespawning || (double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) < (double) this.spawnDuration + (double) this.lifetime + (double) this.despawnDuration || this.HasDespawned)
      return;
    this.DisablePoisonImmediate();
  }

  public void ClearOnRoomChange() => this.DisablePoisonImmediate();

  public void DisablePoisonImmediate()
  {
    this.ResetObject();
    this.gameObject.Recycle();
    this.HasDespawned = true;
    BiomeGenerator.OnBiomeLeftRoom -= new BiomeGenerator.BiomeAction(this.ClearOnRoomChange);
  }

  public void DespawnPoison()
  {
    if ((bool) (UnityEngine.Object) this.particleSystem)
      this.particleSystem.Pause();
    this.isDespawning = true;
    this.collider.enabled = false;
    this.graphic.transform.localScale = Vector3.one;
    this.graphic.transform.DOScale(0.0f, this.despawnDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(this.RecyclePoison));
    AudioManager.Instance.StopLoop(this.puddleLoopInstanceSFX);
  }

  public void RecyclePoison()
  {
    TrapPoison.ActivePoison.Remove(this);
    AudioManager.Instance.StopLoop(this.puddleLoopInstanceSFX);
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.layer != 14 && collider.gameObject.layer != 9)
      return;
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.team || component.team == Health.Team.PlayerTeam && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (LetterBox.IsPlaying || PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.CustomAnimation))
      return;
    component.AddPoison(this.gameObject);
  }

  public void OnTriggerExit2D(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.team)
      return;
    component.RemovePoison();
  }

  public static void RemoveAllPoison()
  {
    for (int index = TrapPoison.ActivePoison.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) TrapPoison.ActivePoison[index] != (UnityEngine.Object) null)
        TrapPoison.ActivePoison[index].DespawnPoison();
    }
    PlayerFarming.Instance.health.ClearPoison();
  }

  public void ResetObject()
  {
    this.isSpawning = true;
    this.isDespawning = false;
    this.collider.enabled = true;
    this.graphic.transform.localScale = Vector3.one;
    if ((bool) (UnityEngine.Object) this.particleSystem)
      this.particleSystem.Clear();
    DOTween.Kill((object) this.graphic);
    this.spawnTimestamp = 0.0f;
    AudioManager.Instance.StopLoop(this.puddleLoopInstanceSFX);
  }
}
