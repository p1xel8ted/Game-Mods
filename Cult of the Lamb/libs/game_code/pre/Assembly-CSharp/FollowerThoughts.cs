// Decompiled with JetBrains decompiler
// Type: FollowerThoughts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;

#nullable disable
public class FollowerThoughts
{
  public static string GetLocalisedName(Thought Type)
  {
    return LocalizationManager.GetTranslation($"FollowerThoughts/{Type}");
  }

  public static string GetLocalisedDescription(Thought Type)
  {
    return LocalizationManager.GetTranslation($"FollowerThoughts/{Type}/Description");
  }

  public static string GetNotificationOnLocalizationKey(Thought type)
  {
    return $"Notifications/{type}/Notification/On";
  }

  public static string GetNotificationOffLocalizationKey(Thought type)
  {
    return $"Notifications/{type}/Notification/Off";
  }

  public static ThoughtData GetData(Thought thought)
  {
    switch (thought)
    {
      case Thought.TestPositive:
        return new ThoughtData(thought)
        {
          Modifier = 30f,
          Duration = 1200f,
          Stacking = 20,
          StackModifier = 5
        };
      case Thought.TestNegative:
        return new ThoughtData(thought)
        {
          Modifier = -30f,
          Duration = 1200f,
          Stacking = 20,
          StackModifier = -5
        };
      case Thought.EnthusiasticNewRecruit:
        return new ThoughtData(thought, Thought.HappyNewRecruit)
        {
          Modifier = 15f,
          Duration = 7200f
        };
      case Thought.HappyNewRecruit:
        return new ThoughtData(thought, Thought.HappyNewRecruit)
        {
          Modifier = 10f,
          Duration = 7200f
        };
      case Thought.UnenthusiasticNewRecruit:
        return new ThoughtData(thought, Thought.HappyNewRecruit)
        {
          Modifier = 5f,
          Duration = 7200f
        };
      case Thought.ReceivedNecklace:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.ReceivedGift:
        return new ThoughtData(thought)
        {
          Modifier = 8f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.DanceCircleFollowed:
        return new ThoughtData(thought, Thought.DanceCircleLed)
        {
          Modifier = 5f,
          Duration = 6000f
        };
      case Thought.DanceCircleLed:
        return new ThoughtData(thought, Thought.DanceCircleLed)
        {
          Modifier = 7f,
          Duration = 6000f
        };
      case Thought.DancedWithLeader:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f
        };
      case Thought.Intimidated:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 1200f
        };
      case Thought.BathroomOutside:
        return new ThoughtData(thought, Thought.BathroomOutside)
        {
          Modifier = -4f,
          Duration = 1200f
        };
      case Thought.BathroomOuthouse:
        return new ThoughtData(thought, Thought.BathroomOutside)
        {
          Modifier = 4f,
          Duration = 1200f
        };
      case Thought.BathroomOuthouse2:
        return new ThoughtData(thought, Thought.BathroomOutside)
        {
          Modifier = 7f,
          Duration = 1200f
        };
      case Thought.UpsetNoSermonYesterday:
        return new ThoughtData(thought, Thought.WatchedSermon)
        {
          Modifier = -15f,
          Duration = 1200f
        };
      case Thought.WatchedSermon:
        return new ThoughtData(thought, Thought.WatchedSermon)
        {
          Modifier = 12f,
          Duration = 1200f
        };
      case Thought.WatchedSermonDevotee:
        return new ThoughtData(thought, Thought.WatchedSermon)
        {
          Modifier = 17f,
          Duration = 1200f
        };
      case Thought.WatchedSermonSameAsYesterday:
        return new ThoughtData(thought, Thought.WatchedSermon)
        {
          Modifier = 5f,
          Duration = 1200f
        };
      case Thought.FollowerAscended:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f
        };
      case Thought.NoTemple:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 1200f
        };
      case Thought.SleptOutisde:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = -15f,
          Duration = 1200f
        };
      case Thought.SleptHouse1:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = -10f,
          Duration = 1200f
        };
      case Thought.SleptHouse2:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = 2f,
          Duration = 1200f
        };
      case Thought.SleptHouse3:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = 7f,
          Duration = 1200f
        };
      case Thought.SleptOutisdeMaterialisticTrait:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = -20f,
          Duration = 1200f
        };
      case Thought.SleptHouse1MaterialisticTrait:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = -10f,
          Duration = 1200f
        };
      case Thought.SleptHouse2MaterialisticTrait:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = 4f,
          Duration = 1200f
        };
      case Thought.SleptHouse3MaterialisticTrait:
        return new ThoughtData(thought, Thought.SleptOutisde)
        {
          Modifier = 14f,
          Duration = 1200f
        };
      case Thought.SleepInterrupted:
        return new ThoughtData(thought)
        {
          Modifier = -4f,
          Duration = 1200f
        };
      case Thought.AteMeal:
        return new ThoughtData(thought)
        {
          Modifier = 2f,
          Duration = 1200f
        };
      case Thought.AteGoodMeal:
        return new ThoughtData(thought)
        {
          Modifier = 6f,
          Duration = 1200f
        };
      case Thought.AteGoodMealFish:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f
        };
      case Thought.AteBadMealFish:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 1200f
        };
      case Thought.AteRottenMeal:
        return new ThoughtData(thought)
        {
          Modifier = -12f,
          Duration = 1200f
        };
      case Thought.AteRawFood:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 1200f
        };
      case Thought.AteSpecialMealGood:
        return new ThoughtData(thought)
        {
          Modifier = 12f,
          Duration = 1200f
        };
      case Thought.AteSpecialMealBad:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 1200f
        };
      case Thought.AtePoopMeal:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 1200f
        };
      case Thought.AteFollowerMeal:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 1200f
        };
      case Thought.AteRottenFollowerMeal:
        return new ThoughtData(thought)
        {
          Modifier = -25f,
          Duration = 1200f
        };
      case Thought.AteFollowerMealCannibal:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 1200f
        };
      case Thought.AteRottenFollowerMealCannibal:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 1200f
        };
      case Thought.AteGrassMealGrassEater:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 1200f
        };
      case Thought.StrangerDied:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 7200f,
          Stacking = 5,
          StackModifier = -3
        };
      case Thought.FriendDied:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 7200f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.LoverDied:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 12000f,
          Stacking = 5,
          StackModifier = -5
        };
      case Thought.EnemyDied:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 7200f,
          Stacking = 5,
          StackModifier = 4
        };
      case Thought.CultMemberDiedScaredOfDeathTrait:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 7200f,
          Stacking = 5,
          StackModifier = -2
        };
      case Thought.CultMemberDiedScaredOfDesensitized:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 7200f,
          Stacking = 5,
          StackModifier = 2
        };
      case Thought.CultMemberWasSacrificed:
        return new ThoughtData(thought, Thought.CultMemberWasSacrificed)
        {
          Modifier = -3f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = -1
        };
      case Thought.CultMemberWasSacrificedAgainstSacrificeTrait:
        return new ThoughtData(thought, Thought.CultMemberWasSacrificed)
        {
          Modifier = -10f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.CultMemberWasSacrificedSacrificeEnthusiastTrait:
        return new ThoughtData(thought, Thought.CultMemberWasSacrificed)
        {
          Modifier = 7f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.GoodChat:
        return new ThoughtData(thought)
        {
          Modifier = 3f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.BadChat:
        return new ThoughtData(thought)
        {
          Modifier = -3f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -3
        };
      case Thought.GoodChatWithLover:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 5
        };
      case Thought.NewFriend:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 6000f,
          Stacking = 10,
          StackModifier = 6
        };
      case Thought.NewEnemy:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 6000f,
          Stacking = 10,
          StackModifier = -6
        };
      case Thought.NewLover:
        return new ThoughtData(thought)
        {
          Modifier = 12f,
          Duration = 8400f,
          Stacking = 10,
          StackModifier = 10
        };
      case Thought.Brainwashed:
        return new ThoughtData(thought)
        {
          Modifier = 100f,
          Duration = 3600f,
          TrackThought = true
        };
      case Thought.BrainwashedHangOverLight:
        return new ThoughtData(thought)
        {
          Modifier = -2f,
          Duration = 1200f
        };
      case Thought.BrainwashedHangOverMild:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 1200f
        };
      case Thought.BrainwashedHangOverPounding:
        return new ThoughtData(thought)
        {
          Modifier = -12f,
          Duration = 1200f
        };
      case Thought.FollowerBrainwashed:
        return new ThoughtData(thought, Thought.FollowerBrainwashed)
        {
          Modifier = -5f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -1,
          TrackThought = true
        };
      case Thought.FollowerBrainwashedSubstanceEncouraged:
        return new ThoughtData(thought, Thought.FollowerBrainwashed)
        {
          Modifier = 5f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 1,
          TrackThought = true
        };
      case Thought.FollowerBrainwashedSubstanceBanned:
        return new ThoughtData(thought, Thought.FollowerBrainwashed)
        {
          Modifier = -5f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -1,
          TrackThought = true
        };
      case Thought.Dissenter:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = -1f
        };
      case Thought.NoLongerDissenting:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 3f
        };
      case Thought.Ill:
        return new ThoughtData(thought)
        {
          Modifier = -100f,
          Duration = -1f
        };
      case Thought.FeelingUnwell:
        return new ThoughtData(thought)
        {
          Modifier = -3f,
          Duration = 2400f
        };
      case Thought.ListenedToDissenter:
        return new ThoughtData(thought)
        {
          Modifier = -2f,
          Duration = 2400f,
          Stacking = 20,
          StackModifier = -1
        };
      case Thought.ListenedToDissenterZealotTrait:
        return new ThoughtData(thought)
        {
          Modifier = 3f,
          Duration = 2400f,
          Stacking = 20,
          StackModifier = -1
        };
      case Thought.HappyToBeBackToNormal:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f
        };
      case Thought.HappyConvert:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 6000f
        };
      case Thought.GratefulConvert:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 6000f
        };
      case Thought.SkepticalConvert:
        return new ThoughtData(thought)
        {
          Modifier = 2f,
          Duration = 6000f
        };
      case Thought.InstantBelieverConvert:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f
        };
      case Thought.ResentfulConvert:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 6000f
        };
      case Thought.GratefulRecued:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 6000f
        };
      case Thought.InstantBelieverRescued:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 6000f
        };
      case Thought.ResentfulRescued:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f
        };
      case Thought.InAweOfLeaderChain:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 7200f
        };
      case Thought.GaveConfessionHappy:
        return new ThoughtData(thought, Thought.GaveConfessionHappy)
        {
          Modifier = 7f,
          Duration = 2400f
        };
      case Thought.GaveConfessionEcstatic:
        return new ThoughtData(thought, Thought.GaveConfessionHappy)
        {
          Modifier = 12f,
          Duration = 2400f
        };
      case Thought.GaveConfessionHonoured:
        return new ThoughtData(thought, Thought.GaveConfessionHappy)
        {
          Modifier = 9f,
          Duration = 2400f
        };
      case Thought.GaveConfessionAnnoyed:
        return new ThoughtData(thought, Thought.GaveConfessionHappy)
        {
          Modifier = 2f,
          Duration = 2400f
        };
      case Thought.GaveConfessionAnxious:
        return new ThoughtData(thought, Thought.GaveConfessionHappy)
        {
          Modifier = 5f,
          Duration = 2400f
        };
      case Thought.GaveConfessionDivine:
        return new ThoughtData(thought, Thought.GaveConfessionHappy)
        {
          Modifier = 20f,
          Duration = 2400f
        };
      case Thought.GrievedAtUnburiedBody:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 7200f,
          Stacking = 10,
          StackModifier = -1
        };
      case Thought.GrievedAtRottenUnburiedBody:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 7200f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.HappyToSeeDeadBody:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.HappyToSeeRottingDeadBody:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.GrievedAtUnburiedBodyFearOfDeathTrait:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 7200f,
          Stacking = 10,
          StackModifier = -1
        };
      case Thought.GrievedAtRottenUnburiedBodyFearOfDeathTrait:
        return new ThoughtData(thought)
        {
          Modifier = -12f,
          Duration = 7200f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.SleptThroughYouButcheringDeadFollower:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 3600f
        };
      case Thought.SawYouButcheringDeadFollower:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 3600f
        };
      case Thought.SawYouButcheringDeadFollowerCannibalTrait:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f
        };
      case Thought.ReactToPoop:
        return new ThoughtData(thought)
        {
          Modifier = -3f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.ReactToPoopCoprophiliacTrait:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.ReactToPoopGermophobeTrait:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.ReactToFullOuthouse:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.ReactToFullOuthouseCoprophiliacTrait:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.ReactToFullOuthouseGermophobeTrait:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.ReactToVomit:
        return new ThoughtData(thought)
        {
          Modifier = -3f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.ReactToVomitCoprophiliacTrait:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.ReactToVomitGermophobeTrait:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.CultHasNewRecruit:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.CultHasNewBuilding:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.CultHasNewBuildingConstructionEnthusiast:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.HappyToBeHealed:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f
        };
      case Thought.CultMemberWasHealed:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 4800f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.InnocentReeducated:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f,
          Stacking = 20,
          StackModifier = 5
        };
      case Thought.InnocentCultMemberReeducated:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -5
        };
      case Thought.InnocentCultMemberReeducatedSleeping:
        return new ThoughtData(thought, Thought.InnocentCultMemberReeducatedSleeping)
        {
          Modifier = 0.0f,
          Duration = 1200f
        };
      case Thought.InnocentCultMemberReeducatedGullibleTrait:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -5
        };
      case Thought.InnocentCultMemberReeducatedGullibleTraitSleeping:
        return new ThoughtData(thought, Thought.InnocentCultMemberReeducatedSleeping)
        {
          Modifier = 0.0f,
          Duration = 1200f
        };
      case Thought.InnocentCultMemberReeducatedCynicalTrait:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 5
        };
      case Thought.InnocentCultMemberReeducatedCynicalTraitSleeping:
        return new ThoughtData(thought, Thought.InnocentCultMemberReeducatedSleeping)
        {
          Modifier = -5f,
          Duration = 1200f
        };
      case Thought.DissenterReeducated:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f
        };
      case Thought.DissenterCultMemberReeducated:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.DissenterCultMemberReeducatedSleeping:
        return new ThoughtData(thought, Thought.InnocentCultMemberReeducatedSleeping)
        {
          Modifier = 1f,
          Duration = 1200f
        };
      case Thought.DissenterCultMemberReeducatedGullibleTrait:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.DissenterCultMemberReeducatedSleepingGullibleTrait:
        return new ThoughtData(thought, Thought.InnocentCultMemberReeducatedSleeping)
        {
          Modifier = -1f,
          Duration = 1200f
        };
      case Thought.DissenterCultMemberReeducatedCynicalTrait:
        return new ThoughtData(thought)
        {
          Modifier = 20f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.DissenterCultMemberReeducatedSleepingCynicalTrait:
        return new ThoughtData(thought, Thought.InnocentCultMemberReeducatedSleeping)
        {
          Modifier = -5f,
          Duration = 1200f
        };
      case Thought.InnocentImprisoned:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.ImprisonedLibertarian:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.InnocentImprisonedDisciplinarian:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.DissenterImprisoned:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.InnocentImprisonedSleeping:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 0
        };
      case Thought.DissenterImprisonedSleeping:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.BecomeStarving:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 2400f
        };
      case Thought.NoLongerStarving:
        return new ThoughtData(thought, Thought.BecomeStarving)
        {
          Modifier = -5f,
          Duration = 2400f
        };
      case Thought.NaturallySkepticalTrait:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = -1f
        };
      case Thought.NaturallyObedientTrait:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = -1f
        };
      case Thought.SomeoneIllFearOfSickTrait:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = -1f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.SomeoneIllLoveOfSickTrait:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = -1f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.SomeoneDissenterZealotTrait:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = -1f,
          Stacking = 10,
          StackModifier = -1
        };
      case Thought.FellAsleepFromExhaustion:
        return new ThoughtData(thought)
        {
          Modifier = -2f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -1
        };
      case Thought.HelpedLeaderInResourceRoom:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.Promoted:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 1200f
        };
      case Thought.ClaimedDwelling:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 2400f
        };
      case Thought.LeaderMurderedAFollower:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = -5
        };
      case Thought.Fasting:
        return new ThoughtData(thought, Thought.Fasting)
        {
          Modifier = 1f,
          Duration = 3600f
        };
      case Thought.AngryAboutFasting:
        return new ThoughtData(thought, Thought.Fasting)
        {
          Modifier = -5f,
          Duration = 3600f
        };
      case Thought.HappyAboutFasting:
        return new ThoughtData(thought, Thought.Fasting)
        {
          Modifier = 5f,
          Duration = 3600f
        };
      case Thought.OldAge:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 4200f
        };
      case Thought.LeaderDidQuest:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f,
          Stacking = 10,
          StackModifier = 5
        };
      case Thought.Bribed:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 5
        };
      case Thought.FollowerIsElderlyLoveElderly:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = -1f,
          Stacking = 20,
          StackModifier = 2
        };
      case Thought.FollowerIsElderlyHateElderly:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = -1f,
          Stacking = 20,
          StackModifier = -2
        };
      case Thought.LeaderMurderedAFollowerHateElderly:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.EnlightenmentRitual:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 3600f
        };
      case Thought.ConstructionRitual:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 3600f
        };
      case Thought.Holiday:
        return new ThoughtData(thought)
        {
          Modifier = 25f,
          Duration = 1200f
        };
      case Thought.AfterHoliday:
        return new ThoughtData(thought)
        {
          Modifier = 25f,
          Duration = 2400f
        };
      case Thought.WorkThroughNight:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 2400f
        };
      case Thought.FishingRitual:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 2400f
        };
      case Thought.FightPitMercy:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 4800f
        };
      case Thought.FightPitExecution:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 4800f
        };
      case Thought.PropogandaSpeakers:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = -1f
        };
      case Thought.LeaderFailedQuest:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 3600f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.HappyFromDungeonNPC:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.SadFromDungeonNPC:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = -5
        };
      case Thought.LeaderBowed:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 3600f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.ReactDecoration:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.ReactDecorationFalseIdols:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 1200f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.DancePit:
        return new ThoughtData(thought)
        {
          Modifier = 30f,
          Duration = 6000f,
          Stacking = 10,
          StackModifier = 5
        };
      case Thought.ReactGrave:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.FeastTable:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 6000f,
          Stacking = 10,
          StackModifier = 5
        };
      case Thought.PrisonReEducation:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 6000f,
          Stacking = int.MaxValue,
          StackModifier = 1
        };
      case Thought.TiredFromMissionary:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = -5
        };
      case Thought.TiredFromMissionaryHappy:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.TiredFromMissionaryScared:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = -5
        };
      case Thought.ReactGraveEnemy:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.ReactGraveLover:
        return new ThoughtData(thought)
        {
          Modifier = 12f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.ReactGraveAfterlife:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.AlmsToThePoorRitual:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.SacrificedOldFollower:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.DemonSuccessfulRun:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.DemonFailedRun:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = -5
        };
      case Thought.ZombieAteMeal:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 4800f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.ZombieKilledFollower:
        return new ThoughtData(thought)
        {
          Modifier = -4f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -4
        };
      case Thought.ZombieDied:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.ZombieAteDeadFollower:
        return new ThoughtData(thought)
        {
          Modifier = -2f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = -2
        };
      case Thought.AttendedWedding:
        return new ThoughtData(thought, Thought.AttendedWedding)
        {
          Modifier = 15f,
          Duration = 3600f,
          Stacking = -1
        };
      case Thought.AttendedWeddingSpouse:
        return new ThoughtData(thought, Thought.AttendedWedding)
        {
          Modifier = -5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = -1
        };
      case Thought.MarriedToLeader:
        return new ThoughtData(thought, Thought.MarriedToLeader)
        {
          Modifier = 25f,
          Duration = -1f
        };
      case Thought.MultiMarriedToLeader:
        return new ThoughtData(thought, Thought.MarriedToLeader)
        {
          Modifier = 10f,
          Duration = -1f
        };
      case Thought.FaithEnforced:
        return new ThoughtData(thought)
        {
          Modifier = 2f,
          Duration = 2400f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.SpouseKiss:
        return new ThoughtData(thought)
        {
          Modifier = 2f,
          Duration = 1200f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.AttendedStrangerFuneral:
      case Thought.AttendedEnemyFuneral:
      case Thought.AttendedFriendFuneral:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.ElderlySacrificedHateElderly:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.ElderlySacrificedLoveElderly:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.LeaderMurderedAFollowerLoveElderly:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 3600f,
          Stacking = 10,
          StackModifier = 1
        };
      case Thought.CultFaith_TestPositive:
        return new ThoughtData(thought)
        {
          Modifier = 20f,
          Duration = 600f,
          Stacking = 10,
          StackModifier = 10
        };
      case Thought.CultFaith_TestNegative:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 600f,
          Stacking = 10,
          StackModifier = -10
        };
      case Thought.Cult_Sermon:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 1800f
        };
      case Thought.Cult_FollowerDied:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -20f,
          Stacking = 100
        };
      case Thought.Cult_FollowerDied_Trait:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -10f,
          Stacking = 100
        };
      case Thought.Cult_FollowerDied_Trait_Scared:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          StackModifier = -2,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -10f,
          Stacking = 100
        };
      case Thought.Cult_FollowerBuried:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          StackModifier = 5,
          Duration = 3600f,
          Stacking = 100
        };
      case Thought.Cult_Murder:
        return new ThoughtData(thought)
        {
          Modifier = -40f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = -30f,
          Stacking = 100
        };
      case Thought.Cult_Sacrifice:
        return new ThoughtData(thought, Thought.Cult_Sacrifice)
        {
          Modifier = -10f,
          Duration = 4800f,
          ReduceOverTime = true,
          StartingModifier = -15f,
          Stacking = 100
        };
      case Thought.Cult_Sacrifice_Trait:
        return new ThoughtData(thought, Thought.Cult_Sacrifice)
        {
          Modifier = 20f,
          Duration = 4800f,
          ReduceOverTime = true,
          StartingModifier = 5f,
          Stacking = 100
        };
      case Thought.Cult_Sacrifice_Trait_Scared:
        return new ThoughtData(thought, Thought.Cult_Sacrifice)
        {
          Modifier = -5f,
          Duration = 4800f,
          ReduceOverTime = true,
          StartingModifier = -5f,
          Stacking = 100
        };
      case Thought.Cult_ButcheredFollowerMeat:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = -20f,
          Stacking = 100
        };
      case Thought.Cult_Imprison:
        return new ThoughtData(thought, Thought.Cult_Imprison)
        {
          Modifier = -15f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -20f,
          Stacking = 100
        };
      case Thought.Cult_Imprison_Trait:
        return new ThoughtData(thought, Thought.Cult_Imprison)
        {
          Modifier = -2f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -5f,
          Stacking = 100
        };
      case Thought.Cult_Libertarian:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 1200f,
          Stacking = 10,
          StackModifier = 2
        };
      case Thought.Cult_CureDissenter:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 15f,
          Stacking = 100
        };
      case Thought.Cult_NewBuilding:
        return new ThoughtData(thought)
        {
          Modifier = 2f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 5f,
          Stacking = 100
        };
      case Thought.Cult_NewFolllower:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 5f,
          Stacking = 100
        };
      case Thought.Cult_NotEnoughBeds:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = -1f,
          TrackThought = true
        };
      case Thought.Cult_Starving:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 6000f
        };
      case Thought.Cult_No_Longer_Starving:
        return new ThoughtData(thought)
        {
          Modifier = 20f,
          Duration = 6000f
        };
      case Thought.Cult_AteGrassMeal:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -5f,
          Stacking = 100
        };
      case Thought.Cult_AteFollowerMeat:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -20f,
          Stacking = 100
        };
      case Thought.Cult_AteFollowerMeatTrait:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = 5f,
          Stacking = 100
        };
      case Thought.Cult_CompleteQuest:
        return new ThoughtData(thought)
        {
          Modifier = 20f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = 15f,
          Stacking = 100
        };
      case Thought.Cult_FailQuest:
        return new ThoughtData(thought)
        {
          Modifier = -45f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = -30f,
          Stacking = 100
        };
      case Thought.Cult_NewRecruitSkeptical:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = -10f,
          Stacking = 100
        };
      case Thought.Cult_NewRecruitObedient:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = 10f,
          Stacking = 100
        };
      case Thought.Cult_DeclinedQuest:
        return new ThoughtData(thought)
        {
          Modifier = -25f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = -30f,
          Stacking = 100
        };
      case Thought.Cult_HateElderly_Trait_Pros:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 10f,
          Stacking = 100
        };
      case Thought.Cult_HateElderly_Trait_Cons:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = -20f,
          Stacking = 100
        };
      case Thought.Cult_LoveElderly_Trait:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 10f,
          Stacking = 100
        };
      case Thought.Cult_MushroomEncouraged_Trait:
        return new ThoughtData(thought)
        {
          Modifier = 20f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = 20f,
          Stacking = 100,
          TrackThought = true
        };
      case Thought.Cult_LoveSick:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 5f,
          Stacking = 100
        };
      case Thought.Cult_FearSick:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = -5f,
          Stacking = 100
        };
      case Thought.Cult_ConstructionEnthusiast_Trait:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 5f,
          Stacking = 100
        };
      case Thought.Cult_SermonEnthusiast_Trait:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 40f,
          Stacking = 100
        };
      case Thought.Cult_GermophobeBecameSick:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = -10f,
          Stacking = 100
        };
      case Thought.Cult_CoprophiliacBecameSick:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 10f,
          Stacking = 100
        };
      case Thought.Cult_NewDecoration:
        return new ThoughtData(thought)
        {
          Modifier = 4f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 2f,
          Stacking = 100
        };
      case Thought.Cult_FalseIdols_Trait:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 2f,
          Stacking = 100
        };
      case Thought.Cult_Materialistic_Trait_Hut:
        return new ThoughtData(thought)
        {
          Modifier = 2f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 2f,
          Stacking = 100
        };
      case Thought.Cult_Materialistic_Trait_House:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 2400f,
          ReduceOverTime = true,
          StartingModifier = 5f,
          Stacking = 100
        };
      case Thought.Cult_Holiday:
        return new ThoughtData(thought)
        {
          Modifier = 80f,
          Duration = 1200f,
          TrackThought = true
        };
      case Thought.Cult_AlmsToPoor:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f
        };
      case Thought.Cult_Fast:
        return new ThoughtData(thought)
        {
          Modifier = -30f,
          Duration = 3600f,
          TrackThought = true
        };
      case Thought.Cult_Feast:
        return new ThoughtData(thought)
        {
          Modifier = 30f,
          Duration = 3600f
        };
      case Thought.Cult_Wedding:
        return new ThoughtData(thought)
        {
          Modifier = 30f,
          Duration = 3600f
        };
      case Thought.Cult_Wedding_JealousSpouse:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 3600f
        };
      case Thought.Cult_FightPit:
        return new ThoughtData(thought)
        {
          Modifier = 20f,
          Duration = 3600f
        };
      case Thought.Cult_BuildingGoodGraves:
        return new ThoughtData(thought)
        {
          Modifier = 2f,
          Duration = 3600f,
          StackModifier = 10
        };
      case Thought.Cult_Funeral:
        return new ThoughtData(thought)
        {
          Modifier = 20f,
          Duration = 3600f
        };
      case Thought.Cult_Ressurection:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 3600f
        };
      case Thought.Cult_InterruptedFollower:
        return new ThoughtData(thought)
        {
          Modifier = -1f,
          Duration = 3600f
        };
      case Thought.Cult_AteGreatMeal:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f
        };
      case Thought.Cult_AteGreatFishMeal:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 3600f
        };
      case Thought.Cult_AteGrassMealTrait:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 3600f
        };
      case Thought.Cult_FasterBuilding:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 3600f
        };
      case Thought.Cult_FollowerBecameIllSleepingNextToIllFollower_Germophobe:
        return new ThoughtData(thought)
        {
          Modifier = -7f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = -1
        };
      case Thought.Cult_FollowerBecameIllSleepingNextToIllFollower_Coprophiliac:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 2400f,
          Stacking = 5,
          StackModifier = 1
        };
      case Thought.Cult_WokeUpFollower:
        return new ThoughtData(thought)
        {
          Modifier = -5f,
          Duration = 6000f
        };
      case Thought.Cult_WokeUpEveryone:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 6000f
        };
      case Thought.Cult_ExhaustedFollower:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 6000f
        };
      case Thought.Cult_ExhaustedEveryone:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 6000f
        };
      case Thought.Cult_NoLongerExhaustedFollower:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 6000f
        };
      case Thought.Cult_NoLongerExhaustedEveryone:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 6000f
        };
      case Thought.Cult_ExhaustedFollowerPassedOut:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 6000f
        };
      case Thought.Cult_FollowerReachedOldAge:
        return new ThoughtData(thought) { Duration = 2400f };
      case Thought.Cult_FollowerDiedOfOldAge:
        return new ThoughtData(thought) { Duration = 2400f };
      case Thought.Cult_DiscoveredNewLocation:
        return new ThoughtData(thought)
        {
          Modifier = 25f,
          Duration = 6000f
        };
      case Thought.Cult_PledgedToYou:
        return new ThoughtData(thought)
        {
          Modifier = 35f,
          Duration = 6000f
        };
      case Thought.Cult_DiedInDungeon:
        return new ThoughtData(thought)
        {
          Modifier = !DifficultyManager.AssistModeEnabled || DifficultyManager.SecondaryDifficulty != DifficultyManager.Difficulty.Easy ? -10f : -5f,
          Duration = 6000f
        };
      case Thought.Cult_KilledMiniBoss:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 6000f
        };
      case Thought.Cult_KilledBoss:
        return new ThoughtData(thought)
        {
          Modifier = 30f,
          Duration = 6000f
        };
      case Thought.Cult_LostRespect:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 6000f
        };
      case Thought.BedCollapsed:
        return new ThoughtData(thought)
        {
          Modifier = 0.0f,
          Duration = 6000f
        };
      case Thought.Cult_LootedCorpse:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 6000f
        };
      case Thought.Cult_WorkThroughNight:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 3600f,
          TrackThought = true
        };
      case Thought.Cult_ConsumeFollower:
        return new ThoughtData(thought)
        {
          Modifier = -30f,
          Duration = 6000f
        };
      case Thought.Cult_Ascend:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f
        };
      case Thought.Cult_Enlightenment:
        return new ThoughtData(thought)
        {
          Modifier = -10f,
          Duration = 3600f,
          TrackThought = true
        };
      case Thought.Cult_DonationRitual:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 6000f
        };
      case Thought.Cult_FishingRitual:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 3600f,
          TrackThought = true
        };
      case Thought.Cult_HarvestRitual:
        return new ThoughtData(thought)
        {
          Modifier = 10f,
          Duration = 6000f
        };
      case Thought.Cult_FaithEnforcer:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 6000f
        };
      case Thought.Cult_TaxEnforcer:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 6000f
        };
      case Thought.DiedFromIllness:
        return new ThoughtData(thought)
        {
          Modifier = -20f,
          Duration = 3600f,
          ReduceOverTime = true,
          StartingModifier = -20f,
          Stacking = 100
        };
      case Thought.Cult_Bless:
        return new ThoughtData(thought)
        {
          Modifier = 5f,
          Duration = 6000f
        };
      case Thought.Cult_Inspire:
        return new ThoughtData(thought)
        {
          Modifier = 7f,
          Duration = 6000f
        };
      case Thought.Cult_NewRecipe:
        return new ThoughtData(thought)
        {
          Modifier = 15f,
          Duration = 6000f
        };
      case Thought.Cult_MurderAtNightFewWitnesses:
        return new ThoughtData(thought)
        {
          Modifier = -15f,
          Duration = 6000f,
          ReduceOverTime = true,
          StartingModifier = -15f,
          Stacking = 100
        };
      case Thought.Cult_MurderAtNightNoWitnesses:
        return new ThoughtData(thought) { Modifier = 0.0f };
      case Thought.Cult_HelpFollower:
        return new ThoughtData(thought) { Modifier = 3f };
      case Thought.Cult_NewDoctrine:
        return new ThoughtData(thought) { Modifier = 10f };
      case Thought.Cult_GaveFollowerItem:
        return new ThoughtData(thought)
        {
          Modifier = 1f,
          Duration = 6000f
        };
      case Thought.BiggestNeed_Homeless:
      case Thought.BiggestNeed_Hungry:
      case Thought.BiggestNeed_Sick:
      case Thought.BiggestNeed_Dissenter:
      case Thought.BiggestNeed_Exhausted:
      case Thought.BiggestNeed_BrokenBed:
        return new ThoughtData(thought) { Modifier = 0.0f };
      case Thought.Cult_BaseUpgraded:
        return new ThoughtData(thought)
        {
          Modifier = 25f,
          Duration = 6000f
        };
      case Thought.FaithIncreased:
        return new ThoughtData(thought)
        {
          Modifier = 30f,
          Duration = 1200f,
          Stacking = 20,
          StackModifier = 5
        };
      case Thought.FaithDecreased:
        return new ThoughtData(thought)
        {
          Modifier = -30f,
          Duration = 1200f,
          Stacking = 20,
          StackModifier = -5
        };
      default:
        return (ThoughtData) null;
    }
  }
}
