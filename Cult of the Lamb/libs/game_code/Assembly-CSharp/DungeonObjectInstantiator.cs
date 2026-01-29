// Decompiled with JetBrains decompiler
// Type: DungeonObjectInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class DungeonObjectInstantiator : BaseMonoBehaviour
{
  public List<DungeonObjectInstantiator.DecorationAndLocation> DecorationsAndLocations = new List<DungeonObjectInstantiator.DecorationAndLocation>();
  [SerializeField]
  public GameObject placeholderObj;
  public bool spawned;
  public bool isSpawning;
  public AsyncOperationHandle<GameObject> handle;

  public void OnEnable() => LocationManager.OnPlayerLocationSet += new System.Action(this.Start);

  public void OnDisable() => LocationManager.OnPlayerLocationSet -= new System.Action(this.Start);

  public void Start()
  {
    if (this.spawned || this.isSpawning)
      return;
    if ((UnityEngine.Object) this.placeholderObj != (UnityEngine.Object) null)
      this.placeholderObj.gameObject.SetActive(false);
    this.isSpawning = true;
    this.StartCoroutine((IEnumerator) this.Spawn());
  }

  public IEnumerator Spawn()
  {
    DungeonObjectInstantiator objectInstantiator = this;
    yield return (object) null;
    foreach (DungeonObjectInstantiator.DecorationAndLocation decorationsAndLocation in objectInstantiator.DecorationsAndLocations)
    {
      if (decorationsAndLocation.Location == PlayerFarming.Location)
      {
        if (decorationsAndLocation.DecorationsAddr != null)
        {
          if (decorationsAndLocation.DecorationsAddr.Length != 0)
          {
            AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) decorationsAndLocation.DecorationsAddr[UnityEngine.Random.Range(0, decorationsAndLocation.DecorationsAddr.Length)]);
            objectInstantiator.handle = asyncOperationHandle;
            asyncOperationHandle.WaitForCompletion();
            ObjectPool.Spawn(asyncOperationHandle.Result, objectInstantiator.transform);
            break;
          }
          break;
        }
        break;
      }
    }
    objectInstantiator.spawned = true;
  }

  public void OnDestroy()
  {
    if (!this.handle.IsValid())
      return;
    Addressables.Release<GameObject>(this.handle);
  }

  [Serializable]
  public class DecorationAndLocation
  {
    public AssetReferenceGameObject[] DecorationsAddr;
    public FollowerLocation Location;
  }
}
