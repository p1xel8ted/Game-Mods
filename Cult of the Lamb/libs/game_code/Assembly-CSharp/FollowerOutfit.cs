// Decompiled with JetBrains decompiler
// Type: FollowerOutfit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Runtime.CompilerServices;

#nullable disable
public class FollowerOutfit
{
  public FollowerInfo _info;
  [CompilerGenerated]
  public bool \u003CIsHooded\u003Ek__BackingField;

  public bool IsHooded
  {
    get => this.\u003CIsHooded\u003Ek__BackingField;
    set => this.\u003CIsHooded\u003Ek__BackingField = value;
  }

  public FollowerOutfitType CurrentOutfit
  {
    get => this._info != null ? this._info.Outfit : FollowerOutfitType.None;
  }

  public FollowerOutfit(FollowerInfo info) => this._info = info;

  public void SetInfo(FollowerInfo info) => this._info = info;

  public void SetOutfit(SkeletonAnimation spine, bool hooded)
  {
    if (this._info == null)
      return;
    this.SetOutfit(spine, this._info.Outfit, this._info.Necklace, hooded);
  }

  public void SetOutfit(
    SkeletonAnimation spine,
    FollowerOutfitType outfit,
    InventoryItem.ITEM_TYPE necklace,
    bool hooded,
    Thought overrideCursedState = Thought.None,
    FollowerHatType hat = FollowerHatType.None,
    bool setData = true)
  {
    this._info.Necklace = necklace;
    if (hat != FollowerHatType.None)
      this._info.Hat = hat;
    this.IsHooded = hooded;
    Thought cursedState = this._info.CursedState;
    if (overrideCursedState != Thought.None)
      this._info.CursedState = overrideCursedState;
    FollowerBrain.SetFollowerCostume(spine.skeleton, this._info, hooded, true, setData);
    this._info.CursedState = cursedState;
  }

  public static string GetHatByFollowerRole(FollowerRole FollowerRole) => "";

  public void SetOutfit(SkeletonGraphic spine)
  {
    FollowerBrain.SetFollowerCostume(spine.Skeleton, this._info, setData: false);
  }
}
