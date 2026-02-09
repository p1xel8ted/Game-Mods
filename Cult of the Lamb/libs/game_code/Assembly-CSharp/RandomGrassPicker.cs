// Decompiled with JetBrains decompiler
// Type: RandomGrassPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RandomGrassPicker : BaseMonoBehaviour
{
  public List<FlowerTypes> Flowers = new List<FlowerTypes>();
  public string DefaultGrass;
  public bool GrassPicked;
  public GameObject createdObject;
  public bool GrassCut;
  public FollowerLocation currentLocation = FollowerLocation.None;
  public bool isCut;
  public bool checkedQuests;
  public bool FoundOne;
  public int r;

  public void Awake()
  {
    if (this.transform.childCount <= 0)
      return;
    this.transform.GetChild(0).gameObject.SetActive(false);
  }

  public void OnEnable()
  {
    if (!this.FoundOne)
      LocationManager.OnPlayerLocationSet += new System.Action(this.UpdateGrass);
    else
      this.SpawnObject();
  }

  public FlowerTypes GetGrassFromLocation()
  {
    foreach (FlowerTypes flower in this.Flowers)
    {
      foreach (FollowerLocation followerLocation in flower.Location)
      {
        if (followerLocation == this.currentLocation)
          return flower;
      }
    }
    return (FlowerTypes) null;
  }

  public void CheckQuests()
  {
    FlowerTypes grassFromLocation = this.GetGrassFromLocation();
    if (this.currentLocation == FollowerLocation.Dungeon1_1 || this.currentLocation == FollowerLocation.Dungeon1_2 || this.currentLocation == FollowerLocation.Dungeon1_3 || this.currentLocation == FollowerLocation.Dungeon1_4)
    {
      for (int index = DataManager.Instance.Objectives.Count - 1; index >= 0; --index)
      {
        ObjectivesData objective = DataManager.Instance.Objectives[index];
        if (this.currentLocation == FollowerLocation.Dungeon1_1)
        {
          if (objective.Type == Objectives.TYPES.COLLECT_ITEM && ((Objectives_CollectItem) objective).ItemType == InventoryItem.ITEM_TYPE.FLOWER_RED)
            grassFromLocation.PercentageChanceToSpawn.y *= 2f;
          else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FLOWER_RED) <= 10)
            grassFromLocation.PercentageChanceToSpawn.y *= 1.5f;
          this.checkedQuests = true;
          break;
        }
        if (this.currentLocation == FollowerLocation.Dungeon1_2)
        {
          if (objective.Type == Objectives.TYPES.COLLECT_ITEM && ((Objectives_CollectItem) objective).ItemType == InventoryItem.ITEM_TYPE.MUSHROOM_SMALL)
            grassFromLocation.PercentageChanceToSpawn.y *= 2f;
          else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL) <= 10)
            grassFromLocation.PercentageChanceToSpawn.y *= 1.25f;
          this.checkedQuests = true;
          break;
        }
        if (this.currentLocation == FollowerLocation.Dungeon1_3)
        {
          if (objective.Type == Objectives.TYPES.COLLECT_ITEM && ((Objectives_CollectItem) objective).ItemType == InventoryItem.ITEM_TYPE.CRYSTAL)
            grassFromLocation.PercentageChanceToSpawn.y *= 2f;
          else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL) <= 10)
            grassFromLocation.PercentageChanceToSpawn.y *= 1.5f;
          this.checkedQuests = true;
          break;
        }
        if (this.currentLocation == FollowerLocation.Dungeon1_4)
        {
          if (objective.Type == Objectives.TYPES.COLLECT_ITEM && ((Objectives_CollectItem) objective).ItemType == InventoryItem.ITEM_TYPE.SPIDER_WEB)
            grassFromLocation.PercentageChanceToSpawn.y *= 2f;
          else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPIDER_WEB) <= 10)
            grassFromLocation.PercentageChanceToSpawn.y *= 1.5f;
          this.checkedQuests = true;
          break;
        }
      }
    }
    this.checkedQuests = true;
  }

  public void OnDisable()
  {
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateGrass);
    if (!(bool) (UnityEngine.Object) this.createdObject)
      return;
    this.createdObject.GetComponent<Health>().OnDie -= new Health.DieAction(this.RandomGrassPicker_OnDie);
    GameManager.GetInstance()?.StartCoroutine((IEnumerator) this.FrameDelayRecycle());
  }

  public void OnDestroy() => LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateGrass);

  public IEnumerator FrameDelayRecycle()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((bool) (UnityEngine.Object) this.createdObject)
      ObjectPool.Recycle(this.createdObject);
  }

  public void SpawnObject()
  {
    try
    {
      if (!this.FoundOne)
        return;
      foreach (FlowerTypes flower in this.Flowers)
      {
        foreach (FollowerLocation followerLocation in flower.Location)
        {
          if (followerLocation == this.currentLocation)
          {
            if (this.GrassPicked)
            {
              ObjectPool.Spawn(flower.Grass, this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
              {
                this.createdObject = obj;
                this.createdObject.transform.position = this.transform.position;
                this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
                if (this.isCut)
                  this.createdObject.GetComponent<LongGrass>().SetCut();
                else
                  this.createdObject.GetComponent<LongGrass>().ResetCut();
              }));
              break;
            }
            ObjectPool.Spawn(flower.Flower, this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
            {
              this.createdObject = obj;
              this.createdObject.transform.position = this.transform.position;
              this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
              if (this.isCut)
                this.createdObject.GetComponent<LongGrass>().SetCut();
              else
                this.createdObject.GetComponent<LongGrass>().ResetCut();
            }));
            break;
          }
        }
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
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
    this.isCut = true;
  }

  public void SpawnDefault()
  {
    ObjectPool.Spawn(this.DefaultGrass, this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
    {
      this.createdObject = obj;
      this.createdObject.transform.position = this.transform.position;
      this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
      if (this.isCut)
        this.createdObject.GetComponent<LongGrass>().SetCut();
      else
        this.createdObject.GetComponent<LongGrass>().ResetCut();
    }));
  }

  public void UpdateGrass()
  {
    this.FoundOne = false;
    this.currentLocation = PlayerFarming.Location;
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateGrass);
    if (!this.checkedQuests)
      this.CheckQuests();
    if (DataManager.Instance.dungeonRun <= 3 && this.currentLocation == FollowerLocation.Dungeon1_1)
    {
      this.SpawnDefault();
    }
    else
    {
      foreach (FlowerTypes flower in this.Flowers)
      {
        foreach (FollowerLocation followerLocation in flower.Location)
        {
          if (followerLocation == this.currentLocation)
          {
            if (this.currentLocation == FollowerLocation.Dungeon1_2 && DataManager.Instance.SozoStoryProgress == -1)
              flower.PercentageChanceToSpawn.y = 0.0f;
            this.FoundOne = true;
            this.r = UnityEngine.Random.Range(0, 100);
            if ((double) this.r <= (double) flower.PercentageChanceToSpawn.y)
            {
              this.GrassPicked = false;
              ObjectPool.Spawn(flower.Flower, this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
              {
                if (!((UnityEngine.Object) obj != (UnityEngine.Object) null))
                  return;
                this.createdObject = obj;
                this.createdObject.transform.position = this.transform.position;
                this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
                if (this.isCut)
                  this.createdObject.GetComponent<LongGrass>().SetCut();
                else
                  this.createdObject.GetComponent<LongGrass>().ResetCut();
              }));
              break;
            }
            this.GrassPicked = true;
            ObjectPool.Spawn(flower.Grass, this.transform.position, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
            {
              if (!((UnityEngine.Object) obj != (UnityEngine.Object) null))
                return;
              this.createdObject = obj;
              this.createdObject.transform.position = this.transform.position;
              this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
              if (this.isCut)
                this.createdObject.GetComponent<LongGrass>().SetCut();
              else
                this.createdObject.GetComponent<LongGrass>().ResetCut();
            }));
            break;
          }
        }
      }
      if (!this.FoundOne)
        this.SpawnDefault();
      LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateGrass);
    }
  }

  [CompilerGenerated]
  public void \u003CSpawnObject\u003Eb__16_0(GameObject obj)
  {
    this.createdObject = obj;
    this.createdObject.transform.position = this.transform.position;
    this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
    if (this.isCut)
      this.createdObject.GetComponent<LongGrass>().SetCut();
    else
      this.createdObject.GetComponent<LongGrass>().ResetCut();
  }

  [CompilerGenerated]
  public void \u003CSpawnObject\u003Eb__16_1(GameObject obj)
  {
    this.createdObject = obj;
    this.createdObject.transform.position = this.transform.position;
    this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
    if (this.isCut)
      this.createdObject.GetComponent<LongGrass>().SetCut();
    else
      this.createdObject.GetComponent<LongGrass>().ResetCut();
  }

  [CompilerGenerated]
  public void \u003CSpawnDefault\u003Eb__18_0(GameObject obj)
  {
    this.createdObject = obj;
    this.createdObject.transform.position = this.transform.position;
    this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
    if (this.isCut)
      this.createdObject.GetComponent<LongGrass>().SetCut();
    else
      this.createdObject.GetComponent<LongGrass>().ResetCut();
  }

  [CompilerGenerated]
  public void \u003CUpdateGrass\u003Eb__20_0(GameObject obj)
  {
    if (!((UnityEngine.Object) obj != (UnityEngine.Object) null))
      return;
    this.createdObject = obj;
    this.createdObject.transform.position = this.transform.position;
    this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
    if (this.isCut)
      this.createdObject.GetComponent<LongGrass>().SetCut();
    else
      this.createdObject.GetComponent<LongGrass>().ResetCut();
  }

  [CompilerGenerated]
  public void \u003CUpdateGrass\u003Eb__20_1(GameObject obj)
  {
    if (!((UnityEngine.Object) obj != (UnityEngine.Object) null))
      return;
    this.createdObject = obj;
    this.createdObject.transform.position = this.transform.position;
    this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
    if (this.isCut)
      this.createdObject.GetComponent<LongGrass>().SetCut();
    else
      this.createdObject.GetComponent<LongGrass>().ResetCut();
  }
}
