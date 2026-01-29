// Decompiled with JetBrains decompiler
// Type: Enemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public enum Enemy
{
  None = 0,
  Archer = 1,
  Brute = 2,
  Scamp = 3,
  Scout = 4,
  Summoner = 5,
  Swordsman = 6,
  BurrowingSpikerWorm = 100, // 0x00000064
  ChaserWorm = 101, // 0x00000065
  BurrowingMiniBoss = 102, // 0x00000066
  BurrowingWorm = 103, // 0x00000067
  FlyingWorm = 104, // 0x00000068
  DivingMaggotWormMiniBoss = 105, // 0x00000069
  MamaChaserWormMiniBoss = 106, // 0x0000006A
  PatrolWorm = 107, // 0x0000006B
  TurretAreaAttack = 108, // 0x0000006C
  Turret = 109, // 0x0000006D
  WormBoss = 110, // 0x0000006E
  Beholder1 = 111, // 0x0000006F
  FlyingHopper = 200, // 0x000000C8
  FrogBoss = 201, // 0x000000C9
  HopperAOE = 202, // 0x000000CA
  HopperBig = 203, // 0x000000CB
  HopperBurp = 204, // 0x000000CC
  HopperPoison = 205, // 0x000000CD
  Hopper = 206, // 0x000000CE
  BatMiniBoss = 207, // 0x000000CF
  HopperMiniBoss = 208, // 0x000000D0
  MortarHopper = 209, // 0x000000D1
  MortarHopperMiniBoss = 210, // 0x000000D2
  Beholder2 = 211, // 0x000000D3
  JellyChargerMiniBoss = 300, // 0x0000012C
  JellySpawner = 301, // 0x0000012D
  JellyExploderBaby = 302, // 0x0000012E
  JellyExploderBig = 303, // 0x0000012F
  JellyExploder = 304, // 0x00000130
  JellySpikerBig = 305, // 0x00000131
  JellySpiker = 306, // 0x00000132
  JellyRingshotTurretMiniBoss = 307, // 0x00000133
  JellyRingshotTurret = 308, // 0x00000134
  JellyTurret = 309, // 0x00000135
  JellySpikerMiniBoss = 310, // 0x00000136
  Beholder3 = 311, // 0x00000137
  JellyBoss = 312, // 0x00000138
  JellyCharger = 313, // 0x00000139
  JellyChargerExploder = 314, // 0x0000013A
  SpiderDashBig = 400, // 0x00000190
  SpiderDashMedium = 401, // 0x00000191
  SpiderDashSmall = 402, // 0x00000192
  Millipede = 403, // 0x00000193
  SpiderJumpBig = 404, // 0x00000194
  SpiderJumpMedium = 405, // 0x00000195
  SpiderJumpSmall = 406, // 0x00000196
  MillipedePoisonMiniBoss = 407, // 0x00000197
  Beholder4 = 408, // 0x00000198
  ScorpionMiniBoss = 409, // 0x00000199
  ScorpionShooterBig = 410, // 0x0000019A
  ScorpionShooterMedium = 411, // 0x0000019B
  SpiderJumpMiniBoss = 412, // 0x0000019C
  SpiderBoss = 413, // 0x0000019D
  SpiderPoisonMortar = 414, // 0x0000019E
  BruteVariant = 500, // 0x000001F4
  SpiderDashBigVariant = 501, // 0x000001F5
  ArcherVariant = 502, // 0x000001F6
  BruteMiniBoss = 503, // 0x000001F7
  SpikerWormVariant = 504, // 0x000001F8
  HopperBurpVariant = 505, // 0x000001F9
  JellyExploderBigVariant = 506, // 0x000001FA
  JellySpiderVariant = 507, // 0x000001FB
  MillipedeVariant = 508, // 0x000001FC
  FlySwarmMiniBoss = 509, // 0x000001FD
  PatrolWormVariant = 510, // 0x000001FE
  SwordsmanVariant = 511, // 0x000001FF
  TurretVariant = 512, // 0x00000200
  ZombieVariant = 513, // 0x00000201
  Guardian1 = 514, // 0x00000202
  Guardian2 = 515, // 0x00000203
  HorderMiniBoss = 516, // 0x00000204
  Beholder5 = 517, // 0x00000205
  ScorpionShooterMediumVariant = 518, // 0x00000206
  DeathCatBoss = 519, // 0x00000207
  SimpleGuardian = 520, // 0x00000208
  DogCharger = 521, // 0x00000209
  DogBurrower = 522, // 0x0000020A
  DogDiveBomb = 523, // 0x0000020B
  DogMiniBoss = 524, // 0x0000020C
  Necromancer = 525, // 0x0000020D
  WolfGuardian = 526, // 0x0000020E
  WolfGuardianMiniboss = 527, // 0x0000020F
  LightningCross = 528, // 0x00000210
  IcicleSlam = 529, // 0x00000211
  LightningTeleporter = 530, // 0x00000212
  LightningScout = 531, // 0x00000213
  QuadLightningSummoner = 532, // 0x00000214
  TrapLayer = 533, // 0x00000215
  SwarmingHopper = 534, // 0x00000216
  LightningTracker = 535, // 0x00000217
  DogMage = 536, // 0x00000218
  Woodman = 537, // 0x00000219
  Oogler = 538, // 0x0000021A
  BellKnight = 539, // 0x0000021B
  SegmentedWorm = 540, // 0x0000021C
  Doogler = 541, // 0x0000021D
  Dorry = 542, // 0x0000021E
  Brogy = 543, // 0x0000021F
  LavaSnail = 544, // 0x00000220
  BigLavaSnail = 545, // 0x00000221
  BulbLamb = 546, // 0x00000222
  Boomerang = 547, // 0x00000223
  Kebab = 548, // 0x00000224
  LambFlesh = 549, // 0x00000225
  BloodWaller = 550, // 0x00000226
  Jailer = 551, // 0x00000227
  KingOfLambsMiniBoss = 552, // 0x00000228
  KingJailerMiniBoss = 553, // 0x00000229
  GuardianMiniBoss = 554, // 0x0000022A
  HoleMiniBoss = 555, // 0x0000022B
  WolfMiniBoss = 556, // 0x0000022C
  DooglerMiniBoss = 557, // 0x0000022D
  LightningMiniBoss = 558, // 0x0000022E
  WarriorTrioMiniBoss = 559, // 0x0000022F
  WolfTurret = 560, // 0x00000230
  WoodmanBig = 561, // 0x00000231
}
