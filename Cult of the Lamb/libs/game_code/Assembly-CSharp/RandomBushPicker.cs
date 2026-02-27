// Decompiled with JetBrains decompiler
// Type: RandomBushPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class RandomBushPicker : BaseMonoBehaviour
{
  public List<ObjectTypes> Objects = new List<ObjectTypes>();
  public GameObject createdObject;
  public bool GrassCut;
  public FollowerLocation currentLocation = FollowerLocation.None;
  public bool isDestroyed;
  public int spawnedIndex;
  public bool FoundOne;
  public int r;

  public void Awake()
  {
    if (this.transform.childCount <= 0)
      return;
    this.transform.GetChild(0).gameObject.SetActive(false);
  }

  public void OnDestroy() => this.Objects.Clear();

  public void OnEnable()
  {
    if (!this.FoundOne)
    {
      if (PlayerFarming.Location == FollowerLocation.None)
        LocationManager.OnPlayerLocationSet += new System.Action(this.UpdateObject);
      else
        this.UpdateObject();
    }
    else
      this.StartCoroutine(this.SpawnObject());
  }

  public void OnDisable()
  {
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateObject);
    if (!(bool) (UnityEngine.Object) this.createdObject)
      return;
    this.createdObject.GetComponent<Health>().OnDie -= new Health.DieAction(this.RandomGrassPicker_OnDie);
    GameManager.GetInstance()?.StartCoroutine(this.FrameDelayRecycle());
  }

  public IEnumerator FrameDelayRecycle()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((bool) (UnityEngine.Object) this.createdObject)
      ObjectPool.Recycle(this.createdObject);
  }

  public IEnumerator SpawnObject()
  {
    RandomBushPicker randomBushPicker = this;
    if (randomBushPicker.FoundOne && randomBushPicker.spawnedIndex != -1 && (UnityEngine.Object) randomBushPicker.GetComponentInChildren<LongGrass>() == (UnityEngine.Object) null)
    {
      foreach (ObjectTypes objectTypes in randomBushPicker.Objects)
      {
        foreach (FollowerLocation followerLocation in objectTypes.Location)
        {
          if (followerLocation == randomBushPicker.currentLocation)
          {
            if (objectTypes.ObjectsAddr[randomBushPicker.spawnedIndex].RuntimeKeyIsValid())
            {
              AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAssetAsync<GameObject>((object) objectTypes.ObjectsAddr[randomBushPicker.spawnedIndex]);
              yield return (object) null;
              opHandle.WaitForCompletion();
              LongGrass component = ObjectPool.Spawn(opHandle.Result, randomBushPicker.transform, randomBushPicker.transform.position).GetComponent<LongGrass>();
              randomBushPicker.createdObject = component.gameObject;
              opHandle = new AsyncOperationHandle<GameObject>();
              break;
            }
            break;
          }
        }
      }
    }
    if ((bool) (UnityEngine.Object) randomBushPicker.createdObject)
    {
      randomBushPicker.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(randomBushPicker.RandomGrassPicker_OnDie);
      randomBushPicker.createdObject.transform.position = randomBushPicker.transform.position;
      if (randomBushPicker.isDestroyed)
        randomBushPicker.createdObject.GetComponent<LongGrass>().SetCut();
      else
        randomBushPicker.createdObject.GetComponent<LongGrass>().ResetCut();
    }
  }

  public void RandomGrassPicker_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!((UnityEngine.Object) Victim.gameObject == (UnityEngine.Object) this.createdObject))
      return;
    this.isDestroyed = true;
  }

  public void UpdateObject()
  {
    if ((UnityEngine.Object) this.GetComponentInChildren<LongGrass>() != (UnityEngine.Object) null)
      return;
    this.FoundOne = false;
    this.currentLocation = PlayerFarming.Location;
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateObject);
    foreach (ObjectTypes objectTypes in this.Objects)
    {
      foreach (FollowerLocation followerLocation in objectTypes.Location)
      {
        if (followerLocation == this.currentLocation)
        {
          this.FoundOne = true;
          this.r = UnityEngine.Random.Range(0, 100);
          if ((double) this.r <= (double) objectTypes.PercentageChanceToSpawn.y)
          {
            this.spawnedIndex = UnityEngine.Random.Range(0, objectTypes.ObjectsAddr.Length);
            if (objectTypes.ObjectsAddr[this.spawnedIndex].RuntimeKeyIsValid())
            {
              Addressables.LoadAssetAsync<GameObject>((object) objectTypes.ObjectsAddr[this.spawnedIndex]).Completed += (Action<AsyncOperationHandle<GameObject>>) (op =>
              {
                this.createdObject = ObjectPool.Spawn(op.Result, this.transform, this.transform.position);
                this.createdObject.transform.position = this.transform.position;
                this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
                if (!((UnityEngine.Object) this.createdObject != (UnityEngine.Object) null))
                  return;
                this.createdObject.transform.position = this.transform.position;
                if (this.isDestroyed)
                  this.createdObject.GetComponent<LongGrass>().SetCut();
                else
                  this.createdObject.GetComponent<LongGrass>().ResetCut();
              });
              break;
            }
            break;
          }
          this.spawnedIndex = -1;
        }
      }
    }
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateObject);
  }

  [CompilerGenerated]
  public void \u003CUpdateObject\u003Eb__15_0(AsyncOperationHandle<GameObject> op)
  {
    this.createdObject = ObjectPool.Spawn(op.Result, this.transform, this.transform.position);
    this.createdObject.transform.position = this.transform.position;
    this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
    if (!((UnityEngine.Object) this.createdObject != (UnityEngine.Object) null))
      return;
    this.createdObject.transform.position = this.transform.position;
    if (this.isDestroyed)
      this.createdObject.GetComponent<LongGrass>().SetCut();
    else
      this.createdObject.GetComponent<LongGrass>().ResetCut();
  }
}
