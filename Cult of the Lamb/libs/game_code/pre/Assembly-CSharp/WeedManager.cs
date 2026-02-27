// Decompiled with JetBrains decompiler
// Type: WeedManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WeedManager : MonoBehaviour
{
  public static List<WeedManager> WeedManagers = new List<WeedManager>();
  public List<WeedManager.Weed> WeedTypes = new List<WeedManager.Weed>();
  public GameObject ChosenWeed;
  public Interaction_Weed Interaction_Weed;
  private int weedTypeChosen = -1;
  private int growthStageOffset;

  public int WeedTypeChosen
  {
    get => this.weedTypeChosen;
    set
    {
      this.weedTypeChosen = Mathf.Clamp(value, 0, this.WeedTypes.Count - 1);
      this.UpdateWeedGrowth();
    }
  }

  public int GrowthStageOffset
  {
    get => this.growthStageOffset;
    set
    {
      this.growthStageOffset = Mathf.Clamp(value, 0, this.WeedTypes[this.WeedTypeChosen].Weeds.Count - 1);
      this.UpdateWeedGrowth();
    }
  }

  private void OnEnable()
  {
    this.OnBrainAssigned();
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
    WeedManager.WeedManagers.Add(this);
  }

  private void OnNewDayStarted() => this.UpdateWeedGrowth();

  private void OnDisable()
  {
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
    WeedManager.WeedManagers.Remove(this);
  }

  private void Hide() => WeedManager.HideAll();

  public static void HideAll()
  {
    foreach (WeedManager weedManager in WeedManager.WeedManagers)
    {
      if ((bool) (UnityEngine.Object) weedManager.ChosenWeed)
        weedManager.ChosenWeed.SetActive(false);
    }
  }

  private void Show() => WeedManager.ShowAll();

  public static void ShowAll()
  {
    foreach (WeedManager weedManager in WeedManager.WeedManagers)
    {
      if ((bool) (UnityEngine.Object) weedManager.ChosenWeed)
        weedManager.ChosenWeed.SetActive(true);
    }
  }

  private void OnBrainAssigned() => this.UpdateWeedType();

  private void UpdateWeedType() => this.UpdateWeedGrowth();

  private void UpdateWeedGrowth()
  {
    if (this.WeedTypeChosen == -1)
      return;
    if ((UnityEngine.Object) this.ChosenWeed != (UnityEngine.Object) null)
      ObjectPool.Recycle(this.ChosenWeed);
    this.ChosenWeed = ObjectPool.Spawn(this.WeedTypes[this.WeedTypeChosen].Weeds[Mathf.Clamp(Mathf.FloorToInt((float) TimeManager.CurrentDay / 2f) + this.growthStageOffset, 0, this.WeedTypes[this.WeedTypeChosen].Weeds.Count - 1)], this.transform);
  }

  [Serializable]
  public struct Weed
  {
    public List<GameObject> Weeds;
  }
}
