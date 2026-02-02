// Decompiled with JetBrains decompiler
// Type: Interaction_WinterTorch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_WinterTorch : MonoBehaviour
{
  public Structure structure;

  public void Start()
  {
    this.structure = this.GetComponent<Structure>();
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
  }

  public void OnDestroy()
  {
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
    if (!((UnityEngine.Object) this.structure != (UnityEngine.Object) null))
      return;
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    this.OnSeasonChanged(SeasonsManager.CurrentSeason);
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnSeasonChanged(SeasonsManager.Season season)
  {
    if (season != SeasonsManager.Season.Winter)
      return;
    this.structure.Brain.UpdateFuel(int.MaxValue);
  }
}
