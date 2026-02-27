// Decompiled with JetBrains decompiler
// Type: Map.NodeType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Map;

public enum NodeType
{
  MinorEnemy = 0,
  EliteEnemy = 1,
  RestSite = 2,
  Treasure = 3,
  Store = 4,
  Boss = 5,
  Mystery = 6,
  Gold = 7,
  Wood = 8,
  Follower = 9,
  Food = 10, // 0x0000000A
  Sherpa = 11, // 0x0000000B
  Tarot = 12, // 0x0000000C
  FirstFloor = 13, // 0x0000000D
  DungeonFloor = 14, // 0x0000000E
  MiniBossFloor = 15, // 0x0000000F
  MarketPlaceCat = 16, // 0x00000010
  MarketPlaceSpider = 17, // 0x00000011
  MarketPlaceChef = 18, // 0x00000012
  Stone = 19, // 0x00000013
  Knucklebones = 20, // 0x00000014
  BloodStone = 21, // 0x00000015
  Fishing = 22, // 0x00000016
  Special_Teleporter = 100, // 0x00000064
  Special_BloodSacrafice = 101, // 0x00000065
  special_Challenge = 102, // 0x00000066
  Special_Healing = 103, // 0x00000067
  Special_RewardChoice = 104, // 0x00000068
  Special_CoinGamble = 105, // 0x00000069
  Special_HappyFollower = 106, // 0x0000006A
  Special_DissentingFollower = 107, // 0x0000006B
  Follower_Beginner = 108, // 0x0000006C
  Follower_Easy = 109, // 0x0000006D
  Follower_Medium = 110, // 0x0000006E
  Follower_Hard = 111, // 0x0000006F
  Poop = 112, // 0x00000070
  Bones = 113, // 0x00000071
  Special_RandomiseMap = 114, // 0x00000072
  Special_KeyRoom = 115, // 0x00000073
  Rare_Gold = 250, // 0x000000FA
  Lore_Haro = 500, // 0x000001F4
  Intro_TeleportHome = 501, // 0x000001F5
  Special_HealthChoice = 502, // 0x000001F6
  Special_EggRoom = 503, // 0x000001F7
  Negative_PreviousMiniboss = 750, // 0x000002EE
  Negative_PlayerDebuff = 751, // 0x000002EF
  None = 1000, // 0x000003E8
}
