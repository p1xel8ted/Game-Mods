// Decompiled with JetBrains decompiler
// Type: FarmStationSozo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FarmStationSozo : FarmPlot
{
  public SkeletonAnimation Spine;
  public SkeletonAnimation SpineArms;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public List<string> MushroomGrowthStates = new List<string>();

  public override void UpdateCropImage()
  {
    int index = Mathf.FloorToInt(this.StructureInfo.GrowthStage);
    Debug.Log((object) $"Growth state: {index.ToString()}  {this.MushroomGrowthStates[index].ToString()}");
    this.Spine.AnimationState.SetAnimation(0, this.MushroomGrowthStates[index], true);
    this.SpineArms.AnimationState.SetAnimation(0, this.MushroomGrowthStates[index], true);
  }
}
