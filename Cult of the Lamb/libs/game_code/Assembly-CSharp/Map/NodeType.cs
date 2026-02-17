// Decompiled with JetBrains decompiler
// Type: Map.NodeType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  FinalBoss = 752, // 0x000002F0
  Special_FindRelic = 753, // 0x000002F1
  Meat = 754, // 0x000002F2
  MarketplaceRelics = 755, // 0x000002F3
  MarketPlaceWeapons = 756, // 0x000002F4
  MarketPlaceClothes = 757, // 0x000002F5
  Special_Grave = 758, // 0x000002F6
  Cotton = 759, // 0x000002F7
  Grapes = 760, // 0x000002F8
  Hops = 761, // 0x000002F9
  Executioner = 762, // 0x000002FA
  FindNote = 763, // 0x000002FB
  Snow_Fruit = 764, // 0x000002FC
  Chilli = 765, // 0x000002FD
  MarketPlaceAnimal = 766, // 0x000002FE
  MarketplaceBlacksmith = 767, // 0x000002FF
  Magma_Stone = 768, // 0x00000300
  Lightning_Shard = 769, // 0x00000301
  Special_DepositFollower = 770, // 0x00000302
  Special_DepositInfectedPet = 771, // 0x00000303
  Special_LegendarySwordTree = 772, // 0x00000304
  Yew_Cursed = 773, // 0x00000305
  MarketplaceDLCResources = 774, // 0x00000306
  None = 1000, // 0x000003E8
}
