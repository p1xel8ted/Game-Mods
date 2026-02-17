// Decompiled with JetBrains decompiler
// Type: BaseShopkeeperBarkManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseShopkeeperBarkManager : MonoBehaviour
{
  [SerializeField]
  public SimpleBark _normalBark;
  [SerializeField]
  public SimpleBarkRepeating _normalBarkRepeating;
  [SerializeField]
  public SimpleBarkRepeating _winterBarks;

  public void Awake()
  {
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.OnSeasonChange);
  }

  public void OnDestroy()
  {
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.OnSeasonChange);
  }

  public void Start()
  {
    if (DataManager.Instance == null)
      return;
    this.CheckBarks(DataManager.Instance.CurrentSeason);
  }

  public void CheckBarks(SeasonsManager.Season season)
  {
    if (season == SeasonsManager.Season.Winter)
    {
      this.SetNormalBarks(false);
      this._winterBarks?.gameObject.SetActive(true);
    }
    else
    {
      this._winterBarks?.gameObject.SetActive(false);
      this.SetNormalBarks(true);
    }
  }

  public void SetNormalBarks(bool active)
  {
    this._normalBark?.gameObject.SetActive(active);
    this._normalBarkRepeating?.gameObject.SetActive(active);
  }

  public void OnSeasonChange(SeasonsManager.Season newSeason) => this.CheckBarks(newSeason);
}
