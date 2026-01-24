// Decompiled with JetBrains decompiler
// Type: FollowerTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
[Serializable]
public class FollowerTrait
{
  public FollowerTrait.TraitType Type;
  public static Dictionary<FollowerTrait.TraitType, FollowerTrait.TraitType> ExclusiveTraits = new Dictionary<FollowerTrait.TraitType, FollowerTrait.TraitType>()
  {
    {
      FollowerTrait.TraitType.Germophobe,
      FollowerTrait.TraitType.Coprophiliac
    },
    {
      FollowerTrait.TraitType.FearOfSickPeople,
      FollowerTrait.TraitType.LoveOfSickPeople
    },
    {
      FollowerTrait.TraitType.Cynical,
      FollowerTrait.TraitType.Gullible
    },
    {
      FollowerTrait.TraitType.Disciplinarian,
      FollowerTrait.TraitType.Libertarian
    },
    {
      FollowerTrait.TraitType.Sickly,
      FollowerTrait.TraitType.IronStomach
    },
    {
      FollowerTrait.TraitType.NaturallyObedient,
      FollowerTrait.TraitType.NaturallySkeptical
    },
    {
      FollowerTrait.TraitType.SacrificeEnthusiast,
      FollowerTrait.TraitType.AgainstSacrifice
    },
    {
      FollowerTrait.TraitType.Faithful,
      FollowerTrait.TraitType.Faithless
    },
    {
      FollowerTrait.TraitType.Industrious,
      FollowerTrait.TraitType.Lazy
    },
    {
      FollowerTrait.TraitType.MushroomEncouraged,
      FollowerTrait.TraitType.MushroomBanned
    },
    {
      FollowerTrait.TraitType.LoveElderly,
      FollowerTrait.TraitType.HateElderly
    },
    {
      FollowerTrait.TraitType.FearOfDeath,
      FollowerTrait.TraitType.DesensitisedToDeath
    },
    {
      FollowerTrait.TraitType.Hedonism,
      FollowerTrait.TraitType.Asceticism
    },
    {
      FollowerTrait.TraitType.Virtuous,
      FollowerTrait.TraitType.Unrepentant
    },
    {
      FollowerTrait.TraitType.Celibate,
      FollowerTrait.TraitType.Lustful
    },
    {
      FollowerTrait.TraitType.CriminalReformed,
      FollowerTrait.TraitType.Lazy
    },
    {
      FollowerTrait.TraitType.MarriedUnhappily,
      FollowerTrait.TraitType.MarriedHappily
    },
    {
      FollowerTrait.TraitType.ProudParent,
      FollowerTrait.TraitType.OverworkedParent
    },
    {
      FollowerTrait.TraitType.Drowsy,
      FollowerTrait.TraitType.Industrious
    },
    {
      FollowerTrait.TraitType.ColdBlooded,
      FollowerTrait.TraitType.WarmBlooded
    },
    {
      FollowerTrait.TraitType.HappilyWidowed,
      FollowerTrait.TraitType.GrievingWidow
    },
    {
      FollowerTrait.TraitType.LockedLoyal,
      FollowerTrait.TraitType.Masochistic
    },
    {
      FollowerTrait.TraitType.Heavyweight,
      FollowerTrait.TraitType.Lightweight
    },
    {
      FollowerTrait.TraitType.Hibernation,
      FollowerTrait.TraitType.Aestivation
    },
    {
      FollowerTrait.TraitType.Mutated,
      FollowerTrait.TraitType.MutatedImmune
    },
    {
      FollowerTrait.TraitType.MutatedVisual,
      FollowerTrait.TraitType.Mutated
    },
    {
      FollowerTrait.TraitType.MutatedImmune,
      FollowerTrait.TraitType.MutatedVisual
    },
    {
      FollowerTrait.TraitType.EmbraceRot,
      FollowerTrait.TraitType.RejectRot
    },
    {
      FollowerTrait.TraitType.MasterfulSnowman,
      FollowerTrait.TraitType.ShoddySnowman
    },
    {
      FollowerTrait.TraitType.Pacifist,
      FollowerTrait.TraitType.Argumentative
    }
  };
  public static List<FollowerTrait.TraitType> StartingTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Germophobe,
    FollowerTrait.TraitType.Coprophiliac,
    FollowerTrait.TraitType.Cynical,
    FollowerTrait.TraitType.Gullible,
    FollowerTrait.TraitType.NaturallySkeptical,
    FollowerTrait.TraitType.NaturallyObedient,
    FollowerTrait.TraitType.FearOfDeath,
    FollowerTrait.TraitType.AgainstSacrifice,
    FollowerTrait.TraitType.Sickly,
    FollowerTrait.TraitType.IronStomach,
    FollowerTrait.TraitType.Materialistic,
    FollowerTrait.TraitType.Zealous,
    FollowerTrait.TraitType.Lazy,
    FollowerTrait.TraitType.Celibate,
    FollowerTrait.TraitType.Lustful,
    FollowerTrait.TraitType.Fashionable,
    FollowerTrait.TraitType.Hedonism,
    FollowerTrait.TraitType.Asceticism,
    FollowerTrait.TraitType.Virtuous,
    FollowerTrait.TraitType.Unrepentant,
    FollowerTrait.TraitType.Snorer,
    FollowerTrait.TraitType.Unlawful,
    FollowerTrait.TraitType.Insomniac,
    FollowerTrait.TraitType.Pettable,
    FollowerTrait.TraitType.Polyamory,
    FollowerTrait.TraitType.WinterExcited,
    FollowerTrait.TraitType.Frigidophile,
    FollowerTrait.TraitType.Hibernation,
    FollowerTrait.TraitType.Aestivation,
    FollowerTrait.TraitType.WinterBody,
    FollowerTrait.TraitType.AnimalLover,
    FollowerTrait.TraitType.LightningEnthusiast,
    FollowerTrait.TraitType.HatesSnowmen,
    FollowerTrait.TraitType.WolfHater,
    FollowerTrait.TraitType.DeepSleeper,
    FollowerTrait.TraitType.LockedLoyal,
    FollowerTrait.TraitType.Masochistic,
    FollowerTrait.TraitType.SeenGod,
    FollowerTrait.TraitType.Pacifist,
    FollowerTrait.TraitType.IceSculpture,
    FollowerTrait.TraitType.Heavyweight,
    FollowerTrait.TraitType.Lightweight
  };
  public static List<FollowerTrait.TraitType> FaithfulTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Faithful,
    FollowerTrait.TraitType.Coprophiliac,
    FollowerTrait.TraitType.Gullible,
    FollowerTrait.TraitType.NaturallyObedient,
    FollowerTrait.TraitType.IronStomach,
    FollowerTrait.TraitType.Zealous,
    FollowerTrait.TraitType.Coprophiliac,
    FollowerTrait.TraitType.LoveOfSickPeople,
    FollowerTrait.TraitType.Gullible,
    FollowerTrait.TraitType.RoyalPooper,
    FollowerTrait.TraitType.Polyamory,
    FollowerTrait.TraitType.Pettable,
    FollowerTrait.TraitType.WarmBlooded
  };
  public static List<FollowerTrait.TraitType> GoodTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.FalseIdols,
    FollowerTrait.TraitType.Cannibal,
    FollowerTrait.TraitType.Gullible,
    FollowerTrait.TraitType.ConstructionEnthusiast,
    FollowerTrait.TraitType.Coprophiliac,
    FollowerTrait.TraitType.DesensitisedToDeath,
    FollowerTrait.TraitType.Disciplinarian,
    FollowerTrait.TraitType.Faithful,
    FollowerTrait.TraitType.HateElderly,
    FollowerTrait.TraitType.Industrious,
    FollowerTrait.TraitType.IronStomach,
    FollowerTrait.TraitType.Libertarian,
    FollowerTrait.TraitType.LoveElderly,
    FollowerTrait.TraitType.LoveOfSickPeople,
    FollowerTrait.TraitType.Materialistic,
    FollowerTrait.TraitType.MushroomBanned,
    FollowerTrait.TraitType.MushroomEncouraged,
    FollowerTrait.TraitType.NaturallyObedient,
    FollowerTrait.TraitType.SacrificeEnthusiast,
    FollowerTrait.TraitType.Zealous,
    FollowerTrait.TraitType.Immortal,
    FollowerTrait.TraitType.DontStarve,
    FollowerTrait.TraitType.GrassEater,
    FollowerTrait.TraitType.SermonEnthusiast,
    FollowerTrait.TraitType.Disciple,
    FollowerTrait.TraitType.Lustful,
    FollowerTrait.TraitType.Fashionable,
    FollowerTrait.TraitType.Nudist,
    FollowerTrait.TraitType.MusicLover,
    FollowerTrait.TraitType.Fertility,
    FollowerTrait.TraitType.DoctrinalExtremist,
    FollowerTrait.TraitType.ViolentExtremist,
    FollowerTrait.TraitType.Allegiance,
    FollowerTrait.TraitType.Hedonism,
    FollowerTrait.TraitType.Asceticism,
    FollowerTrait.TraitType.Unrepentant,
    FollowerTrait.TraitType.RoyalPooper,
    FollowerTrait.TraitType.CriminalReformed,
    FollowerTrait.TraitType.MissionaryInspired,
    FollowerTrait.TraitType.MissionaryExcited,
    FollowerTrait.TraitType.Pettable,
    FollowerTrait.TraitType.MarriedHappily,
    FollowerTrait.TraitType.ProudParent,
    FollowerTrait.TraitType.Polyamory,
    FollowerTrait.TraitType.BishopOfCult,
    FollowerTrait.TraitType.WarmBlooded,
    FollowerTrait.TraitType.ChosenOne,
    FollowerTrait.TraitType.PureBlood,
    FollowerTrait.TraitType.PureBlood_1,
    FollowerTrait.TraitType.PureBlood_2,
    FollowerTrait.TraitType.PureBlood_3,
    FollowerTrait.TraitType.MutatedVisual,
    FollowerTrait.TraitType.Frigidophile,
    FollowerTrait.TraitType.WinterExcited,
    FollowerTrait.TraitType.RotstonePooper,
    FollowerTrait.TraitType.WolfHater,
    FollowerTrait.TraitType.MutatedImmune,
    FollowerTrait.TraitType.WinterBody,
    FollowerTrait.TraitType.AnimalLover,
    FollowerTrait.TraitType.LightningEnthusiast,
    FollowerTrait.TraitType.HappilyWidowed,
    FollowerTrait.TraitType.DeepSleeper,
    FollowerTrait.TraitType.LockedLoyal,
    FollowerTrait.TraitType.Masochistic,
    FollowerTrait.TraitType.SeenGod,
    FollowerTrait.TraitType.Pacifist,
    FollowerTrait.TraitType.Heavyweight,
    FollowerTrait.TraitType.FreezeImmune,
    FollowerTrait.TraitType.InfusibleSnowman,
    FollowerTrait.TraitType.BornToTheRot,
    FollowerTrait.TraitType.MasterfulSnowman,
    FollowerTrait.TraitType.Nudist,
    FollowerTrait.TraitType.FluteLover,
    FollowerTrait.TraitType.MusicLover,
    FollowerTrait.TraitType.FreezeImmune,
    FollowerTrait.TraitType.EmbraceRot,
    FollowerTrait.TraitType.RejectRot,
    FollowerTrait.TraitType.FurnaceAnimal,
    FollowerTrait.TraitType.FurnaceFollower,
    FollowerTrait.TraitType.RemoveRot,
    FollowerTrait.TraitType.ColdEnthusiast,
    FollowerTrait.TraitType.WorkThroughBlizzard
  };
  public static List<FollowerTrait.TraitType> RareStartingTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Faithful,
    FollowerTrait.TraitType.Faithless,
    FollowerTrait.TraitType.Bastard,
    FollowerTrait.TraitType.Argumentative,
    FollowerTrait.TraitType.Scared,
    FollowerTrait.TraitType.RoyalPooper,
    FollowerTrait.TraitType.Poet,
    FollowerTrait.TraitType.ColdBlooded,
    FollowerTrait.TraitType.WarmBlooded
  };
  public static List<FollowerTrait.TraitType> SingleTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.FearOfDeath,
    FollowerTrait.TraitType.AgainstSacrifice,
    FollowerTrait.TraitType.Bastard,
    FollowerTrait.TraitType.Insomniac,
    FollowerTrait.TraitType.Poet,
    FollowerTrait.TraitType.Drowsy,
    FollowerTrait.TraitType.Lazy,
    FollowerTrait.TraitType.Snorer,
    FollowerTrait.TraitType.Unlawful,
    FollowerTrait.TraitType.Hibernation,
    FollowerTrait.TraitType.Aestivation,
    FollowerTrait.TraitType.IceSculpture
  };
  public static List<FollowerTrait.TraitType> ExcludedFromMating = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Disciple,
    FollowerTrait.TraitType.Immortal,
    FollowerTrait.TraitType.ProudParent,
    FollowerTrait.TraitType.OverworkedParent,
    FollowerTrait.TraitType.MarriedHappily,
    FollowerTrait.TraitType.MarriedJealous,
    FollowerTrait.TraitType.MarriedMurderouslyJealous,
    FollowerTrait.TraitType.MarriedUnhappily,
    FollowerTrait.TraitType.CriminalEvangelizing,
    FollowerTrait.TraitType.CriminalHardened,
    FollowerTrait.TraitType.CriminalReformed,
    FollowerTrait.TraitType.CriminalScarred,
    FollowerTrait.TraitType.ExCultLeader,
    FollowerTrait.TraitType.MissionaryExcited,
    FollowerTrait.TraitType.MissionaryInspired,
    FollowerTrait.TraitType.MissionaryTerrified,
    FollowerTrait.TraitType.Spy,
    FollowerTrait.TraitType.BishopOfCult,
    FollowerTrait.TraitType.ExistentialDread,
    FollowerTrait.TraitType.WarmBlooded,
    FollowerTrait.TraitType.PureBlood,
    FollowerTrait.TraitType.MutatedVisual,
    FollowerTrait.TraitType.Blind,
    FollowerTrait.TraitType.FreezeImmune,
    FollowerTrait.TraitType.InfusibleSnowman,
    FollowerTrait.TraitType.BornToTheRot,
    FollowerTrait.TraitType.MasterfulSnowman,
    FollowerTrait.TraitType.ShoddySnowman,
    FollowerTrait.TraitType.HappilyWidowed,
    FollowerTrait.TraitType.GrievingWidow,
    FollowerTrait.TraitType.JiltedLover
  };
  public static List<FollowerTrait.TraitType> UniqueTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.BishopOfCult,
    FollowerTrait.TraitType.Immortal,
    FollowerTrait.TraitType.Disciple,
    FollowerTrait.TraitType.DontStarve,
    FollowerTrait.TraitType.Blind,
    FollowerTrait.TraitType.BornToTheRot
  };
  public static List<FollowerTrait.TraitType> SinTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Virtuous,
    FollowerTrait.TraitType.Unrepentant,
    FollowerTrait.TraitType.Hedonism,
    FollowerTrait.TraitType.Asceticism,
    FollowerTrait.TraitType.Celibate,
    FollowerTrait.TraitType.Lustful
  };
  public static List<FollowerTrait.TraitType> MajorDLCTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.ColdBlooded,
    FollowerTrait.TraitType.WarmBlooded,
    FollowerTrait.TraitType.PureBlood,
    FollowerTrait.TraitType.PureBlood_1,
    FollowerTrait.TraitType.PureBlood_2,
    FollowerTrait.TraitType.PureBlood_3,
    FollowerTrait.TraitType.ChosenOne,
    FollowerTrait.TraitType.Mutated,
    FollowerTrait.TraitType.MutatedImmune,
    FollowerTrait.TraitType.Frigidophile,
    FollowerTrait.TraitType.WinterExcited,
    FollowerTrait.TraitType.RotstonePooper,
    FollowerTrait.TraitType.IceSculpture,
    FollowerTrait.TraitType.WolfHater,
    FollowerTrait.TraitType.Hibernation,
    FollowerTrait.TraitType.Aestivation,
    FollowerTrait.TraitType.WinterBody,
    FollowerTrait.TraitType.AnimalLover,
    FollowerTrait.TraitType.LightningEnthusiast,
    FollowerTrait.TraitType.HatesSnowmen,
    FollowerTrait.TraitType.HappilyWidowed,
    FollowerTrait.TraitType.GrievingWidow,
    FollowerTrait.TraitType.JiltedLover,
    FollowerTrait.TraitType.MutatedVisual,
    FollowerTrait.TraitType.Masochistic,
    FollowerTrait.TraitType.WorkThroughBlizzard,
    FollowerTrait.TraitType.ColdEnthusiast,
    FollowerTrait.TraitType.DeepSleeper,
    FollowerTrait.TraitType.LockedLoyal,
    FollowerTrait.TraitType.SeenGod,
    FollowerTrait.TraitType.Pacifist,
    FollowerTrait.TraitType.Heavyweight,
    FollowerTrait.TraitType.Lightweight,
    FollowerTrait.TraitType.EmbraceRot,
    FollowerTrait.TraitType.RejectRot,
    FollowerTrait.TraitType.FreezeImmune,
    FollowerTrait.TraitType.InfusibleSnowman,
    FollowerTrait.TraitType.BornToTheRot,
    FollowerTrait.TraitType.MasterfulSnowman,
    FollowerTrait.TraitType.ShoddySnowman
  };
  public static List<FollowerTrait.TraitType> WinterSpecificTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Frigidophile,
    FollowerTrait.TraitType.WinterExcited,
    FollowerTrait.TraitType.IceSculpture,
    FollowerTrait.TraitType.WolfHater,
    FollowerTrait.TraitType.Hibernation,
    FollowerTrait.TraitType.Aestivation,
    FollowerTrait.TraitType.WinterBody,
    FollowerTrait.TraitType.AnimalLover,
    FollowerTrait.TraitType.LightningEnthusiast,
    FollowerTrait.TraitType.HatesSnowmen,
    FollowerTrait.TraitType.RotstonePooper,
    FollowerTrait.TraitType.Mutated,
    FollowerTrait.TraitType.MutatedImmune,
    FollowerTrait.TraitType.MutatedVisual,
    FollowerTrait.TraitType.FreezeImmune,
    FollowerTrait.TraitType.InfusibleSnowman,
    FollowerTrait.TraitType.MasterfulSnowman,
    FollowerTrait.TraitType.ShoddySnowman
  };
  public static List<FollowerTrait.TraitType> PureBloodTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.PureBlood_1,
    FollowerTrait.TraitType.PureBlood_2,
    FollowerTrait.TraitType.PureBlood_3,
    FollowerTrait.TraitType.ChosenOne
  };
  public static List<FollowerTrait.TraitType> RequiresOnboardingCompleted = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Poet,
    FollowerTrait.TraitType.Scared,
    FollowerTrait.TraitType.Drowsy,
    FollowerTrait.TraitType.Insomniac,
    FollowerTrait.TraitType.Bastard
  };
  public static SpriteAtlas traitIcons;
  public static AsyncOperationHandle<SpriteAtlas> handle;

  public static void Initialise()
  {
    if ((UnityEngine.Object) FollowerTrait.traitIcons != (UnityEngine.Object) null)
      return;
    FollowerTrait.handle = Addressables.LoadAssetAsync<SpriteAtlas>((object) "TraitIcons");
    FollowerTrait.handle.Completed += (Action<AsyncOperationHandle<SpriteAtlas>>) (asyncOperation =>
    {
      if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
        FollowerTrait.traitIcons = asyncOperation.Result;
      else
        Debug.LogError((object) "Failed to load SpriteAtlas using Addressables");
    });
  }

  public static void DeInitialise()
  {
    if (!FollowerTrait.handle.IsValid())
      return;
    Addressables.Release<SpriteAtlas>(FollowerTrait.handle);
  }

  public static FollowerTrait.TraitType GetStartingTrait()
  {
    List<FollowerTrait.TraitType> traitTypeList = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) FollowerTrait.StartingTraits);
    for (int index = traitTypeList.Count - 1; index >= 0; --index)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.HasTrait(traitTypeList[index]) && FollowerTrait.SingleTraits.Contains(traitTypeList[index]))
        {
          traitTypeList.RemoveAt(index);
          break;
        }
      }
    }
    int num = 0;
    while (++num < 100)
    {
      FollowerTrait.TraitType trait = traitTypeList[UnityEngine.Random.Range(0, traitTypeList.Count)];
      if (!FollowerTrait.IsTraitUnavailable(trait) && !DataManager.Instance.CultTraits.Contains(trait))
        return trait;
    }
    return FollowerTrait.TraitType.None;
  }

  public static FollowerTrait.TraitType GetRareTrait()
  {
    int num = 0;
    while (++num < 100)
    {
      FollowerTrait.TraitType rareStartingTrait = FollowerTrait.RareStartingTraits[UnityEngine.Random.Range(0, FollowerTrait.RareStartingTraits.Count)];
      if (!FollowerTrait.IsTraitUnavailable(rareStartingTrait))
      {
        bool flag = true;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain.HasTrait(rareStartingTrait) && FollowerTrait.SingleTraits.Contains(rareStartingTrait))
          {
            flag = false;
            break;
          }
        }
        if (!DataManager.Instance.CultTraits.Contains(rareStartingTrait) & flag)
          return rareStartingTrait;
      }
    }
    return FollowerTrait.TraitType.None;
  }

  public static bool IsTraitUnavailable(FollowerTrait.TraitType trait)
  {
    return trait == FollowerTrait.TraitType.Fashionable && !DataManager.Instance.TailorEnabled || FollowerTrait.SinTraits.Contains(trait) && !DataManager.Instance.PleasureEnabled || FollowerTrait.WinterSpecificTraits.Contains(trait) && !DataManager.Instance.OnboardedSeasons || FollowerTrait.RequiresOnboardingCompleted.Contains(trait) && DataManager.Instance.BossesCompleted.Count < 1 || !SeasonsManager.Active && FollowerTrait.MajorDLCTraits.Contains(trait) || trait == FollowerTrait.TraitType.CriminalScarred || (trait == FollowerTrait.TraitType.Lazy || trait == FollowerTrait.TraitType.Snorer || trait == FollowerTrait.TraitType.Unlawful || trait == FollowerTrait.TraitType.Insomniac) && TimeManager.CurrentDay < 7 || trait == FollowerTrait.TraitType.ColdBlooded && !DataManager.Instance.FollowerOnboardedWinterHere;
  }

  public static string GetLocalizedTitle(FollowerTrait.TraitType Type)
  {
    string format = LocalizationManager.GetTranslation($"Traits/{Type}");
    if (Type == FollowerTrait.TraitType.BishopOfCult)
      format = string.Format(format, (object) DataManager.Instance.CultName);
    return format;
  }

  public static string GetLocalizedDescription(FollowerTrait.TraitType Type, FollowerBrain brain = null)
  {
    if (Type != FollowerTrait.TraitType.Mutated)
      return LocalizationManager.GetTranslation($"Traits/{Type}/Description");
    string str1 = Mathf.Clamp(brain != null ? FollowerBrainInfo.DAYS_TILL_ROT_DEATH - (TimeManager.CurrentDay - brain._directInfoAccess.DayBecameMutated) : FollowerBrainInfo.DAYS_TILL_ROT_DEATH, 0, int.MaxValue).ToString();
    string str2 = string.Format(LocalizationManager.GetTranslation("Traits/MutatedAdditionalDescription"), (object) str1);
    if (brain != null && brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Weird)
      str2 = LocalizationManager.GetTranslation("Traits/MutatedAdditionalDescriptionNecklace");
    return $"{LocalizationManager.GetTranslation($"Traits/{Type}/Description")} {str2}";
  }

  public static Sprite GetIcon(FollowerTrait.TraitType Type)
  {
    if (Type != FollowerTrait.TraitType.None && (UnityEngine.Object) FollowerTrait.traitIcons != (UnityEngine.Object) null)
    {
      Sprite sprite = FollowerTrait.traitIcons.GetSprite($"Icon_Trait_{Type.ToString()}");
      if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
        return sprite;
    }
    Debug.LogError((object) ("We are missing trait icon - setup a placeholder icon in the sprite atlas for " + Type.ToString()));
    return (Sprite) null;
  }

  public static void RemoveExclusiveTraits(FollowerBrain Brain, FollowerTrait.TraitType TraitType)
  {
    foreach (KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType> exclusiveTrait in FollowerTrait.ExclusiveTraits)
    {
      if (exclusiveTrait.Key == TraitType)
        Brain.RemoveTrait(exclusiveTrait.Value);
      if (exclusiveTrait.Value == TraitType)
        Brain.RemoveTrait(exclusiveTrait.Key);
    }
  }

  public static void AddCultTrait(FollowerTrait.TraitType TraitType)
  {
    if (!DataManager.Instance.CultTraits.Contains(TraitType))
      DataManager.Instance.CultTraits.Add(TraitType);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      foreach (KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType> exclusiveTrait in FollowerTrait.ExclusiveTraits)
      {
        if (exclusiveTrait.Key == TraitType)
          allBrain.RemoveTrait(exclusiveTrait.Value);
        if (exclusiveTrait.Value == TraitType)
          allBrain.RemoveTrait(exclusiveTrait.Key);
      }
    }
  }

  public static bool Contains(List<FollowerTrait> List, FollowerTrait.TraitType TraitType)
  {
    foreach (FollowerTrait followerTrait in List)
    {
      if (followerTrait.Type == TraitType)
        return true;
    }
    return false;
  }

  public static FollowerTrait GetTrait(List<FollowerTrait> List, FollowerTrait.TraitType TraitType)
  {
    foreach (FollowerTrait trait in List)
    {
      if (trait.Type == TraitType)
        return trait;
    }
    return (FollowerTrait) null;
  }

  public static bool IsPositiveTrait(FollowerTrait.TraitType traitType)
  {
    return FollowerTrait.GoodTraits.Contains(traitType);
  }

  public enum TraitType
  {
    None,
    NaturallySkeptical,
    NaturallyObedient,
    DesensitisedToDeath,
    FearOfDeath,
    Cannibal,
    GrassEater,
    Disciplinarian,
    Libertarian,
    SacrificeEnthusiast,
    AgainstSacrifice,
    Faithful,
    Faithless,
    FearOfSickPeople,
    LoveOfSickPeople,
    Sickly,
    IronStomach,
    Zealous,
    Materialistic,
    FalseIdols,
    Cynical,
    Gullible,
    Germophobe,
    Coprophiliac,
    Industrious,
    Lazy,
    SermonEnthusiast,
    ConstructionEnthusiast,
    MushroomEncouraged,
    MushroomBanned,
    LoveElderly,
    HateElderly,
    Immortal,
    DontStarve,
    ExCultLeader,
    Disciple,
    Bastard,
    Scared,
    Argumentative,
    RoyalPooper,
    Terrified,
    Celibate,
    Lustful,
    Fashionable,
    DoctrinalExtremist,
    ViolentExtremist,
    Fertility,
    Allegiance,
    Hedonism,
    Asceticism,
    Virtuous,
    Unrepentant,
    Snorer,
    PureBlood_1,
    PureBlood_2,
    PureBlood_3,
    PureBlood,
    ChosenOne,
    Unlawful,
    Insomniac,
    Poet,
    CriminalScarred,
    CriminalReformed,
    CriminalHardened,
    CriminalEvangelizing,
    MissionaryInspired,
    MissionaryExcited,
    MissionaryTerrified,
    Drowsy,
    Pettable,
    MarriedHappily,
    MarriedUnhappily,
    Zombie,
    ProudParent,
    OverworkedParent,
    MarriedJealous,
    MarriedDevoted,
    MarriedMurderouslyJealous,
    ExistentialDread,
    Polyamory,
    Spy,
    BishopOfCult,
    ColdBlooded,
    WarmBlooded,
    Chionophile,
    Heliophile,
    WinterExcited,
    WorkThroughBlizzard,
    ColdEnthusiast,
    Frigidophile,
    Mutated,
    MutatedImmune,
    MutatedVisual,
    RotstonePooper,
    Hibernation,
    Aestivation,
    WinterBody,
    AnimalLover,
    LightningEnthusiast,
    HatesSnowmen,
    WolfHater,
    HappilyWidowed,
    GrievingWidow,
    JiltedLover,
    DeepSleeper,
    LockedLoyal,
    Masochistic,
    SeenGod,
    Pacifist,
    Heavyweight,
    Lightweight,
    IceSculpture,
    FurnaceFollower,
    FurnaceAnimal,
    EmbraceRot,
    RejectRot,
    RemoveRot,
    Nudist,
    MusicLover,
    FluteLover,
    Blind,
    FreezeImmune,
    InfusibleSnowman,
    BornToTheRot,
    MasterfulSnowman,
    ShoddySnowman,
  }
}
