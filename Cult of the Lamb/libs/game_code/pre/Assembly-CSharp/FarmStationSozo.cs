// Decompiled with JetBrains decompiler
// Type: FarmStationSozo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
    Debug.Log((object) $"Growth state: {(object) index}  {this.MushroomGrowthStates[index].ToString()}");
    this.Spine.AnimationState.SetAnimation(0, this.MushroomGrowthStates[index], true);
    this.SpineArms.AnimationState.SetAnimation(0, this.MushroomGrowthStates[index], true);
  }
}
