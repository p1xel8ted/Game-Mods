// Decompiled with JetBrains decompiler
// Type: SpineExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public static class SpineExtensions
{
  public static IEnumerator YieldForAnimation(
    this SkeletonAnimation skeletonAnimation,
    string animation)
  {
    skeletonAnimation.AnimationState.SetAnimation(0, animation, false);
    while (!skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
      yield return (object) null;
  }

  public static IEnumerator YieldForAnimation(
    this SkeletonGraphic skeletonGraphic,
    string animation)
  {
    skeletonGraphic.SetAnimation(animation);
    while (!skeletonGraphic.AnimationState.GetCurrent(0).IsComplete)
      yield return (object) null;
  }

  public static void SetAnimation(this SkeletonGraphic skeletonGraphic, string animation)
  {
    skeletonGraphic.SetAnimation(animation, false);
  }

  public static void SetAnimation(
    this SkeletonGraphic skeletonGraphic,
    string animation,
    bool loop)
  {
    skeletonGraphic.AnimationState.SetAnimation(0, animation, loop);
  }

  public static void ConfigureFollower(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo)
  {
    FollowerOutfitType outfit = followerInfo.Outfit;
    if (outfit != FollowerOutfitType.Old && followerInfo.Special != FollowerSpecialType.SozoOld)
      outfit = FollowerBrain.GetOutfitFromCursedState(followerInfo);
    FollowerHatType hat = followerInfo.Hat;
    switch (hat)
    {
      case FollowerHatType.Hooded:
      case FollowerHatType.OldAge:
        hat = FollowerHatType.None;
        break;
    }
    skeletonGraphic.transform.localScale = Vector3.one * 1.2785f;
    if (followerInfo != null && followerInfo.CursedState == Thought.Child)
      skeletonGraphic.transform.localScale = Vector3.one;
    FollowerBrain.SetFollowerCostume(skeletonGraphic.Skeleton, followerInfo.XPLevel, followerInfo.SkinName, followerInfo.SkinColour, outfit, hat, followerInfo.Clothing, followerInfo.Customisation, followerInfo.Special, followerInfo.Necklace, followerInfo.ClothingVariant, followerInfo);
    if (followerInfo.CursedState == Thought.Child)
    {
      skeletonGraphic.transform.localScale = Vector3.one;
      if (followerInfo.Traits.Contains(FollowerTrait.TraitType.Zombie))
        skeletonGraphic.SetAnimation(followerInfo.Age < 10 ? "Baby/Baby-zombie/baby-idle-sit-zombie" : "Baby/Baby-zombie/baby-idle-stand-zombie", true);
      else
        skeletonGraphic.SetAnimation(followerInfo.Age < 10 ? "Baby/baby-idle" : "Baby/baby-idle-stand", true);
    }
    else if (followerInfo.CursedState == Thought.Ill)
      skeletonGraphic.SetAnimation("Sick/idle-sick", true);
    else if (followerInfo.CursedState == Thought.Injured)
      skeletonGraphic.SetAnimation("Injured/idle", true);
    else if (followerInfo.CursedState == Thought.Freezing)
      skeletonGraphic.SetAnimation("Freezing/idle", true);
    else if (followerInfo.Traits.Contains(FollowerTrait.TraitType.Zombie))
      skeletonGraphic.SetAnimation("Zombie/zombie-idle", true);
    else if (FollowerBrainStats.BrainWashed && followerInfo.CursedState != Thought.Child)
    {
      skeletonGraphic.AnimationState.SetAnimation(1, "Emotions/emotion-brainwashed", true);
      skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
    }
    else if (followerInfo.Traits.Contains(FollowerTrait.TraitType.ExistentialDread) || followerInfo.Traits.Contains(FollowerTrait.TraitType.MissionaryTerrified))
      skeletonGraphic.SetAnimation("Existential Dread/dread-idle", true);
    else
      skeletonGraphic.SetAnimation("idle", true);
  }

  public static void ConfigureFollower(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfoSnapshot followerInfoSnapshot)
  {
    skeletonGraphic.ConfigureFollowerSkin(followerInfoSnapshot);
    skeletonGraphic.ConfigureEmotion(followerInfoSnapshot);
  }

  public static void ConfigureAnimal(
    this SkeletonGraphic skeletonGraphic,
    StructuresData.Ranchable_Animal info)
  {
    if (info.Type == InventoryItem.ITEM_TYPE.NONE)
      return;
    int growthState = info.GrowthState;
    Skin newSkin = new Skin("Skin");
    string animalSpineName = SpineExtensions.GetAnimalSpineName(info.Type, false);
    string str1 = $"{SpineExtensions.GetAnimalSpineName(info.Type, false)}/{SpineExtensions.GetAnimalSpineName(info.Type, false)}";
    string str2 = "";
    string skinName = info.WorkedToday && Interaction_Ranchable.shearables.Contains(info.Type) || info.Ailment != Interaction_Ranchable.Ailment.None ? $"{str2}{str1}_Sheared" : (!info.WorkedReady ? str2 + str1 : $"{str2}{str1}_Ready");
    newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin(skinName));
    if (info.Age < 2)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Baby"));
    else if (info.Age >= 15)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Prime"));
    else if ((double) info.Satiation <= 25.0)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Starving"));
    else if (growthState >= 6)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Fat"));
    else
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Normal"));
    if (info.Ailment == Interaction_Ranchable.Ailment.Feral)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Feral"));
    else if (info.Ailment == Interaction_Ranchable.Ailment.Injured)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Injured"));
    else if (info.Ailment == Interaction_Ranchable.Ailment.Stinky)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Stinky"));
    else if (info.State == Interaction_Ranchable.State.Overcrowded)
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("State/Starving"));
    switch (info.Type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Horns/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Ears/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Body/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Face/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Claws/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Shell/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Body/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Eyes/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Head/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Body/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Face/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Shell/{info.Head}"));
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Tail/{info.Horns}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Ears/{info.Ears}"));
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"{animalSpineName}/Head/{info.Head}"));
        break;
    }
    if (info.IsPlayersAnimal())
      newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("Necklaces/1"));
    skeletonGraphic.Skeleton.SetSkin(newSkin);
    skeletonGraphic.Skeleton.SetSlotsToSetupPose();
    skeletonGraphic.Skeleton.SetBonesToSetupPose();
    skeletonGraphic.Skeleton.UpdateCache();
    WorshipperData.SlotsAndColours[] animalColors = AnimalData.Instance.GetAnimalColors(info.Type);
    foreach (WorshipperData.SlotAndColor slotAndColour in animalColors[Mathf.Clamp(info.Colour, 0, animalColors.Length - 1)].SlotAndColours)
    {
      Slot slot = skeletonGraphic.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
    if (info.State == Interaction_Ranchable.State.Dead)
      skeletonGraphic.SetAnimation("dead-" + SpineExtensions.GetAnimalSpineName(info.Type, true), true);
    else
      skeletonGraphic.SetAnimation(skeletonGraphic.startingAnimation, true);
  }

  public static string GetAnimalSpineName(InventoryItem.ITEM_TYPE type, bool lower)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        return !lower ? "Goat" : "goat";
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        return !lower ? "Turtle" : "turtle";
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        return !lower ? "Crab" : "crab";
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        return !lower ? "Spider" : "spider";
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        return !lower ? "Snail" : "snail";
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        return !lower ? "Cow" : "cow";
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        return !lower ? "Llama" : "llama";
      default:
        return !lower ? "Goat" : "goat";
    }
  }

  public static void ConfigureFollowerSkin(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo)
  {
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(followerInfo.SkinName);
    skeletonGraphic.ConfigureFollowerSkin(colourData, followerInfo.SkinVariation, followerInfo.SkinColour, followerInfo.ID, followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated) ? (followerInfo.RottingUnique ? 2 : 1) : 0);
  }

  public static void ConfigureFollowerSkin(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfoSnapshot followerInfoSnapshot)
  {
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(followerInfoSnapshot.SkinName);
    Skin newSkin = new Skin("New Skin");
    newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin(colourData.Skin[Mathf.Min(followerInfoSnapshot.SkinVariation, colourData.Skin.Count - 1)].Skin));
    newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin(!string.IsNullOrEmpty(followerInfoSnapshot.ClothingVariant) ? followerInfoSnapshot.ClothingVariant : FollowerBrain.GetClothingName(followerInfoSnapshot.Clothing)));
    if (followerInfoSnapshot.Rotten != 0)
    {
      Random.InitState(followerInfoSnapshot.ID);
      if (followerInfoSnapshot.Rotten == 2)
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin("Mutation/Dark"));
      else
        newSkin.AddSkin(skeletonGraphic.Skeleton.Data.FindSkin($"Mutation/{Random.Range(1, 6)}"));
    }
    foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Min(followerInfoSnapshot.SkinColour, colourData.SlotAndColours.Count - 1)].SlotAndColours)
    {
      Slot slot = skeletonGraphic.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
    ClothingData clothingData = TailorManager.GetClothingData(followerInfoSnapshot.Clothing);
    if ((Object) clothingData != (Object) null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in clothingData.SlotAndColours[Mathf.Min(DataManager.Instance.GetClothingColour(clothingData.ClothingType), clothingData.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = skeletonGraphic.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
    skeletonGraphic.Skeleton.SetSkin(newSkin);
  }

  public static void ConfigureFollowerSkin(
    this SkeletonGraphic skeletonGraphic,
    WorshipperData.SkinAndData skinAndData,
    int variant = 0,
    int colour = 0,
    int id = 0,
    int rotten = 0)
  {
    skeletonGraphic.Skeleton.SetSkin(skinAndData.Skin[Mathf.Min(variant, skinAndData.Skin.Count - 1)].Skin);
    if (rotten != 0)
    {
      Random.InitState(id);
      if (rotten == 2)
        skeletonGraphic.Skeleton.SetSkin("Mutation/Dark");
      else
        skeletonGraphic.Skeleton.SetSkin($"Mutation/{Random.Range(1, 6)}");
    }
    else
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in skinAndData.SlotAndColours[Mathf.Min(colour, skinAndData.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = skeletonGraphic.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
  }

  public static void ConfigureFollowerOutfit(
    this SkeletonGraphic skeletonGraphic,
    ClothingData clothingData,
    int colour = 0,
    string variant = "")
  {
    if (!clothingData.Variants.Contains(variant))
      variant = DataManager.Instance.GetClothingVariant(clothingData.ClothingType);
    skeletonGraphic.Skeleton.SetSkin(!string.IsNullOrEmpty(variant) ? variant : FollowerBrain.GetClothingName(clothingData.ClothingType));
    foreach (WorshipperData.SlotAndColor slotAndColour in clothingData.SlotAndColours[Mathf.Min(colour, clothingData.SlotAndColours.Count - 1)].SlotAndColours)
    {
      Slot slot = skeletonGraphic.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  public static void ConfigureFollowerOutfit(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo)
  {
    new FollowerOutfit(followerInfo).SetOutfit(skeletonGraphic);
  }

  public static void ConfigureEmotion(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo)
  {
    skeletonGraphic.ConfigureEmotion(followerInfo.HasThought(Thought.Dissenter), FollowerBrainStats.BrainWashed, followerInfo.Illness, 1200f, CultFaithManager.CurrentFaith);
  }

  public static void ConfigureEmotion(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfoSnapshot followerInfoSnapshot)
  {
    skeletonGraphic.ConfigureEmotion(followerInfoSnapshot.Dissenter, followerInfoSnapshot.Brainwashed, followerInfoSnapshot.Illness, followerInfoSnapshot.Rest, followerInfoSnapshot.CultFaith);
  }

  public static void ConfigureEmotion(
    this SkeletonGraphic skeletonGraphic,
    bool dissenter,
    bool brainwashed,
    float illness,
    float rest,
    float faith)
  {
    if (dissenter)
      skeletonGraphic.SetFaceAnimation("Emotions/emotion-dissenter", true);
    else if (brainwashed)
      skeletonGraphic.SetFaceAnimation("Emotions/emotion-enlightened", true);
    else if ((double) illness > 0.0)
      skeletonGraphic.SetFaceAnimation("Emotions/emotion-sick", true);
    else if ((double) rest <= 20.0)
    {
      skeletonGraphic.SetFaceAnimation("Emotions/emotion-tired", true);
    }
    else
    {
      if ((double) faith >= 0.0 && (double) faith <= 25.0)
        skeletonGraphic.SetFaceAnimation("Emotions/emotion-angry", true);
      if ((double) faith > 25.0 && (double) faith <= 40.0)
        skeletonGraphic.SetFaceAnimation("Emotions/emotion-unhappy", true);
      if ((double) faith > 40.0 && (double) faith <= 80.0)
        skeletonGraphic.SetFaceAnimation("Emotions/emotion-normal", true);
      if ((double) faith <= 75.0)
        return;
      skeletonGraphic.SetFaceAnimation("Emotions/emotion-happy", true);
    }
  }

  public static void SetFaceAnimation(
    this SkeletonGraphic skeletonGraphic,
    string animation,
    bool loop)
  {
    skeletonGraphic.AnimationState.SetAnimation(1, animation, loop);
  }

  public static void ConfigurePrison(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo,
    StructuresData structureData,
    bool ui = false)
  {
    string animation = "Prison/stocks";
    if (followerInfo.CursedState == Thought.BecomeStarving)
      animation = string.Join("-", animation, "hungry");
    if (structureData.Type == StructureBrain.TYPES.PRISON && DataManager.Instance.Followers_Dead_IDs.Contains(followerInfo.ID))
    {
      if (structureData.Rotten)
        animation = string.Join("-", animation, "rotten");
      else
        animation = string.Join("-", animation, "dead");
      if (followerInfo.FrozeToDeath)
        animation = string.Join("-", animation, "frozen");
    }
    if (ui)
      animation = string.Join("-", animation, nameof (ui));
    skeletonGraphic.SetAnimation(animation);
  }
}
