// Decompiled with JetBrains decompiler
// Type: DemonModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.FollowerSelect;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DemonModel
{
  public const int kDemonArchetypeLimit = 1;
  public const int Shooty = 0;
  public const int Chompy = 1;
  public const int Arrows = 2;
  public const int Collector = 3;
  public const int Exploder = 4;
  public const int Spirit = 5;
  public const int Baal = 6;
  public const int Aym = 7;
  public const int Leshy = 8;
  public const int Heket = 9;
  public const int Kallamar = 10;
  public const int Shamura = 11;
  public const int ChosenChild = 12;
  public const int Rot = 13;

  public static List<FollowerSelectEntry> AvailableFollowersForDemonConversion()
  {
    List<Follower> followerList = new List<Follower>((IEnumerable<Follower>) FollowerManager.FollowersAtLocation(FollowerLocation.Base));
    List<FollowerSelectEntry> followerSelectEntryList = new List<FollowerSelectEntry>();
    for (int index = followerList.Count - 1; index >= 0; --index)
    {
      int id = followerList[index].Brain.Info.ID;
      if (followerList[index].Brain.Info.CursedState != Thought.None)
        followerSelectEntryList.Add(new FollowerSelectEntry(followerList[index], FollowerManager.GetFollowerCursedStateAvailability(followerList[index])));
      else
        followerSelectEntryList.Add(new FollowerSelectEntry(followerList[index], FollowerManager.GetFollowerAvailabilityStatus(followerList[index].Brain)));
    }
    return followerSelectEntryList;
  }

  public static int GetDemonType(FollowerInfo followerInfo)
  {
    return DemonModel.GetDemonType(followerInfo.ID);
  }

  public static int GetDemonType(int id)
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(id);
    if (infoById != null && infoById.Traits.Contains(FollowerTrait.TraitType.Mutated))
      return 13;
    Random.InitState(id);
    switch (id)
    {
      case 99990:
        return 8;
      case 99991:
        return 9;
      case 99992:
        return 10;
      case 99993:
        return 11;
      case 99994:
        return 6;
      case 99995:
        return 7;
      case 100000:
        return 12;
      default:
        return Random.Range(0, 6);
    }
  }

  public static string GetTitle(int demonID, int followerID)
  {
    if (followerID == 0)
      return "";
    int demonLevel = FollowerInfo.GetInfoByID(followerID).GetDemonLevel();
    return $"{DemonModel.GetDemonName(demonID)} {demonLevel.ToNumeral()}";
  }

  public static string GetDemonName(int demonID)
  {
    switch (demonID)
    {
      case 0:
        return LocalizationManager.GetTranslation("Interactions/Demon/Projectile");
      case 1:
        return LocalizationManager.GetTranslation("Interactions/Demon/Chomper");
      case 2:
        return LocalizationManager.GetTranslation("Interactions/Demon/Arrows");
      case 3:
        return LocalizationManager.GetTranslation("Interactions/Demon/Collector");
      case 4:
        return LocalizationManager.GetTranslation("Interactions/Demon/Exploder");
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
      case 11:
      case 12:
        return LocalizationManager.GetTranslation("Interactions/Demon/Spirit");
      case 13:
        return LocalizationManager.GetTranslation("Interactions/Demon/Rot");
      default:
        return "";
    }
  }

  public static string GetDescription(int demonID, FollowerInfo follower)
  {
    int demonLevel = follower.GetDemonLevel();
    switch (demonID)
    {
      case 0:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Projectile/Description") + (demonLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Projectile/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Shooty\"><size=24>+{demonLevel}</size>"}";
      case 1:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Chomper/Description") + (demonLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Chomper/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Chomp\"><size=24>+{demonLevel}</size>"}";
      case 2:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Arrows/Description") + (demonLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Arrows/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Arrows\"><size=24>+{demonLevel}</size>"}";
      case 3:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Collector/Description") + (demonLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Collector/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Collector\"><size=24>+{demonLevel}</size>"}";
      case 4:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Exploder/Description") + (demonLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Exploder/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Exploder\"><size=24>+{demonLevel}</size>"}";
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
      case 11:
      case 12:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Description") + (demonLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Hearts\"><size=24>+{demonLevel}</size>"}";
      case 13:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Rot/Description") + (demonLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Rot/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Bomb\"><size=24>+{demonLevel}</size>"}";
      default:
        return "";
    }
  }

  public static string GetDescription(int demonID)
  {
    switch (demonID)
    {
      case 0:
        return LocalizationManager.GetTranslation("Interactions/Demon/Projectile/Description");
      case 1:
        return LocalizationManager.GetTranslation("Interactions/Demon/Chomper/Description");
      case 2:
        return LocalizationManager.GetTranslation("Interactions/Demon/Arrows/Description");
      case 3:
        return LocalizationManager.GetTranslation("Interactions/Demon/Collector/Description");
      case 4:
        return LocalizationManager.GetTranslation("Interactions/Demon/Exploder/Description");
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
      case 11:
      case 12:
        return LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Description");
      case 13:
        return LocalizationManager.GetTranslation("Interactions/Demon/Rot/Description");
      default:
        return "";
    }
  }

  public static string GetDemonUpgradeDescription(int demonID)
  {
    switch (demonID)
    {
      case 0:
        return LocalizationManager.GetTranslation("Interactions/Demon/Projectile/Upgrade");
      case 1:
        return LocalizationManager.GetTranslation("Interactions/Demon/Chomper/Upgrade");
      case 2:
        return LocalizationManager.GetTranslation("Interactions/Demon/Arrows/Upgrade");
      case 3:
        return LocalizationManager.GetTranslation("Interactions/Demon/Collector/Upgrade");
      case 4:
        return LocalizationManager.GetTranslation("Interactions/Demon/Exploder/Upgrade");
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
      case 11:
      case 12:
        return LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Upgrade");
      case 13:
        return LocalizationManager.GetTranslation("Interactions/Demon/Rot/Upgrade");
      default:
        return "";
    }
  }

  public static string GetDemonIcon(int demonID)
  {
    switch (demonID)
    {
      case 0:
        return "<size=14.7><sprite name=\"icon_Demon_Shooty\">";
      case 1:
        return "<size=14.7><sprite name=\"icon_Demon_Chomp\">";
      case 2:
        return "<size=14.7><sprite name=\"icon_Demon_Arrows\">";
      case 3:
        return "<size=14.7><sprite name=\"icon_Demon_Collector\">";
      case 4:
        return "<size=14.7><sprite name=\"icon_Demon_Exploder\">";
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
      case 11:
      case 12:
        return "<size=14.7><sprite name=\"icon_Demon_Hearts\">";
      case 13:
        return "<size=30><sprite name=\"icon_Demon_Bomb\">";
      default:
        return "";
    }
  }
}
