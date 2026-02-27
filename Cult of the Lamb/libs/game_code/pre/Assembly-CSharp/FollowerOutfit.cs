// Decompiled with JetBrains decompiler
// Type: FollowerOutfit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class FollowerOutfit
{
  private FollowerInfo _info;

  public bool IsHooded { get; private set; }

  public FollowerOutfitType CurrentOutfit { get; private set; }

  public HatType CurrentHat { get; private set; }

  public FollowerOutfit(FollowerInfo info) => this._info = info;

  public void SetOutfit(SkeletonAnimation spine, bool hooded)
  {
    if (this._info == null)
      return;
    this.SetOutfit(spine, this._info.Outfit, this._info.Necklace, hooded, hat: this.CurrentHat);
  }

  public void SetOutfit(
    SkeletonAnimation spine,
    FollowerOutfitType outfit,
    InventoryItem.ITEM_TYPE necklace,
    bool hooded,
    Thought overrideCursedState = Thought.None,
    HatType hat = HatType.None)
  {
    Skin newSkin = new Skin("New Skin");
    Skin skin = spine.Skeleton.Data.FindSkin(this._info.SkinName);
    if (skin != null)
    {
      newSkin.AddSkin(skin);
    }
    else
    {
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Cat"));
      this._info.SkinName = "Cat";
    }
    string outfitSkinName = this.GetOutfitSkinName(outfit);
    if (!string.IsNullOrEmpty(outfitSkinName))
      newSkin.AddSkin(spine.skeleton.Data.FindSkin(outfitSkinName));
    if (necklace != InventoryItem.ITEM_TYPE.NONE)
      newSkin.AddSkin(spine.skeleton.Data.FindSkin("Necklaces/" + necklace.ToString()));
    switch (outfit)
    {
      case FollowerOutfitType.Old:
        newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Other/Old"));
        break;
      case FollowerOutfitType.HorseTown:
        newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Clothes/HorseTown"));
        break;
    }
    if (!hooded)
    {
      if (this._info.TaxEnforcer)
        hat = HatType.TaxEnforcer;
      else if (this._info.FaithEnforcer)
        hat = HatType.FaithEnforcer;
    }
    if (hooded)
    {
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Clothes/Hooded_Lvl1"));
      this.IsHooded = true;
    }
    else
    {
      this.IsHooded = false;
      switch (hat)
      {
        case HatType.Chef:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Chef"));
          break;
        case HatType.TaxEnforcer:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Enforcer"));
          break;
        case HatType.FaithEnforcer:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/FaithEnforcer"));
          break;
        case HatType.Farm:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Farm"));
          break;
        case HatType.Lumberjack:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Lumberjack"));
          break;
        case HatType.Miner:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Miner"));
          break;
        case HatType.Refiner:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Refinery"));
          break;
      }
    }
    if (FollowerBrainStats.BrainWashed)
      newSkin.AddSkin(spine.skeleton.Data.FindSkin("Other/Brainwashed"));
    else if (this._info.CursedState == Thought.Dissenter || overrideCursedState == Thought.Dissenter)
      newSkin.AddSkin(spine.skeleton.Data.FindSkin("Other/Dissenter"));
    if (outfit == FollowerOutfitType.Ghost)
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Other/Ghost"));
    spine.Skeleton.SetSkin(newSkin);
    spine.skeleton.SetSlotsToSetupPose();
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(this._info.SkinName);
    if (colourData != null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(this._info.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = spine.skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
    this.CurrentOutfit = outfit;
    this.CurrentHat = hat;
  }

  public static string GetHatByFollowerRole(FollowerRole FollowerRole) => "";

  public void SetOutfit(SkeletonGraphic spine, bool hooded)
  {
    this.SetOutfit(spine, this._info.Outfit, this._info.Necklace, hooded);
  }

  public void SetOutfit(
    SkeletonGraphic spine,
    FollowerOutfitType outfit,
    InventoryItem.ITEM_TYPE necklace,
    bool hooded,
    HatType hat = HatType.None)
  {
    Skin newSkin = new Skin("New Skin");
    Skin skin = spine.Skeleton.Data.FindSkin(this._info.SkinName);
    if (skin != null)
    {
      newSkin.AddSkin(skin);
    }
    else
    {
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Cat"));
      this._info.SkinName = "Cat";
    }
    string outfitSkinName = this.GetOutfitSkinName(outfit);
    if (!string.IsNullOrEmpty(outfitSkinName))
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin(outfitSkinName));
    if (necklace != InventoryItem.ITEM_TYPE.NONE)
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Necklaces/" + necklace.ToString()));
    if (outfit == FollowerOutfitType.Old)
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Other/Old"));
    if (hooded)
    {
      this.GetHoodedSkinName(outfit);
      newSkin.AddSkin(spine.Skeleton.Data.FindSkin(outfitSkinName));
      this.IsHooded = true;
    }
    else
    {
      this.IsHooded = false;
      switch (hat)
      {
        case HatType.Chef:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Chef"));
          break;
        case HatType.TaxEnforcer:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Enforcer "));
          break;
        case HatType.FaithEnforcer:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/FaithEnforcer"));
          break;
        case HatType.Farm:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Farm"));
          break;
        case HatType.Lumberjack:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Lumberjack"));
          break;
        case HatType.Miner:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Miner"));
          break;
        case HatType.Refiner:
          newSkin.AddSkin(spine.Skeleton.Data.FindSkin("Hats/Refinery"));
          break;
      }
    }
    spine.Skeleton.SetSkin(newSkin);
    spine.Skeleton.SetSlotsToSetupPose();
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(this._info.SkinName);
    if (colourData == null)
      return;
    foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(this._info.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
    {
      Slot slot = spine.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  public string GetHoodedSkinName(FollowerOutfitType outfit)
  {
    string hoodedSkinName;
    switch (this._info.FollowerRole)
    {
      case FollowerRole.Worshipper:
        hoodedSkinName = "Clothes/Hooded_Lvl3";
        break;
      case FollowerRole.Monk:
        hoodedSkinName = "Clothes/Hooded_Lvl5";
        break;
      default:
        hoodedSkinName = "Clothes/Hooded_Lvl1";
        break;
    }
    return hoodedSkinName;
  }

  public string GetOutfitSkinName(FollowerOutfitType outfit)
  {
    string outfitSkinName = "";
    if (CheatConsole.Robes)
      return "Clothes/Robes_Lvl5";
    switch (outfit)
    {
      case FollowerOutfitType.Rags:
        outfitSkinName = "Clothes/Rags";
        break;
      case FollowerOutfitType.Sherpa:
        outfitSkinName = "Clothes/Sherpa";
        break;
      case FollowerOutfitType.Warrior:
        outfitSkinName = "Clothes/Warrior";
        break;
      case FollowerOutfitType.Follower:
        switch (this._info.FollowerRole)
        {
          case FollowerRole.Worshipper:
            outfitSkinName = "Clothes/Robes_Lvl3";
            break;
          case FollowerRole.Monk:
            outfitSkinName = "Clothes/Robes_Lvl5";
            break;
          default:
            outfitSkinName = "Clothes/Robes_Lvl1";
            break;
        }
        break;
      case FollowerOutfitType.Worshipper:
        outfitSkinName = "Clothes/Robes_Lvl3";
        break;
      case FollowerOutfitType.Worker:
        outfitSkinName = "Clothes/Robes_Lvl1";
        break;
      case FollowerOutfitType.Old:
        outfitSkinName = "Clothes/HorseTown";
        break;
      case FollowerOutfitType.Holiday:
        outfitSkinName = "Clothes/Holiday";
        break;
    }
    return outfitSkinName;
  }
}
