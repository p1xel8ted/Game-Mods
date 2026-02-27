// Decompiled with JetBrains decompiler
// Type: SpineExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
    skeletonGraphic.AnimationState.SetAnimation(0, animation, false);
  }

  public static void ConfigureFollower(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo)
  {
    skeletonGraphic.ConfigureFollowerSkin(followerInfo);
    skeletonGraphic.ConfigureFollowerOutfit(followerInfo);
    skeletonGraphic.ConfigureEmotion(followerInfo);
  }

  public static void ConfigureFollower(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfoSnapshot followerInfoSnapshot)
  {
    skeletonGraphic.ConfigureFollowerSkin(followerInfoSnapshot);
    skeletonGraphic.ConfigureEmotion(followerInfoSnapshot);
  }

  public static void ConfigureFollowerSkin(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo)
  {
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(followerInfo.SkinName);
    skeletonGraphic.ConfigureFollowerSkin(colourData, followerInfo.SkinVariation, followerInfo.SkinColour);
  }

  public static void ConfigureFollowerSkin(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfoSnapshot followerInfoSnapshot)
  {
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(followerInfoSnapshot.SkinName);
    skeletonGraphic.ConfigureFollowerSkin(colourData, followerInfoSnapshot.SkinVariation, followerInfoSnapshot.SkinColour);
  }

  public static void ConfigureFollowerSkin(
    this SkeletonGraphic skeletonGraphic,
    WorshipperData.SkinAndData skinAndData,
    int variant = 0,
    int colour = 0)
  {
    skeletonGraphic.Skeleton.SetSkin(skinAndData.Skin[Mathf.Min(variant, skinAndData.Skin.Count - 1)].Skin);
    foreach (WorshipperData.SlotAndColor slotAndColour in skinAndData.SlotAndColours[Mathf.Min(colour, skinAndData.SlotAndColours.Count - 1)].SlotAndColours)
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
    new FollowerOutfit(followerInfo).SetOutfit(skeletonGraphic, followerInfo.Outfit, followerInfo.Necklace, false);
    if (followerInfo.TaxEnforcer)
    {
      skeletonGraphic.Skeleton.SetAttachment("HAT", "Hats/Hat_Enforcer");
    }
    else
    {
      if (!followerInfo.FaithEnforcer)
        return;
      skeletonGraphic.Skeleton.SetAttachment("HAT", "Hats/Hat_FaithEnforcer");
    }
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
      SetFaceAnimation("Emotions/emotion-dissenter", true);
    else if (brainwashed)
      SetFaceAnimation("Emotions/emotion-enlightened", true);
    else if ((double) illness > 0.0)
      SetFaceAnimation("Emotions/emotion-sick", true);
    else if ((double) rest <= 20.0)
    {
      SetFaceAnimation("Emotions/emotion-tired", true);
    }
    else
    {
      if ((double) faith >= 0.0 && (double) faith <= 25.0)
        SetFaceAnimation("Emotions/emotion-angry", true);
      if ((double) faith > 25.0 && (double) faith <= 40.0)
        SetFaceAnimation("Emotions/emotion-unhappy", true);
      if ((double) faith > 40.0 && (double) faith <= 80.0)
        SetFaceAnimation("Emotions/emotion-normal", true);
      if ((double) faith <= 75.0)
        return;
      SetFaceAnimation("Emotions/emotion-happy", true);
    }

    void SetFaceAnimation(string animation, bool loop)
    {
      skeletonGraphic.AnimationState.SetAnimation(1, animation, loop);
    }
  }

  public static void ConfigurePrison(
    this SkeletonGraphic skeletonGraphic,
    FollowerInfo followerInfo,
    StructuresData structureData,
    bool ui = false)
  {
    string animation = "Prison/stocks";
    if (followerInfo.IsStarving)
      animation = string.Join("-", animation, "hungry");
    if (structureData.Type == StructureBrain.TYPES.PRISON && DataManager.Instance.Followers_Dead_IDs.Contains(followerInfo.ID))
    {
      if (structureData.Rotten)
        animation = string.Join("-", animation, "rotten");
      else
        animation = string.Join("-", animation, "dead");
    }
    if (ui)
      animation = string.Join("-", animation, nameof (ui));
    skeletonGraphic.SetAnimation(animation);
  }
}
