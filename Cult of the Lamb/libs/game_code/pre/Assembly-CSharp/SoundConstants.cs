// Decompiled with JetBrains decompiler
// Type: SoundConstants
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
    NoMusic = 9999, // 0x0000270F
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
  }
}
