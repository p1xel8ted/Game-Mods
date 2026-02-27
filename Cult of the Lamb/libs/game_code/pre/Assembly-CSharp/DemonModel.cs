// Decompiled with JetBrains decompiler
// Type: DemonModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
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

  public static List<Follower> AvailableFollowersForDemonConversion()
  {
    List<Follower> followerList = new List<Follower>((IEnumerable<Follower>) FollowerManager.FollowersAtLocation(FollowerLocation.Base));
    for (int index = followerList.Count - 1; index >= 0; --index)
    {
      int id = followerList[index].Brain.Info.ID;
      if (followerList[index].Brain.Info.CursedState != Thought.None || FollowerManager.FollowerLocked(id))
        followerList.RemoveAt(index);
    }
    return followerList;
  }

  public static int GetDemonType(FollowerInfo followerInfo)
  {
    return DemonModel.GetDemonType(followerInfo.ID);
  }

  public static int GetDemonType(int id)
  {
    Random.InitState(id);
    return Random.Range(0, 6);
  }

  public static string GetTitle(int demonID, int followerID)
  {
    return followerID == 0 ? "" : $"{DemonModel.GetDemonName(demonID)} {FollowerInfo.GetInfoByID(followerID).XPLevel.ToNumeral()}";
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
        return LocalizationManager.GetTranslation("Interactions/Demon/Spirit");
      default:
        return "";
    }
  }

  public static string GetDescription(int demonID, FollowerInfo follower)
  {
    switch (demonID)
    {
      case 0:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Projectile/Description") + (follower.XPLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Projectile/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Shooty\"><size=24>+{follower.XPLevel}</size>"}";
      case 1:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Chomper/Description") + (follower.XPLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Chomper/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Chomp\"><size=24>+{follower.XPLevel}</size>"}";
      case 2:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Arrows/Description") + (follower.XPLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Arrows/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Arrows\"><size=24>+{follower.XPLevel}</size>"}";
      case 3:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Collector/Description") + (follower.XPLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Collector/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Collector\"><size=24>+{follower.XPLevel}</size>"}";
      case 4:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Exploder/Description") + (follower.XPLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Exploder/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Exploder\"><size=24>+{follower.XPLevel}</size>"}";
      case 5:
        return $"{LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Description") + (follower.XPLevel > 1 ? $"\n<color=#FFD201>{LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Upgrade")}</color>" : "")} {$"\n\n<sprite name=\"icon_Demon_Hearts\"><size=24>+{follower.XPLevel}</size>"}";
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
        return LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Description");
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
        return LocalizationManager.GetTranslation("Interactions/Demon/Spirit/Upgrade");
      default:
        return "";
    }
  }

  public static string GetDemonIcon(int demonID)
  {
    switch (demonID)
    {
      case 0:
        return "<sprite name=\"icon_Demon_Shooty\">";
      case 1:
        return "<sprite name=\"icon_Demon_Chomp\">";
      case 2:
        return "<sprite name=\"icon_Demon_Arrows\">";
      case 3:
        return "<sprite name=\"icon_Demon_Collector\">";
      case 4:
        return "<sprite name=\"icon_Demon_Exploder\">";
      case 5:
        return "<sprite name=\"icon_Demon_Hearts\">";
      default:
        return "";
    }
  }
}
