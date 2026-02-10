// Decompiled with JetBrains decompiler
// Type: WinterOnly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WinterOnly : MonoBehaviour
{
  [SerializeField]
  public List<GameObject> elements = new List<GameObject>();

  public void Awake()
  {
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
  }

  public void OnEnable() => this.UpdateElements();

  public void OnDestroy()
  {
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
  }

  public void UpdateElements()
  {
    bool flag = SeasonsManager.Active && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter;
    foreach (GameObject element in this.elements)
      element.SetActive(flag);
  }

  public void OnSeasonChanged(SeasonsManager.Season newSeason) => this.UpdateElements();
}
