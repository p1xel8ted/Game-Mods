// Decompiled with JetBrains decompiler
// Type: SoundConstants
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public static class SoundConstants
{
  public static string GetImpactSoundPathForMaterial(SoundConstants.SoundMaterial soundMaterial)
  {
    switch (soundMaterial)
    {
      case SoundConstants.SoundMaterial.Bone:
        return "event:/material/bone_impact";
      case SoundConstants.SoundMaterial.Glass:
        return "event:/material/stained_glass_impact";
      case SoundConstants.SoundMaterial.Stone:
        return "event:/material/stone_impact";
      case SoundConstants.SoundMaterial.Wood:
        return "event:/material/wood_impact";
      case SoundConstants.SoundMaterial.WoodBarrel:
        return "event:/material/wood_barrel_impact";
      case SoundConstants.SoundMaterial.Tree:
        return "event:/material/tree_chop";
      case SoundConstants.SoundMaterial.Grass:
        return "event:/player/tall_grass_cut";
      case SoundConstants.SoundMaterial.Coins:
        return "event:/rituals/coins";
      case SoundConstants.SoundMaterial.Books:
        return "event:/dlc/material/obstacle/book_pile_destroy";
      case SoundConstants.SoundMaterial.Metal:
        return "event:/dlc/material/obstacle/metal_destroy";
      case SoundConstants.SoundMaterial.Ice:
        return "event:/dlc/material/obstacle/ice_destroy";
      case SoundConstants.SoundMaterial.Tent:
        return "event:/dlc/material/obstacle/tent_impact";
      case SoundConstants.SoundMaterial.Mutation:
        return "event:/dlc/material/obstacle/mutationpile_impact";
      default:
        return string.Empty;
    }
  }

  public static string GetBreakSoundPathForMaterial(SoundConstants.SoundMaterial soundMaterial)
  {
    switch (soundMaterial)
    {
      case SoundConstants.SoundMaterial.Bone:
        return "event:/material/bone_break";
      case SoundConstants.SoundMaterial.Glass:
        return "event:/material/stained_glass_break";
      case SoundConstants.SoundMaterial.Stone:
        return "event:/material/stone_break";
      case SoundConstants.SoundMaterial.Wood:
        return "event:/material/wood_break";
      case SoundConstants.SoundMaterial.WoodBarrel:
        return "event:/material/wood_barrel_break";
      case SoundConstants.SoundMaterial.Tree:
        return "event:/material/tree_break";
      case SoundConstants.SoundMaterial.Grass:
        return "event:/player/tall_grass_cut";
      case SoundConstants.SoundMaterial.Coins:
        return "event:/rituals/coins";
      case SoundConstants.SoundMaterial.Books:
        return "event:/dlc/material/obstacle/book_pile_destroy";
      case SoundConstants.SoundMaterial.Metal:
        return "event:/dlc/material/obstacle/metal_destroy";
      case SoundConstants.SoundMaterial.Ice:
        return "event:/dlc/material/obstacle/ice_destroy";
      case SoundConstants.SoundMaterial.Tent:
        return "event:/dlc/material/obstacle/tent_destroy";
      case SoundConstants.SoundMaterial.Mutation:
        return "event:/dlc/material/obstacle/mutationpile_destroy";
      default:
        return string.Empty;
    }
  }

  public enum SoundEventType
  {
    None,
    OneShot2D,
    OneShotAtPosition,
    OneShotAttached,
    Loop,
  }

  public enum RoomID
  {
    StandardRoom = 0,
    CultLeaderAmbience = 1,
    OfferingCombat = 2,
    Shop = 3,
    Sozo = 4,
    SpecialCombat = 5,
    Healing = 6,
    StandardAmbience = 7,
    BossEntryAmbience = 8,
    MainBossA = 9,
    MainBossB = 10, // 0x0000000A
    FollowerAmbience = 11, // 0x0000000B
    EndRoomAmbience = 12, // 0x0000000C
    BeholderBattle = 13, // 0x0000000D
    AltStandardRoom = 15, // 0x0000000F
    Chemach = 16, // 0x00000010
    MainBossC = 17, // 0x00000011
    LoreStoneRoom = 18, // 0x00000012
    YngyaShrine = 19, // 0x00000013
    PuzzleRoom = 20, // 0x00000014
    NoMusic = 9999, // 0x0000270F
  }

  public enum BaseID
  {
    StandardAmbience = 0,
    NoFollowers = 1,
    BigEnergy = 2,
    Temple = 3,
    bongos_singing = 4,
    fight_pit_drums = 5,
    DungeonDoor = 6,
    blood_moon = 7,
    nudist_ritual = 8,
    nudist_intro = 9,
    baby_fol_first_born = 10, // 0x0000000A
    winter_starts = 11, // 0x0000000B
    winter_random = 12, // 0x0000000C
    woolhaven = 13, // 0x0000000D
    NoMusic = 9999, // 0x0000270F
  }

  public enum FlockadeGameState
  {
    Title,
    SetupPhase,
    BattlePhase,
    StingerWin,
    StingerLose,
    StingerDraw,
  }

  public enum SoundMaterial
  {
    None,
    Bone,
    Glass,
    Stone,
    Wood,
    WoodBarrel,
    Tree,
    Grass,
    Coins,
    Books,
    Metal,
    Ice,
    Tent,
    Mutation,
  }
}
