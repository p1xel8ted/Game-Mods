// Decompiled with JetBrains decompiler
// Type: Demon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class Demon : MonoBehaviour
{
  public int followerID;
  public FollowerInfo followerInfo;
  public int forcedLevel = -1;
  public GameObject _Master;
  public Health MasterHealth;
  public StateMachine MasterState;

  public FollowerInfo FollowerInfo
  {
    get
    {
      if (this.followerInfo == null)
        this.followerInfo = FollowerInfo.GetInfoByID(this.followerID);
      return this.followerInfo;
    }
  }

  public int Level
  {
    get
    {
      if (this.forcedLevel != -1)
        return this.forcedLevel;
      if (this.followerInfo == null)
        this.followerInfo = FollowerInfo.GetInfoByID(this.followerID);
      return this.followerInfo != null ? this.followerInfo.GetDemonLevel() : 1;
    }
  }

  public GameObject Master
  {
    get
    {
      if ((Object) this._Master == (Object) null)
      {
        this._Master = GameObject.FindGameObjectWithTag("Player");
        if ((Object) this._Master != (Object) null)
        {
          this.MasterState = this._Master.GetComponent<StateMachine>();
          this.MasterHealth = this._Master.GetComponent<Health>();
          ((HealthPlayer) this.MasterHealth).OnPlayerDied += new HealthPlayer.HPUpdated(this.Health_OnDie);
        }
      }
      return this._Master;
    }
    set => this._Master = value;
  }

  public virtual void Start()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public virtual void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    ((HealthPlayer) this.MasterHealth).OnPlayerDied -= new HealthPlayer.HPUpdated(this.Health_OnDie);
  }

  public virtual void Init(int followerID, int forcedLevel = -1)
  {
    this.forcedLevel = forcedLevel;
    this.followerID = followerID;
    this.StartCoroutine((IEnumerator) this.SetSkin());
  }

  public static float GetDemonLeftovers()
  {
    float demonLeftovers = 0.0f;
    for (int index = 0; index < Demon_Arrows.Demons.Count; ++index)
    {
      if ((bool) (Object) Demon_Arrows.Demons[index].GetComponent<Demon_Arrows>())
      {
        Demon component = Demon_Arrows.Demons[index].GetComponent<Demon>();
        demonLeftovers += component.Level > 1 ? 0.1f * (float) component.Level : 0.0f;
      }
    }
    return demonLeftovers;
  }

  public virtual IEnumerator SetSkin()
  {
    yield break;
  }

  public void SetMaster(GameObject newMaster)
  {
    if ((Object) this.MasterHealth != (Object) null)
      ((HealthPlayer) this.MasterHealth).OnPlayerDied -= new HealthPlayer.HPUpdated(this.Health_OnDie);
    this._Master = newMaster;
    if (!((Object) this._Master != (Object) null))
      return;
    this.MasterState = this._Master.GetComponent<StateMachine>();
    this.MasterHealth = this._Master.GetComponent<Health>();
    ((HealthPlayer) this.MasterHealth).OnPlayerDied += new HealthPlayer.HPUpdated(this.Health_OnDie);
  }

  public void Health_OnDie(HealthPlayer player) => Object.Destroy((Object) this.gameObject);

  public virtual void BiomeGenerator_OnBiomeChangeRoom()
  {
    this.transform.position = this.Master.transform.position + Vector3.right;
  }
}
