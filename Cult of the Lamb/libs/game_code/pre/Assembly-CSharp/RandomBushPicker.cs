// Decompiled with JetBrains decompiler
// Type: RandomBushPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RandomBushPicker : BaseMonoBehaviour
{
  public List<ObjectTypes> Objects = new List<ObjectTypes>();
  private GameObject createdObject;
  public bool GrassCut;
  public FollowerLocation currentLocation = FollowerLocation.None;
  private bool isDestroyed;
  private int spawnedIndex;
  private bool FoundOne;
  private int r;

  private void Awake() => this.transform.GetChild(0).gameObject.SetActive(false);

  private void OnDestroy() => this.Objects.Clear();

  private void OnEnable()
  {
    if (!this.FoundOne)
    {
      if ((GameManager.SandboxDungeonEnabled ? (int) BiomeGenerator.Instance.DungeonLocation : (int) PlayerFarming.Location) == -1)
        LocationManager.OnPlayerLocationSet += new System.Action(this.UpdateObject);
      else
        this.UpdateObject();
    }
    else
      this.SpawnObject();
  }

  private void OnDisable()
  {
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateObject);
    if (!(bool) (UnityEngine.Object) this.createdObject)
      return;
    this.createdObject.GetComponent<Health>().OnDie -= new Health.DieAction(this.RandomGrassPicker_OnDie);
    GameManager.GetInstance()?.StartCoroutine((IEnumerator) this.FrameDelayRecycle());
  }

  private IEnumerator FrameDelayRecycle()
  {
    yield return (object) new WaitForEndOfFrame();
    if ((bool) (UnityEngine.Object) this.createdObject)
      ObjectPool.Recycle(this.createdObject);
  }

  private void SpawnObject()
  {
    if (this.FoundOne && this.spawnedIndex != -1 && (UnityEngine.Object) this.GetComponentInChildren<LongGrass>() == (UnityEngine.Object) null)
    {
      foreach (ObjectTypes objectTypes in this.Objects)
      {
        foreach (FollowerLocation followerLocation in objectTypes.Location)
        {
          if (followerLocation == this.currentLocation)
          {
            if ((UnityEngine.Object) objectTypes.Objects[this.spawnedIndex] != (UnityEngine.Object) null)
            {
              this.createdObject = ObjectPool.Spawn(objectTypes.Objects[this.spawnedIndex], this.transform, this.transform.position).GetComponent<LongGrass>().gameObject;
              break;
            }
            break;
          }
        }
      }
    }
    if (!(bool) (UnityEngine.Object) this.createdObject)
      return;
    this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
    this.createdObject.transform.position = this.transform.position;
    if (this.isDestroyed)
      this.createdObject.GetComponent<LongGrass>().SetCut();
    else
      this.createdObject.GetComponent<LongGrass>().ResetCut();
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
    this.isDestroyed = true;
  }

  private void UpdateObject()
  {
    if ((UnityEngine.Object) this.GetComponentInChildren<LongGrass>() != (UnityEngine.Object) null)
      return;
    this.FoundOne = false;
    this.currentLocation = GameManager.SandboxDungeonEnabled ? BiomeGenerator.Instance.DungeonLocation : PlayerFarming.Location;
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
            this.spawnedIndex = UnityEngine.Random.Range(0, objectTypes.Objects.Length);
            if ((UnityEngine.Object) objectTypes.Objects[this.spawnedIndex] != (UnityEngine.Object) null)
            {
              this.createdObject = ObjectPool.Spawn(objectTypes.Objects[this.spawnedIndex], this.transform, this.transform.position);
              this.createdObject.transform.position = this.transform.position;
              this.createdObject.GetComponent<Health>().OnDie += new Health.DieAction(this.RandomGrassPicker_OnDie);
              break;
            }
            break;
          }
          this.spawnedIndex = -1;
        }
      }
    }
    if ((UnityEngine.Object) this.createdObject != (UnityEngine.Object) null)
    {
      this.createdObject.transform.position = this.transform.position;
      if (this.isDestroyed)
        this.createdObject.GetComponent<LongGrass>().SetCut();
      else
        this.createdObject.GetComponent<LongGrass>().ResetCut();
    }
    LocationManager.OnPlayerLocationSet -= new System.Action(this.UpdateObject);
  }
}
