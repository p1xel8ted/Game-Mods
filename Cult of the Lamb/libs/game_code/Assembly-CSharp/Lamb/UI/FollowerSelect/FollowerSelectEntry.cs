// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerSelect.FollowerSelectEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Lamb.UI.FollowerSelect;

public class FollowerSelectEntry
{
  [CompilerGenerated]
  public FollowerInfo \u003CFollowerInfo\u003Ek__BackingField;
  [CompilerGenerated]
  public FollowerSelectEntry.Status \u003CAvailabilityStatus\u003Ek__BackingField;

  public FollowerInfo FollowerInfo
  {
    set => this.\u003CFollowerInfo\u003Ek__BackingField = value;
    get => this.\u003CFollowerInfo\u003Ek__BackingField;
  }

  public FollowerSelectEntry.Status AvailabilityStatus
  {
    set => this.\u003CAvailabilityStatus\u003Ek__BackingField = value;
    get => this.\u003CAvailabilityStatus\u003Ek__BackingField;
  }

  public FollowerSelectEntry(
    FollowerInfo followerInfo,
    FollowerSelectEntry.Status availabilityStatus = FollowerSelectEntry.Status.Available)
  {
    this.FollowerInfo = followerInfo;
    this.AvailabilityStatus = availabilityStatus;
  }

  public FollowerSelectEntry(int followerID, FollowerSelectEntry.Status availabilityStatus = FollowerSelectEntry.Status.Available)
  {
    this.FollowerInfo = FollowerInfo.GetInfoByID(followerID);
    this.AvailabilityStatus = availabilityStatus;
  }

  public FollowerSelectEntry(
    FollowerBrain followerBrain,
    FollowerSelectEntry.Status availabilityStatus = FollowerSelectEntry.Status.Available)
  {
    this.FollowerInfo = followerBrain._directInfoAccess;
    this.AvailabilityStatus = availabilityStatus;
  }

  public FollowerSelectEntry(Follower follower, FollowerSelectEntry.Status availabilityStatus = FollowerSelectEntry.Status.Available)
  {
    this.FollowerInfo = follower.Brain._directInfoAccess;
    this.AvailabilityStatus = availabilityStatus;
  }

  public FollowerSelectEntry(SimFollower simFollower, FollowerSelectEntry.Status availabilityStatus = FollowerSelectEntry.Status.Available)
  {
    this.FollowerInfo = simFollower.Brain._directInfoAccess;
    this.AvailabilityStatus = availabilityStatus;
  }

  public enum Status
  {
    Available,
    Unavailable,
    UnavailableDissenting,
    UnavailableElderly,
    UnavailableMissionary,
    UnavailableDead,
    UnavailableInFightPit,
    UnavailableDemon,
    UnavailableLowLevel,
    UnavailableMaxLevel,
    UnavailableAlreadyFaithEnforcer,
    UnavailableAlreadyTaxEnforcer,
    UnavailableImprisoned,
    UnavailableNewRecruit,
    UnavailableAlreadyInBed,
    UnavailableDoesNotNeedHealing,
    UnavailableAlreadyDisciple,
    UnavailableAlreadyWearingOutfit,
    UnavailableNotMarried,
    UnavailableAlreadyAssignedDrink,
    UnavailableBusy,
    UnavailableChild,
    UnavailableIll,
    UnavailableStarving,
    UnavailableAlreadyPlayedKnucklebonesToday,
    UnavailableExistentialDread,
    UnavailableMissionaryTerrified,
    UnavailableInNursery,
    UnavailableZombie,
    UnavailableTooOldForDaycare,
    UnavailableAlreadyMarriedToFollower,
    UnavailableTraitManipulating,
    UnavailableTooManyTraits,
    UnavailableNotEnoughTraits,
    UnavailableNotChild,
    UnavailableIncorrectTraits,
    UnavailableFreezing,
  }
}
