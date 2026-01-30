// Decompiled with JetBrains decompiler
// Type: FollowerBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerBrain
{
  public FollowerBrainInfo Info;
  public FollowerBrainStats Stats;
  public FollowerInfo _directInfoAccess;
  public FollowerLocation DesiredLocation;
  public FollowerOutfitType TemporaryOutfitStore;
  public bool _followingPlayer;
  public bool _inRitual;
  public NotificationCentre.NotificationType LeftCultWithReason;
  [CompilerGenerated]
  public bool \u003CLeftCult\u003Ek__BackingField;
  public float bathroomOffset = -1f;
  public List<FollowerBrain> targetedFollowers = new List<FollowerBrain>();
  public float timer;
  public string ReturnString;
  public float ModifierTotal;
  public static List<int> MultipleSpouseFaith = new List<int>()
  {
    30,
    15,
    10,
    5,
    3,
    -2,
    -5,
    -7,
    -10
  };
  public Action<float> OnNewThought;
  public System.Action OnAdoration;
  public static Dictionary<FollowerBrain.AdorationActions, int> AdorationsAndActions = new Dictionary<FollowerBrain.AdorationActions, int>()
  {
    {
      FollowerBrain.AdorationActions.Sermon,
      5
    },
    {
      FollowerBrain.AdorationActions.Bless,
      13
    },
    {
      FollowerBrain.AdorationActions.BigGift,
      50
    },
    {
      FollowerBrain.AdorationActions.Quest,
      50
    },
    {
      FollowerBrain.AdorationActions.Gift,
      30
    },
    {
      FollowerBrain.AdorationActions.Necklace,
      25
    },
    {
      FollowerBrain.AdorationActions.Inspire,
      20
    },
    {
      FollowerBrain.AdorationActions.Intimidate,
      10
    },
    {
      FollowerBrain.AdorationActions.Bribe,
      15
    },
    {
      FollowerBrain.AdorationActions.Ritual_AlmsToPoor,
      30
    },
    {
      FollowerBrain.AdorationActions.FaithEnforce,
      5
    },
    {
      FollowerBrain.AdorationActions.HappyFollowerNPC,
      60
    },
    {
      FollowerBrain.AdorationActions.ConfessionBooth,
      75
    },
    {
      FollowerBrain.AdorationActions.FirePit,
      30
    },
    {
      FollowerBrain.AdorationActions.FightPitMercy,
      20
    },
    {
      FollowerBrain.AdorationActions.FightPitDeath,
      50
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl2,
      20
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl3,
      30
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl4,
      40
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl5,
      50
    },
    {
      FollowerBrain.AdorationActions.SmoochSpouse,
      10
    },
    {
      FollowerBrain.AdorationActions.AssignEnforcer,
      30
    },
    {
      FollowerBrain.AdorationActions.PetDog,
      5
    },
    {
      FollowerBrain.AdorationActions.CuddleBaby,
      150
    },
    {
      FollowerBrain.AdorationActions.SermonLvl1,
      10
    },
    {
      FollowerBrain.AdorationActions.BribeLvl1,
      20
    },
    {
      FollowerBrain.AdorationActions.BlessLvl1,
      35
    },
    {
      FollowerBrain.AdorationActions.InspireLvl1,
      40
    },
    {
      FollowerBrain.AdorationActions.IntimidateLvl1,
      30
    },
    {
      FollowerBrain.AdorationActions.BigGiftLvl1,
      50
    },
    {
      FollowerBrain.AdorationActions.GiftLvl1,
      30
    },
    {
      FollowerBrain.AdorationActions.NecklaceLvl1,
      30
    },
    {
      FollowerBrain.AdorationActions.LevelUp,
      100
    },
    {
      FollowerBrain.AdorationActions.BecomeDisciple,
      50
    },
    {
      FollowerBrain.AdorationActions.Bully,
      30
    },
    {
      FollowerBrain.AdorationActions.WonFight,
      40
    },
    {
      FollowerBrain.AdorationActions.Reassure,
      15
    },
    {
      FollowerBrain.AdorationActions.KB_Win,
      100
    },
    {
      FollowerBrain.AdorationActions.KB_Draw,
      30
    },
    {
      FollowerBrain.AdorationActions.Frigidophile,
      100
    },
    {
      FollowerBrain.AdorationActions.LockedLoyal,
      5
    },
    {
      FollowerBrain.AdorationActions.GiftPet,
      50
    },
    {
      FollowerBrain.AdorationActions.WinterExcited,
      20
    }
  };
  public System.Action OnGetXP;
  public int SpeakersInRange;
  public Dictionary<int, int> ReactionDictionary = new Dictionary<int, int>();
  public Action<FollowerState, FollowerState> OnStateChanged;
  public FollowerState _currentState;
  [CompilerGenerated]
  public bool \u003CShouldReconsiderTask\u003Ek__BackingField;
  public FollowerBrain.PendingTaskData PendingTask;
  public Action<FollowerTask, FollowerTask> OnTaskChanged;
  public FollowerTask _nextTask;
  public FollowerTask _currentTask;
  public List<Structures_Outhouse> cachedToilets = new List<Structures_Outhouse>();
  public System.Action OnBecomeDissenter;
  public List<Structures_Meal> cachedMeals = new List<Structures_Meal>();
  public List<Structures_Kitchen> cachedKitchens = new List<Structures_Kitchen>();
  public static FollowerBrain.DwellingAssignmentChanged OnDwellingAssigned;
  public static FollowerBrain.DwellingAssignmentChanged OnDwellingCleared;
  public static FollowerBrain.DwellingAssignmentChanged OnDwellingAssignedAwaitClaim;
  public static Action<int> OnBrainAdded;
  public static Action<int> OnBrainRemoved;
  public static Dictionary<int, FollowerBrain> _brainsByID = new Dictionary<int, FollowerBrain>();
  public static List<FollowerBrain> AllBrains = new List<FollowerBrain>();
  public static List<FollowerHatType> TemporaryHats = new List<FollowerHatType>()
  {
    FollowerHatType.Bartender,
    FollowerHatType.Chef,
    FollowerHatType.Farm,
    FollowerHatType.Lumberjack,
    FollowerHatType.Miner,
    FollowerHatType.Refinery,
    FollowerHatType.Medic,
    FollowerHatType.Ranch,
    FollowerHatType.Handyman
  };
  public static StructureBrain.TYPES DEBUG_TESTING_POOP_TYPE = StructureBrain.TYPES.NONE;
  public static Dictionary<FollowerBrain.PleasureActions, int> PleasureAndActions = new Dictionary<FollowerBrain.PleasureActions, int>()
  {
    {
      FollowerBrain.PleasureActions.Drink_Beer,
      18
    },
    {
      FollowerBrain.PleasureActions.Drink_Cocktail,
      25
    },
    {
      FollowerBrain.PleasureActions.Drink_Gin,
      20
    },
    {
      FollowerBrain.PleasureActions.Drink_Wine,
      18
    },
    {
      FollowerBrain.PleasureActions.Drink_Eggnog,
      50
    },
    {
      FollowerBrain.PleasureActions.Drink_Poop_Juice,
      35
    },
    {
      FollowerBrain.PleasureActions.Drink_Mushroom_Juice,
      10
    },
    {
      FollowerBrain.PleasureActions.Drink_Chilli,
      18
    },
    {
      FollowerBrain.PleasureActions.Drink_Lightning,
      18
    },
    {
      FollowerBrain.PleasureActions.Drink_Sin,
      50
    },
    {
      FollowerBrain.PleasureActions.Drink_Grass,
      40
    },
    {
      FollowerBrain.PleasureActions.Purge,
      55
    },
    {
      FollowerBrain.PleasureActions.PurgeDissenter,
      65
    },
    {
      FollowerBrain.PleasureActions.DoctrinalExtremist,
      2
    },
    {
      FollowerBrain.PleasureActions.ViolentExtremist,
      2
    },
    {
      FollowerBrain.PleasureActions.FertilityTrait,
      25
    },
    {
      FollowerBrain.PleasureActions.DeathTrait,
      2
    },
    {
      FollowerBrain.PleasureActions.Testing,
      65
    },
    {
      FollowerBrain.PleasureActions.Testing_Half,
      25
    },
    {
      FollowerBrain.PleasureActions.NudismWinner,
      55
    },
    {
      FollowerBrain.PleasureActions.NudismDrunk,
      65
    },
    {
      FollowerBrain.PleasureActions.RemovedDissent,
      15
    },
    {
      FollowerBrain.PleasureActions.ConfessionBooth,
      50
    },
    {
      FollowerBrain.PleasureActions.Cannibal,
      25
    },
    {
      FollowerBrain.PleasureActions.DrinkingFestival,
      15
    },
    {
      FollowerBrain.PleasureActions.Single,
      1
    },
    {
      FollowerBrain.PleasureActions.Zero,
      0
    },
    {
      FollowerBrain.PleasureActions.Murderer,
      35
    },
    {
      FollowerBrain.PleasureActions.DrumCircle,
      100
    },
    {
      FollowerBrain.PleasureActions.Twitch,
      100
    },
    {
      FollowerBrain.PleasureActions.SinNPC,
      100
    },
    {
      FollowerBrain.PleasureActions.Masochistic,
      2
    },
    {
      FollowerBrain.PleasureActions.GroupSpa,
      33
    }
  };

  public List<TaskAndTime> _taskMemory
  {
    get => this._directInfoAccess.TaskMemory;
    set => this._directInfoAccess.TaskMemory = value;
  }

  public FollowerLocation HomeLocation => this._directInfoAccess.HomeLocation;

  public FollowerLocation Location
  {
    get => this._directInfoAccess.Location;
    set => this._directInfoAccess.Location = value;
  }

  public Vector3 LastPosition
  {
    get => this._directInfoAccess.LastPosition;
    set => this._directInfoAccess.LastPosition = value;
  }

  public FollowerTaskType SavedFollowerTaskType
  {
    get => this._directInfoAccess.SavedFollowerTaskType;
    set => this._directInfoAccess.SavedFollowerTaskType = value;
  }

  public FollowerLocation SavedFollowerTaskLocation
  {
    get => this._directInfoAccess.SavedFollowerTaskLocation;
    set => this._directInfoAccess.SavedFollowerTaskLocation = value;
  }

  public Vector3 SavedFollowerTaskDestination
  {
    get => this._directInfoAccess.SavedFollowerTaskDestination;
    set => this._directInfoAccess.SavedFollowerTaskDestination = value;
  }

  public bool FollowingPlayer
  {
    get => this._followingPlayer;
    set
    {
      if (this._followingPlayer == value)
        return;
      this._followingPlayer = value;
      this.CheckChangeState();
    }
  }

  public bool InRitual
  {
    get => this._inRitual;
    set
    {
      if (this._inRitual == value)
        return;
      this._inRitual = value;
      this.CheckChangeState();
    }
  }

  public bool DiedFromMurder
  {
    get => this._directInfoAccess.DiedFromMurder;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedFromMurder = value;
    }
  }

  public bool DiedInPrison
  {
    get => this._directInfoAccess.DiedInPrison;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedInPrison = value;
    }
  }

  public bool DiedOfIllness
  {
    get => this._directInfoAccess.DiedOfIllness;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedOfIllness = value;
    }
  }

  public bool DiedOfInjury
  {
    get => this._directInfoAccess.DiedOfInjury;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedOfInjury = value;
    }
  }

  public bool DiedOfOldAge
  {
    get => this._directInfoAccess.DiedOfOldAge;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedOfOldAge = value;
    }
  }

  public bool DiedOfStarvation
  {
    get => this._directInfoAccess.DiedOfStarvation;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedOfStarvation = value;
    }
  }

  public bool FrozeToDeath
  {
    get => this._directInfoAccess.FrozeToDeath;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.FrozeToDeath = value;
    }
  }

  public bool DiedFromRot
  {
    get => this._directInfoAccess.DiedFromRot;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedFromRot = value;
    }
  }

  public bool DiedFromTwitchChat
  {
    get => this._directInfoAccess.DiedFromTwitchChat;
    set
    {
      if (value)
        this._directInfoAccess.TimeOfDeath = TimeManager.CurrentDay;
      this._directInfoAccess.DiedFromTwitchChat = value;
    }
  }

  public bool BurntToDeath
  {
    get => this._directInfoAccess.BurntToDeath;
    set => this._directInfoAccess.BurntToDeath = value;
  }

  public bool DiedFromOverheating
  {
    get => this._directInfoAccess.DiedFromOverheating;
    set => this._directInfoAccess.DiedFromOverheating = value;
  }

  public bool DiedFromLightning
  {
    get => this._directInfoAccess.DiedFromLightning;
    set => this._directInfoAccess.DiedFromLightning = value;
  }

  public bool DiedFromDeadlyDish
  {
    get => this._directInfoAccess.DiedFromDeadlyDish;
    set => this._directInfoAccess.DiedFromDeadlyDish = value;
  }

  public bool DiedFromMissionary
  {
    get => this._directInfoAccess.DiedFromMissionary;
    set => this._directInfoAccess.DiedFromMissionary = value;
  }

  public bool _leavingCult
  {
    get => this._directInfoAccess.LeavingCult;
    set => this._directInfoAccess.LeavingCult = value;
  }

  public bool LeavingCult
  {
    get => this._leavingCult;
    set
    {
      if (this._leavingCult == value)
        return;
      this._leavingCult = value;
      this.CheckChangeTask();
    }
  }

  public bool CanWork
  {
    get
    {
      return SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard || DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.WorkThroughBlizzard) || this.HasTrait(FollowerTrait.TraitType.FreezeImmune) || this.HasTrait(FollowerTrait.TraitType.Mutated) || this.Info.IsSnowman;
    }
  }

  public bool LeftCult
  {
    get => this.\u003CLeftCult\u003Ek__BackingField;
    set => this.\u003CLeftCult\u003Ek__BackingField = value;
  }

  public FollowerBrain(FollowerInfo info)
  {
    if (info.HomeLocation == FollowerLocation.Church)
      info.HomeLocation = FollowerLocation.Base;
    this.Info = new FollowerBrainInfo(info, this);
    this.Stats = new FollowerBrainStats(info, this);
    this._directInfoAccess = info;
    this.DesiredLocation = this.Location;
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.OnStructureMoved);
    StructureManager.OnStructureUpgraded += new StructureManager.StructureChanged(this.OnStructureUpgraded);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
    FollowerBrainStats.OnExhaustionStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnExhaustionStateChanged);
    FollowerBrainStats.OnDrunkStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnDrunkStateChanged);
    FollowerBrainStats.OnIllnessStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnIllnessStateChanged);
    FollowerBrainStats.OnHappinessStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnHappinessStateChanged);
    FollowerBrainStats.OnSatiationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnSatiationStateChanged);
    FollowerBrainStats.OnStarvationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationStateChanged);
    FollowerBrainStats.OnInjuredStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnInjuredStateChanged);
    FollowerBrainStats.OnRestStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnRestStateChanged);
    FollowerBrainStats.OnMotivatedChanged += new FollowerBrainStats.StatusChangedEvent(this.OnMotivatedChanged);
    FollowerBrainStats.OnStarvationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationChanged);
    StructuresData.OnResearchBegin += new System.Action(this.OnResearchBegin);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    TimeManager.OnScheduleChanged += new System.Action(this.OnScheduleChanged);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
    SeasonsManager.OnBlizzardBegan += new SeasonsManager.WeatherTypeEvent(this.OnBlizzardBegan);
    SeasonsManager.OnBlizzardEnded += new SeasonsManager.WeatherTypeEvent(this.OnBlizzardEnded);
    SeasonsManager.OnWeatherBegan += new SeasonsManager.WeatherTypeEvent(this.OnWeatherBegan);
    SeasonsManager.OnWeatherEnded += new SeasonsManager.WeatherTypeEvent(this.OnWeatherEnded);
    SeasonsManager.OnDefaultWeatherBegan += new SeasonsManager.DeafultWeatherEvent(this.OnDefaultWeatherBegan);
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
    if (!this._directInfoAccess.TraitsSet)
    {
      this.AddTrait(FollowerTrait.GetStartingTrait());
      this.AddTrait(FollowerTrait.GetStartingTrait());
      if ((double) UnityEngine.Random.value <= 0.20000000298023224)
        this.AddTrait(FollowerTrait.GetRareTrait());
      else if ((double) UnityEngine.Random.value <= 0.40000000596046448)
        this.AddTrait(FollowerTrait.GetStartingTrait());
      this._directInfoAccess.TraitsSet = true;
    }
    if (this._directInfoAccess.Outfit == FollowerOutfitType.Rags && !DataManager.Instance.Followers_Recruit.Contains(this._directInfoAccess))
      this._directInfoAccess.Outfit = FollowerOutfitType.Follower;
    else if (this._directInfoAccess.Clothing == FollowerClothingType.None && this._directInfoAccess.ClothingVariant == null)
      this._directInfoAccess.ClothingVariant = "";
    this._directInfoAccess.HasReceivedNecklace = this._directInfoAccess.Necklace != 0;
    if (this.Info.CursedState == Thought.Child)
    {
      if (this.Info.ID == 100000)
        this.HardSwapToTask((FollowerTask) new FollowerTask_ChosenChild());
      else if (this.HasTrait(FollowerTrait.TraitType.Zombie))
        this.HardSwapToTask((FollowerTask) new FollowerTask_ChildZombie(this.Info.ID));
      else
        this.HardSwapToTask((FollowerTask) new FollowerTask_Child(this.Info.ID));
    }
    if (info.ID == 99996 && this.HasTrait(FollowerTrait.TraitType.Unlawful))
      this.RemoveTrait(FollowerTrait.TraitType.Unlawful);
    if (this._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated) && this._directInfoAccess.DayBecameMutated == -1)
      this._directInfoAccess.DayBecameMutated = info.CursedState != Thought.Child ? TimeManager.CurrentDay : TimeManager.CurrentDay + 5;
    if ((double) info.Reeducation != 0.0 || info.CursedState != Thought.Dissenter)
      return;
    this.RemoveCurseState(Thought.Dissenter);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CureDissenter, info.ID);
  }

  public void SeasonsManager_OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    if (this.CurrentTaskType == FollowerTaskType.FindPlaceToDie && this.CurrentTask != null)
    {
      this.FrozeToDeath = false;
      this.CurrentTask.Abort();
    }
    List<Structures_WoolyShack> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_WoolyShack>();
    if (newSeason == SeasonsManager.Season.Winter)
    {
      if (structuresOfType.Count > 0 && structuresOfType[0].ScarfAvailable())
        this.HardSwapToTask((FollowerTask) new FollowerTask_CollectScarf());
      if (this.HasTrait(FollowerTrait.TraitType.WinterExcited))
      {
        this.AddAdoration(FollowerBrain.AdorationActions.WinterExcited, (System.Action) null);
        CultFaithManager.AddThought(Thought.WinterExcited, this.Info.ID);
      }
      this.CheckWinterThoughts();
    }
    else
    {
      if (this.Info.Customisation == FollowerCustomisationType.Scarf)
      {
        if (structuresOfType.Count > 0)
          structuresOfType[0].RemoveScarf();
        this.Info.Customisation = FollowerCustomisationType.None;
        Follower followerById = FollowerManager.FindFollowerByID(this.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
          FollowerBrain.SetFollowerCostume(followerById.Spine.Skeleton, this._directInfoAccess, forceUpdate: true);
      }
      this.CheckNonWinterThoughts();
    }
    this.CheckChangeState();
  }

  public void OnBlizzardBegan(SeasonsManager.WeatherEvent weatherEvent)
  {
    if (FollowerBrainStats.ShouldWork && !this.HasTrait(FollowerTrait.TraitType.Hibernation) && this.CanWork)
      this.AddThought(Thought.Snowplower);
    this.AddRandomThoughtFromList(Thought.Blizzard_1, Thought.Blizzard_2);
    if (this.Info.ID != 100007)
      return;
    this.AddThought(Thought.Yngya_2);
  }

  public void OnBlizzardEnded(SeasonsManager.WeatherEvent weatherEvent)
  {
    if (!FollowerBrainStats.ShouldWork || this.HasTrait(FollowerTrait.TraitType.Hibernation) || !this.CanWork)
      return;
    this.AddThought(Thought.Snowplower);
  }

  public void OnWeatherBegan(SeasonsManager.WeatherEvent weatherEvent) => this.CheckChangeTask();

  public void OnWeatherEnded(SeasonsManager.WeatherEvent weatherEvent)
  {
    if (weatherEvent == SeasonsManager.WeatherEvent.Typhoon && this.Info.CursedState == Thought.Soaking)
      this.RemoveCurseState(Thought.Soaking);
    this.CheckChangeTask();
  }

  public void OnDefaultWeatherBegan(WeatherSystemController.WeatherType weather)
  {
    if (this.Info.ID != 100007 || weather != WeatherSystemController.WeatherType.Snowing)
      return;
    this.AddThought(Thought.Yngya_1);
  }

  public void Cleanup()
  {
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.OnStructureMoved);
    StructureManager.OnStructureUpgraded -= new StructureManager.StructureChanged(this.OnStructureUpgraded);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
    FollowerBrainStats.OnIllnessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnIllnessStateChanged);
    FollowerBrainStats.OnHappinessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnHappinessStateChanged);
    FollowerBrainStats.OnSatiationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnSatiationStateChanged);
    FollowerBrainStats.OnRestStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnRestStateChanged);
    FollowerBrainStats.OnMotivatedChanged -= new FollowerBrainStats.StatusChangedEvent(this.OnMotivatedChanged);
    FollowerBrainStats.OnStarvationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationChanged);
    FollowerBrainStats.OnStarvationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationStateChanged);
    FollowerBrainStats.OnExhaustionStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnExhaustionStateChanged);
    FollowerBrainStats.OnInjuredStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnInjuredStateChanged);
    FollowerBrainStats.OnDrunkStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnDrunkStateChanged);
    StructuresData.OnResearchBegin -= new System.Action(this.OnResearchBegin);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    TimeManager.OnScheduleChanged -= new System.Action(this.OnScheduleChanged);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
    SeasonsManager.OnWeatherBegan -= new SeasonsManager.WeatherTypeEvent(this.OnWeatherBegan);
    SeasonsManager.OnWeatherEnded -= new SeasonsManager.WeatherTypeEvent(this.OnWeatherEnded);
    SeasonsManager.OnBlizzardBegan -= new SeasonsManager.WeatherTypeEvent(this.OnBlizzardBegan);
    SeasonsManager.OnBlizzardEnded -= new SeasonsManager.WeatherTypeEvent(this.OnBlizzardEnded);
    SeasonsManager.OnDefaultWeatherBegan -= new SeasonsManager.DeafultWeatherEvent(this.OnDefaultWeatherBegan);
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
    if (this.CurrentTask != null && this.CurrentTask.State != FollowerTaskState.Done)
      this.CurrentTask.Abort();
    if (!this.Info.SkinName.Contains("Mushroom") || DataManager.Instance.SozoAteMushroomDay != int.MaxValue)
      return;
    DataManager.Instance.SozoAteMushroomDay = TimeManager.CurrentDay;
  }

  public void ResetStats()
  {
    this._directInfoAccess.ResetStats();
    this.Info.CursedState = Thought.None;
    this.RemoveThought(Thought.Dissenter, true);
    this.RemoveThought(Thought.OldAge, true);
    this.RemoveThought(Thought.BecomeStarving, true);
    this.RemoveThought(Thought.Ill, true);
    this.RemoveThought(Thought.Freezing, true);
    this.RemoveThought(Thought.Freezing_2, true);
    this.RemoveThought(Thought.Overheating, true);
    this.Stats.Reeducation = 0.0f;
    this.Stats.Illness = 0.0f;
    this.Stats.Starvation = 0.0f;
    this.Stats.Exhaustion = 0.0f;
    this.Stats.Drunk = 0.0f;
    this.Stats.Freezing = 0.0f;
    this.Stats.Overheating = 0.0f;
    this.Stats.Aflame = 0.0f;
    this.LeavingCult = false;
    this.DiedInPrison = false;
    this.DiedOfIllness = false;
    this.DiedOfInjury = false;
    this.DiedFromMurder = false;
    this.DiedOfOldAge = false;
    this.DiedOfStarvation = false;
    this.FrozeToDeath = false;
    this.DiedFromRot = false;
    this.DiedFromTwitchChat = false;
    this.DiedFromDeadlyDish = false;
    this.DiedFromMissionary = false;
    this.DiedFromOverheating = false;
    this.BurntToDeath = false;
    this.DiedFromLightning = false;
    this._directInfoAccess.TaxEnforcer = false;
    this._directInfoAccess.FaithEnforcer = false;
    this._directInfoAccess.Hat = FollowerHatType.None;
    this._directInfoAccess.DayBecameMutated = -1;
    this._directInfoAccess.SpouseFollowerID = -1;
    this.ClearThought(Thought.OldAge);
    this.ClearThought(Thought.Dissenter);
    this.ClearThought(Thought.Ill);
    this.ClearThought(Thought.BecomeStarving);
    this.ClearThought(Thought.Freezing);
    this.ClearThought(Thought.Freezing_2);
    this.ClearThought(Thought.Overheating);
    this.ClearThought(Thought.Aflame);
    if (this._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Spy))
      this._directInfoAccess.Traits.Remove(FollowerTrait.TraitType.Spy);
    if (this._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.Mutated))
      this._directInfoAccess.DayBecameMutated = TimeManager.CurrentDay;
    TwitchFollowers.SendFollowers();
  }

  public FollowerOutfit CreateOutfit() => new FollowerOutfit(this._directInfoAccess);

  public void OnNewDay()
  {
    this.Stats.PaidTithes = false;
    this.Stats.ReceivedBlessing = false;
    this.Stats.Bribed = false;
    this._directInfoAccess.TaxedToday = false;
    this._directInfoAccess.FaithedToday = false;
    this.Stats.Inspired = false;
    this.Stats.PetDog = false;
    if (!this.Stats.Cuddled && this.Info.CursedState == Thought.Child && TimeManager.CurrentDay - this._directInfoAccess.DayJoined > 1 && FollowerInfo.GetInfoByID(this.Info.ID) != null && this.Info.Age < 18 && this.Info.ID != 100000)
    {
      CultFaithManager.AddThought(Thought.ChildNeglected_0, this.Info.ID);
      this.AddThought((Thought) UnityEngine.Random.Range(397, 401));
    }
    this.Stats.Cuddled = false;
    this.Stats.ScaredTraitInteracted = false;
    this.Stats.Intimidated = false;
    this.Stats.ReeducatedAction = false;
    this.Stats.KissedAction = false;
    this._directInfoAccess.FollowersReeducatedToday.Clear();
    if (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_OnMissionary) && !(this.CurrentTask is FollowerTask_MissionaryComplete))
      this._directInfoAccess.MissionaryExhaustion = Mathf.Clamp(this._directInfoAccess.MissionaryExhaustion - 0.5f, 1f, (float) int.MaxValue);
    if (this.CurrentTask == null)
    {
      if (!this.HasHome)
      {
        if (this.HasTrait(FollowerTrait.TraitType.Materialistic))
          this.AddThought(Thought.SleptOutisdeMaterialisticTrait);
        else
          this.AddThought(Thought.SleptOutisde);
      }
      else
      {
        StructureBrain.TYPES structureTypeById = StructureManager.GetStructureTypeByID(this.HomeID);
        if (this.HasTrait(FollowerTrait.TraitType.Materialistic))
        {
          switch (structureTypeById)
          {
            case StructureBrain.TYPES.BED:
              this.AddThought(Thought.SleptHouse1MaterialisticTrait);
              break;
            case StructureBrain.TYPES.BED_2:
              this.AddThought(Thought.SleptHouse2MaterialisticTrait);
              break;
            case StructureBrain.TYPES.BED_3:
              this.AddThought(Thought.SleptHouse3MaterialisticTrait);
              break;
            case StructureBrain.TYPES.SHARED_HOUSE:
              this.AddThought(Thought.SleptHouse3MaterialisticTrait);
              break;
          }
        }
        else
        {
          switch (structureTypeById)
          {
            case StructureBrain.TYPES.BED:
              this.AddThought(Thought.SleptHouse1);
              break;
            case StructureBrain.TYPES.BED_2:
              this.AddThought(Thought.SleptHouse2);
              break;
            case StructureBrain.TYPES.BED_3:
              this.AddThought(Thought.SleptHouse3);
              break;
            case StructureBrain.TYPES.SHARED_HOUSE:
              this.AddThought(Thought.SleptHouse3);
              break;
          }
        }
      }
    }
    if (this.Info.Age++ >= this.Info.LifeExpectancy && this.Info.CursedState != Thought.OldAge && this.Info.CursedState != Thought.Zombie && !this.HasTrait(FollowerTrait.TraitType.Immortal) && DataManager.Instance.OnboardedOldFollower && (this.CurrentTask == null || !this.CurrentTask.BlockThoughts))
    {
      if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToReachOldAge < 600.0 / (double) DifficultyManager.GetTimeBetweenOldAgeMultiplier())
        return;
      this.MakeOld();
    }
    if (this.Info.CursedState == Thought.Child && this.Info.Age < 18)
      this.Info.Age += 3;
    if (this._directInfoAccess.WakeUpDay == TimeManager.CurrentDay - 1 && !FollowerManager.FollowerLocked(this.Info.ID))
      this.MakeExhausted();
    if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.Info.ID))
    {
      if (this.HasTrait(FollowerTrait.TraitType.LockedLoyal) && !this.CanLevelUp())
      {
        this.AddAdoration(FollowerBrain.AdorationActions.LockedLoyal, (System.Action) null);
        this.AddThought(Thought.LockedUpHappy);
      }
      if (this.HasTrait(FollowerTrait.TraitType.Masochistic) && !this.CanGiveSin())
      {
        this.AddPleasure(FollowerBrain.PleasureActions.Masochistic);
        this.AddThought(Thought.LockedUpHappy);
      }
    }
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || TimeManager.CurrentDay != SeasonsManager.SeasonTimestamp || !this.HasTrait(FollowerTrait.TraitType.Hibernation))
      return;
    this.AddThought(Thought.BeforeHibernation);
    this.Info.Special = FollowerSpecialType.Fat;
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    Follower followerById = FollowerManager.FindFollowerByID(this.Info.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
      return;
    FollowerBrain.SetFollowerCostume(followerById.Spine.Skeleton, this._directInfoAccess, forceUpdate: true);
  }

  public void Tick(float deltaGameTime)
  {
    if (this._currentState == null || this._currentState.Type == FollowerStateType.Default)
      this.CheckChangeState();
    if (this.CurrentTask == null)
      this.CheckChangeTask();
    if (this.Info.ID == 100006 && this.Info.CursedState != Thought.Child && this.CurrentTask != null && this.CurrentTask.Type != FollowerTaskType.ManualControl && DataManager.Instance.HasMidasHiding)
      this.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    else if (DataManager.Instance.Followers_TraitManipulating_IDs.Contains(this.Info.ID) && this.CurrentTaskType != FollowerTaskType.TraitManipulating)
    {
      this.HardSwapToTask((FollowerTask) new FollowerTask_TraitManipulating(this.Info.ID));
    }
    else
    {
      if ((double) this.Stats.Freezing > 0.0 && SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      {
        this.Stats.Freezing = 0.0f;
        this.CheckChangeState();
        this.CheckChangeTask();
      }
      FollowerState currentState = this._currentState;
      if (currentState != null && currentState.Type == FollowerStateType.Cold && !this.CanGetCold())
        this.CheckChangeState();
      this.DiedFromRot = this._directInfoAccess.DayBecameMutated != -1 && TimeManager.CurrentDay - this._directInfoAccess.DayBecameMutated >= FollowerBrainInfo.DAYS_TILL_ROT_DEATH && this.HasTrait(FollowerTrait.TraitType.Mutated) && this.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_Weird && this.Info.CursedState != Thought.Child;
      this.UpdateThoughts();
      if ((double) this.Stats.Bathroom < (double) this.Stats.TargetBathroom && !TimeManager.IsNight)
      {
        float num = 0.166666672f;
        this.Stats.Bathroom += deltaGameTime * num * this.Stats.BathroomFillRate;
      }
      if ((double) this.Stats.Reeducation != 0.0 && !this.LeavingCult && this.CurrentTaskType != FollowerTaskType.Imprisoned && this.Info.CursedState == Thought.Dissenter)
      {
        if ((double) this.Stats.Reeducation > 0.0 && !this.Stats.GivenDissentWarning)
        {
          float itemQuantity = (float) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
          this.Stats.DissentGold = Mathf.Floor(UnityEngine.Random.Range(itemQuantity * 0.15f, itemQuantity * 0.25f));
          if ((UnityEngine.Object) NotificationCentre.Instance != (UnityEngine.Object) null)
            NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/DissenterLeavingTomorrow", this.Info.Name, this.Stats.DissentGold.ToString());
          this.Stats.GivenDissentWarning = true;
        }
        if ((double) this.Stats.Reeducation >= 100.0 && PlayerFarming.Location == FollowerLocation.Base && DataManager.Instance.OnboardingFinished)
          this.LeavingCult = true;
      }
      if (this.HasTrait(FollowerTrait.TraitType.Spy) && TimeManager.CurrentDay > DataManager.Instance.SpyJoinedDay + 5 && PlayerFarming.Location == FollowerLocation.Base)
        this.LeavingCult = true;
      if (this.CurrentTask != null)
        this.CurrentTask.Tick(deltaGameTime);
      if ((double) (this.timer += Time.deltaTime) > 2.0)
      {
        this.timer = 0.0f;
        this.RandomChanceEvents();
      }
      if (FollowerManager.FollowerLocked(this.Info.ID) || !this._directInfoAccess.IsSnowman || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || PlayerFarming.Location == FollowerLocation.Base || this._directInfoAccess.Necklace == InventoryItem.ITEM_TYPE.Necklace_Winter || this.HasTrait(FollowerTrait.TraitType.InfusibleSnowman))
        return;
      this.Die(NotificationCentre.NotificationType.MeltedToDeath);
    }
  }

  public void RandomChanceEvents()
  {
  }

  public string GetThoughtString(float SubTextSize)
  {
    this.ReturnString = "";
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      this.ModifierTotal = 0.0f;
      int num = -1;
      while (++num < thought.Quantity)
        this.ModifierTotal += num <= 0 ? thought.Modifier : (float) thought.StackModifier;
      this.ReturnString = $"{this.ReturnString}{((double) this.ModifierTotal >= 0.0 ? "<color=green><b>+" : "<color=red><b>")}{this.ModifierTotal.ToString()}</b></color> {FollowerThoughts.GetLocalisedName(thought.ThoughtType, this.Info.ID)}{(thought.Quantity > 1 ? $"{$" <size={SubTextSize}>(x"}{thought.Quantity.ToString()})" : "")}</size>\n{$"<size={SubTextSize}><i>"}{FollowerThoughts.GetLocalisedDescription(thought.ThoughtType, thought.FollowerID)}</i></size>\n\n";
    }
    return this.ReturnString;
  }

  public static void AddMarriageThoughts()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.MarriedToLeader)
        followerBrainList.Add(allBrain);
    }
    int count = followerBrainList.Count;
    Debug.Log((object) ("MarriedBrains.Count: " + count.ToString()));
    count = followerBrainList.Count;
    switch (count)
    {
      case 0:
        break;
      case 1:
        followerBrainList[0].AddThought(Thought.MarriedToLeader);
        break;
      default:
        Debug.Log((object) "Add married thoughts!");
        int index1 = -1;
        while (++index1 < followerBrainList.Count)
        {
          if (!followerBrainList[index1].HasTrait(FollowerTrait.TraitType.Polyamory))
          {
            int index2 = -1;
            while (++index2 < followerBrainList.Count)
            {
              if (index1 != index2)
              {
                IDAndRelationship relationship = followerBrainList[index1].Info.GetOrCreateRelationship(followerBrainList[index2].Info.ID);
                if (relationship.Relationship > -11)
                {
                  relationship.Relationship = -11;
                  relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Enemies;
                }
              }
            }
            ThoughtData data = FollowerThoughts.GetData(Thought.MultiMarriedToLeader);
            data.Init();
            data.Modifier = (float) FollowerBrain.MultipleSpouseFaith[Mathf.Clamp(followerBrainList.Count, 0, FollowerBrain.MultipleSpouseFaith.Count - 1)];
            followerBrainList[index1].AddThought(data);
          }
        }
        break;
    }
  }

  public void AddThought(ThoughtData thought, bool forced = false)
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockThoughts && !forced)
      return;
    float happiness = this.Stats.Happiness;
    if (this.OnNewThought != null)
      this.OnNewThought(thought.Modifier);
    int index = -1;
    while (++index < this.Stats.Thoughts.Count)
    {
      ThoughtData thought1 = this.Stats.Thoughts[index];
      if (thought1.ThoughtGroup == thought.ThoughtGroup && thought1.Stacking <= 0)
      {
        Debug.Log((object) $"REPLACE! {thought.ThoughtGroup.ToString()}  {thought.ThoughtType.ToString()}");
        this.Stats.Thoughts[index] = thought;
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
      if (thought1.ThoughtType == thought.ThoughtType && thought1.Stacking > 0)
      {
        if (thought1.Quantity < thought1.Stacking)
        {
          ++thought1.Quantity;
        }
        else
        {
          thought1.CoolDowns.RemoveAt(0);
          thought1.TimeStarted.RemoveAt(0);
        }
        thought1.CoolDowns.Add(thought.Duration);
        thought1.TimeStarted.Add(TimeManager.TotalElapsedGameTime);
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
    }
    this.Stats.Thoughts.Add(thought);
    this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
  }

  public void AddThought(Thought thought, bool InsetAtZero = false, bool forced = false)
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockThoughts && !forced)
      return;
    float happiness = this.Stats.Happiness;
    ThoughtData data = FollowerThoughts.GetData(thought);
    data.Init();
    if (data != null && this.OnNewThought != null)
      this.OnNewThought(data.Modifier);
    int index = -1;
    while (++index < this.Stats.Thoughts.Count)
    {
      ThoughtData thought1 = this.Stats.Thoughts[index];
      if (thought1.ThoughtGroup == data.ThoughtGroup && thought1.Stacking <= 0)
      {
        this.Stats.Thoughts[index] = data;
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
      if (thought1.ThoughtType == thought && thought1.Stacking > 0)
      {
        if (thought1.Quantity < thought1.Stacking)
        {
          ++thought1.Quantity;
        }
        else
        {
          thought1.CoolDowns.RemoveAt(0);
          thought1.TimeStarted.RemoveAt(0);
        }
        thought1.CoolDowns.Add(data.Duration);
        thought1.TimeStarted.Add(TimeManager.TotalElapsedGameTime);
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
    }
    if (InsetAtZero)
      this.Stats.Thoughts.Insert(0, data);
    else
      this.Stats.Thoughts.Add(data);
    this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
  }

  public bool HasThought(Thought thought)
  {
    foreach (ThoughtData thought1 in this.Stats.Thoughts)
    {
      if (thought1.ThoughtType == thought)
        return true;
    }
    return false;
  }

  public void UpdateThoughts()
  {
    float happiness = this.Stats.Happiness;
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      if ((double) thought.Duration != -1.0 && (double) TimeManager.TotalElapsedGameTime - (double) thought.TimeStarted[0] > (double) thought.CoolDowns[0])
      {
        if (thought.Quantity > 1)
        {
          --thought.Quantity;
          thought.CoolDowns.RemoveAt(0);
          thought.TimeStarted.RemoveAt(0);
        }
        else
          this.Stats.Thoughts.Remove(thought);
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        switch (thought.ThoughtType)
        {
          case Thought.Brainwashed:
            float num = UnityEngine.Random.value;
            if ((double) num <= 0.20000000298023224)
              this.AddThought(Thought.BrainwashedHangOverLight);
            else if ((double) num <= 0.20000000298023224)
              this.AddThought(Thought.BrainwashedHangOverMild);
            else
              this.AddThought(Thought.BrainwashedHangOverPounding);
            if ((double) UnityEngine.Random.value <= (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.MushroomBanned) ? 0.5 : 0.699999988079071))
              return;
            DataManager.Instance.LastFollowerToBecomeIll = TimeManager.TotalElapsedGameTime;
            this.MakeSick();
            return;
          case Thought.Ill:
            Debug.Log((object) "REMOVE ILL THOUGHT AND DIE");
            this.CheckChangeTask();
            return;
          case Thought.FeelingUnwell:
            if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToBecomeIll < 600.0 / (double) DifficultyManager.GetTimeBetweenIllnessMultiplier())
              return;
            DataManager.Instance.LastFollowerToBecomeIll = TimeManager.TotalElapsedGameTime;
            this.ApplyCurseState(Thought.Ill);
            return;
          case Thought.OldAge:
            if (!this.HasTrait(FollowerTrait.TraitType.Immortal) && this.Info.CursedState == Thought.OldAge)
              this.DiedOfOldAge = DataManager.Instance.OnboardingFinished || TimeManager.CurrentDay > 10;
            this.CheckChangeTask();
            return;
          default:
            return;
        }
      }
    }
  }

  public void RemoveThought(Thought Thought, bool RemoveAllStack)
  {
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      if (thought.ThoughtType == Thought)
      {
        thought.Duration = 0.0f;
        thought.CoolDowns[0] = 0.0f;
        if (RemoveAllStack)
          thought.Quantity = 1;
      }
    }
  }

  public void ClearThought(Thought thought)
  {
    for (int index = this.Stats.Thoughts.Count - 1; index >= 0; --index)
    {
      if (this.Stats.Thoughts[index].ThoughtType == thought)
        this.Stats.Thoughts.RemoveAt(index);
    }
  }

  public ThoughtData GetThought(Thought Thought)
  {
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      if (thought.ThoughtType == Thought)
        return thought;
    }
    return (ThoughtData) null;
  }

  public bool HasTrait(FollowerTrait.TraitType TraitType)
  {
    return this._directInfoAccess != null && this._directInfoAccess.Traits != null && this._directInfoAccess.Traits.Contains(TraitType) || DataManager.Instance.CultTraits.Contains(TraitType);
  }

  public void AddTrait(FollowerTrait.TraitType TraitType, bool showNotification = false)
  {
    if (!this.CanAddTrait(TraitType))
      return;
    FollowerTrait.RemoveExclusiveTraits(this, TraitType);
    foreach (int cultTrait in DataManager.Instance.CultTraits)
      FollowerTrait.RemoveExclusiveTraits(this, TraitType);
    switch (TraitType)
    {
      case FollowerTrait.TraitType.DesensitisedToDeath:
        this.RemoveThought(Thought.EnemyDied, true);
        this.RemoveThought(Thought.FriendDied, true);
        this.RemoveThought(Thought.LoverDied, true);
        this.RemoveThought(Thought.StrangerDied, true);
        this.RemoveThought(Thought.GrievedAtUnburiedBody, true);
        this.RemoveThought(Thought.GrievedAtRottenUnburiedBody, true);
        break;
      case FollowerTrait.TraitType.FearOfDeath:
        this.RemoveThought(Thought.EnemyDied, true);
        this.RemoveThought(Thought.FriendDied, true);
        this.RemoveThought(Thought.LoverDied, true);
        this.RemoveThought(Thought.StrangerDied, true);
        this.RemoveThought(Thought.GrievedAtUnburiedBody, true);
        this.RemoveThought(Thought.GrievedAtRottenUnburiedBody, true);
        break;
      case FollowerTrait.TraitType.Cannibal:
        this.RemoveThought(Thought.AteFollowerMeal, true);
        this.RemoveThought(Thought.AteRottenFollowerMeal, true);
        break;
      case FollowerTrait.TraitType.Disciplinarian:
        this.RemoveThought(Thought.InnocentImprisoned, true);
        this.RemoveThought(Thought.InnocentImprisonedSleeping, true);
        break;
      case FollowerTrait.TraitType.Libertarian:
        this.RemoveThought(Thought.InnocentImprisoned, true);
        this.RemoveThought(Thought.InnocentImprisonedSleeping, true);
        break;
      case FollowerTrait.TraitType.SacrificeEnthusiast:
        this.RemoveThought(Thought.CultMemberWasSacrificed, true);
        this.RemoveThought(Thought.CultMemberWasSacrificedAgainstSacrificeTrait, true);
        break;
      case FollowerTrait.TraitType.AgainstSacrifice:
        this.RemoveThought(Thought.CultMemberWasSacrificed, true);
        this.RemoveThought(Thought.CultMemberWasSacrificedSacrificeEnthusiastTrait, true);
        break;
      case FollowerTrait.TraitType.Mutated:
        this.AddRandomThoughtFromList(Thought.Mutated_1, Thought.Mutated_2);
        break;
      case FollowerTrait.TraitType.HappilyWidowed:
        this.AddThought(Thought.HappilyWidowed);
        break;
      case FollowerTrait.TraitType.GrievingWidow:
        this.AddThought(Thought.GrievingWidow);
        break;
    }
    this._directInfoAccess.Traits.Add(TraitType);
    if (TraitType == FollowerTrait.TraitType.Mutated)
      this._directInfoAccess.DayBecameMutated = this._directInfoAccess.CursedState != Thought.Child ? TimeManager.CurrentDay : TimeManager.CurrentDay + 5;
    if (TraitType == FollowerTrait.TraitType.WarmBlooded)
    {
      this.RemoveCurseState(Thought.Freezing);
      this.CheckChangeState();
    }
    if (showNotification)
    {
      float faithDelta = FollowerTrait.IsPositiveTrait(TraitType) ? 1f : 0.0f;
      NotificationCentre.Instance.PlayFaithNotification("Notifications/GainedTrait", faithDelta, NotificationBase.Flair.None, this.Info.ID, this.Info.Name, FollowerTrait.GetLocalizedTitle(TraitType));
    }
    TwitchFollowers.SendFollowers();
  }

  public void RemoveTrait(FollowerTrait.TraitType TraitType, bool showNotification = false)
  {
    if (!this._directInfoAccess.Traits.Contains(TraitType))
      return;
    this._directInfoAccess.Traits.Remove(TraitType);
    TwitchFollowers.SendFollowers();
    if (!showNotification)
      return;
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/LostTrait", this.Info.Name, FollowerTrait.GetLocalizedTitle(TraitType));
  }

  public bool IsTraitImmune(FollowerTrait.TraitType TraitType)
  {
    return TraitType == FollowerTrait.TraitType.Mutated && this.HasTrait(FollowerTrait.TraitType.MutatedImmune) || TraitType == FollowerTrait.TraitType.MutatedVisual && this.HasTrait(FollowerTrait.TraitType.MutatedImmune);
  }

  public bool CanAddTrait(FollowerTrait.TraitType TraitType)
  {
    return !this.Info.IsSnowman && !this.IsTraitImmune(TraitType) && (!this.Info.SkinName.Contains("Webber") || TraitType != FollowerTrait.TraitType.AnimalLover) && (this.Info.ID != 100007 || TraitType != FollowerTrait.TraitType.Celibate) && !this.Info.Traits.Contains(TraitType) && (TraitType != FollowerTrait.TraitType.Celibate || !this.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_1)) && (TraitType != FollowerTrait.TraitType.Celibate || !this.Info.SkinName.Contains("Bug") || !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.DepositFollower));
  }

  public void ClearMarriageTraits(bool showNotification = false)
  {
    this.RemoveTrait(FollowerTrait.TraitType.MarriedDevoted, showNotification);
    this.RemoveTrait(FollowerTrait.TraitType.MarriedHappily, showNotification);
    this.RemoveTrait(FollowerTrait.TraitType.MarriedJealous, showNotification);
    this.RemoveTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous, showNotification);
    this.RemoveTrait(FollowerTrait.TraitType.MarriedUnhappily, showNotification);
  }

  public bool ThoughtExists(Thought thought)
  {
    foreach (ThoughtData thought1 in this.Stats.Thoughts)
    {
      if (thought1.ThoughtType == thought)
        return true;
    }
    return false;
  }

  public bool ThoughtGroupExists(Thought thought)
  {
    foreach (ThoughtData thought1 in this.Stats.Thoughts)
    {
      if (thought1.ThoughtGroup == thought)
        return true;
    }
    return false;
  }

  public void Die(
    NotificationCentre.NotificationType deathNotificationType)
  {
    if (this.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Deaths_Door && PlayerFarming.Location != FollowerLocation.Base && deathNotificationType != NotificationCentre.NotificationType.Ascended && deathNotificationType != NotificationCentre.NotificationType.DiedOnMissionary && deathNotificationType != NotificationCentre.NotificationType.MurderedByYou && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEaten && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEatenBySozo && deathNotificationType != NotificationCentre.NotificationType.MeltedToDeath && deathNotificationType != NotificationCentre.NotificationType.KilledInAFightPit && deathNotificationType != NotificationCentre.NotificationType.SacrificeFollower && deathNotificationType != NotificationCentre.NotificationType.BurntToDeath && deathNotificationType != NotificationCentre.NotificationType.SacrificedAwayFromCult)
    {
      this.ResetStats();
      if (this.Info.Age > this.Info.LifeExpectancy)
        this.Info.LifeExpectancy = this.Info.Age + UnityEngine.Random.Range(20, 30);
      else
        this.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
      this.Info.Necklace = InventoryItem.ITEM_TYPE.NONE;
    }
    else
    {
      this.Cleanup();
      this.ClearDwelling();
      this._directInfoAccess.DLCPets.Clear();
      FollowerManager.FollowerDie(this.Info.ID, deathNotificationType);
    }
  }

  public void Leave(
    NotificationCentre.NotificationType leaveNotificationType)
  {
    this._directInfoAccess.LeftCultDay = TimeManager.CurrentDay;
    FollowerManager.FollowerLeave(this.Info.ID, leaveNotificationType);
    Debug.Log((object) "Follower Brain Leave()");
    switch (leaveNotificationType)
    {
      case NotificationCentre.NotificationType.LeaveCultUnhappy:
        DataManager.Instance.Followers_Dissented.Add(this._directInfoAccess);
        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/DissenterLeftCult", this.Info.Name, this.Stats.DissentGold.ToString());
        break;
      case NotificationCentre.NotificationType.BecamePossessed:
        DataManager.Instance.Followers_Possessed.Add(this._directInfoAccess);
        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/PossessedLeftCult", this.Info.Name);
        break;
      case NotificationCentre.NotificationType.LeaveCultSpy:
        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/SpyLeftCult", this.Info.Name, this.Stats.DissentGold.ToString());
        break;
    }
    this.Cleanup();
    this.ClearDwelling();
    FollowerManager.RemoveFollower(this.Info.ID);
    FollowerManager.RemoveFollowerBrain(this.Info.ID);
  }

  public void AddAdoration(FollowerBrain.AdorationActions Action, System.Action Callback)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this.Info.ID);
    if (this.Info.XPLevel <= 1)
    {
      switch (Action)
      {
        case FollowerBrain.AdorationActions.Sermon:
          Action = FollowerBrain.AdorationActions.SermonLvl1;
          break;
        case FollowerBrain.AdorationActions.Bless:
          Action = FollowerBrain.AdorationActions.BlessLvl1;
          break;
      }
    }
    this.AddAdoration(followerById, Action, Callback);
  }

  public void AddAdoration(
    Follower follower,
    FollowerBrain.AdorationActions Action,
    System.Action Callback)
  {
    int addorationToAdd = this.GetAddorationToAdd(Action);
    if (this.Stats.HasLevelledUp || !DataManager.Instance.ShowLoyaltyBars || DataManager.Instance.Followers_Recruit.Contains(this._directInfoAccess) || this._directInfoAccess.IsSnowman || this.HasTrait(FollowerTrait.TraitType.Mutated))
    {
      if (Callback == null)
        return;
      Callback();
    }
    else
    {
      float adoration = this.Stats.Adoration;
      this.Stats.Adoration += (float) addorationToAdd;
      if ((bool) (UnityEngine.Object) follower && follower.gameObject.activeInHierarchy)
      {
        follower.AdorationUI.BarController.SetBarSize(adoration / this.Stats.MAX_ADORATION, false, true);
        follower.StartCoroutine((IEnumerator) this.AddAdorationIE(follower, Action, Callback));
      }
      else
      {
        if (Callback == null)
          return;
        Callback();
      }
    }
  }

  public IEnumerator AddAdorationIE(
    Follower follower,
    FollowerBrain.AdorationActions Action,
    System.Action Callback)
  {
    yield return (object) follower.StartCoroutine((IEnumerator) follower.AdorationUI.IncreaseAdorationIE());
    if ((double) this.Stats.Adoration >= (double) this.Stats.MAX_ADORATION)
      yield return (object) follower.StartCoroutine((IEnumerator) follower.LevelUpRoutine());
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public bool GetWillLevelUp(FollowerBrain.AdorationActions Action)
  {
    return (double) this.Stats.Adoration + (double) this.GetAddorationToAdd(Action) >= (double) this.Stats.MAX_ADORATION;
  }

  public int GetAddorationToAdd(FollowerBrain.AdorationActions Action)
  {
    int adorationsAndAction = FollowerBrain.AdorationsAndActions[Action];
    if (this.HasTrait(FollowerTrait.TraitType.Cynical))
      adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 0.85f);
    else if (this.HasTrait(FollowerTrait.TraitType.Gullible))
      adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 1.15f);
    if (this.HasTrait(FollowerTrait.TraitType.Nudist) && (this.Info.Clothing == FollowerClothingType.Naked || FollowerBrainStats.IsNudism))
      adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 1.5f);
    if (this.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Loyalty)
      adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 1.25f);
    if (this._directInfoAccess.BornInCult)
      adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 2f);
    return adorationsAndAction;
  }

  public bool IsMaxLevel() => this.Info.XPLevel >= 10;

  public void BecomeDisciple()
  {
    this.AddTrait(FollowerTrait.TraitType.Disciple);
    this.Info.IsDisciple = true;
  }

  public void GetXP(float Delta)
  {
  }

  public void LevelDown()
  {
    --this.Info.XPLevel;
    this.RemoveTrait(FollowerTrait.TraitType.Disciple);
  }

  public float DevotionToGive
  {
    get
    {
      float num = Mathf.Max(1f, 1f * (float) ((double) ((float) (1.0 + (this.HasTrait(FollowerTrait.TraitType.Faithful) ? 0.15000000596046448 : 0.0) + (!this.HasTrait(FollowerTrait.TraitType.Fashionable) || !TailorManager.GetClothingData(this.Info.Clothing).SpecialClothing ? 0.0 : 0.20000000298023224) - (this.HasTrait(FollowerTrait.TraitType.Faithless) ? 0.15000000596046448 : 0.0) + (this.HasTrait(FollowerTrait.TraitType.MushroomBanned) ? 0.05000000074505806 : 0.0) + (this.ThoughtExists(Thought.Intimidated) ? 0.10000000149011612 : 0.0) - (this.HasTrait(FollowerTrait.TraitType.Lazy) ? 0.10000000149011612 : 0.0)) + CultFaithManager.CurrentFaith / 500f) + (FollowerBrainStats.IsEnlightened ? 0.25 : 0.0) + (this.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_1 ? 0.15000000596046448 : 0.0) + (this.HasTrait(FollowerTrait.TraitType.MarriedHappily) ? 0.20000000298023224 : 0.0) - (this.HasTrait(FollowerTrait.TraitType.MarriedUnhappily) ? 0.20000000298023224 : 0.0)));
      return this.HasTrait(FollowerTrait.TraitType.Spy) ? 0.0f : num;
    }
  }

  public float ResourceHarvestingMultiplier
  {
    get
    {
      return Mathf.Max(1f, 1f * (float) (1.0 + (this.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_4 ? 0.25 : 0.0)));
    }
  }

  public bool CheckForInteraction(FollowerBrain otherBrain, float distance)
  {
    if ((double) distance < 6.0 && (double) this.Stats.Social <= 0.0 && (this.CurrentTask == null || !this.CurrentTask.BlockSocial) && this.Info.CursedState == Thought.None && otherBrain.Info.CursedState == Thought.None && (double) otherBrain.Stats.Social <= 0.0 && (otherBrain.CurrentTask == null || !otherBrain.CurrentTask.BlockReactTasks && !otherBrain.CurrentTask.BlockTaskChanges && !otherBrain.CurrentTask.BlockSocial) && otherBrain.CurrentTask != null)
      this.HardSwapToTask((FollowerTask) new FollowerTask_Chat(otherBrain.Info.ID, true));
    bool flag1;
    bool flag2;
    bool flag3;
    if (TimeManager.CurrentPhase != DayPhase.Night)
    {
      if ((double) distance < 6.0)
      {
        if (this.Info.FaithEnforcer && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) > 360.0 && !otherBrain._directInfoAccess.FaithedToday && (double) Vector3.Distance(this.LastPosition, otherBrain.LastPosition) < 5.0 && (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_FaithEnforce) && !(this.CurrentTask is FollowerTask_ManualControl)))
        {
          // ISSUE: explicit reference operation
          ref int local1 = @otherBrain.Info.ID;
          flag1 = false;
          ref bool local2 = ref flag1;
          flag2 = false;
          ref bool local3 = ref flag2;
          // ISSUE: explicit reference operation
          ref bool local4 = @false;
          // ISSUE: explicit reference operation
          ref bool local5 = @true;
          flag3 = false;
          ref bool local6 = ref flag3;
          if (!FollowerManager.FollowerLocked(in local1, in local2, in local3, in local4, in local5, in local6))
          {
            StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
            this.HardSwapToTask((FollowerTask) new FollowerTask_FaithEnforce(otherBrain));
            return true;
          }
        }
        if (this.Info.TaxEnforcer && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) > 360.0 && !otherBrain._directInfoAccess.TaxedToday && (double) Vector3.Distance(this.LastPosition, otherBrain.LastPosition) < 5.0 && (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_TaxEnforce) && !(this.CurrentTask is FollowerTask_ManualControl)))
        {
          // ISSUE: explicit reference operation
          ref int local7 = @otherBrain.Info.ID;
          flag1 = false;
          ref bool local8 = ref flag1;
          flag2 = false;
          ref bool local9 = ref flag2;
          // ISSUE: explicit reference operation
          ref bool local10 = @false;
          // ISSUE: explicit reference operation
          ref bool local11 = @true;
          flag3 = false;
          ref bool local12 = ref flag3;
          if (!FollowerManager.FollowerLocked(in local7, in local8, in local9, in local10, in local11, in local12))
          {
            StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
            this.HardSwapToTask((FollowerTask) new FollowerTask_TaxEnforce(otherBrain));
            return true;
          }
        }
        if (this.Location == FollowerLocation.Base)
        {
          if ((double) UnityEngine.Random.value < 0.5 && this.Info.ID == 99996 && otherBrain.Info.SkinName == "Mushroom" && this.CurrentTaskType != FollowerTaskType.FightFollower && this.CurrentTaskType != FollowerTaskType.ChaseFollower && !DataManager.Instance.SozoNoLongerBrainwashed && !ObjectiveManager.GroupExists("Objectives/GroupTitle/SozoStory"))
          {
            this.HardSwapToTask((FollowerTask) new FollowerTask_ChaseFollower(otherBrain.Info.ID, true, float.MaxValue));
            return true;
          }
          if ((double) UnityEngine.Random.value < 0.5 && this.Info.CursedState == Thought.Child && this.HasTrait(FollowerTrait.TraitType.Zombie) && otherBrain.Info.CursedState == Thought.Child && !otherBrain.HasTrait(FollowerTrait.TraitType.Zombie) && this.CurrentTaskType != FollowerTaskType.ChaseFollower)
          {
            this.HardSwapToTask((FollowerTask) new FollowerTask_BabyZombieChaseFollower(otherBrain.Info.ID, true));
            return true;
          }
        }
      }
      if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
      {
        FollowerTask currentTask1 = this.CurrentTask;
        if ((currentTask1 != null ? (currentTask1.BlockReactTasks ? 1 : 0) : 0) == 0 && (double) distance < 6.0 && (double) UnityEngine.Random.value < 0.0099999997764825821 && this.CurrentTaskType != FollowerTaskType.ManualControl && (double) this.GetTimeSinceTask(FollowerTaskType.ReactSnow) > 40.0)
        {
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactSnow());
          return true;
        }
        FollowerTask currentTask2 = this.CurrentTask;
        if ((currentTask2 != null ? (currentTask2.BlockTaskChanges ? 1 : 0) : 0) == 0 && this.Location == FollowerLocation.Base && (double) UnityEngine.Random.value < 1.0 / 400.0 && (double) this.GetTimeSinceTask(FollowerTaskType.SnowballFight) > 60.0)
        {
          this.HardSwapToTask((FollowerTask) new FollowerTask_SnowballFight());
          return true;
        }
      }
    }
    bool flag4;
    bool flag5;
    if (this.Location == FollowerLocation.Base)
    {
      bool flag6;
      bool flag7;
      if ((double) distance < 2.0)
      {
        if (!this.Info.HasTrait(FollowerTrait.TraitType.Blind))
        {
          if (otherBrain.Info.HasTrait(FollowerTrait.TraitType.Blind) && (double) UnityEngine.Random.value < 0.10000000149011612)
          {
            // ISSUE: explicit reference operation
            ref int local13 = @this.Info.ID;
            flag1 = false;
            ref bool local14 = ref flag1;
            flag2 = false;
            ref bool local15 = ref flag2;
            // ISSUE: explicit reference operation
            ref bool local16 = @false;
            // ISSUE: explicit reference operation
            ref bool local17 = @true;
            flag3 = false;
            ref bool local18 = ref flag3;
            if (!FollowerManager.FollowerLocked(in local13, in local14, in local15, in local16, in local17, in local18))
            {
              // ISSUE: explicit reference operation
              ref int local19 = @otherBrain.Info.ID;
              flag4 = false;
              ref bool local20 = ref flag4;
              flag6 = false;
              ref bool local21 = ref flag6;
              // ISSUE: explicit reference operation
              ref bool local22 = @false;
              // ISSUE: explicit reference operation
              ref bool local23 = @true;
              flag7 = false;
              ref bool local24 = ref flag7;
              if (FollowerManager.FollowerLocked(in local19, in local20, in local21, in local22, in local23, in local24) || PlayerFarming.Location != FollowerLocation.Base)
                goto label_29;
            }
            else
              goto label_29;
          }
          else
            goto label_29;
        }
        if ((double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastFollowerBump)
        {
          Follower followerById = FollowerManager.FindFollowerByID(otherBrain.Info.ID);
          if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
          {
            this.HardSwapToTask((FollowerTask) new FollowerTask_Bump(followerById, true));
            return true;
          }
        }
      }
label_29:
      if ((double) distance < 2.5 && this.Info.HasTrait(FollowerTrait.TraitType.ChosenOne))
      {
        // ISSUE: explicit reference operation
        ref int local25 = @otherBrain.Info.ID;
        flag1 = false;
        ref bool local26 = ref flag1;
        flag2 = false;
        ref bool local27 = ref flag2;
        // ISSUE: explicit reference operation
        ref bool local28 = @false;
        // ISSUE: explicit reference operation
        ref bool local29 = @true;
        flag3 = false;
        ref bool local30 = ref flag3;
        if (!FollowerManager.FollowerLocked(in local25, in local26, in local27, in local28, in local29, in local30) && !otherBrain.Stats.ReceivedBlessing)
        {
          Follower followerById = FollowerManager.FindFollowerByID(otherBrain.Info.ID);
          if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
            followerById.BlessFollower(this.LastPosition);
          return false;
        }
      }
      if ((double) distance < 2.0 && (double) UnityEngine.Random.value < 0.0099999997764825821 && !this.HasTrait(FollowerTrait.TraitType.Blind) && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) > 600.0 && TailorManager.GetClothingData(otherBrain.Info.Clothing).SpecialClothing && !this.IsJealousSpouse(otherBrain) && (double) this.GetTimeSinceTask(FollowerTaskType.ReactClothing) > 120.0)
      {
        StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
        this.HardSwapToTask((FollowerTask) new FollowerTask_ReactClothing(otherBrain.Info.ID));
        return true;
      }
      if ((double) distance < 8.0 && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.FollowerKnucklebonesMatch > 600.0 && Interaction_KnucklebonesBuilding.KnuckleboneBuildings.Count > 0 && otherBrain.Info.Age > 14 && (this.CurrentTask == null || !this.CurrentTask.BlockSocial) && (otherBrain.CurrentTask == null || !otherBrain.CurrentTask.BlockSocial && !otherBrain.CurrentTask.BlockTaskChanges))
      {
        // ISSUE: explicit reference operation
        ref int local31 = @this.Info.ID;
        flag1 = false;
        ref bool local32 = ref flag1;
        flag2 = false;
        ref bool local33 = ref flag2;
        // ISSUE: explicit reference operation
        ref bool local34 = @false;
        // ISSUE: explicit reference operation
        ref bool local35 = @true;
        flag3 = false;
        ref bool local36 = ref flag3;
        if (!FollowerManager.FollowerLocked(in local31, in local32, in local33, in local34, in local35, in local36))
        {
          // ISSUE: explicit reference operation
          ref int local37 = @otherBrain.Info.ID;
          flag4 = false;
          ref bool local38 = ref flag4;
          flag6 = false;
          ref bool local39 = ref flag6;
          // ISSUE: explicit reference operation
          ref bool local40 = @false;
          // ISSUE: explicit reference operation
          ref bool local41 = @true;
          flag7 = false;
          ref bool local42 = ref flag7;
          if (!FollowerManager.FollowerLocked(in local37, in local38, in local39, in local40, in local41, in local42))
          {
            DataManager.Instance.FollowerKnucklebonesMatch = TimeManager.TotalElapsedGameTime;
            this.HardSwapToTask((FollowerTask) new FollowerTask_Knucklebones(otherBrain, Interaction_KnucklebonesBuilding.KnuckleboneBuildings[0].Structure.Brain.Data.ID, true));
            return true;
          }
        }
      }
      if ((double) distance < 2.0 && this.CurrentTaskType != FollowerTaskType.RunFromSomething && otherBrain.Info.HasTrait(FollowerTrait.TraitType.Zombie) && !this.Info.HasTrait(FollowerTrait.TraitType.Zombie) && this.CurrentTaskType != FollowerTaskType.Sleep && this.CurrentTaskType != FollowerTaskType.SleepBedRest)
      {
        ref int local43 = ref this._directInfoAccess.ID;
        flag1 = false;
        ref bool local44 = ref flag1;
        flag2 = false;
        ref bool local45 = ref flag2;
        // ISSUE: explicit reference operation
        ref bool local46 = @false;
        flag5 = true;
        ref bool local47 = ref flag5;
        flag3 = true;
        ref bool local48 = ref flag3;
        if (!FollowerManager.FollowerLocked(in local43, in local44, in local45, in local46, in local47, in local48))
        {
          this.HardSwapToTask((FollowerTask) new FollowerTask_RunAway(otherBrain));
          return true;
        }
      }
    }
    if ((double) distance < 6.0 && otherBrain.CurrentTask is FollowerTask_Dissent && otherBrain.CurrentTask.State == FollowerTaskState.Doing && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) > 360.0 && this.CurrentTaskType != FollowerTaskType.DissentListen && !this.HasTrait(FollowerTrait.TraitType.Zealous))
    {
      StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
      this.HardSwapToTask((FollowerTask) new FollowerTask_DissentListen(otherBrain.Info.ID));
      return true;
    }
    if ((double) distance < 5.0 && this.HasTrait(FollowerTrait.TraitType.CriminalHardened) && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastStolenFromFollowers)
    {
      ref int local49 = ref this._directInfoAccess.ID;
      flag1 = false;
      ref bool local50 = ref flag1;
      flag2 = false;
      ref bool local51 = ref flag2;
      // ISSUE: explicit reference operation
      ref bool local52 = @false;
      flag5 = true;
      ref bool local53 = ref flag5;
      flag3 = false;
      ref bool local54 = ref flag3;
      if (!FollowerManager.FollowerLocked(in local49, in local50, in local51, in local52, in local53, in local54))
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (this.Info.ID != allBrain.Info.ID && otherBrain.Info.ID != allBrain.Info.ID && (double) Vector3.Distance(allBrain.LastPosition, PlayerFarming.Instance.transform.position) < 3.0)
          {
            DataManager.Instance.TimeSinceLastStolenFromFollowers = TimeManager.TotalElapsedGameTime + 0.5f;
            flag1 = false;
            return flag1;
          }
        }
        this.HardSwapToTask((FollowerTask) new FollowerTask_StealFromFollowers(FollowerManager.FindFollowerByID(this.Info.ID), otherBrain, 5f));
        DataManager.Instance.TimeSinceLastStolenFromFollowers = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(2400, 4800);
        return true;
      }
    }
    if ((double) distance < 5.0 && otherBrain.CurrentTask is FollowerTask_StealFromFollowers && otherBrain.CurrentTask.State == FollowerTaskState.Doing && this.CurrentTaskType != FollowerTaskType.ReactFollowerSteal && this.CurrentTaskType != FollowerTaskType.StealFromFollowers && (double) this.GetTimeSinceTask(FollowerTaskType.ReactFollowerSteal) > 30.0)
    {
      this.HardSwapToTask((FollowerTask) new FollowerTask_ReactFollowerSteal(otherBrain));
      return true;
    }
    if (this.CurrentTask != null && !this.CurrentTask.BlockTaskChanges && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastFollowerFight && this.Info.CursedState == Thought.None && PlayerFarming.Location == FollowerLocation.Base)
    {
      ref int local55 = ref this._directInfoAccess.ID;
      flag2 = false;
      ref bool local56 = ref flag2;
      // ISSUE: explicit reference operation
      ref bool local57 = @false;
      flag5 = false;
      ref bool local58 = ref flag5;
      flag3 = true;
      ref bool local59 = ref flag3;
      flag4 = false;
      ref bool local60 = ref flag4;
      if (!FollowerManager.FollowerLocked(in local55, in local56, in local57, in local58, in local59, in local60) && (this.HasTrait(FollowerTrait.TraitType.Argumentative) || this.HasTrait(FollowerTrait.TraitType.MarriedJealous) || this.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous) || this.HasTrait(FollowerTrait.TraitType.HatesSnowmen)))
      {
        if ((double) DataManager.Instance.TimeSinceLastFollowerFight == -1.0)
        {
          this.SetTimeSinceLastFight();
          return false;
        }
        if (this.HasTrait(FollowerTrait.TraitType.Argumentative))
        {
          FollowerBrain.GetAvailableBrainsWithNecklaceTargeted(ref this.targetedFollowers);
          foreach (FollowerBrain targetedFollower in this.targetedFollowers)
          {
            ref int local61 = ref targetedFollower._directInfoAccess.ID;
            flag2 = false;
            ref bool local62 = ref flag2;
            // ISSUE: explicit reference operation
            ref bool local63 = @false;
            flag5 = false;
            ref bool local64 = ref flag5;
            flag3 = true;
            ref bool local65 = ref flag3;
            flag4 = false;
            ref bool local66 = ref flag4;
            if (!FollowerManager.FollowerLocked(in local61, in local62, in local63, in local64, in local65, in local66) && targetedFollower.Info.CursedState == Thought.None && (targetedFollower.CurrentTask == null || !targetedFollower.CurrentTask.BlockSocial))
            {
              this.DoFightLogic(targetedFollower, distance);
              flag1 = true;
              return flag1;
            }
          }
          this.targetedFollowers.Clear();
          foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          {
            if (!FollowerManager.FollowerLocked(in allBrain._directInfoAccess.ID) && allBrain.Info.CursedState == Thought.None && (allBrain.CurrentTask == null || !allBrain.CurrentTask.BlockSocial))
            {
              this.DoFightLogic(allBrain, distance);
              return true;
            }
          }
        }
        if (this.HasTrait(FollowerTrait.TraitType.MarriedJealous))
        {
          foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          {
            if (this.IsValidSpouseFightTarget(allBrain))
            {
              this.DoFightLogic(allBrain, distance);
              flag1 = true;
              return flag1;
            }
          }
        }
        if (this.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous))
        {
          foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          {
            if (this.IsValidSpouseFightTarget(allBrain))
            {
              this.DoFightLogic(allBrain, distance);
              flag1 = true;
              return flag1;
            }
          }
        }
        if (this.HasTrait(FollowerTrait.TraitType.HatesSnowmen))
        {
          foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          {
            if (this.IsValidSnowmanFightTarget(allBrain))
            {
              this.DoFightLogic(allBrain, distance);
              flag1 = true;
              return flag1;
            }
          }
        }
      }
    }
    if (this.HasTrait(FollowerTrait.TraitType.Zombie) && PlayerFarming.Location == FollowerLocation.Base && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastFollowerEaten)
    {
      ref int local67 = ref this._directInfoAccess.ID;
      flag2 = false;
      ref bool local68 = ref flag2;
      // ISSUE: explicit reference operation
      ref bool local69 = @false;
      flag5 = false;
      ref bool local70 = ref flag5;
      flag3 = true;
      ref bool local71 = ref flag3;
      flag4 = false;
      ref bool local72 = ref flag4;
      if (!FollowerManager.FollowerLocked(in local67, in local68, in local69, in local70, in local71, in local72))
      {
        if (Interaction_Daycare.IsInDaycare(this.Info.ID))
        {
          DataManager.Instance.TimeSinceLastFollowerEaten = TimeManager.TotalElapsedGameTime + 240f;
          return false;
        }
        FollowerBrain.GetAvailableBrainsWithNecklaceTargeted(ref this.targetedFollowers);
        foreach (FollowerBrain targetedFollower in this.targetedFollowers)
        {
          if (targetedFollower.Info.CursedState == Thought.None && (targetedFollower.CurrentTask == null || !targetedFollower.CurrentTask.BlockSocial))
          {
            ref int local73 = ref targetedFollower._directInfoAccess.ID;
            flag2 = false;
            ref bool local74 = ref flag2;
            // ISSUE: explicit reference operation
            ref bool local75 = @false;
            flag5 = false;
            ref bool local76 = ref flag5;
            flag3 = true;
            ref bool local77 = ref flag3;
            flag4 = false;
            ref bool local78 = ref flag4;
            if (!FollowerManager.FollowerLocked(in local73, in local74, in local75, in local76, in local77, in local78))
            {
              this.DoFightLogic(targetedFollower, distance);
              flag1 = true;
              return flag1;
            }
          }
        }
        this.targetedFollowers.Clear();
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.Info.CursedState == Thought.None && (allBrain.CurrentTask == null || !allBrain.CurrentTask.BlockSocial) && !FollowerManager.FollowerLocked(in allBrain._directInfoAccess.ID))
          {
            this.DoFightLogic(allBrain, distance);
            return true;
          }
        }
      }
    }
    return false;
  }

  public bool IsJealousSpouse(FollowerBrain otherFollower)
  {
    return (this.HasTrait(FollowerTrait.TraitType.MarriedJealous) || this.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous)) && otherFollower.Info.MarriedToLeader;
  }

  public bool IsValidSpouseFightTarget(FollowerBrain followerBrain)
  {
    if (FollowerManager.FollowerLocked(in followerBrain._directInfoAccess.ID) || followerBrain.Info.CursedState != Thought.None || !followerBrain.Info.MarriedToLeader)
      return false;
    return followerBrain.CurrentTask == null || !followerBrain.CurrentTask.BlockSocial;
  }

  public bool IsValidSnowmanFightTarget(FollowerBrain followerBrain)
  {
    return !FollowerManager.FollowerLocked(in followerBrain._directInfoAccess.ID) && followerBrain.Info.CursedState == Thought.None && (followerBrain.CurrentTask == null || !followerBrain.CurrentTask.BlockSocial) && followerBrain._directInfoAccess.IsSnowman;
  }

  public void DoFightLogic(FollowerBrain targetFollowerBrain, float distance)
  {
    if ((double) UnityEngine.Random.value < 0.5 || this.Location != FollowerLocation.Base || (double) distance > 5.0)
      this.HardSwapToTask((FollowerTask) new FollowerTask_FightFollower(targetFollowerBrain.Info.ID, true));
    else
      this.HardSwapToTask((FollowerTask) new FollowerTask_ChaseFollower(targetFollowerBrain.Info.ID, true));
    this.SetTimeSinceLastFight();
  }

  public void SetTimeSinceLastFight()
  {
    if (this.HasTrait(FollowerTrait.TraitType.Zombie))
      DataManager.Instance.TimeSinceLastFollowerEaten = TimeManager.TotalElapsedGameTime + 240f;
    else
      DataManager.Instance.TimeSinceLastFollowerFight = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(2400, 6000);
  }

  public static void SetTimeSinceLastBump()
  {
    DataManager.Instance.TimeSinceLastFollowerBump = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(600f, 2400f);
  }

  public bool CheckForSpeakers(Structure structure)
  {
    if (structure.Type == StructureBrain.TYPES.PROPAGANDA_SPEAKER && structure.Structure_Info.Fuel > 0 && !structure.Structure_Info.IsCollapsed && !structure.Structure_Info.IsSnowedUnder)
    {
      BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
      if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
      {
        boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
      }
      boxCollider2D.size = Vector2.one * Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE;
      boxCollider2D.transform.position = structure.Brain.Data.Position;
      boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
      if (boxCollider2D.OverlapPoint((Vector2) this.LastPosition))
        return true;
    }
    return false;
  }

  public bool CheckForInteraction(Structure structure, float CheckDistance)
  {
    StructureBrain brain = structure.Brain;
    if (TimeManager.IsNight || this.CurrentOverrideTaskType != FollowerTaskType.None || this.Info.CursedState == Thought.Child || this.Info.HasTrait(FollowerTrait.TraitType.Zombie) || this.Info.HasTrait(FollowerTrait.TraitType.Mutated))
      return false;
    switch (structure.Type)
    {
      case StructureBrain.TYPES.GRAVE:
      case StructureBrain.TYPES.GRAVE2:
        if (brain.Data.FollowerID != -1 && this.CurrentTaskType != FollowerTaskType.BuryBody && (double) this.GetTimeSinceTask(FollowerTaskType.ReactGrave) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 1200.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactGrave(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.DEAD_WORSHIPPER:
        if (this.CurrentTaskType != FollowerTaskType.BuryBody && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0 && (double) this.GetTimeSinceTask(FollowerTaskType.ReactCorpse) > 60.0 && !brain.ReservedForTask)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactCorpse(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.VOMIT:
        if (this.CurrentTaskType != FollowerTaskType.Ill && this.CurrentTaskType != FollowerTaskType.CleanWaste && (double) this.GetTimeSinceTask(FollowerTaskType.ReactVomit) > 60.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0 && structure.VariantIndex == 0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactVomit(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.PRISON:
        if (brain.Data.FollowerID != -1 && brain.Data.FollowerID != this._directInfoAccess.ID && brain.Data.FollowerID != 99996 && this.CurrentTaskType != FollowerTaskType.ReactPrisoner && (double) this.GetTimeSinceTask(FollowerTaskType.ReactPrisoner) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 1200.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          if (this.HasTrait(FollowerTrait.TraitType.Unlawful) && PlayerFarming.Location == FollowerLocation.Base && (double) Vector3.Distance(PlayerFarming.Instance.transform.position, structure.transform.position) < 10.0)
            this.HardSwapToTask((FollowerTask) new FollowerTask_BreakFollowerFromPrison(structure.Structure_Info.ID));
          else
            this.HardSwapToTask((FollowerTask) new FollowerTask_ReactPrisoner(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.OUTHOUSE:
        if (brain.IsFull && (double) this.GetTimeSinceTask(FollowerTaskType.ReactOuthouse) > 60.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactOuthouse(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.POOP:
        if (structure.Structure_Info.FollowerID != this.Info.ID && this.CurrentTaskType != FollowerTaskType.CleanWaste && (double) this.GetTimeSinceTask(FollowerTaskType.ReactPoop) > 60.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactPoop(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.DECORATION_LAMB_STATUE:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
      case StructureBrain.TYPES.DECORATION_POND:
      case StructureBrain.TYPES.ICE_SCULPTURE:
      case StructureBrain.TYPES.ICE_SCULPTURE_1:
      case StructureBrain.TYPES.ICE_SCULPTURE_2:
      case StructureBrain.TYPES.ICE_SCULPTURE_3:
        if ((double) this.GetTimeSinceTask(FollowerTaskType.ReactDecoration) > 120.0 && !this.HasTrait(FollowerTrait.TraitType.Blind) && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactDecorations(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
        if (FollowerBrainStats.ShouldWork && this.CanWork && this.Info.CursedState != Thought.Dissenter && brain.SoulCount < brain.SoulMax && ((Structures_Shrine_Passive) brain).PrayAvailable(structure.Type) && !brain.ReservedForTask && (double) Vector3.Distance(this.LastPosition, brain.Data.Position) < (double) Structures_Shrine_Passive.Range(brain.Data.Type))
        {
          float num = structure.Type != StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST || !this.Info.IsDisciple ? 0.5f : 0.25f;
          if ((double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 1200.0 * (double) num && (structure.Type != StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST || this.Info.IsDisciple))
          {
            StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
            this.HardSwapToTask((FollowerTask) new FollowerTask_PrayPassive(structure.Structure_Info.ID));
            return false;
          }
          break;
        }
        break;
      case StructureBrain.TYPES.POOP_MASSIVE:
        if ((double) Vector3.Distance(this.LastPosition, structure.Brain.Data.Position) < 1.5 && !structure.Brain.ReservedForTask && this.Info.ID != structure.Brain.Data.FollowerID && this.CurrentTaskType != FollowerTaskType.StuckInPoo)
        {
          this.HardSwapToTask((FollowerTask) new FollowerTask_StuckInPoo(structure.Brain));
          break;
        }
        break;
      case StructureBrain.TYPES.HATCHERY:
        if (brain.Data.HasEgg && (double) this.GetTimeSinceTask(FollowerTaskType.ReactEgg) > 60.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactEgg(brain as Structures_Hatchery));
          return true;
        }
        break;
      case StructureBrain.TYPES.TOXIC_WASTE:
        if ((double) Vector3.Distance(this.LastPosition, structure.Brain.Data.Position) < 1.5 && !structure.Brain.ReservedForTask && this.Info.ID != structure.Brain.Data.FollowerID && this.CurrentTaskType != FollowerTaskType.StuckInPoo && !this.Info.HasTrait(FollowerTrait.TraitType.Mutated) && !this.Info.HasTrait(FollowerTrait.TraitType.MutatedVisual))
        {
          this.HardSwapToTask((FollowerTask) new FollowerTask_StuckInPoo(structure.Brain));
          break;
        }
        break;
      default:
        return false;
    }
    return false;
  }

  public bool CheckForLockedFollowerInteraction(FollowerBrain otherBrain, float distance)
  {
    if (TimeManager.CurrentPhase != DayPhase.Night && (double) distance < 6.0 && FollowerManager.IsChildOf(otherBrain.Info.ID, this.Info.ID) && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) > 360.0 && (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_HugFollower) && !(this.CurrentTask is FollowerTask_ManualControl) && !(this.CurrentTask is FollowerTask_Imprisoned)) && (this.HasTrait(FollowerTrait.TraitType.ProudParent) || this.HasTrait(FollowerTrait.TraitType.OverworkedParent)))
    {
      StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
      this.HardSwapToTask((FollowerTask) new FollowerTask_HugFollower(otherBrain));
      return true;
    }
    if (this.CurrentTaskType == FollowerTaskType.RunFromSomething || this.Info.CursedState != Thought.Child || PlayerFarming.Location != FollowerLocation.Base || !otherBrain.Info.HasTrait(FollowerTrait.TraitType.Zombie) || this.Info.HasTrait(FollowerTrait.TraitType.Zombie) || this.CurrentTaskType == FollowerTaskType.Sleep || this.CurrentTaskType == FollowerTaskType.SleepBedRest || (double) distance >= 2.0 || (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) <= 120.0)
      return false;
    StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
    this.HardSwapToTask((FollowerTask) new FollowerTask_RunAway(otherBrain));
    return true;
  }

  public bool CheckForLambInteraction(Vector3 position)
  {
    float num = Vector3.Distance(position, PlayerFarming.Instance.transform.position);
    if (this.Info.CursedState == Thought.None && (this.HasTrait(FollowerTrait.TraitType.Bastard) || this.HasTrait(FollowerTrait.TraitType.CriminalHardened)) && (double) num < 1.0 && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastStolenCoins && PlayerFarming.Location == FollowerLocation.Base && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) > 20 && (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Idle || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Moving) && !LetterBox.IsPlaying)
    {
      if ((double) DataManager.Instance.TimeSinceLastStolenCoins != -1.0)
      {
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (this.Info.ID != allBrain.Info.ID && (double) Vector3.Distance(allBrain.LastPosition, PlayerFarming.Instance.transform.position) < 3.0)
          {
            DataManager.Instance.TimeSinceLastStolenCoins = TimeManager.TotalElapsedGameTime + 0.5f;
            return false;
          }
        }
        this.HardSwapToTask((FollowerTask) new FollowerTask_StealFromLamb());
      }
      DataManager.Instance.TimeSinceLastStolenCoins = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(2400, 4800);
      TaskAndTime.SetTaskTime(TimeManager.TotalElapsedGameTime, FollowerTaskType.Jeer, this);
      return true;
    }
    if (this.Info.CursedState == Thought.None && (this.HasTrait(FollowerTrait.TraitType.Bastard) || this.HasTrait(FollowerTrait.TraitType.CriminalHardened)) && (double) num < 5.0 && (double) num > 2.0 && PlayerFarming.Location == FollowerLocation.Base && (double) this.GetTimeSinceTask(FollowerTaskType.Jeer) > 120.0 && this.CurrentTaskType != FollowerTaskType.Jeer)
    {
      TaskAndTime.SetTaskTime(TimeManager.TotalElapsedGameTime, FollowerTaskType.Jeer, this);
      this.HardSwapToTask((FollowerTask) new FollowerTask_Jeer());
      return true;
    }
    if (this.Info.CursedState != Thought.None || this._directInfoAccess.LeavingCult || !this.HasTrait(FollowerTrait.TraitType.Scared) && !this.HasTrait(FollowerTrait.TraitType.CriminalScarred) || (double) num >= 2.0 || PlayerFarming.Location != FollowerLocation.Base || this.CurrentTaskType == FollowerTaskType.RunFromPlayer)
      return false;
    this.HardSwapToTask((FollowerTask) new FollowerTask_RunAwayFromLamb());
    return true;
  }

  public bool CheckForSimInteraction(StructureBrain structureBrain)
  {
    if (structureBrain.Data.Type == StructureBrain.TYPES.PROPAGANDA_SPEAKER && structureBrain.Data.Fuel > 0 && (double) Vector3.Distance(structureBrain.Data.Position, this.LastPosition) < (double) Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE)
      ++this.SpeakersInRange;
    switch (structureBrain.Data.Type)
    {
      case StructureBrain.TYPES.DECORATION_LAMB_STATUE:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
      case StructureBrain.TYPES.DECORATION_POND:
      case StructureBrain.TYPES.ICE_SCULPTURE:
      case StructureBrain.TYPES.ICE_SCULPTURE_1:
      case StructureBrain.TYPES.ICE_SCULPTURE_2:
      case StructureBrain.TYPES.ICE_SCULPTURE_3:
        if ((double) this.GetTimeSinceTask(FollowerTaskType.ReactDecoration) > 120.0 && !this.HasTrait(FollowerTrait.TraitType.Blind) && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactDecorations(structureBrain.Data.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
        if (FollowerBrainStats.ShouldWork && this.CanWork && this.Info.CursedState != Thought.Dissenter && structureBrain.SoulCount < structureBrain.SoulMax && ((Structures_Shrine_Passive) structureBrain).PrayAvailable(structureBrain.Data.Type) && !structureBrain.ReservedForTask && (double) Vector3.Distance(this.LastPosition, structureBrain.Data.Position) < (double) Structures_Shrine_Passive.Range(structureBrain.Data.Type))
        {
          float num = structureBrain.Data.Type != StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST || !this.Info.IsDisciple ? 0.5f : 0.25f;
          if ((double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 1200.0 * (double) num && (structureBrain.Data.Type != StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST || this.Info.IsDisciple))
          {
            StructureAndTime.SetTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure);
            this.HardSwapToTask((FollowerTask) new FollowerTask_PrayPassive(structureBrain.Data.ID));
            return false;
          }
          break;
        }
        break;
      default:
        return false;
    }
    return false;
  }

  public Follower.ComplaintType GetMostPressingComplaint()
  {
    if (this.Info.FirstTimeSpeakingToPlayer)
      return Follower.ComplaintType.FirstTimeSpeakingToPlayer;
    Follower.ComplaintType pressingComplaint = Follower.ComplaintType.None;
    using (IEnumerator<Follower.ComplaintType> enumerator = this.GetOrderedComplaints().GetEnumerator())
    {
      if (enumerator.MoveNext())
        pressingComplaint = enumerator.Current;
    }
    return pressingComplaint;
  }

  public IEnumerable<Follower.ComplaintType> GetOrderedComplaints()
  {
    if ((double) this.Stats.Illness > 0.0)
      yield return Follower.ComplaintType.Sick;
    if ((double) this.Stats.Satiation <= 30.0)
      yield return Follower.ComplaintType.Hunger;
    if (!this.HasHome)
      yield return Follower.ComplaintType.Homeless;
    if (this.HasHome && !this.HomeIsCorrectLevel)
      yield return Follower.ComplaintType.NeedBetterHouse;
  }

  public void OnStructureAdded(StructuresData structure)
  {
    if (structure.Type == StructureBrain.TYPES.TEMPLE)
      this.RemoveThought(Thought.NoTemple, true);
    this.CheckChangeTask();
  }

  public void OnStructureMoved(StructuresData structure)
  {
    if (structure.ID != this.CurrentTaskUsingStructureID)
      return;
    this.CurrentTask.Abort();
  }

  public void OnStructureUpgraded(StructuresData structure)
  {
    if (structure.ID != this.CurrentTaskUsingStructureID)
      return;
    this.CurrentTask.Abort();
  }

  public void OnStructureRemoved(StructuresData structure)
  {
    if (structure.ID == this.CurrentTaskUsingStructureID)
      this.CurrentTask.Abort();
    if (structure.ID == this.HomeID)
      this.ClearDwelling();
    this.CheckChangeTask();
  }

  public void OnHappinessStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
  }

  public void OnStarvationChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID || newState == oldState)
      return;
    switch (newState)
    {
      case FollowerStatState.Off:
        this.RemoveCurseState(Thought.BecomeStarving);
        this.RemoveThought(Thought.BecomeStarving, true);
        this.AddThought(Thought.NoLongerStarving);
        break;
      case FollowerStatState.On:
        this.AddThought(Thought.BecomeStarving);
        if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || this.HasTrait(FollowerTrait.TraitType.ColdEnthusiast) || this.HasTrait(FollowerTrait.TraitType.WarmBlooded) || this.HasTrait(FollowerTrait.TraitType.Chionophile) || this.HasTrait(FollowerTrait.TraitType.WinterBody))
          break;
        this.AddThought(Thought.ColdHunger);
        break;
    }
  }

  public void MakeDrunk()
  {
    if ((double) this.Stats.Drunk > 0.0 || this.CurrentTask != null && this.CurrentTask.BlockThoughts)
      return;
    this.Stats.Drunk = 50f;
    FollowerBrainStats.StatStateChangedEvent drunkStateChanged = FollowerBrainStats.OnDrunkStateChanged;
    if (drunkStateChanged != null)
      drunkStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeState();
    this.AddThought((Thought) UnityEngine.Random.Range(350, 356));
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.BecomeDrunk, this.Info, NotificationFollower.Animation.Drunk);
  }

  public void MakeOld()
  {
    if (this.Info.CursedState != Thought.None || this._directInfoAccess.IsSnowman || this.HasTrait(FollowerTrait.TraitType.Mutated) || this.CurrentTask != null && this.CurrentTask.BlockThoughts || this.Info.ID == 99996 && !DataManager.Instance.SozoNoLongerBrainwashed)
      return;
    this.ApplyCurseState(Thought.OldAge);
    if (this.Info.CursedState != Thought.OldAge)
      return;
    this.Info.OldAge = true;
    this.Info.TaxEnforcer = false;
    this.Info.FaithEnforcer = false;
    this.Info.Outfit = FollowerOutfitType.Old;
    this.Info.Hat = FollowerHatType.None;
    DataManager.Instance.LastFollowerToReachOldAge = TimeManager.TotalElapsedGameTime;
  }

  public void MakeChild()
  {
    this.Info.Age = 1;
    this.ApplyCurseState(Thought.Child, force: true);
  }

  public void MakeAdult()
  {
    this.RemoveCurseState(Thought.Child);
    if (this.Info.ID == 100006)
      return;
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.ReachedAdulthood, this.Info, NotificationFollower.Animation.Happy);
  }

  public void MakeExhausted()
  {
    if ((double) this.Stats.Exhaustion > 0.0 || this.CurrentTask != null && this.CurrentTask.BlockThoughts || this.HasTrait(FollowerTrait.TraitType.Zombie) || this.HasTrait(FollowerTrait.TraitType.MissionaryTerrified) || this.HasTrait(FollowerTrait.TraitType.ExistentialDread) || this.HasTrait(FollowerTrait.TraitType.Zombie) || this.HasTrait(FollowerTrait.TraitType.Mutated) || this.Info.IsSnowman)
      return;
    this.Stats.Exhaustion = 50f;
    FollowerBrainStats.StatStateChangedEvent exhaustionStateChanged = FollowerBrainStats.OnExhaustionStateChanged;
    if (exhaustionStateChanged != null)
      exhaustionStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeState();
    if (DataManager.Instance.WokeUpEveryoneDay == -3)
    {
      DataManager.Instance.WokeUpEveryoneDay = -2;
      CultFaithManager.AddThought(Thought.Cult_ExhaustedEveryone);
    }
    else
    {
      if (DataManager.Instance.WokeUpEveryoneDay != -1)
        return;
      CultFaithManager.AddThought(Thought.Cult_ExhaustedFollower, this.Info.ID);
    }
  }

  public void MakeInjured(bool force = false)
  {
    if (this.Info.CursedState != Thought.None || this.Info.HasTrait(FollowerTrait.TraitType.Zombie) || this._directInfoAccess.IsSnowman || this.Info.HasTrait(FollowerTrait.TraitType.Mutated) || this.CurrentTask != null && this.CurrentTask.BlockThoughts && !force)
      return;
    this.ApplyCurseState(Thought.Injured, force: force);
    this.AddThought((Thought) UnityEngine.Random.Range(363, 368));
    this.Stats.Injured = 50f;
    FollowerBrainStats.StatStateChangedEvent injuredStateChanged = FollowerBrainStats.OnInjuredStateChanged;
    if (injuredStateChanged != null)
      injuredStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeState();
  }

  public void MakeSick(bool forced = false)
  {
    if (this.Info.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.RotCantIll, this.Info, NotificationFollower.Animation.Happy);
    }
    else
    {
      if (this.Info.CursedState != Thought.None || this.Info.HasTrait(FollowerTrait.TraitType.Zombie) || this.Info.HasTrait(FollowerTrait.TraitType.Mutated) || this._directInfoAccess.IsSnowman || this.CurrentTask != null && this.CurrentTask.BlockThoughts && !forced)
        return;
      this.ApplyCurseState(Thought.Ill, force: forced);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.Info.ID != this.Info.ID && !DataManager.Instance.Followers_Recruit.Contains(allBrain._directInfoAccess))
        {
          if (allBrain.HasTrait(FollowerTrait.TraitType.FearOfSickPeople))
            CultFaithManager.AddThought(Thought.Cult_FearSick, allBrain.Info.ID);
          else if (allBrain.HasTrait(FollowerTrait.TraitType.LoveOfSickPeople))
            CultFaithManager.AddThought(Thought.Cult_LoveSick, allBrain.Info.ID);
        }
      }
      this.Stats.Illness = 50f;
      FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
      if (illnessStateChanged != null)
        illnessStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
      this.CheckChangeState();
    }
  }

  public bool CanFreeze()
  {
    return this.CanFreezeExcludingScarf() && this.Info.Customisation != FollowerCustomisationType.Scarf && !this._directInfoAccess.IsSnowman && !this._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.FreezeImmune) && !this._directInfoAccess.Traits.Contains(FollowerTrait.TraitType.WarmBlooded) && this.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_Frozen;
  }

  public bool CanFreezeExcludingScarf()
  {
    return this.Info.Protection != FollowerProtectionType.Cold && this.Info.Protection != FollowerProtectionType.All && !this.Info.HasTrait(FollowerTrait.TraitType.WarmBlooded);
  }

  public bool CanGetCold()
  {
    return SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (double) WarmthBar.WarmthNormalized <= 0.0;
  }

  public bool CanOverheat()
  {
    return this.Info.Protection != FollowerProtectionType.Hot && this.Info.Protection != FollowerProtectionType.All && this.Info.CursedState != Thought.OldAge;
  }

  public bool CanSoak()
  {
    return this.Info.Protection != FollowerProtectionType.Rain && this.Info.Protection != FollowerProtectionType.All && this.Info.CursedState != Thought.OldAge;
  }

  public Structures_LightningRod IsWithinLightningRodRange()
  {
    return StructureManager.IsWithinLightningRod(this.LastPosition);
  }

  public void MakeSoaking()
  {
    if (this.Info.CursedState != Thought.None || !this.CanSoak())
      return;
    this.ApplyCurseState(Thought.Soaking);
    this.CheckChangeState();
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.Soaking, this.Info, NotificationFollower.Animation.Soaking);
    this.Stats.Soaking = 50f;
    FollowerBrainStats.StatStateChangedEvent soakingStateChanged = FollowerBrainStats.OnSoakingStateChanged;
    if (soakingStateChanged != null)
      soakingStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeTask();
  }

  public void MakeFreezing()
  {
    if (this.Info.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.RotCantFreeze, this.Info, NotificationFollower.Animation.Happy);
    }
    else
    {
      if (this.Info.CursedState != Thought.None && !this.CanFreeze())
        return;
      this.ApplyCurseState(Thought.Freezing);
      this.CheckChangeState();
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.Freezing, this.Info, NotificationFollower.Animation.Freezing);
      this.Stats.Freezing = 50f;
      FollowerBrainStats.StatStateChangedEvent freezingStateChanged = FollowerBrainStats.OnFreezingStateChanged;
      if (freezingStateChanged != null)
        freezingStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
      this.CheckChangeTask();
      if (!this.HasTrait(FollowerTrait.TraitType.Frigidophile))
        return;
      this.AddAdoration(FollowerBrain.AdorationActions.Frigidophile, (System.Action) null);
    }
  }

  public void MakePopsicle()
  {
    if (this.Info.CursedState != Thought.None || !this.CanFreeze())
      return;
    this.ApplyCurseState(Thought.Freezing);
    this.CheckChangeState();
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.Freezing, this.Info, NotificationFollower.Animation.Freezing);
    this.Stats.Freezing = 100f;
    FollowerBrainStats.StatStateChangedEvent freezingStateChanged = FollowerBrainStats.OnFreezingStateChanged;
    if (freezingStateChanged != null)
      freezingStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeTask();
    if (!this.HasTrait(FollowerTrait.TraitType.Frigidophile))
      return;
    this.AddAdoration(FollowerBrain.AdorationActions.Frigidophile, (System.Action) null);
  }

  public void MakeOverheating()
  {
    if (this.Info.CursedState != Thought.None || !this.CanOverheat())
      return;
    this.ApplyCurseState(Thought.Overheating);
    this.CheckChangeState();
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.Overheating, this.Info, NotificationFollower.Animation.Overheating);
    this.Stats.Overheating = 50f;
    FollowerBrainStats.StatStateChangedEvent overheatingStateChanged = FollowerBrainStats.OnOverheatingStateChanged;
    if (overheatingStateChanged != null)
      overheatingStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeTask();
  }

  public void MakeStarve()
  {
    if (this.Info.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.RotCantStarve, this.Info, NotificationFollower.Animation.Happy);
    }
    else
    {
      if (this.Info.CursedState != Thought.None || this.HasTrait(FollowerTrait.TraitType.DontStarve) || this._directInfoAccess.IsSnowman || this.Info.HasTrait(FollowerTrait.TraitType.Mutated) || this.CurrentTask != null && this.CurrentTask.BlockThoughts)
        return;
      this.ApplyCurseState(Thought.BecomeStarving);
      this.Stats.Starvation = 37.5f;
      FollowerBrainStats.StatStateChangedEvent starvationStateChanged = FollowerBrainStats.OnStarvationStateChanged;
      if (starvationStateChanged != null)
        starvationStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.Starving, this.Info, NotificationFollower.Animation.Unhappy);
      this.CheckChangeState();
      this.CheckChangeTask();
    }
  }

  public void MakeDissenter(bool ignoreImmunity = false)
  {
    if (this.Info.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.RotCantDissent, this.Info, NotificationFollower.Animation.Happy);
    }
    else
    {
      if (this.Info.CursedState != Thought.None || (this.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Loyalty || this.Info.HasTrait(FollowerTrait.TraitType.Zombie) || this.Info.HasTrait(FollowerTrait.TraitType.BishopOfCult)) && !ignoreImmunity || this.Info.HasTrait(FollowerTrait.TraitType.Zombie) || this._directInfoAccess.IsSnowman || this.Info.HasTrait(FollowerTrait.TraitType.Mutated))
        return;
      this.ApplyCurseState(Thought.Dissenter);
      this.CheckChangeState();
      FollowerManager.FindFollowerByID(this.Info.ID)?.SetOutfit(this.Info.Outfit, false);
      if (this.Info.CursedState != Thought.Dissenter)
        return;
      FollowerBrainStats.StatStateChangedEvent reeducationStateChanged = FollowerBrainStats.OnReeducationStateChanged;
      if (reeducationStateChanged == null)
        return;
      reeducationStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    }
  }

  public void OnExhaustionStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID)
      return;
    if (newState != oldState)
    {
      switch (newState)
      {
        case FollowerStatState.Off:
          Debug.Log((object) "EXHAUSTION OFF!!!");
          if (DataManager.Instance.WokeUpEveryoneDay == -4)
          {
            CultFaithManager.AddThought(Thought.Cult_NoLongerExhaustedEveryone);
            DataManager.Instance.WokeUpEveryoneDay = -5;
            break;
          }
          if (DataManager.Instance.WokeUpEveryoneDay == -1)
          {
            CultFaithManager.AddThought(Thought.Cult_NoLongerExhaustedFollower, followerId);
            break;
          }
          break;
        case FollowerStatState.On:
          Debug.Log((object) "EXHAUSTION ON!!!");
          break;
      }
    }
    this.CheckChangeState();
  }

  public void OnDrunkStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID)
      return;
    if (newState != oldState)
    {
      switch (newState)
      {
        case FollowerStatState.Off:
          Debug.Log((object) "DRUNK OFF!!!");
          bool flag = true;
          foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
          {
            if (allBrain.Info.IsDrunk)
              flag = false;
          }
          if (flag)
          {
            NotificationCentre.Instance.PlayGenericNotification("Notifications/Cult_No_Longer_Drunk/Notification/On");
            break;
          }
          break;
        case FollowerStatState.On:
          Debug.Log((object) "DRUNK ON!!!");
          break;
      }
    }
    this.CheckChangeState();
  }

  public void OnIllnessStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID)
      return;
    if (newState != oldState && newState == FollowerStatState.Off)
    {
      Debug.Log((object) "ILLNESS OFF!!!");
      this.RemoveThought(Thought.Ill, true);
      this.RemoveCurseState(Thought.Ill);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        allBrain.RemoveThought(Thought.SomeoneIllGermophobe, false);
        allBrain.RemoveThought(Thought.SomeoneIllLoveOfSickTrait, false);
      }
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerIll, this.Info, NotificationFollower.Animation.Happy);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FollowerRecoverIllness, followerId);
    }
    this.CheckChangeState();
  }

  public void OnInjuredStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID)
      return;
    if (newState != oldState && newState == FollowerStatState.Off)
    {
      Debug.Log((object) "ILLNESS OFF!!!");
      this.RemoveThought(Thought.Injured, true);
      this.RemoveCurseState(Thought.Injured);
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerInjured, this.Info, NotificationFollower.Animation.Happy);
    }
    this.CheckChangeState();
  }

  public void OnSatiationStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeTask();
  }

  public void OnStarvationStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeTask();
  }

  public void OnRestStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeState();
    this.CheckChangeTask();
  }

  public void OnBathroomStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeTask();
  }

  public void OnMotivatedChanged(int followerID)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeState();
  }

  public void OnResearchBegin() => this.CheckChangeTask();

  public void OnNewPhaseStarted()
  {
    this.CheckChangeTask();
    switch (TimeManager.CurrentPhase)
    {
      case DayPhase.Afternoon:
        this._directInfoAccess.WakeUpDay = -1;
        break;
      case DayPhase.Night:
        if (DataManager.Instance.WokeUpEveryoneDay == -2)
        {
          DataManager.Instance.WokeUpEveryoneDay = -4;
          break;
        }
        if (DataManager.Instance.WokeUpEveryoneDay == -5)
        {
          DataManager.Instance.WokeUpEveryoneDay = -1;
          break;
        }
        break;
    }
    if (this.Info.NudistWinner && !FollowerBrainStats.IsNudism)
    {
      this.Info.NudistWinner = false;
      this.CheckChangeState();
    }
    if (this.CurrentTask == null)
      return;
    this.CurrentTask.OnNewPhaseStarted();
  }

  public void OnScheduleChanged() => this.CheckChangeTask();

  public float GetHungerScore()
  {
    float hungerScore = 0.0f;
    if (!FollowerBrainStats.Fasting)
      hungerScore = (float) Mathf.FloorToInt(this.Stats.Starvation + (100f - this.Stats.Satiation)) + (float) this.Info.ID / 1000f;
    return hungerScore;
  }

  public float GetHungerScoreNotStarving()
  {
    float scoreNotStarving = 0.0f;
    if (this.Info.CursedState != Thought.BecomeStarving && !FollowerBrainStats.Fasting && this.CurrentTaskType != FollowerTaskType.EatMeal && this.CurrentTaskType != FollowerTaskType.EatStoredFood)
      scoreNotStarving = (float) Mathf.FloorToInt(this.Stats.Starvation + (100f - this.Stats.Satiation)) + (float) this.Info.ID / 1000f;
    return scoreNotStarving;
  }

  public float GetHungerScoreNoCursedState()
  {
    float scoreNoCursedState = 0.0f;
    if (this.Info.CursedState == Thought.None && !FollowerBrainStats.Fasting && this.CurrentTaskType != FollowerTaskType.EatMeal && this.CurrentTaskType != FollowerTaskType.EatStoredFood)
      scoreNoCursedState = (float) Mathf.FloorToInt(this.Stats.Starvation + (100f - this.Stats.Satiation)) + (float) this.Info.ID / 1000f;
    return scoreNoCursedState;
  }

  public FollowerState CurrentState
  {
    get => this._currentState;
    set
    {
      FollowerState currentState = this._currentState;
      this._currentState = value;
      Action<FollowerState, FollowerState> onStateChanged = this.OnStateChanged;
      if (onStateChanged == null)
        return;
      onStateChanged(this._currentState, currentState);
    }
  }

  public void CheckChangeState()
  {
    FollowerState followerState = this.GetFollowerState();
    if (followerState == null || this.CurrentState != null && this.CurrentState.Type == followerState.Type)
      return;
    this.CurrentState = followerState;
  }

  public FollowerState GetFollowerState()
  {
    if (this.CurrentTaskType == FollowerTaskType.Floating)
      return (FollowerState) new FollowerState_Floating();
    if (this.Info.HasTrait(FollowerTrait.TraitType.Zombie) && this.Info.CursedState != Thought.Child)
      return (FollowerState) new FollowerState_Zombie();
    if (this.Info.HasTrait(FollowerTrait.TraitType.ExistentialDread))
      return (FollowerState) new FollowerState_ExistentialDread();
    if (this.Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      return (FollowerState) new FollowerState_ExistentialDread();
    if ((double) this.Stats.Drunk > 0.0)
      return (FollowerState) new FollowerState_Drunk();
    if (this._directInfoAccess.IsSnowman && this.Info.CursedState != Thought.Child)
      return (FollowerState) new FollowerState_Snowman();
    if (this.Info.CursedState == Thought.Child && !this.HasTrait(FollowerTrait.TraitType.Zombie))
      return (FollowerState) new FollowerState_Child();
    if (this.Info.CursedState == Thought.Child && this.HasTrait(FollowerTrait.TraitType.Zombie))
      return (FollowerState) new FollowerState_ChildZombie();
    if (this.Info.CursedState == Thought.Ill)
      return (FollowerState) new FollowerState_Ill();
    if (this.Info.CursedState == Thought.BecomeStarving)
      return (FollowerState) new FollowerState_Starving();
    if (this.FollowingPlayer)
      return (FollowerState) new FollowerState_Following();
    if (this.InRitual)
      return (FollowerState) new FollowerState_Ritual();
    if ((double) this.Stats.Exhaustion > 0.0)
      return (FollowerState) new FollowerState_Exhausted();
    if ((double) this.Stats.Injured > 0.0 || this.Info.CursedState == Thought.Injured)
      return (FollowerState) new FollowerState_Injured();
    if (this.Stats.Motivated)
      return (FollowerState) new FollowerState_Motivated();
    if (FollowerBrainStats.IsNudism && this.Info.CursedState != Thought.Dissenter && this.Info.CursedState != Thought.Child)
      return (FollowerState) new FollowerState_Nude();
    if (FollowerBrainStats.IsPurge && this.Info.CursedState != Thought.Dissenter)
      return (FollowerState) new FollowerState_Riot();
    if (this.CanFreeze() && (double) this.Stats.Freezing > 0.0)
      return (FollowerState) new FollowerState_Freezing();
    if ((double) this.Stats.Soaking > 0.0)
      return (FollowerState) new FollowerState_Soaking();
    if ((double) this.Stats.Overheating > 0.0)
      return (FollowerState) new FollowerState_Overheating();
    if ((double) this.Stats.Aflame > 0.0)
      return (FollowerState) new FollowerState_Aflame();
    if (this.CanFreeze() && SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      return (FollowerState) new FollowerState_Cold();
    if (this.CanFreeze() && this.CanGetCold())
      return (FollowerState) new FollowerState_Cold();
    return this.Info.CursedState == Thought.OldAge ? (FollowerState) new FollowerState_OldAge() : (FollowerState) new FollowerState_Default();
  }

  public bool ShouldReconsiderTask
  {
    get => this.\u003CShouldReconsiderTask\u003Ek__BackingField;
    set => this.\u003CShouldReconsiderTask\u003Ek__BackingField = value;
  }

  public FollowerTask CurrentTask
  {
    get => (FollowerTask) this._currentTask?.ChangeLocationTask ?? this._currentTask;
    set
    {
      if (this._currentTask != null && this._currentTask.State != FollowerTaskState.Done)
        Debug.Log((object) this._currentTask.Type);
      FollowerTask currentTask = this._currentTask;
      this._currentTask = value;
      currentTask?.ReleaseReservations();
      Action<FollowerTask, FollowerTask> onTaskChanged = this.OnTaskChanged;
      if (onTaskChanged != null)
        onTaskChanged(this._currentTask, currentTask);
      if (this._currentTask != null)
        this._currentTask.Start();
      if (currentTask == null)
        return;
      TaskAndTime.SetTaskTime(TimeManager.TotalElapsedGameTime, currentTask.Type, this);
    }
  }

  public FollowerTaskType CurrentTaskType
  {
    get => this.CurrentTask != null ? this.CurrentTask.Type : FollowerTaskType.None;
  }

  public int CurrentTaskUsingStructureID
  {
    get => this.CurrentTask != null ? this.CurrentTask.UsingStructureID : 0;
  }

  public void CheckChangeTask()
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockTaskChanges)
      return;
    this.ShouldReconsiderTask = true;
  }

  public bool BeginReconsider()
  {
    this._currentTask?.ReleaseReservations();
    this._nextTask?.ReleaseReservations();
    FollowerTask personalTask = this.GetPersonalTask(this.HomeLocation);
    if (personalTask != null)
    {
      this._currentTask?.ClaimReservations();
      this._nextTask?.ClaimReservations();
      FollowerTaskType followerTaskType = this.CurrentTaskType != FollowerTaskType.ChangeLocation ? this.CurrentTaskType : ((FollowerTask_ChangeLocation) this.CurrentTask).ParentType;
      if (personalTask.Type != followerTaskType)
        this.TransitionToTask(personalTask);
      this.ShouldReconsiderTask = false;
    }
    else
      this.PendingTask = new FollowerBrain.PendingTaskData();
    return this.ShouldReconsiderTask;
  }

  public int TryClaimExistingTask(List<FollowerTask> availableTasks)
  {
    if (this.CurrentTask != null)
    {
      for (int index = 0; index < availableTasks.Count; ++index)
      {
        FollowerTask availableTask = availableTasks[index];
        if (availableTask != null && availableTask.GetUniqueTaskCode() == this.CurrentTask.GetUniqueTaskCode() && availableTask.GetPriorityCategory(this.Info.FollowerRole, this.Info.WorkerPriority, this) != PriorityCategory.Ignore)
        {
          this.PendingTask.KeepExistingTask = true;
          this.PendingTask.Task = availableTask;
          this.PendingTask.ListIndex = index;
          availableTasks[index] = (FollowerTask) null;
          return index;
        }
      }
    }
    return -1;
  }

  public int ClaimNextAvailableTask(List<FollowerTask> availableTasks)
  {
    for (int index1 = 0; index1 < 6; ++index1)
    {
      for (int index2 = 0; index2 < availableTasks.Count; ++index2)
      {
        FollowerTask availableTask = availableTasks[index2];
        if (availableTask != null && FollowerTask.RequiredFollowerLevel(this.Info.FollowerRole, availableTask.Type) && availableTask.GetPriorityCategory(this.Info.FollowerRole, this.Info.WorkerPriority, this) == (PriorityCategory) index1 && index1 != 5)
        {
          this.PendingTask.KeepExistingTask = false;
          this.PendingTask.Task = availableTask;
          this.PendingTask.ListIndex = index2;
          availableTasks[index2] = (FollowerTask) null;
          return index2;
        }
      }
    }
    return -1;
  }

  public void EndReconsider()
  {
    this._currentTask?.ClaimReservations();
    this._nextTask?.ClaimReservations();
    if (this.PendingTask.Task != null)
    {
      if (!this.PendingTask.KeepExistingTask)
        this.TransitionToTask(this.PendingTask.Task);
    }
    else
    {
      FollowerTask fallbackTask = this.GetFallbackTask(TimeManager.GetScheduledActivity(this.HomeLocation));
      if (fallbackTask != null && this.CurrentTaskType != fallbackTask.Type)
        this.TransitionToTask(fallbackTask);
    }
    this.ShouldReconsiderTask = false;
  }

  public void CompleteCurrentTask() => this.TransitionToTask((FollowerTask) null);

  public void TransitionToTask(FollowerTask nextTask)
  {
    nextTask?.Init(this);
    if (this.CurrentTask == null)
    {
      this.CurrentTask = nextTask;
    }
    else
    {
      this._nextTask?.ReleaseReservations();
      this._nextTask = nextTask;
      this._nextTask?.ClaimReservations();
      this.CurrentTask.End();
    }
  }

  public void HardSwapToTask(FollowerTask nextTask)
  {
    nextTask?.Init(this);
    if (this.CurrentTask == null)
    {
      this.CurrentTask = nextTask;
    }
    else
    {
      this._nextTask?.ReleaseReservations();
      this._nextTask = nextTask;
      this._nextTask?.ClaimReservations();
      this.CurrentTask.Abort();
    }
  }

  public void ContinueToNextTask()
  {
    FollowerTask nextTask = this._nextTask;
    this._nextTask = (FollowerTask) null;
    nextTask?.ReleaseReservations();
    this.CurrentTask = nextTask;
  }

  public FollowerTaskType CurrentOverrideTaskType
  {
    get => this._directInfoAccess.CurrentOverrideTaskType;
    set => this._directInfoAccess.CurrentOverrideTaskType = value;
  }

  public StructureBrain.TYPES CurrentOverrideStructureType
  {
    get => this._directInfoAccess.CurrentOverrideStructureType;
    set => this._directInfoAccess.CurrentOverrideStructureType = value;
  }

  public int OverrideDayIndex
  {
    get => this._directInfoAccess.OverrideDayIndex;
    set => this._directInfoAccess.OverrideDayIndex = value;
  }

  public bool OverrideTaskCompleted
  {
    get => this._directInfoAccess.OverrideTaskCompleted;
    set => this._directInfoAccess.OverrideTaskCompleted = value;
  }

  public void SetPersonalOverrideTask(
    FollowerTaskType Type,
    StructureBrain.TYPES OverrideStructureType = StructureBrain.TYPES.NONE)
  {
    this.CompleteCurrentTask();
    this.OverrideTaskCompleted = false;
    this.OverrideDayIndex = TimeManager.CurrentDay;
    this.ClearPersonalOverrideTaskProvider();
    this.CurrentOverrideTaskType = Type;
    this.CurrentOverrideStructureType = OverrideStructureType;
    this.CheckChangeTask();
  }

  public void ClearPersonalOverrideTaskProvider()
  {
    this.CurrentOverrideTaskType = FollowerTaskType.None;
    this.CurrentOverrideStructureType = StructureBrain.TYPES.NONE;
  }

  public bool CheckOverrideComplete()
  {
    switch (this.CurrentOverrideTaskType)
    {
      case FollowerTaskType.EatMeal:
        return (double) this.Stats.Satiation > 60.0 && TimeManager.CurrentDay > this.OverrideDayIndex;
      case FollowerTaskType.Sleep:
        return TimeManager.CurrentDay > this.OverrideDayIndex;
      default:
        return false;
    }
  }

  public FollowerTask GetWeatherEventTask()
  {
    if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
      return (FollowerTask) null;
    if (!this.CanWork)
    {
      if (this.HasHome && !this.GetAssignedDwellingStructure().IsCollapsed)
        return (FollowerTask) new FollowerTask_Sleep();
      if (this.CurrentTaskType != FollowerTaskType.FakeLeisure && this.CurrentTaskType != FollowerTaskType.HuddleForWarmthFollower && this.CurrentTaskType != FollowerTaskType.HuddleForWarmthLeader)
        return (FollowerTask) new FollowerTask_FakeLeisure();
    }
    return (FollowerTask) null;
  }

  public FollowerTask GetPersonalOverrideTask()
  {
    FollowerTask personalOverrideTask = (FollowerTask) null;
    if (this.CurrentOverrideTaskType != FollowerTaskType.None)
    {
      if (this.CheckOverrideComplete())
      {
        this.ClearPersonalOverrideTaskProvider();
      }
      else
      {
        switch (this.CurrentOverrideTaskType)
        {
          case FollowerTaskType.EatMeal:
            return this.CheckEatTask(true);
          case FollowerTaskType.Sleep:
            return this.Info.CursedState == Thought.Ill && this.CurrentTaskType != FollowerTaskType.ClaimDwelling ? (FollowerTask) new FollowerTask_SleepBedRest() : (FollowerTask) new FollowerTask_Sleep();
          case FollowerTaskType.SleepBedRest:
            return this.CurrentTaskType != FollowerTaskType.ClaimDwelling ? (FollowerTask) new FollowerTask_SleepBedRest() : (FollowerTask) null;
          case FollowerTaskType.Drinking:
            return this.CheckDrinkTask();
          default:
            return (FollowerTask) null;
        }
      }
    }
    return personalOverrideTask;
  }

  public float GetTimeSinceTask(FollowerTaskType taskType)
  {
    float timeSinceTask = 0.0f;
    if (this.CurrentTaskType != taskType)
      timeSinceTask = TimeManager.TotalElapsedGameTime - TaskAndTime.GetLastTaskTime(taskType, this);
    return timeSinceTask;
  }

  public FollowerTask GetPersonalTask(FollowerLocation location)
  {
    if (this.LeavingCult)
    {
      if (!this.HasTrait(FollowerTrait.TraitType.Spy))
        return (FollowerTask) new FollowerTask_LeaveCult(NotificationCentre.NotificationType.LeaveCultUnhappy);
      this.Stats.DissentGold = (float) Mathf.RoundToInt((float) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) * 0.1f);
      return (FollowerTask) new FollowerTask_SpyLeaveCult(NotificationCentre.NotificationType.LeaveCultSpy);
    }
    if (this.Info.CursedState != Thought.Zombie)
    {
      if (this.DiedOfStarvation)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromStarvation);
      if (this.DiedOfIllness)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromIllness);
      if (this.DiedOfOldAge)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromOldAge);
      if (this.DiedOfInjury)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedOfInjury);
      if (this.FrozeToDeath)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.FrozeToDeath);
      if (this.DiedFromRot && PlayerFarming.Location == FollowerLocation.Base)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromRot);
      if (this.DiedFromOverheating)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromOverheating);
      if (this.BurntToDeath)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.BurntToDeath);
    }
    ScheduledActivity activity = TimeManager.GetScheduledActivity(location);
    if (this.Info.CursedState == Thought.Child)
    {
      if (!Interaction_Daycare.IsInDaycare(this.Info.ID))
      {
        FollowerTask_ClaimDwelling claimDwallingTask = this.TryGetClaimDwallingTask(activity, location);
        if (claimDwallingTask != null)
          return (FollowerTask) claimDwallingTask;
      }
      if (this.Info.ID == 100000)
        return (FollowerTask) new FollowerTask_ChosenChild();
      if (this.HasTrait(FollowerTrait.TraitType.Zombie))
        return (FollowerTask) new FollowerTask_ChildZombie(this.Info.ID);
      if (this.Info.HasTrait(FollowerTrait.TraitType.Insomniac) && TimeManager.CurrentPhase != DayPhase.Night)
        return Interaction_Daycare.IsInDaycare(this.Info.ID) ? (FollowerTask) new FollowerTask_Sleep(true) : (FollowerTask) new FollowerTask_Sleep();
      if (this.Info.HasTrait(FollowerTrait.TraitType.Hibernation) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !this._directInfoAccess.WorkThroughNight)
        return Interaction_Daycare.IsInDaycare(this.Info.ID) ? (FollowerTask) new FollowerTask_Sleep(true) : (FollowerTask) new FollowerTask_Sleep(hibernating: true);
      if (this.Info.HasTrait(FollowerTrait.TraitType.Aestivation) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring && !this._directInfoAccess.WorkThroughNight)
        return Interaction_Daycare.IsInDaycare(this.Info.ID) ? (FollowerTask) new FollowerTask_Sleep(true) : (FollowerTask) new FollowerTask_Sleep(hibernating: true);
      if (activity != ScheduledActivity.Sleep || this.Info.HasTrait(FollowerTrait.TraitType.Insomniac) || this._directInfoAccess.IsSnowman)
        return (FollowerTask) new FollowerTask_Child(this.Info.ID);
      return Interaction_Daycare.IsInDaycare(this.Info.ID) ? (FollowerTask) new FollowerTask_Sleep(true) : (FollowerTask) new FollowerTask_Sleep();
    }
    if (activity == ScheduledActivity.Sleep && this._directInfoAccess.WorkThroughNight)
      activity = ScheduledActivity.Work;
    if (activity == ScheduledActivity.Work && !FollowerBrainStats.ShouldWork && !this.CanWork)
      activity = ScheduledActivity.Leisure;
    FollowerTask stateTask = this.GetStateTask(location);
    if (stateTask != null)
    {
      this.FollowingPlayer = false;
      return stateTask;
    }
    if (this.Info.HasTrait(FollowerTrait.TraitType.Hibernation) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !this._directInfoAccess.WorkThroughNight)
      return (FollowerTask) new FollowerTask_Sleep(hibernating: true);
    if (this.Info.HasTrait(FollowerTrait.TraitType.Aestivation) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring && !this._directInfoAccess.WorkThroughNight)
      return (FollowerTask) new FollowerTask_Sleep(hibernating: true);
    if ((double) this.Stats.Exhaustion > 0.0 && ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToPassOut > 1200.0 || (double) this.Stats.Exhaustion >= 100.0))
    {
      if ((double) UnityEngine.Random.value < 0.02500000037252903)
      {
        DataManager.Instance.LastFollowerToPassOut = TimeManager.TotalElapsedGameTime;
        return (FollowerTask) new FollowerTask_Sleep(true);
      }
    }
    else
    {
      if (this.HasTrait(FollowerTrait.TraitType.OverworkedParent) && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToPassOut > 1200.0)
      {
        DataManager.Instance.LastFollowerToPassOut = TimeManager.TotalElapsedGameTime;
        return (FollowerTask) new FollowerTask_Sleep(true, true);
      }
      if (this.HasTrait(FollowerTrait.TraitType.Mutated))
      {
        if ((double) this.GetTimeSinceTask(FollowerTaskType.Vomit) > 175.0 && (double) UnityEngine.Random.value < 0.014999999664723873)
          return (FollowerTask) new FollowerTask_Vomit();
        if ((double) this.GetTimeSinceTask(FollowerTaskType.Bathroom) > 175.0 && (double) UnityEngine.Random.value < 0.02500000037252903)
          return (FollowerTask) new FollowerTask_InstantPoop();
      }
    }
    if (this.FollowingPlayer)
      return (FollowerTask) new FollowerTask_FollowPlayer();
    if (this.CurrentTaskType == FollowerTaskType.ManualControl)
      return (FollowerTask) new FollowerTask_ManualControl();
    FollowerTask personalOverrideTask = this.GetPersonalOverrideTask();
    if (personalOverrideTask != null)
      return personalOverrideTask;
    FollowerTask weatherEventTask = this.GetWeatherEventTask();
    if (weatherEventTask != null && !TimeManager.IsNight)
      return weatherEventTask;
    if (!this.Stats.WorkerBeenGivenOrders && this.Info.FollowerRole == FollowerRole.Worker && activity != ScheduledActivity.Sleep)
      return (FollowerTask) new FollowerTask_AwaitInstruction();
    if (this.Stats.CachedLumber > 0 && this.CurrentTaskType != FollowerTaskType.Lumberjack)
      return (FollowerTask) new FollowerTask_DepositWood(this.Stats.CachedLumberjackStationID);
    FollowerTask overrideTask = TimeManager.GetOverrideTask(this);
    if (overrideTask != null)
      return overrideTask;
    FollowerTask_ClaimDwelling claimDwallingTask1 = this.TryGetClaimDwallingTask(activity, location);
    if (claimDwallingTask1 != null)
      return (FollowerTask) claimDwallingTask1;
    if (this.Info.CursedState != Thought.None && (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_Imprisoned)))
    {
      if (TimeManager.CurrentPhase != DayPhase.Night && this.HasTrait(FollowerTrait.TraitType.Insomniac) && this.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_5)
        return (FollowerTask) new FollowerTask_Sleep();
      switch (this.Info.CursedState)
      {
        case Thought.Dissenter:
          if (activity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_Dissent();
          break;
        case Thought.Ill:
          if ((double) this.Stats.Illness == 0.0)
          {
            this.RemoveThought(Thought.Ill, true);
            this.RemoveCurseState(Thought.Ill);
            return (FollowerTask) null;
          }
          if (activity != ScheduledActivity.Sleep)
          {
            if (this._directInfoAccess.CursedStateVariant == 0)
              return (FollowerTask) new FollowerTask_Ill();
            if (this._directInfoAccess.CursedStateVariant == 1)
              return (FollowerTask) new FollowerTask_IllPoopy();
            break;
          }
          break;
        case Thought.BecomeStarving:
          if (activity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_Starving();
          break;
        case Thought.OldAge:
          if (activity != ScheduledActivity.Sleep)
          {
            if (this.CurrentTask != null && (this.CurrentTaskType == FollowerTaskType.EatMeal || this.CurrentTaskType == FollowerTaskType.EatStoredFood))
            {
              this.ShouldReconsiderTask = false;
              return (FollowerTask) null;
            }
            return (double) this.Stats.Drunk > 0.0 ? (FollowerTask) new FollowerTask_Drunk() : (FollowerTask) new FollowerTask_OldAge();
          }
          break;
        case Thought.Zombie:
          if (activity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_Zombie();
          break;
        case Thought.Injured:
          if ((double) this.Stats.Injured == 0.0)
          {
            this.RemoveThought(Thought.Injured, true);
            this.RemoveCurseState(Thought.Injured);
            return (FollowerTask) null;
          }
          if (activity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_Injured();
          break;
        case Thought.Freezing:
          if (activity != ScheduledActivity.Sleep)
          {
            if ((double) this.Stats.Freezing != 0.0)
              return (FollowerTask) new FollowerTask_Freezing();
            this.RemoveThought(Thought.Freezing, true);
            this.RemoveThought(Thought.Freezing_2, true);
            this.RemoveCurseState(Thought.Freezing);
            return (FollowerTask) null;
          }
          break;
        case Thought.Overheating:
          if (activity != ScheduledActivity.Sleep)
          {
            if ((double) this.Stats.Overheating != 0.0)
              return (FollowerTask) new FollowerTask_Overheating();
            this.RemoveThought(Thought.Overheating, true);
            this.RemoveCurseState(Thought.Overheating);
            return (FollowerTask) null;
          }
          break;
        case Thought.Soaking:
          if (activity != ScheduledActivity.Sleep)
          {
            if ((double) this.Stats.Soaking != 0.0)
              return (FollowerTask) new FollowerTask_Soaking();
            this.RemoveThought(Thought.Soaking, true);
            this.RemoveCurseState(Thought.Soaking);
            return (FollowerTask) null;
          }
          break;
        case Thought.Aflame:
          if (activity != ScheduledActivity.Sleep)
          {
            if ((double) this.Stats.Aflame != 0.0)
              return (FollowerTask) new FollowerTask_Aflame();
            this.RemoveThought(Thought.Aflame, true);
            this.RemoveCurseState(Thought.Aflame);
            return (FollowerTask) null;
          }
          break;
      }
    }
    if (this.HasTrait(FollowerTrait.TraitType.Zombie))
      return (FollowerTask) new FollowerTask_Zombie();
    if (this.HasTrait(FollowerTrait.TraitType.ExistentialDread))
      return (FollowerTask) new FollowerTask_ExistentialDread();
    if (this.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      return (FollowerTask) new FollowerTask_MissionaryTerrified();
    if (this.HasTrait(FollowerTrait.TraitType.Terrified))
      return (FollowerTask) new FollowerTask_Terrified();
    if (TimeManager.CurrentPhase != DayPhase.Night)
    {
      Structure ofType = Structure.GetOfType(StructureBrain.TYPES.PUB);
      if ((UnityEngine.Object) ofType == (UnityEngine.Object) null)
        ofType = Structure.GetOfType(StructureBrain.TYPES.PUB_2);
      Structures_Pub pub = (Structures_Pub) null;
      int num = -1;
      if ((UnityEngine.Object) ofType != (UnityEngine.Object) null && ofType.Brain != null)
      {
        pub = (Structures_Pub) ofType.Brain;
        num = pub.GetFollowerDrinkReservedSeat(this.Info.ID);
        if (num >= pub.FoodStorage.Data.Inventory.Count)
        {
          pub.Data.ReservedFollowers.Remove(num);
          num = -1;
        }
      }
      if (this.Info.IsDrunk)
        return (FollowerTask) new FollowerTask_Drunk();
      if (pub != null && num != -1 && Structures_Pub.IsDrinking)
        return (FollowerTask) new FollowerTask_Drink(num, pub.FoodStorage.Data.Inventory[num], pub);
      if (this.Info.TaxEnforcer || this.Info.FaithEnforcer)
      {
        if (TimeManager.CurrentPhase == DayPhase.Afternoon || TimeManager.CurrentPhase == DayPhase.Dusk)
        {
          if (this.HasTrait(FollowerTrait.TraitType.Poet))
            return (FollowerTask) new FollowerTask_Poet();
          if (PlayerFarming.Location == FollowerLocation.Base && this.HasTrait(FollowerTrait.TraitType.FluteLover))
            return (FollowerTask) new FollowerTask_Flute();
        }
      }
      else
      {
        if (this.HasTrait(FollowerTrait.TraitType.Poet))
          return (FollowerTask) new FollowerTask_Poet();
        if (PlayerFarming.Location == FollowerLocation.Base && this.HasTrait(FollowerTrait.TraitType.FluteLover))
          return (FollowerTask) new FollowerTask_Flute();
      }
    }
    if (this.HasTrait(FollowerTrait.TraitType.AnimalLover) && TimeManager.CurrentPhase != DayPhase.Night && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastAnimalLoverPet > 1200.0 && StructureManager.HasAnimalsInTheBase())
    {
      DataManager.Instance.LastAnimalLoverPet = TimeManager.TotalElapsedGameTime;
      return (FollowerTask) new FollowerTask_PetAnimal();
    }
    if (this.HasTrait(FollowerTrait.TraitType.IceSculpture) && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastIceSculptureBuild > 600.0)
    {
      DataManager.Instance.LastIceSculptureBuild = TimeManager.TotalElapsedGameTime;
      return (FollowerTask) new FollowerTask_IceSculpture();
    }
    if (this.Info.ID == 99996 && !DataManager.Instance.SozoNoLongerBrainwashed && (!Structures_Pub.IsDrinking || this.CurrentOverrideTaskType != FollowerTaskType.Drinking))
      return (FollowerTask) new FollowerTask_TrippingBalls();
    if (this.Info.IsDrunk)
      return (FollowerTask) new FollowerTask_Drunk();
    if (TimeManager.CurrentPhase != DayPhase.Night && this.HasTrait(FollowerTrait.TraitType.Insomniac) && this.Info.Necklace != InventoryItem.ITEM_TYPE.Necklace_5 && FollowerBrainStats.ShouldSleep)
      return (FollowerTask) new FollowerTask_Sleep();
    return TimeManager.CurrentPhase != DayPhase.Night && this.HasTrait(FollowerTrait.TraitType.Drowsy) && (double) UnityEngine.Random.value > 0.5 && (double) this.GetTimeSinceTask(FollowerTaskType.Sleep) > 30.0 && (double) this.GetTimeSinceTask(FollowerTaskType.SleepBedRest) > 30.0 ? (FollowerTask) new FollowerTask_Sleep(true, true) : (FollowerTask) null;
  }

  public FollowerTask_ClaimDwelling TryGetClaimDwallingTask(
    ScheduledActivity activity,
    FollowerLocation location)
  {
    if (!this.HasHome && (activity == ScheduledActivity.Work || activity == ScheduledActivity.Sleep || activity == ScheduledActivity.Leisure || this.CurrentTaskType == FollowerTaskType.SleepBedRest))
    {
      Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(location, this._directInfoAccess);
      if (freeDwellingAndSlot != null && !StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask)
      {
        this.AssignDwelling(freeDwellingAndSlot, this.Info.ID, false);
        StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask = true;
        return new FollowerTask_ClaimDwelling(freeDwellingAndSlot);
      }
    }
    if (this.HasHome && (activity == ScheduledActivity.Work || activity == ScheduledActivity.Sleep || activity == ScheduledActivity.Leisure))
    {
      Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(this._directInfoAccess.DwellingID);
      if (structureById != null)
      {
        if (!structureById.Data.FollowersClaimedSlots.Contains(this.Info.ID))
        {
          Dwelling.DwellingAndSlot dwellingAndSlot = new Dwelling.DwellingAndSlot(this._directInfoAccess.DwellingID, this._directInfoAccess.DwellingSlot, this._directInfoAccess.DwellingLevel);
          this.AssignDwelling(dwellingAndSlot, this.Info.ID, false);
          StructureManager.GetStructureByID<Structures_Bed>(dwellingAndSlot.ID).ReservedForTask = true;
          return new FollowerTask_ClaimDwelling(dwellingAndSlot);
        }
        if (structureById.IsCollapsed)
        {
          Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(location, this._directInfoAccess);
          if (freeDwellingAndSlot != null && !StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask)
          {
            this.ClearDwelling();
            this.AssignDwelling(freeDwellingAndSlot, this.Info.ID, true);
            StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask = true;
            return new FollowerTask_ClaimDwelling(freeDwellingAndSlot);
          }
        }
      }
      else
        this._directInfoAccess.DwellingID = Dwelling.NO_HOME;
    }
    return (FollowerTask_ClaimDwelling) null;
  }

  public static List<FollowerTask> GetTopPriorityFollowerTasks(
    FollowerLocation location,
    out ScheduledActivity selectedActivity)
  {
    selectedActivity = TimeManager.GetScheduledActivity(location);
    if (!FollowerBrainStats.ShouldWork)
      selectedActivity = ScheduledActivity.Leisure;
    else if (!FollowerBrainStats.ShouldSleep && TimeManager.CurrentPhase == DayPhase.Night)
      selectedActivity = ScheduledActivity.Work;
    return FollowerBrain.GetTopPriorityFollowerTasks(selectedActivity, location);
  }

  public static List<FollowerTask> GetTopPriorityFollowerTasks(
    ScheduledActivity activity,
    FollowerLocation location)
  {
    switch (activity)
    {
      case ScheduledActivity.Work:
        return FollowerBrain.GetDesiredTask_Work(location);
      case ScheduledActivity.Study:
        return FollowerBrain.GetDesiredTask_Study(location);
      case ScheduledActivity.Sleep:
        return FollowerBrain.GetDesiredTask_Sleep();
      case ScheduledActivity.Pray:
        return FollowerBrain.GetDesiredTask_Pray();
      case ScheduledActivity.Leisure:
        return FollowerBrain.GetDesiredTask_Leisure(location);
      default:
        throw new ArgumentException($"Unrecognised ScheduledActivity.{activity}");
    }
  }

  public FollowerTask GetStateTask(FollowerLocation location)
  {
    if ((double) this.Stats.Bathroom > 15.0 && !TimeManager.IsNight && this.CurrentTaskType != FollowerTaskType.Bathroom && this.CurrentOverrideTaskType == FollowerTaskType.None && this.CurrentTaskType != FollowerTaskType.Sleep)
    {
      if ((double) this.bathroomOffset == -1.0)
        this.bathroomOffset = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(0, 240 /*0xF0*/);
      else if ((double) TimeManager.TotalElapsedGameTime > (double) this.bathroomOffset)
      {
        this.bathroomOffset = -1f;
        FollowerTask_Bathroom stateTask = (FollowerTask_Bathroom) null;
        if (StructureManager.TryGetAllStructuresOfType<Structures_Outhouse>(ref this.cachedToilets, location))
        {
          foreach (Structures_Outhouse cachedToilet in this.cachedToilets)
          {
            if (!cachedToilet.ReservedForTask && !cachedToilet.IsFull)
            {
              stateTask = new FollowerTask_Bathroom(cachedToilet.Data.ID);
              break;
            }
          }
          if (stateTask == null)
            stateTask = new FollowerTask_Bathroom(this.cachedToilets[UnityEngine.Random.Range(0, this.cachedToilets.Count)].Data.ID);
        }
        if (stateTask == null)
          stateTask = new FollowerTask_Bathroom();
        this.cachedToilets.Clear();
        return (FollowerTask) stateTask;
      }
    }
    if (Structures_Pub.IsDrinking)
    {
      List<Structures_Pub> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Pub>();
      List<Objectives_Drink> objectivesDrinkList = new List<Objectives_Drink>();
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective is Objectives_Drink objectivesDrink && objectivesDrink.TargetFollower == this.Info.ID && structuresOfType.Count > 0)
        {
          for (int index = 0; index < structuresOfType[0].FoodStorage.Data.Inventory.Count; ++index)
          {
            if (structuresOfType[0].FoodStorage.Data.Inventory[index] != null && objectivesDrink.DrinkType == (InventoryItem.ITEM_TYPE) structuresOfType[0].FoodStorage.Data.Inventory[index].type)
              return (FollowerTask) new FollowerTask_Drink(index, structuresOfType[0].FoodStorage.Data.Inventory[index], structuresOfType[0]);
          }
        }
      }
      if (structuresOfType.Count > 0)
      {
        for (int index = 0; index < structuresOfType[0].FoodStorage.Data.Inventory.Count; ++index)
        {
          if (structuresOfType[0].FoodStorage.Data.Inventory[index] != null && structuresOfType[0].FoodStorage.Data.Inventory[index].QuantityReserved <= 0 && !structuresOfType[0].IsDrinkReserved(index))
          {
            foreach (CookingData.MealEffect mealEffect in CookingData.GetMealEffects((InventoryItem.ITEM_TYPE) structuresOfType[0].FoodStorage.Data.Inventory[index].type))
            {
              if (mealEffect.MealEffectType == CookingData.MealEffectType.RemoveFreezing && this.Info.CursedState == Thought.Freezing || mealEffect.MealEffectType == CookingData.MealEffectType.RemovesIllness && this.Info.CursedState == Thought.Ill || mealEffect.MealEffectType == CookingData.MealEffectType.RemoveMutation && this.Info.HasTrait(FollowerTrait.TraitType.Mutated) || mealEffect.MealEffectType == CookingData.MealEffectType.RemoveMajorNegativeStates && this.Info.CursedState != Thought.None)
                return (FollowerTask) new FollowerTask_Drink(index, structuresOfType[0].FoodStorage.Data.Inventory[index], structuresOfType[0]);
            }
          }
        }
      }
    }
    else if (CookingData.GetMealFromStructureType(this.CurrentOverrideStructureType) != InventoryItem.ITEM_TYPE.NONE)
    {
      FollowerTask stateTask = this.CheckDrinkTask();
      if (stateTask != null)
        return stateTask;
    }
    return this.CheckEatTask() ?? (FollowerTask) null;
  }

  public void ApplyCurseState(Thought Curse, Thought SpecialThought = Thought.None, bool force = false)
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockThoughts && !force || Curse == Thought.OldAge && this.Info.HasTrait(FollowerTrait.TraitType.Immortal) || this.Info.HasTrait(FollowerTrait.TraitType.Mutated) && Curse != Thought.Child || this.Info.CursedState != Thought.None)
      return;
    this.Info.CursedState = Curse;
    switch (Curse)
    {
      case Thought.Dissenter:
        this.RemoveThought(Thought.NoLongerDissenting, true);
        this.AddThought(Thought.Dissenter, true);
        if ((UnityEngine.Object) NotificationCentre.Instance != (UnityEngine.Object) null)
          NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.BecomeDissenter, this.Info, NotificationFollower.Animation.Dissenting);
        this.Stats.Reeducation = 50f;
        this.Stats.GivenDissentWarning = false;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain != this && allBrain.HasTrait(FollowerTrait.TraitType.Zealous))
            allBrain.AddThought(Thought.SomeoneDissenterZealotTrait);
        }
        System.Action onBecomeDissenter = this.OnBecomeDissenter;
        if (onBecomeDissenter != null)
        {
          onBecomeDissenter();
          break;
        }
        break;
      case Thought.Ill:
        this.AddThought(Thought.Ill, true);
        this.Stats.Illness = 50f;
        if (SpecialThought == Thought.Cult_FollowerBecameIllSleepingNextToIllFollower)
        {
          if (this.HasTrait(FollowerTrait.TraitType.Germophobe))
          {
            CultFaithManager.AddThought(Thought.Cult_FollowerBecameIllSleepingNextToIllFollower_Germophobe, this.Info.ID);
            break;
          }
          if (this.HasTrait(FollowerTrait.TraitType.Coprophiliac))
          {
            CultFaithManager.AddThought(Thought.Cult_FollowerBecameIllSleepingNextToIllFollower_Coprophiliac, this.Info.ID);
            break;
          }
          CultFaithManager.AddThought(Thought.Cult_FollowerBecameIllSleepingNextToIllFollower, this.Info.ID);
          break;
        }
        if (this.HasTrait(FollowerTrait.TraitType.Germophobe))
        {
          CultFaithManager.AddThought(Thought.Cult_GermophobeBecameSick, this.Info.ID);
          break;
        }
        if (this.HasTrait(FollowerTrait.TraitType.Coprophiliac))
        {
          CultFaithManager.AddThought(Thought.Cult_CoprophiliacBecameSick, this.Info.ID);
          break;
        }
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.BecomeIll, this.Info, NotificationFollower.Animation.Sick);
        break;
      case Thought.BecomeStarving:
        this.AddThought(Thought.BecomeStarving, true);
        if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !this.HasTrait(FollowerTrait.TraitType.ColdEnthusiast) && !this.HasTrait(FollowerTrait.TraitType.WarmBlooded) && !this.HasTrait(FollowerTrait.TraitType.Chionophile) && !this.HasTrait(FollowerTrait.TraitType.WinterBody))
        {
          this.AddThought(Thought.ColdHunger);
          break;
        }
        break;
      case Thought.OldAge:
        this.AddThought(Thought.OldAge, true);
        DataManager.Instance.Followers_Elderly_IDs.Add(this.Info.ID);
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.LoveElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_LoveElderly_Trait, this.Info.ID);
          break;
        }
        CultFaithManager.AddThought(Thought.Cult_FollowerReachedOldAge, this.Info.ID);
        break;
      case Thought.Zombie:
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.ZombieSpawned, this.Info, NotificationFollower.Animation.Unhappy);
        break;
      case Thought.Child:
        this.AddThought(Thought.Child);
        break;
      case Thought.Freezing:
        this.AddRandomThoughtFromList(Thought.Freezing, Thought.Freezing_2);
        break;
      case Thought.Overheating:
        this.AddThought(Thought.Overheating);
        break;
      case Thought.Soaking:
        this.AddThought(Thought.Soaking);
        break;
      case Thought.Aflame:
        this.AddThought(Thought.Aflame);
        break;
    }
    this.CheckChangeState();
    if (this.CurrentTask != null && (this.CurrentTask.Type == FollowerTaskType.Imprisoned || this.CurrentTask.Type == FollowerTaskType.ManualControl) || PlayerFarming.Location == FollowerLocation.Church)
      return;
    this.CompleteCurrentTask();
  }

  public event FollowerBrain.CursedEvent OnCursedStateRemoved;

  public void RemoveCurseState(Thought Thought)
  {
    Thought cursedState = this.Info.CursedState;
    if (this.Info.CursedState == Thought)
      this.Info.CursedState = Thought.None;
    if (!this.ThoughtExists(Thought))
      return;
    switch (Thought)
    {
      case Thought.Dissenter:
        this.RemoveThought(Thought.Dissenter, true);
        this.LeavingCult = false;
        this.AddThought(Thought.NoLongerDissenting);
        this.Stats.Reeducation = 0.0f;
        using (List<FollowerBrain>.Enumerator enumerator = FollowerBrain.AllBrains.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FollowerBrain current = enumerator.Current;
            if (current != this)
              current.RemoveThought(Thought.SomeoneDissenterZealotTrait, false);
          }
          break;
        }
      case Thought.Ill:
        this.Stats.Illness = 0.0f;
        if (cursedState == Thought.Ill)
        {
          FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
          if (illnessStateChanged != null)
            illnessStateChanged(this.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        }
        this.CompleteCurrentTask();
        break;
      case Thought.BecomeStarving:
        this.RemoveThought(Thought.BecomeStarving, true);
        break;
      case Thought.OldAge:
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerOld, this.Info, NotificationFollower.Animation.Happy);
        this.ClearThought(Thought.OldAge);
        break;
      case Thought.Injured:
        this.Stats.Injured = 0.0f;
        this.CompleteCurrentTask();
        break;
      case Thought.Child:
        this.RemoveThought(Thought.Child, true);
        Follower followerById = FollowerManager.FindFollowerByID(this.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          followerById.Spine.transform.localScale = Vector3.one;
          FollowerBrain.SetFollowerCostume(followerById.Spine.Skeleton, this._directInfoAccess, forceUpdate: true);
          break;
        }
        break;
      case Thought.Freezing:
        this.RemoveThought(Thought.Freezing, true);
        this.RemoveThought(Thought.Freezing_2, true);
        this.AddRandomThoughtFromList(Thought.NoLongerFreezing_1, Thought.NoLongerFreezing_2);
        this.Stats.Freezing = 0.0f;
        if (cursedState == Thought.Freezing)
        {
          FollowerBrainStats.StatStateChangedEvent freezingStateChanged = FollowerBrainStats.OnFreezingStateChanged;
          if (freezingStateChanged != null)
            freezingStateChanged(this.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        }
        this.CompleteCurrentTask();
        break;
      case Thought.Overheating:
        this.Stats.Overheating = 0.0f;
        if (cursedState == Thought.Overheating)
        {
          FollowerBrainStats.StatStateChangedEvent overheatingStateChanged = FollowerBrainStats.OnOverheatingStateChanged;
          if (overheatingStateChanged != null)
            overheatingStateChanged(this.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        }
        this.CompleteCurrentTask();
        break;
      case Thought.Aflame:
        this.Stats.Aflame = 0.0f;
        if (cursedState == Thought.Aflame)
        {
          FollowerBrainStats.StatStateChangedEvent aflameStateChanged = FollowerBrainStats.OnAflameStateChanged;
          if (aflameStateChanged != null)
            aflameStateChanged(this.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        }
        this.CompleteCurrentTask();
        break;
    }
    this.CheckChangeState();
    FollowerBrain.CursedEvent cursedStateRemoved = this.OnCursedStateRemoved;
    if (cursedStateRemoved == null)
      return;
    cursedStateRemoved();
  }

  public FollowerTask GetFallbackTask(ScheduledActivity activity)
  {
    if (this.CurrentTaskType == FollowerTaskType.ManualControl)
      return (FollowerTask) new FollowerTask_ManualControl();
    if (this.Info.CursedState != Thought.None && this._directInfoAccess.WorkThroughNight && activity == ScheduledActivity.Sleep)
      return this.GetPersonalTask(this.Location);
    if (activity == ScheduledActivity.Sleep && FollowerBrainStats.ShouldWork && this.CanWork && this._directInfoAccess.WorkThroughNight)
      activity = ScheduledActivity.Work;
    if (!FollowerBrainStats.ShouldWork && activity != ScheduledActivity.Sleep && !this.CanWork && !this.Info.IsSnowman)
      activity = ScheduledActivity.Leisure;
    switch (activity)
    {
      case ScheduledActivity.Work:
        Structures_Shrine result1;
        if (!StructureManager.TryGetFirstStructureOfType<Structures_Shrine>(out result1, this.HomeLocation, (Func<Structures_Shrine, bool>) (s => s != null && s.Prayers.Count < s.PrayersMax && s.SoulCount < s.SoulMax)))
          return (FollowerTask) new FollowerTask_FakeLeisure();
        return this.HasTrait(FollowerTrait.TraitType.Spy) ? (FollowerTask) new FollowerTask_SpyPray(result1.Data.ID) : (FollowerTask) new FollowerTask_Pray(result1.Data.ID);
      case ScheduledActivity.Study:
        Structures_Temple result2;
        StructureManager.TryGetFirstStructureOfType<Structures_Temple>(out result2, this.HomeLocation);
        return (FollowerTask) new FollowerTask_Study(result2.Data.ID);
      case ScheduledActivity.Sleep:
        return this.Info.CursedState == Thought.Ill ? (FollowerTask) new FollowerTask_SleepBedRest() : (FollowerTask) new FollowerTask_Sleep();
      case ScheduledActivity.Pray:
        Structures_Shrine result3;
        if (!StructureManager.TryGetFirstStructureOfType<Structures_Shrine>(out result3, this.HomeLocation, (Func<Structures_Shrine, bool>) (s => s != null && s.Prayers.Count < s.PrayersMax && s.SoulCount < s.SoulMax)))
          return (FollowerTask) new FollowerTask_Idle();
        return this.HasTrait(FollowerTrait.TraitType.Spy) ? (FollowerTask) new FollowerTask_SpyPray(result3.Data.ID) : (FollowerTask) new FollowerTask_Pray(result3.Data.ID);
      case ScheduledActivity.Leisure:
        return (FollowerTask) new FollowerTask_FakeLeisure();
      default:
        throw new ArgumentException($"Unrecognised ScheduledActivity.{activity}");
    }
  }

  public void NewRoleSet(FollowerRole followerRole)
  {
    int followerTaskFromRole = (int) FollowerTask.GetFollowerTaskFromRole(followerRole);
    List<FollowerTask> unoccupiedTasksOfType = FollowerBrain.GetAllUnoccupiedTasksOfType((FollowerTaskType) followerTaskFromRole);
    List<FollowerTask> occupiedTasksOfType = FollowerBrain.GetAllOccupiedTasksOfType((FollowerTaskType) followerTaskFromRole);
    this.Info.FollowerRole = followerRole;
    if (unoccupiedTasksOfType.Count > 0 || occupiedTasksOfType.Count <= 0)
      return;
    FollowerTask followerTask = occupiedTasksOfType[UnityEngine.Random.Range(0, occupiedTasksOfType.Count)];
    followerTask.Brain.Info.FollowerRole = FollowerRole.Worker;
    followerTask.Abort();
  }

  public int GetMealTypeCountForKitchen(
    in StructureBrain.TYPES mealType,
    in Structures_Kitchen kitchen)
  {
    int typeCountForKitchen = 0;
    foreach (InventoryItem inventoryItem in kitchen.FoodStorage.Data.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem.type == CookingData.GetMealFromStructureType(mealType))
        ++typeCountForKitchen;
    }
    return typeCountForKitchen;
  }

  public void CancelTargetedMeal(StructureBrain.TYPES mealType)
  {
    int num1 = StructureManager.AccumulateOnAllStructures((Func<StructureBrain, int>) (s =>
    {
      int num2;
      switch (s)
      {
        case Structures_Meal structuresMeal2:
          num2 = structuresMeal2.Data.Type == mealType ? 1 : 0;
          break;
        case Structures_Kitchen kitchen2:
          num2 = this.GetMealTypeCountForKitchen(in mealType, in kitchen2);
          break;
        default:
          num2 = 0;
          break;
      }
      return num2;
    }));
    int num3 = 0;
    List<FollowerTask> occupiedTasksOfType = FollowerBrain.GetAllOccupiedTasksOfType(FollowerTaskType.EatMeal);
    occupiedTasksOfType.AddRange((IEnumerable<FollowerTask>) FollowerBrain.GetAllOccupiedTasksOfType(FollowerTaskType.EatStoredFood));
    foreach (FollowerTask followerTask in occupiedTasksOfType)
    {
      if (followerTask.Type == FollowerTaskType.EatMeal && ((FollowerTask_EatMeal) followerTask).MealType == mealType || followerTask.Type == FollowerTaskType.EatStoredFood && ((FollowerTask_EatStoredFood) followerTask)._foodType == CookingData.GetMealFromStructureType(mealType))
        ++num3;
    }
    if (num3 < num1)
      return;
    for (int index = 0; index < occupiedTasksOfType.Count; ++index)
    {
      if ((occupiedTasksOfType[index].Type == FollowerTaskType.EatMeal || occupiedTasksOfType[index].Type == FollowerTaskType.EatStoredFood) && occupiedTasksOfType[index].State != FollowerTaskState.Doing)
      {
        FollowerBrain brain = occupiedTasksOfType[0].Brain;
        occupiedTasksOfType[0].Abort();
        FollowerTask_FakeLeisure nextTask = new FollowerTask_FakeLeisure();
        brain.HardSwapToTask((FollowerTask) nextTask);
        break;
      }
    }
  }

  public int GetDrinkOfTypeCountFromPub(in InventoryItem.ITEM_TYPE drinkType, in Structures_Pub pub)
  {
    int typeCountFromPub = 0;
    foreach (InventoryItem inventoryItem in pub.FoodStorage.Data.Inventory)
    {
      if (inventoryItem != null && (InventoryItem.ITEM_TYPE) inventoryItem.type == drinkType)
        ++typeCountFromPub;
    }
    return typeCountFromPub;
  }

  public void CancelTargetedDrink(InventoryItem.ITEM_TYPE drinkType)
  {
    int num1 = StructureManager.AccumulateOnAllStructures(this.Location, (Func<StructureBrain, int>) (s => !(s is Structures_Pub pub) ? 0 : this.GetDrinkOfTypeCountFromPub(in drinkType, in pub)));
    int num2 = 0;
    List<FollowerTask> occupiedTasksOfType = FollowerBrain.GetAllOccupiedTasksOfType(FollowerTaskType.Drinking);
    foreach (FollowerTask followerTask in occupiedTasksOfType)
    {
      if (followerTask.Type == FollowerTaskType.Drinking && ((FollowerTask_Drink) followerTask).DrinkType == drinkType)
        ++num2;
    }
    if (num2 < num1)
      return;
    for (int index = 0; index < occupiedTasksOfType.Count; ++index)
    {
      if (occupiedTasksOfType[index].Type == FollowerTaskType.Drinking && occupiedTasksOfType[index].State != FollowerTaskState.Doing)
      {
        FollowerBrain brain = occupiedTasksOfType[0].Brain;
        if (brain == null)
          break;
        occupiedTasksOfType[0].Abort();
        brain.HardSwapToTask((FollowerTask) new FollowerTask_FakeLeisure());
        break;
      }
    }
  }

  public static List<FollowerTask> GetDesiredTask_Work(FollowerLocation location)
  {
    SortedList<float, FollowerTask> sortedTasks = new SortedList<float, FollowerTask>((IComparer<float>) new FollowerBrain.DuplicateKeyComparer<float>());
    List<StructureBrain> structureBrainList = StructureManager.StructuresAtLocation(location);
    int count = structureBrainList.Count;
    for (int index = 0; index < count; ++index)
    {
      if (structureBrainList[index] is ITaskProvider)
        ((ITaskProvider) structureBrainList[index]).GetAvailableTasks(ScheduledActivity.Work, sortedTasks);
    }
    return new List<FollowerTask>((IEnumerable<FollowerTask>) sortedTasks.Values);
  }

  public static bool IsTaskAvailable(FollowerTaskType taskType)
  {
    List<FollowerTask> followerTaskList = new List<FollowerTask>((IEnumerable<FollowerTask>) FollowerBrain.GetDesiredTask_Work(FollowerLocation.Base));
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask != null)
        followerTaskList.Add(allBrain.CurrentTask);
    }
    foreach (FollowerTask followerTask in followerTaskList)
    {
      if (followerTask.Type == taskType)
        return true;
    }
    return false;
  }

  public static List<FollowerTask> GetAllUnoccupiedTasksOfType(FollowerTaskType taskType)
  {
    List<FollowerTask> followerTaskList = new List<FollowerTask>((IEnumerable<FollowerTask>) FollowerBrain.GetDesiredTask_Work(FollowerLocation.Base));
    List<FollowerTask> unoccupiedTasksOfType = new List<FollowerTask>();
    foreach (FollowerTask followerTask in followerTaskList)
    {
      if (followerTask.Type == taskType)
        unoccupiedTasksOfType.Add(followerTask);
    }
    return unoccupiedTasksOfType;
  }

  public static List<FollowerTask> GetAllOccupiedTasksOfType(FollowerTaskType taskType)
  {
    List<FollowerTask> followerTaskList = new List<FollowerTask>((IEnumerable<FollowerTask>) FollowerBrain.GetDesiredTask_Work(FollowerLocation.Base));
    List<FollowerTask> occupiedTasksOfType = new List<FollowerTask>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask != null)
        followerTaskList.Add(allBrain.CurrentTask);
    }
    foreach (FollowerTask followerTask in followerTaskList)
    {
      if (followerTask.Type == taskType)
        occupiedTasksOfType.Add(followerTask);
    }
    return occupiedTasksOfType;
  }

  public static List<FollowerTask> GetDesiredTask_Study(FollowerLocation location)
  {
    return new List<FollowerTask>();
  }

  public static List<FollowerTask> GetDesiredTask_Sleep() => new List<FollowerTask>();

  public static List<FollowerTask> GetDesiredTask_Pray() => new List<FollowerTask>();

  public static List<FollowerTask> GetDesiredTask_Leisure(FollowerLocation location)
  {
    SortedList<float, FollowerTask> sortedTasks = new SortedList<float, FollowerTask>((IComparer<float>) new FollowerBrain.DuplicateKeyComparer<float>());
    List<StructureBrain> structureBrainList = StructureManager.StructuresAtLocation(location);
    int count = structureBrainList.Count;
    for (int index = 0; index < count; ++index)
    {
      if (structureBrainList[index] is ITaskProvider)
        ((ITaskProvider) structureBrainList[index]).GetAvailableTasks(ScheduledActivity.Leisure, sortedTasks);
    }
    return new List<FollowerTask>((IEnumerable<FollowerTask>) sortedTasks.Values);
  }

  public int GetUnreservedMealCountFromKitchen(in Structures_Kitchen kitchen)
  {
    if (!kitchen.IsContainingFoodStorage)
      return 0;
    int countFromKitchen = 0;
    foreach (InventoryItem inventoryItem in kitchen.FoodStorage.Data.Inventory)
    {
      if (inventoryItem.UnreservedQuantity > 0)
        countFromKitchen += inventoryItem.UnreservedQuantity;
    }
    return countFromKitchen;
  }

  public FollowerTask CheckEatTask(bool Force = false)
  {
    if (this.CurrentTaskType == FollowerTaskType.Sleep || this._directInfoAccess.IsSnowman)
      return (FollowerTask) null;
    InventoryItem.ITEM_TYPE itemType = InventoryItem.ITEM_TYPE.NONE;
    List<Objectives_EatMeal> objectivesEatMealList = new List<Objectives_EatMeal>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_EatMeal)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(((Objectives_EatMeal) objective).TargetFollower);
        if ((infoById != null ? (infoById.Location == FollowerLocation.Base ? 1 : 0) : 0) != 0)
        {
          objectivesEatMealList.Add((Objectives_EatMeal) objective);
          if (((Objectives_EatMeal) objective).TargetFollower == this.Info.ID)
            itemType = CookingData.GetMealFromStructureType(((Objectives_EatMeal) objective).MealType);
        }
      }
    }
    bool flag1 = Force;
    float num1 = HungerBar.MAX_HUNGER - 10f;
    if (GameManager.IsDungeon(PlayerFarming.Location))
      num1 = HungerBar.MAX_HUNGER - 30f;
    if (!FollowerBrainStats.Fasting)
    {
      if ((double) this.Stats.Starvation > 0.0)
        flag1 = true;
      else if ((double) HungerBar.Count < (double) num1 || DataManager.Instance.MealsCooked <= 1)
      {
        int num2 = StructureManager.AccumulateOnAllStructures((Func<StructureBrain, int>) (s =>
        {
          int num3;
          switch (s)
          {
            case Structures_Meal _:
              num3 = 1;
              break;
            case Structures_Kitchen kitchen2:
              num3 = this.GetUnreservedMealCountFromKitchen(in kitchen2);
              break;
            default:
              num3 = 0;
              break;
          }
          return num3;
        }));
        FollowerBrain[] array = FollowerBrain.AllBrains.ToArray();
        Array.Sort<FollowerBrain>(array, (Comparison<FollowerBrain>) ((a, b) => a._directInfoAccess.Starvation.CompareTo(b._directInfoAccess.Starvation)));
        Array.Sort<FollowerBrain>(array, (Comparison<FollowerBrain>) ((a, b) => a._directInfoAccess.Satiation.CompareTo(b._directInfoAccess.Satiation)));
        for (int index = 0; index < array.Length; ++index)
        {
          if (array[index] == this && index < num2)
            flag1 = true;
        }
      }
    }
    else if (this.Info.CursedState == Thought.BecomeStarving)
      flag1 = true;
    if (flag1 || itemType != InventoryItem.ITEM_TYPE.NONE || CookingData.GetMealFromStructureType(this.CurrentOverrideStructureType) != InventoryItem.ITEM_TYPE.NONE)
    {
      List<Structures_Meal> structuresMealList = new List<Structures_Meal>();
      StructureManager.TryGetAllStructuresOfType<Structures_Meal>(ref this.cachedMeals, this.Location);
      FollowerTask_EatMeal followerTaskEatMeal = (FollowerTask_EatMeal) null;
      foreach (Structures_Meal cachedMeal in this.cachedMeals)
      {
        if (cachedMeal.CanEat() && !cachedMeal.ReservedForTask && !cachedMeal.Data.Rotten && (this.CurrentOverrideStructureType == StructureBrain.TYPES.NONE || cachedMeal.Data.Type == this.CurrentOverrideStructureType) && (!cachedMeal.Data.Burned || cachedMeal.Data.Burned & Force) && (this.Info.CursedState != Thought.Zombie || cachedMeal.Data.Type == StructureBrain.TYPES.MEAL_FOLLOWER_MEAT))
        {
          if (itemType != InventoryItem.ITEM_TYPE.NONE && CookingData.GetMealFromStructureType(cachedMeal.Data.Type) != itemType)
          {
            structuresMealList.Add(cachedMeal);
          }
          else
          {
            bool flag2 = true;
            foreach (Objectives_EatMeal objectivesEatMeal in objectivesEatMealList)
            {
              if (objectivesEatMeal.MealType == cachedMeal.Data.Type && this.Info.ID != objectivesEatMeal.TargetFollower)
              {
                flag2 = false;
                break;
              }
            }
            if (this.CurrentOverrideStructureType != cachedMeal.Data.Type)
            {
              foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
              {
                if (allBrain.CurrentOverrideStructureType == cachedMeal.Data.Type && allBrain.CurrentTaskType != FollowerTaskType.EatMeal)
                {
                  flag2 = false;
                  break;
                }
              }
            }
            if (flag2)
            {
              followerTaskEatMeal = new FollowerTask_EatMeal(cachedMeal.Data.ID);
              break;
            }
          }
        }
      }
      if (followerTaskEatMeal != null)
      {
        this.cachedMeals.Clear();
        return (FollowerTask) followerTaskEatMeal;
      }
      if (structuresMealList.Count > 0 & flag1)
        return (FollowerTask) new FollowerTask_EatMeal(structuresMealList[0].Data.ID);
      StructureManager.TryGetAllStructuresOfType<Structures_Kitchen>(ref this.cachedKitchens, this.Location);
      FollowerTask_EatStoredFood taskEatStoredFood = (FollowerTask_EatStoredFood) null;
      foreach (Structures_Kitchen cachedKitchen in this.cachedKitchens)
      {
        if (cachedKitchen.IsContainingFoodStorage)
        {
          List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
          foreach (InventoryItem inventoryItem in cachedKitchen.FoodStorage.Data.Inventory)
          {
            if (inventoryItem.UnreservedQuantity > 0 && (this.CurrentOverrideStructureType == StructureBrain.TYPES.NONE || StructuresData.GetMealStructureType((InventoryItem.ITEM_TYPE) inventoryItem.type) == this.CurrentOverrideStructureType) && (this.Info.CursedState != Thought.Zombie || inventoryItem.type == 133))
            {
              if (itemType != InventoryItem.ITEM_TYPE.NONE && (InventoryItem.ITEM_TYPE) inventoryItem.type != itemType)
              {
                itemTypeList.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
              }
              else
              {
                bool flag3 = true;
                foreach (Objectives_EatMeal objectivesEatMeal in objectivesEatMealList)
                {
                  if (objectivesEatMeal.MealType == CookingData.GetStructureFromMealType((InventoryItem.ITEM_TYPE) inventoryItem.type) && this.Info.ID != objectivesEatMeal.TargetFollower)
                  {
                    flag3 = false;
                    break;
                  }
                }
                if (flag3)
                {
                  taskEatStoredFood = new FollowerTask_EatStoredFood(cachedKitchen.Data.ID, (InventoryItem.ITEM_TYPE) inventoryItem.type);
                  break;
                }
              }
            }
          }
          if (itemTypeList.Count > 0 & flag1)
          {
            taskEatStoredFood = new FollowerTask_EatStoredFood(cachedKitchen.Data.ID, itemTypeList[0]);
            break;
          }
        }
      }
      if (taskEatStoredFood != null)
      {
        this.cachedKitchens.Clear();
        return (FollowerTask) taskEatStoredFood;
      }
      this.CurrentOverrideStructureType = StructureBrain.TYPES.NONE;
    }
    return (FollowerTask) null;
  }

  public FollowerTask CheckDrinkTask()
  {
    InventoryItem.ITEM_TYPE fromStructureType = CookingData.GetMealFromStructureType(this.CurrentOverrideStructureType);
    foreach (Structures_Pub pub in StructureManager.GetAllStructuresOfType<Structures_Pub>())
    {
      if (!Structures_Pub.IsDrinking && pub.FoodStorage != null)
      {
        for (int index = 0; index < pub.FoodStorage.Data.Inventory.Count; ++index)
        {
          if (pub.FoodStorage.Data.Inventory[index] != null && (InventoryItem.ITEM_TYPE) pub.FoodStorage.Data.Inventory[index].type == fromStructureType)
            return (FollowerTask) new FollowerTask_Drink(index, pub.FoodStorage.Data.Inventory[index], pub);
        }
      }
    }
    if (this.CurrentOverrideTaskType == FollowerTaskType.Drinking)
      this.CurrentOverrideStructureType = StructureBrain.TYPES.NONE;
    return (FollowerTask) null;
  }

  public bool HasHome
  {
    get
    {
      return this._directInfoAccess.DwellingID != Dwelling.NO_HOME || this._directInfoAccess.IsSnowman;
    }
  }

  public bool HomeIsCorrectLevel => this._directInfoAccess.DwellingLevel >= this.Info.XPLevel;

  public int HomeID => this._directInfoAccess.DwellingID;

  public void AssignDwelling(Dwelling.DwellingAndSlot d, int followerID, bool Claimed)
  {
    Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(d.ID);
    if (structureById == null)
    {
      d = StructureManager.GetFreeDwellingAndSlot(this.Location, this._directInfoAccess);
      structureById = StructureManager.GetStructureByID<Structures_Bed>(d.ID);
    }
    this._directInfoAccess.PreviousDwellingID = this._directInfoAccess.DwellingID;
    this._directInfoAccess.DwellingID = d.ID;
    this._directInfoAccess.DwellingSlot = Mathf.Clamp(d.dwellingslot, 0, structureById.SlotCount - 1);
    this._directInfoAccess.DwellingLevel = d.dwellingLevel;
    if (Claimed && !structureById.Data.FollowersClaimedSlots.Contains(followerID))
      structureById.Data.FollowersClaimedSlots.Add(followerID);
    structureById.Data.FollowerID = followerID;
    if (!structureById.Data.MultipleFollowerIDs.Contains(followerID))
      structureById.Data.MultipleFollowerIDs.Add(followerID);
    if (Claimed)
    {
      FollowerBrain.DwellingAssignmentChanged dwellingAssigned = FollowerBrain.OnDwellingAssigned;
      if (dwellingAssigned == null)
        return;
      dwellingAssigned(this.Info.ID, d);
    }
    else
    {
      FollowerBrain.DwellingAssignmentChanged assignedAwaitClaim = FollowerBrain.OnDwellingAssignedAwaitClaim;
      if (assignedAwaitClaim == null)
        return;
      assignedAwaitClaim(this.Info.ID, d);
    }
  }

  public void ClearDwelling()
  {
    Dwelling.DwellingAndSlot d = new Dwelling.DwellingAndSlot(this._directInfoAccess.DwellingID, this._directInfoAccess.DwellingSlot, this._directInfoAccess.DwellingLevel);
    Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(this._directInfoAccess.DwellingID);
    if (structureById != null)
    {
      if (structureById.Data.FollowerID == this._directInfoAccess.ID)
        structureById.Data.FollowerID = -1;
      structureById.Data.FollowersClaimedSlots.Remove(this._directInfoAccess.ID);
      structureById.Data.MultipleFollowerIDs.Remove(this._directInfoAccess.ID);
    }
    this._directInfoAccess.PreviousDwellingID = this._directInfoAccess.DwellingID;
    this._directInfoAccess.DwellingID = Dwelling.NO_HOME;
    this._directInfoAccess.DwellingSlot = 0;
    this._directInfoAccess.DwellingLevel = 0;
    if (d.ID != Dwelling.NO_HOME)
    {
      FollowerBrain.DwellingAssignmentChanged onDwellingCleared = FollowerBrain.OnDwellingCleared;
      if (onDwellingCleared != null)
        onDwellingCleared(this.Info.ID, d);
    }
    if (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_Sleep))
      return;
    this.CurrentTask.Abort();
  }

  public Structures_Bed GetAssignedDwellingStructure()
  {
    return StructureManager.GetStructureByID<Structures_Bed>(this._directInfoAccess.DwellingID);
  }

  public Dwelling.DwellingAndSlot GetDwellingAndSlot()
  {
    return this.HasHome && this._directInfoAccess != null ? new Dwelling.DwellingAndSlot(this._directInfoAccess.DwellingID, this._directInfoAccess.DwellingSlot, this._directInfoAccess.DwellingLevel) : (Dwelling.DwellingAndSlot) null;
  }

  public void SetNewHomeLocation(FollowerLocation location)
  {
    this._directInfoAccess.HomeLocation = location;
    if (!this.HasHome || this.GetAssignedDwellingStructure().Data.Location == this.HomeLocation)
      return;
    this.ClearDwelling();
  }

  public bool CanLevelUp() => (double) this.Stats.Adoration >= (double) this.Stats.MAX_ADORATION;

  public bool CanGiveSin() => (double) this.Info.Pleasure >= 65.0;

  public static List<FollowerBrain> AllAvailableFollowerBrains()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
        followerBrainList.Add(allBrain);
    }
    return followerBrainList;
  }

  public static bool CanFollowerGiveQuest(FollowerInfo follower)
  {
    if (FollowerManager.FollowerLocked(in follower.ID, exludeChild: true) || follower.CursedState != Thought.None && follower.CursedState != Thought.Child || follower.Traits.Contains(FollowerTrait.TraitType.Mutated) && follower.CursedState != Thought.Child)
      return false;
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
    if (brain != null && brain.CurrentTask != null && brain.CanLevelUp() || brain != null && brain.CurrentTask != null && brain.CurrentTask.BlockTaskChanges && brain.CurrentTask.Type != FollowerTaskType.Child && brain.Info.ID != 100000 || (double) brain.Stats.Adoration >= (double) brain.Stats.MAX_ADORATION || follower.IsSnowman && follower.CursedState != Thought.Child)
      return false;
    if (!FollowerManager.UniqueFollowerIDs.Contains(follower.ID))
    {
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective.Follower == follower.ID)
          return false;
      }
      foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
      {
        if (completedObjective.Follower == follower.ID)
          return false;
      }
    }
    return true;
  }

  public static bool CanContinueToGiveQuest(FollowerInfo follower, bool exludeChild = false)
  {
    if (FollowerManager.FollowerLocked(in follower.ID, exludeChild: in exludeChild) || follower.CursedState != Thought.None && follower.CursedState != Thought.Child)
      return false;
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
    return brain == null || !brain.CanLevelUp();
  }

  public static FollowerBrain RandomAvailableBrainNoCurseState(List<FollowerBrain> brainsToCheck)
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain followerBrain in brainsToCheck)
    {
      if (!FollowerManager.FollowerLocked(followerBrain.Info.ID) && followerBrain.Info.CursedState == Thought.None && TimeManager.CurrentDay - followerBrain._directInfoAccess.DayJoined >= 2)
        followerBrainList.Add(followerBrain);
    }
    return followerBrainList.Count <= 0 ? (FollowerBrain) null : followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
  }

  public static FollowerBrain RandomAvailableBrainToFreeze()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && TimeManager.CurrentDay - allBrain._directInfoAccess.DayJoined >= 2 && allBrain.CanFreeze())
      {
        if (allBrain.HasTrait(FollowerTrait.TraitType.ColdBlooded) && (double) UnityEngine.Random.value < 0.25 || allBrain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Targeted)
          return allBrain;
        followerBrainList.Add(allBrain);
      }
    }
    return followerBrainList.Count <= 0 ? (FollowerBrain) null : followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
  }

  public static FollowerBrain ClosestAvailableBrainNoCurse()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.CursedState == Thought.None && allBrain.CurrentTaskType != FollowerTaskType.GetPlayerAttention)
        followerBrainList.Add(allBrain);
    }
    float num1 = float.MaxValue;
    FollowerBrain followerBrain1 = (FollowerBrain) null;
    foreach (FollowerBrain followerBrain2 in followerBrainList)
    {
      float num2 = Vector3.Distance(followerBrain2.LastPosition, PlayerFarming.Instance.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        followerBrain1 = followerBrain2;
      }
    }
    return followerBrain1;
  }

  public static FollowerBrain AddBrain(FollowerInfo info)
  {
    FollowerBrain followerBrain = new FollowerBrain(info);
    FollowerBrain.AllBrains.Add(followerBrain);
    FollowerBrain.AllBrains.Sort((Comparison<FollowerBrain>) ((a, b) =>
    {
      if (a.Info == null && b.Info == null)
        return 0;
      if (a.Info == null && b.Info != null)
        return -1;
      return a.Info != null && b.Info == null ? 1 : a.Info.ID.CompareTo(b.Info.ID);
    }));
    Action<int> onBrainAdded = FollowerBrain.OnBrainAdded;
    if (onBrainAdded != null)
      onBrainAdded(info.ID);
    return followerBrain;
  }

  public static void RemoveBrain(int id) => FollowerBrain.RemoveBrain(id, false);

  public static void RemoveBrain(int ID, bool isReset)
  {
    for (int index = FollowerBrain.AllBrains.Count - 1; index >= 0; --index)
    {
      if (FollowerBrain.AllBrains[index].Info != null)
      {
        if (FollowerBrain.AllBrains[index].Info.ID == ID)
        {
          if (!isReset && FollowerBrain.AllBrains[index]._directInfoAccess != null && FollowerBrain.AllBrains[index]._directInfoAccess.SpouseFollowerID != -1)
          {
            FollowerBrain.AllBrains[index].ClearMarriageTraits();
            FollowerBrain.AllBrains[index]._directInfoAccess.SpouseFollowerID = -1;
          }
          FollowerBrain.AllBrains.RemoveAt(index);
        }
        else if (!isReset && FollowerBrain.AllBrains[index]._directInfoAccess != null && FollowerBrain.AllBrains[index]._directInfoAccess.SpouseFollowerID == ID)
        {
          IDAndRelationship relationship = FollowerBrain.AllBrains[index].Info.GetOrCreateRelationship(ID);
          if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
            FollowerBrain.AllBrains[index].AddTrait(FollowerTrait.TraitType.HappilyWidowed, true);
          else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
            FollowerBrain.AllBrains[index].AddTrait(FollowerTrait.TraitType.GrievingWidow, true);
          FollowerBrain.AllBrains[index].ClearMarriageTraits(true);
          FollowerBrain.AllBrains[index]._directInfoAccess.SpouseFollowerID = -1;
        }
      }
    }
    Structures_Missionary.RemoveFollower(ID);
    Structures_Prison.RemoveFollower(ID);
    Structures_TraitManipulator.RemoveFollower(ID);
    Structures_Demon_Summoner.RemoveFollower(ID);
    Action<int> onBrainRemoved = FollowerBrain.OnBrainRemoved;
    if (onBrainRemoved == null)
      return;
    onBrainRemoved(ID);
  }

  public static int DiscipleCount()
  {
    int num = 0;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Stats.HasLevelledUp)
        ++num;
    }
    return num;
  }

  public static FollowerBrain FindBrainByID(int ID)
  {
    int num1 = 0;
    int num2 = FollowerBrain.AllBrains.Count;
    int index = (num2 - num1) / 2;
    while (num1 < num2)
    {
      if (FollowerBrain.AllBrains[index].Info != null && FollowerBrain.AllBrains[index].Info.ID == ID)
        return FollowerBrain.AllBrains[index];
      int num3 = FollowerBrain.AllBrains[index].Info == null ? 1 : (FollowerBrain.AllBrains[index].Info.ID > ID ? 1 : 0);
      num1 = num3 != 0 ? num1 : index + 1;
      num2 = num3 != 0 ? index : num2;
      index = num1 + (num2 - num1) / 2;
    }
    return (FollowerBrain) null;
  }

  public static bool FindBrainByLevelExists(int Level)
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.XPLevel >= Level)
        return true;
    }
    return false;
  }

  public static FollowerBrain GetOrCreateBrain(FollowerInfo info)
  {
    return info == null ? (FollowerBrain) null : (info != null ? FollowerBrain.FindBrainByID(info.ID) : (FollowerBrain) null) ?? FollowerBrain.AddBrain(info);
  }

  public static List<FollowerBrain> GetBrainsWithinRadius(Vector3 position, float radius)
  {
    List<FollowerBrain> brainsWithinRadius = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((double) Vector3.Distance(allBrain.LastPosition, position) < (double) radius)
        brainsWithinRadius.Add(allBrain);
    }
    return brainsWithinRadius;
  }

  public static void GetAvailableBrainsWithNecklaceTargeted(
    ref List<FollowerBrain> outBrains,
    bool checkCursedState = true)
  {
    outBrains.Clear();
    for (int index = 0; index < FollowerBrain.AllBrains.Count; ++index)
    {
      bool flag1;
      bool flag2;
      if ((!checkCursedState || FollowerBrain.AllBrains[index].Info.CursedState == Thought.None) && FollowerBrain.AllBrains[index].Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Targeted)
      {
        // ISSUE: explicit reference operation
        ref int local1 = @FollowerBrain.AllBrains[index].Info.ID;
        // ISSUE: explicit reference operation
        ref bool local2 = @!checkCursedState;
        // ISSUE: explicit reference operation
        ref bool local3 = @false;
        // ISSUE: explicit reference operation
        ref bool local4 = @false;
        flag1 = true;
        ref bool local5 = ref flag1;
        flag2 = false;
        ref bool local6 = ref flag2;
        if (!FollowerManager.FollowerLocked(in local1, in local2, in local3, in local4, in local5, in local6) && !FollowerBrain.AllBrains[index].HasTrait(FollowerTrait.TraitType.Mutated))
        {
          outBrains.Add(FollowerBrain.AllBrains[index]);
          continue;
        }
      }
      if (!DataManager.Instance.FirstRotFollowerAilmentAvoided)
      {
        // ISSUE: explicit reference operation
        ref int local7 = @FollowerBrain.AllBrains[index].Info.ID;
        // ISSUE: explicit reference operation
        ref bool local8 = @!checkCursedState;
        // ISSUE: explicit reference operation
        ref bool local9 = @false;
        // ISSUE: explicit reference operation
        ref bool local10 = @false;
        flag1 = true;
        ref bool local11 = ref flag1;
        flag2 = false;
        ref bool local12 = ref flag2;
        if (!FollowerManager.FollowerLocked(in local7, in local8, in local9, in local10, in local11, in local12) && FollowerBrain.AllBrains[index].HasTrait(FollowerTrait.TraitType.Mutated))
        {
          outBrains.Clear();
          outBrains.Add(FollowerBrain.AllBrains[index]);
          DataManager.Instance.FirstRotFollowerAilmentAvoided = true;
          break;
        }
      }
    }
  }

  public static void SetFollowerCostume(
    Skeleton skeleton,
    FollowerInfo info,
    bool hooded = false,
    bool forceUpdate = false,
    bool setData = true)
  {
    FollowerOutfitType outfit1 = info.Outfit;
    FollowerOutfitType outfit2 = info.Outfit;
    FollowerClothingType clothing1 = info.Clothing;
    FollowerClothingType clothing2 = info.Clothing;
    FollowerCustomisationType customisation1 = info.Customisation;
    FollowerCustomisationType customisation2 = info.Customisation;
    FollowerHatType hat1 = info.Hat;
    FollowerHatType hat2 = info.Hat;
    FollowerSpecialType special1 = info.Special;
    FollowerSpecialType special2 = info.Special;
    InventoryItem.ITEM_TYPE necklace1 = info.Necklace;
    InventoryItem.ITEM_TYPE necklace2 = info.Necklace;
    string clothingVariant1 = info.ClothingVariant;
    string clothingVariant2 = info.ClothingVariant;
    if (hooded)
      hat1 = info.CursedState != Thought.OldAge ? FollowerHatType.Hooded : FollowerHatType.OldAge;
    else if (info.NudistWinner && FollowerBrainStats.IsNudism)
      hat1 = FollowerHatType.NudistWinner;
    else if (info.FaithEnforcer || info.TaxEnforcer)
    {
      hat1 = info.FaithEnforcer ? FollowerHatType.FaithEnforcer : FollowerHatType.Enforcer;
    }
    else
    {
      switch (hat1)
      {
        case FollowerHatType.Hooded:
          hat1 = FollowerHatType.None;
          break;
        case FollowerHatType.OldAge:
          hat1 = FollowerHatType.None;
          break;
        case FollowerHatType.NudistWinner:
          hat1 = FollowerHatType.None;
          break;
      }
    }
    if (info.Outfit != FollowerOutfitType.Ghost)
    {
      if (FollowerBrainStats.IsNudism && (DataManager.Instance.Followers.Contains(info) || info.IsFakeBrain))
        outfit1 = FollowerOutfitType.Naked;
      else if (outfit1 != FollowerOutfitType.Rags)
        outfit1 = FollowerBrain.GetOutfitFromCursedState(info);
      else if (info.StartingCursedState != Thought.None)
      {
        FollowerOutfitType outfitFromCursedState = FollowerBrain.GetOutfitFromCursedState(info);
        if (outfitFromCursedState != FollowerOutfitType.None)
          outfit1 = outfitFromCursedState;
      }
    }
    if (DataManager.Instance.Followers_OnMissionary_IDs.Contains(info.ID) || outfit2 == FollowerOutfitType.Sherpa)
      outfit1 = FollowerOutfitType.Sherpa;
    if (info.ID == 99995)
      special1 = clothing1 != FollowerClothingType.None ? FollowerSpecialType.None : FollowerSpecialType.Robes_Aym;
    else if (info.ID == 99994)
      special1 = clothing1 != FollowerClothingType.None ? FollowerSpecialType.None : FollowerSpecialType.Robes_Baal;
    else if (info.ID == 100006 && special1 != FollowerSpecialType.Midas_Arm)
      special1 = clothing1 != FollowerClothingType.None || info.CursedState == Thought.Child ? FollowerSpecialType.None : FollowerSpecialType.Midas;
    else if (info.ID == 99996)
      special1 = DataManager.Instance.SozoNoLongerBrainwashed ? FollowerSpecialType.SozoOld : FollowerSpecialType.Sozo;
    if (outfit1 != FollowerOutfitType.Ghost)
    {
      if (info.CursedState == Thought.Child)
        outfit1 = FollowerBrain.GetOutfitFromCursedState(info);
      else if (info.Location == FollowerLocation.Base && FollowerBrainStats.IsHoliday)
        outfit1 = FollowerOutfitType.Holiday;
      else if (info.CursedState == Thought.OldAge)
        outfit1 = FollowerOutfitType.Old;
      else if (info.CursedState == Thought.Freezing)
        outfit1 = FollowerOutfitType.Freezing;
      else if (info.CursedState == Thought.Overheating)
        outfit1 = FollowerOutfitType.Overheating;
      else if (FollowerBrainStats.BrainWashed && !FollowerBrainStats.IsNudism)
        outfit1 = FollowerOutfitType.Brainwashed;
      else if (DataManager.Instance.Followers_OnMissionary_IDs.Contains(info.ID))
        outfit1 = FollowerOutfitType.Sherpa;
      else if (outfit1 == FollowerOutfitType.Follower || outfit1 == FollowerOutfitType.Worshipper || outfit1 == FollowerOutfitType.Worker)
        outfit1 = FollowerOutfitType.None;
    }
    if (outfit1 == outfit2 && hat1 == hat2 && clothing1 == clothing2 && customisation1 == customisation2 && special1 == special2 && necklace1 == necklace2 && clothingVariant1 == clothingVariant2 && !forceUpdate)
      return;
    if (!info.ShowingNecklace)
      necklace1 = InventoryItem.ITEM_TYPE.NONE;
    if (setData)
    {
      info.Outfit = outfit1;
      info.Hat = hat1;
      info.Clothing = clothing1;
      info.Customisation = customisation1;
      info.Special = special1;
      info.ClothingPreviousVariant = !string.IsNullOrEmpty(info.ClothingVariant) ? info.ClothingVariant : info.ClothingPreviousVariant;
      info.ClothingVariant = clothingVariant1;
    }
    if ((FollowerBrainStats.IsHoliday || FollowerBrainStats.IsNudism) && special1 != FollowerSpecialType.SozoOld && (special1 == FollowerSpecialType.Robes_Aym || special1 == FollowerSpecialType.Robes_Baal || special1 == FollowerSpecialType.Sozo))
      special1 = FollowerSpecialType.None;
    if (skeleton == null || skeleton.Data == null)
      return;
    FollowerBrain.SetFollowerCostume(skeleton, info.XPLevel, info.SkinName, info.SkinColour, outfit1, hat1, clothing1, customisation1, special1, necklace1, info.ClothingVariant, info);
  }

  public static Skin SetFollowerCostume(
    Skeleton skeleton,
    int followerLevel,
    string skinName,
    int skinColor,
    FollowerOutfitType outfit,
    FollowerHatType hat,
    FollowerClothingType clothing,
    FollowerCustomisationType customisation,
    FollowerSpecialType special,
    InventoryItem.ITEM_TYPE necklace,
    string variant = "",
    FollowerInfo info = null)
  {
    Skin newSkin = new Skin("New Skin");
    Skin skin = skeleton.Data.FindSkin(skinName);
    ClothingData clothingData = TailorManager.GetClothingData(clothing);
    if (special == FollowerSpecialType.Snowman_Great || special == FollowerSpecialType.Snowman_Average || special == FollowerSpecialType.Snowman_Bad)
    {
      skin = skeleton.Data.FindSkin("Cat");
      outfit = FollowerOutfitType.None;
    }
    if (skin != null)
      newSkin.AddSkin(skin);
    else
      newSkin.AddSkin(skeleton.Data.FindSkin("Cat"));
    string skinName1 = string.IsNullOrEmpty(variant) ? FollowerBrain.GetClothingName(clothing) : variant;
    if (!string.IsNullOrEmpty(skinName1) && (special == FollowerSpecialType.None || clothing != FollowerClothingType.None))
      newSkin.AddSkin(skeleton.Data.FindSkin(skinName1));
    if (necklace != InventoryItem.ITEM_TYPE.NONE)
      newSkin.AddSkin(skeleton.Data.FindSkin(FollowerBrain.GetNecklaceName(necklace)));
    if (info != null && info.BornInCult && necklace == InventoryItem.ITEM_TYPE.NONE)
      newSkin.AddSkin(skeleton.Data.FindSkin("Necklaces/Necklace_Bell"));
    if (outfit == FollowerOutfitType.Old && clothing == FollowerClothingType.None && special != FollowerSpecialType.SozoOld)
      newSkin.AddSkin(skeleton.Data.FindSkin("Clothes/Robes_Old"));
    if (outfit != FollowerOutfitType.None)
      newSkin.AddSkin(skeleton.Data.FindSkin(FollowerBrain.GetOutfitName(outfit)));
    if (outfit == FollowerOutfitType.Child && clothing == FollowerClothingType.None)
      newSkin.AddSkin(skeleton.Data.FindSkin("Clothes/Baby"));
    if ((special == FollowerSpecialType.None || special == FollowerSpecialType.Skinny || special == FollowerSpecialType.Fat) && info != null)
      special = info.CursedState == Thought.Child || !info.Traits.Contains(FollowerTrait.TraitType.Hibernation) ? (info.CursedState == Thought.Child || !info.Traits.Contains(FollowerTrait.TraitType.Aestivation) ? FollowerSpecialType.None : (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || TimeManager.CurrentDay != SeasonsManager.SeasonTimestamp ? (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 0.25 ? FollowerSpecialType.None : FollowerSpecialType.Skinny) : FollowerSpecialType.Fat)) : (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || TimeManager.CurrentDay != SeasonsManager.SeasonTimestamp ? (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 0.25 ? FollowerSpecialType.None : FollowerSpecialType.Skinny) : FollowerSpecialType.Fat);
    if (special == FollowerSpecialType.Rot || info != null && info.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      UnityEngine.Random.InitState(info.ID);
      if (info.RottingUnique)
        newSkin.AddSkin(skeleton.Data.FindSkin("Mutation/Dark"));
      else
        newSkin.AddSkin(skeleton.Data.FindSkin($"Mutation/{UnityEngine.Random.Range(1, 6)}"));
    }
    else
    {
      switch (special)
      {
        case FollowerSpecialType.None:
        case FollowerSpecialType.Sozo:
          break;
        case FollowerSpecialType.SozoOld:
          newSkin.AddSkin(skeleton.Data.FindSkin("SozoOld"));
          if (outfit == FollowerOutfitType.Old)
            newSkin.AddSkin(skeleton.Data.FindSkin("Other/Old"));
          if (!string.IsNullOrEmpty(skinName1) && (special == FollowerSpecialType.None || clothing != FollowerClothingType.None))
            newSkin.AddSkin(skeleton.Data.FindSkin(skinName1));
          if (outfit != FollowerOutfitType.None && outfit != FollowerOutfitType.Old)
          {
            newSkin.AddSkin(skeleton.Data.FindSkin(FollowerBrain.GetOutfitName(outfit)));
            break;
          }
          break;
        case FollowerSpecialType.Palworld_Dragon:
        case FollowerSpecialType.Palworld_Frozen:
        case FollowerSpecialType.Palworld_Rocky:
        case FollowerSpecialType.Palworld_Dark:
        case FollowerSpecialType.Midas_Arm:
        case FollowerSpecialType.Gold:
          newSkin.AddSkin(skeleton.Data.FindSkin($"Eggs/{special}"));
          break;
        case FollowerSpecialType.Snowman_Great:
        case FollowerSpecialType.Snowman_Average:
        case FollowerSpecialType.Snowman_Bad:
          newSkin.AddSkin(skeleton.Data.FindSkin(skinName));
          break;
        case FollowerSpecialType.Fat:
          newSkin.AddSkin(skeleton.Data.FindSkin("Other/HibernateFat"));
          break;
        case FollowerSpecialType.Skinny:
          newSkin.AddSkin(skeleton.Data.FindSkin("Other/HibernateThin"));
          break;
        default:
          if (clothing == FollowerClothingType.None)
          {
            newSkin.AddSkin(skeleton.Data.FindSkin($"Clothes/{special}"));
            break;
          }
          break;
      }
    }
    if (info != null && (outfit == FollowerOutfitType.Child || outfit == FollowerOutfitType.Old) && info.Traits.Contains(FollowerTrait.TraitType.Zombie))
      newSkin.AddSkin(skeleton.Data.FindSkin("Other/Zombie"));
    if (hat == FollowerHatType.Hooded || hat == FollowerHatType.OldAge)
      newSkin.AddSkin(skeleton.Data.FindSkin(FollowerBrain.GetRobesName(followerLevel, info != null && info.CursedState == Thought.OldAge)));
    if (hat != FollowerHatType.None)
      newSkin.AddSkin(skeleton.Data.FindSkin(FollowerBrain.GetHatName(hat, followerLevel)));
    if (customisation != FollowerCustomisationType.None)
      newSkin.AddSkin(skeleton.Data.FindSkin(FollowerBrain.GetCustomisationName(customisation)));
    skeleton.SetSkin(newSkin);
    skeleton.SetSlotsToSetupPose();
    if (info != null)
      FollowerBrain.AddAttachments(skeleton, info);
    if (special != FollowerSpecialType.Snowman_Great && special != FollowerSpecialType.Snowman_Average && special != FollowerSpecialType.Snowman_Bad && special != FollowerSpecialType.Rot && (info == null || !info.Traits.Contains(FollowerTrait.TraitType.Mutated)))
    {
      WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(skinName);
      if (colourData != null)
      {
        foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(skinColor, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
        {
          Slot slot = skeleton.FindSlot(slotAndColour.Slot);
          if (slot != null)
            slot.SetColor(slotAndColour.color);
        }
      }
    }
    Color color = Color.white;
    bool flag = true;
    if (special == FollowerSpecialType.Snowman_Bad || special == FollowerSpecialType.Snowman_Average || special == FollowerSpecialType.Snowman_Great)
      flag = false;
    else if (special == FollowerSpecialType.Rot || info != null && info.Traits.Contains(FollowerTrait.TraitType.Mutated))
      flag = false;
    else if (special != FollowerSpecialType.None || outfit == FollowerOutfitType.Rags || outfit == FollowerOutfitType.Holiday || outfit == FollowerOutfitType.Child || outfit == FollowerOutfitType.Old && clothing == FollowerClothingType.None)
    {
      if (special != FollowerSpecialType.Sozo && special != FollowerSpecialType.SozoOld && special != FollowerSpecialType.Midas && special != FollowerSpecialType.Midas_Arm || clothing == FollowerClothingType.None)
        flag = false;
    }
    else if (outfit == FollowerOutfitType.Naked && FollowerBrainStats.IsNudism)
    {
      flag = false;
      color = TailorManager.GetClothingData(FollowerClothingType.Naked).SlotAndColours[0].SlotAndColours[0].color;
    }
    if ((UnityEngine.Object) clothingData != (UnityEngine.Object) null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in clothingData.SlotAndColours[Mathf.Min(DataManager.Instance.GetClothingColour(clothing), clothingData.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(flag ? slotAndColour.color : color);
      }
    }
    return newSkin;
  }

  public static string GetNecklaceName(InventoryItem.ITEM_TYPE necklace)
  {
    switch (necklace)
    {
      case InventoryItem.ITEM_TYPE.Necklace_Deaths_Door:
        return "Necklaces/Necklace_13";
      case InventoryItem.ITEM_TYPE.Necklace_Winter:
        return "Necklaces/Necklace_14";
      case InventoryItem.ITEM_TYPE.Necklace_Frozen:
        return "Necklaces/Necklace_15";
      case InventoryItem.ITEM_TYPE.Necklace_Weird:
        return "Necklaces/Necklace_16";
      case InventoryItem.ITEM_TYPE.Necklace_Targeted:
        return "Necklaces/Necklace_17";
      default:
        return $"Necklaces/{necklace}";
    }
  }

  public static FollowerOutfitType GetOutfitFromCursedState(FollowerInfo info)
  {
    if (info.StartingCursedState != Thought.None)
      return FollowerBrain.GetOutfitFromCursedState(info.StartingCursedState);
    if (info.CursedState == Thought.OldAge)
      return FollowerOutfitType.Old;
    if (FollowerBrainStats.BrainWashed && info.CursedState != Thought.Child || info.SozoBrainshed && info.CursedState == Thought.None)
      return FollowerOutfitType.Brainwashed;
    return info.Traits.Contains(FollowerTrait.TraitType.Zombie) && info.CursedState != Thought.Child ? FollowerOutfitType.Zombie : FollowerBrain.GetOutfitFromCursedState(info.CursedState);
  }

  public static FollowerOutfitType GetOutfitFromCursedState(Thought cursedState)
  {
    FollowerOutfitType outfitFromCursedState;
    switch (cursedState)
    {
      case Thought.Dissenter:
        outfitFromCursedState = FollowerOutfitType.Dissenter;
        break;
      case Thought.OldAge:
        outfitFromCursedState = FollowerOutfitType.Old;
        break;
      case Thought.Injured:
        outfitFromCursedState = FollowerOutfitType.Injured;
        break;
      case Thought.Child:
        outfitFromCursedState = FollowerOutfitType.Child;
        break;
      default:
        outfitFromCursedState = FollowerOutfitType.None;
        break;
    }
    return outfitFromCursedState;
  }

  public static string GetOutfitName(FollowerOutfitType outfit)
  {
    string str = "Clothes/";
    string outfitName;
    switch (outfit)
    {
      case FollowerOutfitType.Follower:
      case FollowerOutfitType.Worker:
        outfitName = str + "Robes_Lvl1";
        break;
      case FollowerOutfitType.Worshipper:
        outfitName = str + "Robes_Lvl3";
        break;
      case FollowerOutfitType.Old:
        return "Other/Old";
      case FollowerOutfitType.Ghost:
        return "Other/Ghost";
      case FollowerOutfitType.Dissenter:
        return "Other/Dissenter";
      case FollowerOutfitType.Brainwashed:
        return "Other/Brainwashed";
      case FollowerOutfitType.Freezing:
        return "Other/Freezing";
      case FollowerOutfitType.Overheating:
        return "Other/Overheated";
      case FollowerOutfitType.Naked:
        outfitName = str + "Naked_Base";
        break;
      case FollowerOutfitType.Injured:
        return "Other/Injured";
      case FollowerOutfitType.Child:
        return "Other/Baby";
      case FollowerOutfitType.Zombie:
        return "Other/Zombie";
      default:
        outfitName = str + outfit.ToString();
        break;
    }
    return outfitName;
  }

  public static string GetClothingName(FollowerClothingType clothing)
  {
    string str = "Clothes/";
    string clothingName;
    switch (clothing)
    {
      case FollowerClothingType.None:
        return FollowerBrain.GetOutfitName(FollowerOutfitType.Follower);
      case FollowerClothingType.Jumper:
        clothingName = str + "Winter1";
        break;
      case FollowerClothingType.Singlet:
        clothingName = str + "Summer1";
        break;
      case FollowerClothingType.Robes_Fancy:
        clothingName = str + "WeddingDress1";
        break;
      case FollowerClothingType.Raincoat:
        clothingName = str + "Rain1";
        break;
      case FollowerClothingType.Suit_Fancy:
        clothingName = str + "WeddingSuit1";
        break;
      case FollowerClothingType.Cultist_DLC:
        clothingName = str + "Cultist_1";
        break;
      case FollowerClothingType.Cultist_DLC2:
        clothingName = str + "Cultist_2";
        break;
      case FollowerClothingType.Heretic_DLC:
        clothingName = str + "Heretic_1";
        break;
      case FollowerClothingType.Heretic_DLC2:
        clothingName = str + "Heretic_2";
        break;
      default:
        clothingName = str + clothing.ToString();
        break;
    }
    return clothingName;
  }

  public static string GetCustomisationName(FollowerCustomisationType customisation)
  {
    return customisation == FollowerCustomisationType.Scarf ? "Necklaces/Scarf" : "";
  }

  public static string GetHatName(FollowerHatType hat, int followerLevel)
  {
    switch (hat)
    {
      case FollowerHatType.Hooded:
        if (followerLevel == 2)
          return "Clothes/Hooded_Lvl2";
        if (followerLevel == 3)
          return "Clothes/Hooded_Lvl3";
        if (followerLevel == 4)
          return "Clothes/Hooded_Lvl4";
        return followerLevel >= 5 ? "Clothes/Hooded_Lvl5" : "Clothes/Hooded_Lvl1";
      case FollowerHatType.OldAge:
        return "Clothes/Hooded_HorseTown";
      case FollowerHatType.NudistWinner:
        return "Hats/Nudist";
      default:
        return $"Hats/{hat}";
    }
  }

  public static string GetRobesName(int followerLevel, bool isOld)
  {
    if (isOld)
      return "Clothes/Robes_Old";
    switch (followerLevel)
    {
      case 3:
      case 4:
        return "Clothes/Robes_Lvl4";
      case 5:
      case 6:
        return "Clothes/Robes_Lvl5";
      default:
        return "Clothes/Robes_Lvl3";
    }
  }

  public static void AddAttachments(Skeleton skeleton, FollowerInfo info)
  {
    if (!info.IsDisciple)
      return;
    skeleton.SetAttachment("halo", "Other/Halo");
  }

  public void AssignClothing(FollowerClothingType clothingType, string variant = "")
  {
    this.Info.Clothing = clothingType;
    this.Info.ClothingVariant = clothingType != FollowerClothingType.None || !string.IsNullOrEmpty(variant) ? variant : this.Info.ClothingPreviousVariant;
    if (string.IsNullOrEmpty(this.Info.ClothingVariant))
      this.Info.ClothingVariant = "";
    if (this.Info.Protection != FollowerProtectionType.Rain || this.Info.CursedState != Thought.Soaking)
      return;
    this.RemoveCurseState(Thought.Soaking);
  }

  public void AssignClothing(FollowerClothingType clothingType, string variant, Skeleton skeleton)
  {
    this.AssignClothing(clothingType, variant);
    FollowerBrain.SetFollowerCostume(skeleton, this._directInfoAccess, forceUpdate: true);
  }

  public StructureBrain.TYPES GetPoopType()
  {
    StructureBrain.TYPES poopType = StructureBrain.TYPES.POOP;
    if (this.Info.SkinName.Contains("Poop") && FollowerPet.GetPetCount(FollowerPet.FollowerPetType.Poop) == 0)
      poopType = StructureBrain.TYPES.POOP_PET;
    else if (this.Info.HasTrait(FollowerTrait.TraitType.RoyalPooper))
      poopType = StructureBrain.TYPES.POOP_GOLD;
    else if (this.Info.HasTrait(FollowerTrait.TraitType.RotstonePooper) || this.Info.HasTrait(FollowerTrait.TraitType.Mutated))
    {
      poopType = StructureBrain.TYPES.POOP_ROTSTONE;
    }
    else
    {
      float num1 = 0.075f;
      if (TimeManager.CurrentDay - DataManager.Instance.DaySinceLastSpecialPoop > 5)
        num1 = 1f;
      if ((double) UnityEngine.Random.value < (double) num1 && TimeManager.CurrentDay >= 5 || DataManager.Instance.ForceSpecialPoo)
      {
        float num2 = UnityEngine.Random.value;
        poopType = (double) num2 >= 0.30000001192092896 ? ((double) num2 >= 0.60000002384185791 ? ((double) num2 >= 0.800000011920929 ? ((double) num2 >= 0.89999997615814209 || !DataManager.Instance.HasBuiltShrine1 ? StructureBrain.TYPES.POOP_MASSIVE : StructureBrain.TYPES.POOP_DEVOTION) : StructureBrain.TYPES.POOP_GLOW) : StructureBrain.TYPES.POOP_RAINBOW) : StructureBrain.TYPES.POOP_GOLD;
      }
    }
    if (poopType != StructureBrain.TYPES.POOP)
      DataManager.Instance.DaySinceLastSpecialPoop = TimeManager.CurrentDay;
    DataManager.Instance.ForceSpecialPoo = false;
    return poopType;
  }

  public event FollowerBrain.PleasureEvent OnPleasureAdded;

  public static int GetPleasureAmount(FollowerBrain.PleasureActions pleasureAction)
  {
    return FollowerBrain.PleasureAndActions[pleasureAction];
  }

  public void AddPleasure(FollowerBrain.PleasureActions pleasureAction, float multiplier = 1f)
  {
    if (!DataManager.Instance.PleasureEnabled)
      return;
    this.AddPleasure(Mathf.RoundToInt((float) FollowerBrain.PleasureAndActions[pleasureAction] * multiplier));
  }

  public void AddPleasureInt(int amount) => this.AddPleasure(amount);

  public void AddPleasure(int amount)
  {
    if (!DataManager.Instance.PleasureEnabled || this._directInfoAccess.IsSnowman)
      return;
    Debug.Log((object) "AddPleasure!".Colour(Color.yellow));
    float num = 1f;
    if (this.HasTrait(FollowerTrait.TraitType.Hedonism))
      num += 0.15f;
    if (this.HasTrait(FollowerTrait.TraitType.Asceticism))
      num -= 0.1f;
    if (this.HasTrait(FollowerTrait.TraitType.Virtuous))
      num -= 0.2f;
    if (this.HasTrait(FollowerTrait.TraitType.Unrepentant))
      num += 0.15f;
    if (this.HasTrait(FollowerTrait.TraitType.HappilyWidowed))
      num += 0.1f;
    if (this.HasTrait(FollowerTrait.TraitType.GrievingWidow))
      num -= 0.1f;
    amount = Mathf.RoundToInt((float) amount * num);
    this.Info.Pleasure = Mathf.Clamp(this.Info.Pleasure + amount, 0, 65);
    this.Info.TotalPleasure += amount;
    FollowerBrain.PleasureEvent onPleasureAdded = this.OnPleasureAdded;
    if (onPleasureAdded == null)
      return;
    onPleasureAdded();
  }

  public static void UpdateInactiveFollowersThought(Thought thought)
  {
    if (thought != Thought.Cult_NudistRitual || FollowerBrainStats.IsNudism)
      return;
    List<int> followersOnMissionaryIds = DataManager.Instance.Followers_OnMissionary_IDs;
    followersOnMissionaryIds.AddRange((IEnumerable<int>) DataManager.Instance.Followers_LeftInTheDungeon_IDs);
    for (int index = 0; index < followersOnMissionaryIds.Count; ++index)
    {
      FollowerBrain brainById = FollowerBrain.FindBrainByID(followersOnMissionaryIds[index]);
      if (brainById != null && brainById.Info.NudistWinner)
      {
        brainById.Info.NudistWinner = false;
        brainById.Info.Hat = FollowerHatType.None;
        brainById.CheckChangeState();
      }
    }
  }

  public void AddRandomThoughtFromList(params Thought[] thoughts)
  {
    this.AddThought(thoughts[UnityEngine.Random.Range(0, thoughts.Length)]);
  }

  public void CheckWinterThoughts()
  {
    this.AddRandomThoughtFromList(Thought.WinterStarted_1, Thought.WinterStarted_2, Thought.WinterStarted_3);
    if (this.HasTrait(FollowerTrait.TraitType.ColdEnthusiast) || this.HasTrait(FollowerTrait.TraitType.WarmBlooded) || this.HasTrait(FollowerTrait.TraitType.Chionophile) || this.HasTrait(FollowerTrait.TraitType.WinterExcited) || this.HasTrait(FollowerTrait.TraitType.WinterBody))
      this.AddRandomThoughtFromList(Thought.ColdPositive_1, Thought.ColdPositive_2, Thought.ColdPositive_3);
    else if (this.HasTrait(FollowerTrait.TraitType.ColdBlooded))
      this.AddRandomThoughtFromList(Thought.ColdNegative_1, Thought.ColdNegative_2);
    if (this.HasTrait(FollowerTrait.TraitType.Aestivation))
      this.AddThought(Thought.Aestivation);
    if (!this.HasTrait(FollowerTrait.TraitType.Poet) && !this.HasTrait(FollowerTrait.TraitType.IceSculpture))
      return;
    this.AddRandomThoughtFromList(Thought.WinterInspired_1, Thought.WinterInspired_2);
  }

  public void CheckNonWinterThoughts()
  {
    this.AddRandomThoughtFromList(Thought.WinterEnded_1, Thought.WinterEnded_2, Thought.WinterEnded_3);
    if (this.HasTrait(FollowerTrait.TraitType.Hibernation))
    {
      this.AddRandomThoughtFromList(Thought.Hibernation, Thought.AfterHibernation);
      this.Info.Special = FollowerSpecialType.Skinny;
      if (!GameManager.IsDungeon(PlayerFarming.Location))
      {
        Follower followerById = FollowerManager.FindFollowerByID(this.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
          FollowerBrain.SetFollowerCostume(followerById.Spine.Skeleton, this._directInfoAccess, forceUpdate: true);
      }
    }
    if (this.Info.ID != 100007)
      return;
    this.AddThought(Thought.Yngya_3);
  }

  [CompilerGenerated]
  public int \u003CCheckEatTask\u003Eb__280_0(StructureBrain s)
  {
    int num;
    switch (s)
    {
      case Structures_Meal _:
        num = 1;
        break;
      case Structures_Kitchen kitchen:
        num = this.GetUnreservedMealCountFromKitchen(in kitchen);
        break;
      default:
        num = 0;
        break;
    }
    return num;
  }

  public enum AdorationActions
  {
    Sermon,
    BigGift,
    Gift,
    Necklace,
    Quest,
    Inspire,
    Bribe,
    Intimidate,
    ConfessionBooth,
    Bless,
    Ritual_AlmsToPoor,
    FaithEnforce,
    AscendedFollower_Lvl2,
    HappyFollowerNPC,
    FirePit,
    FightPitMercy,
    FightPitDeath,
    AscendedFollower_Lvl3,
    AscendedFollower_Lvl4,
    AscendedFollower_Lvl5,
    SmoochSpouse,
    AssignEnforcer,
    PetDog,
    BlessLvl1,
    SermonLvl1,
    BribeLvl1,
    InspireLvl1,
    IntimidateLvl1,
    BigGiftLvl1,
    GiftLvl1,
    NecklaceLvl1,
    LevelUp,
    BecomeDisciple,
    Bully,
    WonFight,
    CuddleBaby,
    Reassure,
    KB_Win,
    KB_Draw,
    KB_Lose,
    Frigidophile,
    LockedLoyal,
    GiftPet,
    WinterExcited,
  }

  public struct PendingTaskData
  {
    public bool KeepExistingTask;
    public FollowerTask Task;
    public int ListIndex;
  }

  public delegate void CursedEvent();

  public delegate void DwellingAssignmentChanged(int followerID, Dwelling.DwellingAndSlot d);

  public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
  {
    public int Compare(TKey x, TKey y)
    {
      int num = x.CompareTo((object) y);
      return num == 0 ? -1 : -num;
    }
  }

  public delegate void PleasureEvent();

  public enum PleasureActions
  {
    Drink_Beer,
    Drink_Cocktail,
    Drink_Gin,
    Drink_Wine,
    Drink_Eggnog,
    Drink_Poop_Juice,
    Drink_Mushroom_Juice,
    DestroyStructure,
    Single,
    DoctrinalExtremist,
    ViolentExtremist,
    FertilityTrait,
    DeathTrait,
    Testing,
    NudismWinner,
    NudismDrunk,
    RemovedDissent,
    ConfessionBooth,
    Purge,
    PurgeDissenter,
    Cannibal,
    DrinkingFestival,
    Testing_Half,
    Murderer,
    DrumCircle,
    Twitch,
    Zero,
    SinNPC,
    Masochistic,
    Drink_Chilli,
    Drink_Lightning,
    Drink_Sin,
    Drink_Grass,
    GroupSpa,
  }
}
