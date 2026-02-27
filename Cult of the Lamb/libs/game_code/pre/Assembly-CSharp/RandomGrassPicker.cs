// Decompiled with JetBrains decompiler
// Type: RandomGrassPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RandomGrassPicker : BaseMonoBehaviour
{
  public List<FlowerTypes> Flowers = new List<FlowerTypes>();
  public string DefaultGrass;
  private bool GrassPicked;
  private GameObject createdObject;
  public bool GrassCut;
  public FollowerLocation currentLocation = FollowerLocation.None;
  private bool isCut;
  private bool FoundOne;
  private int r;

  private void Awake() => this.transform.GetChild(0).gameObject.SetActive(false);

  private void OnEnable()
  {
    if (!this.FoundOne)
      LocationManager.OnPlayerLocationSet += new System.Action(this.UpdateGrass);
    else
      this.SpawnObject();
  }

  private void OnDisable()
  {
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateGrass);
    if (!(bool) (UnityEngine.Object) this.createdObject)
      return;
    this.createdObject.GetComponent<Health>().OnDie -= new Health.DieAction(this.RandomGrassPicker_OnDie);
    GameManager.GetInstance()?.StartCoroutine((IEnumerator) this.FrameDelayRecycle());
  }

  private void OnDestroy() => LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateGrass);

  private IEnumerator FrameDelayRecycle()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((bool) (UnityEngine.Object) this.createdObject)
      ObjectPool.Recycle(this.createdObject);
  }

  private void SpawnObject()
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

  private void RandomGrassPicker_OnDie(
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

  private void SpawnDefault()
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

  private void UpdateGrass()
  {
    this.FoundOne = false;
    this.currentLocation = GameManager.SandboxDungeonEnabled ? BiomeGenerator.Instance.DungeonLocation : PlayerFarming.Location;
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateGrass);
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
}
