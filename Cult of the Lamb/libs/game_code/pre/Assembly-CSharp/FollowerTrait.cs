// Decompiled with JetBrains decompiler
// Type: FollowerTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
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
    FollowerTrait.TraitType.Lazy
  };
  public static List<FollowerTrait.TraitType> GoodTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Faithful,
    FollowerTrait.TraitType.Coprophiliac,
    FollowerTrait.TraitType.Gullible,
    FollowerTrait.TraitType.NaturallyObedient,
    FollowerTrait.TraitType.IronStomach,
    FollowerTrait.TraitType.Zealous,
    FollowerTrait.TraitType.Coprophiliac,
    FollowerTrait.TraitType.LoveOfSickPeople,
    FollowerTrait.TraitType.Gullible
  };
  public static List<FollowerTrait.TraitType> RareStartingTraits = new List<FollowerTrait.TraitType>()
  {
    FollowerTrait.TraitType.Faithful,
    FollowerTrait.TraitType.Faithless
  };

  public static FollowerTrait.TraitType GetStartingTrait()
  {
    List<FollowerTrait.TraitType> traitTypeList = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) FollowerTrait.StartingTraits);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.HasTrait(FollowerTrait.TraitType.FearOfDeath) && traitTypeList.Contains(FollowerTrait.TraitType.FearOfDeath))
        traitTypeList.Remove(FollowerTrait.TraitType.FearOfDeath);
      if (allBrain.HasTrait(FollowerTrait.TraitType.AgainstSacrifice) && traitTypeList.Contains(FollowerTrait.TraitType.AgainstSacrifice))
        traitTypeList.Remove(FollowerTrait.TraitType.AgainstSacrifice);
    }
    int num = 0;
    while (++num < 100)
    {
      FollowerTrait.TraitType startingTrait = traitTypeList[UnityEngine.Random.Range(0, traitTypeList.Count)];
      if (!DataManager.Instance.CultTraits.Contains(startingTrait))
        return startingTrait;
    }
    return FollowerTrait.TraitType.None;
  }

  public static FollowerTrait.TraitType GetRareTrait()
  {
    int num = 0;
    while (++num < 100)
    {
      FollowerTrait.TraitType rareStartingTrait = FollowerTrait.RareStartingTraits[UnityEngine.Random.Range(0, FollowerTrait.RareStartingTraits.Count)];
      if (!DataManager.Instance.CultTraits.Contains(rareStartingTrait))
        return rareStartingTrait;
    }
    return FollowerTrait.TraitType.None;
  }

  public static string GetLocalizedTitle(FollowerTrait.TraitType Type)
  {
    return LocalizationManager.GetTranslation($"Traits/{Type}");
  }

  public static string GetLocalizedDescription(FollowerTrait.TraitType Type)
  {
    return LocalizationManager.GetTranslation($"Traits/{Type}/Description");
  }

  public static Sprite GetIcon(FollowerTrait.TraitType Type)
  {
    return Resources.Load<SpriteAtlas>("Atlases/TraitIcons").GetSprite($"Icon_Trait_{Type.ToString()}");
  }

  public static void RemoveExclusiveTraits(FollowerBrain Brain, FollowerTrait.TraitType TraitType)
  {
    foreach (KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType> exclusiveTrait in FollowerTrait.ExclusiveTraits)
    {
      if (exclusiveTrait.Key == TraitType)
        Brain.RemoveTrait(exclusiveTrait.Value, false);
      if (exclusiveTrait.Value == TraitType)
        Brain.RemoveTrait(exclusiveTrait.Key, false);
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
          allBrain.RemoveTrait(exclusiveTrait.Value, false);
        if (exclusiveTrait.Value == TraitType)
          allBrain.RemoveTrait(exclusiveTrait.Key, false);
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
    switch (traitType)
    {
      case FollowerTrait.TraitType.NaturallyObedient:
      case FollowerTrait.TraitType.DesensitisedToDeath:
      case FollowerTrait.TraitType.Cannibal:
      case FollowerTrait.TraitType.GrassEater:
      case FollowerTrait.TraitType.Disciplinarian:
      case FollowerTrait.TraitType.Libertarian:
      case FollowerTrait.TraitType.SacrificeEnthusiast:
      case FollowerTrait.TraitType.Faithful:
      case FollowerTrait.TraitType.LoveOfSickPeople:
      case FollowerTrait.TraitType.IronStomach:
      case FollowerTrait.TraitType.Zealous:
      case FollowerTrait.TraitType.Materialistic:
      case FollowerTrait.TraitType.FalseIdols:
      case FollowerTrait.TraitType.Gullible:
      case FollowerTrait.TraitType.Coprophiliac:
      case FollowerTrait.TraitType.Industrious:
      case FollowerTrait.TraitType.ConstructionEnthusiast:
      case FollowerTrait.TraitType.MushroomEncouraged:
      case FollowerTrait.TraitType.MushroomBanned:
      case FollowerTrait.TraitType.LoveElderly:
      case FollowerTrait.TraitType.HateElderly:
      case FollowerTrait.TraitType.Immortal:
      case FollowerTrait.TraitType.DontStarve:
        return true;
      default:
        return false;
    }
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
  }
}
