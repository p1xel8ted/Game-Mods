// Decompiled with JetBrains decompiler
// Type: WorldManipulatorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class WorldManipulatorManager
{
  public static void TriggerManipulation(
    WorldManipulatorManager.Manipulations manipulation,
    float delay = 0.0f,
    bool twitch = false)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() =>
    {
      string str = "";
      switch (manipulation)
      {
        case WorldManipulatorManager.Manipulations.GainRandomHeart:
          switch (HealthPlayer.GainRandomHeart())
          {
            case 0:
              str = "Inventory/BLACK_HEART";
              break;
            case 1:
              str = "Inventory/BLUE_HEART";
              break;
            case 2:
              str = "Inventory/RED_HEART";
              break;
          }
          break;
        case WorldManipulatorManager.Manipulations.HealHearts:
          int healing = UnityEngine.Random.Range((int) ((double) PlayerFarming.Instance.health.totalHP / 2.0), (int) PlayerFarming.Instance.health.totalHP + 1);
          PlayerFarming.Instance.GetComponent<HealthPlayer>().Heal((float) healing);
          str = healing.ToString();
          break;
        case WorldManipulatorManager.Manipulations.GainTarot:
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, PlayerFarming.Instance.transform.position + Vector3.down, 0.0f).GetComponent<Interaction_TarotCard>().ForceAllow = true;
          break;
        case WorldManipulatorManager.Manipulations.DealDamageToAllEnemies:
          Health.DamageAllEnemies((float) (5.0 + (double) DataManager.GetWeaponDamageMultiplier(DataManager.Instance.CurrentWeaponLevel) * 2.0), Health.DamageAllEnemiesType.Manipulation);
          break;
        case WorldManipulatorManager.Manipulations.ReceiveDemon:
          int num = 0;
          while (++num < 30)
          {
            int type = UnityEngine.Random.Range(0, 5);
            if (!DataManager.Instance.Followers_Demons_Types.Contains(type))
            {
              BiomeGenerator.Instance.SpawnDemon(type);
              break;
            }
          }
          break;
        case WorldManipulatorManager.Manipulations.InvincibleForTime:
          float duration = 30f;
          PlayerFarming.Instance.playerController.MakeUntouchable(duration);
          str = duration.ToString();
          break;
        case WorldManipulatorManager.Manipulations.TakeDamage:
          int Damage = Mathf.Max(1, UnityEngine.Random.Range(0, (int) ((double) PlayerFarming.Instance.health.HP / 2.0)));
          PlayerFarming.Instance.GetComponent<HealthPlayer>().DealDamage((float) Damage, PlayerFarming.Instance.gameObject, PlayerFarming.Instance.transform.position, false, Health.AttackTypes.Melee, true, (Health.AttackFlags) 0);
          str = Damage.ToString();
          break;
        case WorldManipulatorManager.Manipulations.IncreaseEnemyModifiersChance:
          DataManager.Instance.EnemyModifiersChanceMultiplier += 3f;
          break;
        case WorldManipulatorManager.Manipulations.SpawnBombs:
          BiomeGenerator.SpawnBombsInRoom(UnityEngine.Random.Range(15, 25));
          break;
        case WorldManipulatorManager.Manipulations.LoseAllSpecialHearts:
          HealthPlayer.LoseAllSpecialHearts();
          break;
        case WorldManipulatorManager.Manipulations.DropPoisonOnAttack:
          DataManager.Instance.SpawnPoisonOnAttack = true;
          break;
        case WorldManipulatorManager.Manipulations.AllEnemiesHaveModifiersInNextRoom:
          DataManager.Instance.EnemiesInNextRoomHaveModifiers = true;
          DataManager.Instance.CurrentRoomCoordinates = new Vector2((float) BiomeGenerator.Instance.CurrentRoom.x, (float) BiomeGenerator.Instance.CurrentRoom.y);
          RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(WorldManipulatorManager.OnRoomCleared);
          break;
        case WorldManipulatorManager.Manipulations.ResetTempleCooldowns:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => UpgradeSystem.ClearAllCoolDowns())));
          break;
        case WorldManipulatorManager.Manipulations.InstantlyBuildStructures:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => StructureManager.BuildAllStructures())));
          break;
        case WorldManipulatorManager.Manipulations.GainFaith:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => CultFaithManager.AddThought(Thought.FaithIncreased))));
          break;
        case WorldManipulatorManager.Manipulations.ResurrectBuriedFollower:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.ResurrectBurriedFollower())));
          break;
        case WorldManipulatorManager.Manipulations.CureCursedFollowers:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.CureAllCursedFollowers())));
          break;
        case WorldManipulatorManager.Manipulations.ClearAllWaste:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => StructureManager.ClearAllWaste())));
          break;
        case WorldManipulatorManager.Manipulations.AllFollowersPoopOrVomit:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.MakeAllFollowersPoopOrVomit())));
          break;
        case WorldManipulatorManager.Manipulations.BreakAllBeds:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => StructureManager.BreakRandomBeds())));
          break;
        case WorldManipulatorManager.Manipulations.SkipTime:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => TimeManager.SkipTime(600f))));
          break;
        case WorldManipulatorManager.Manipulations.SleepFollowers:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.MakeAllFollowersFallAsleep())));
          break;
        case WorldManipulatorManager.Manipulations.RandomCursedState:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.GiveFollowersRandomCurse(UnityEngine.Random.Range(Mathf.Clamp(Mathf.RoundToInt((float) DataManager.Instance.Followers.Count / 6f), 1, DataManager.Instance.Followers.Count), Mathf.Clamp(Mathf.RoundToInt((float) DataManager.Instance.Followers.Count / 4f), 1, DataManager.Instance.Followers.Count) + 1)))));
          break;
        case WorldManipulatorManager.Manipulations.KillRandomFollower:
          GameManager.GetInstance().StartCoroutine((IEnumerator) WorldManipulatorManager.WaitTillPlayerIsAtBase((System.Action) (() => FollowerManager.KillRandomFollower(true))));
          break;
      }
      if (!twitch)
        return;
      if (string.IsNullOrEmpty(str))
        NotificationCentre.Instance.PlayTwitchNotification(WorldManipulatorManager.GetNotification(manipulation), "UI/Twitch/ThanksTwitchChat");
      else
        NotificationCentre.Instance.PlayTwitchNotification(WorldManipulatorManager.GetNotification(manipulation), str, "UI/Twitch/ThanksTwitchChat");
    })));
  }

  private static IEnumerator WaitTillPlayerIsAtBase(System.Action callback)
  {
    while (PlayerFarming.Location != FollowerLocation.Base && !GameManager.IsDungeon(PlayerFarming.Location))
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  private static void OnRoomCleared()
  {
    if (!(DataManager.Instance.CurrentRoomCoordinates != new Vector2((float) BiomeGenerator.Instance.CurrentRoom.x, (float) BiomeGenerator.Instance.CurrentRoom.y)))
      return;
    DataManager.Instance.EnemiesInNextRoomHaveModifiers = false;
    DataManager.Instance.CurrentRoomCoordinates = Vector2.zero;
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(WorldManipulatorManager.OnRoomCleared);
  }

  public static string GetLocalisation(WorldManipulatorManager.Manipulations manipulation)
  {
    return LocalizationManager.GetTranslation($"Manipulations/{manipulation}");
  }

  public static string GetNotification(WorldManipulatorManager.Manipulations manipulation)
  {
    return $"Manipulations/{manipulation}/Notification";
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleDungeonPositiveManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    ts.Add(WorldManipulatorManager.Manipulations.GainRandomHeart);
    ts.Add(WorldManipulatorManager.Manipulations.DealDamageToAllEnemies);
    ts.Add(WorldManipulatorManager.Manipulations.GainTarot);
    ts.Add(WorldManipulatorManager.Manipulations.ReceiveDemon);
    ts.Add(WorldManipulatorManager.Manipulations.InvincibleForTime);
    if ((double) PlayerFarming.Instance.health.HP < (double) PlayerFarming.Instance.health.totalHP)
      ts.Add(WorldManipulatorManager.Manipulations.HealHearts);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleDungeonNegativeManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    ts.Add(WorldManipulatorManager.Manipulations.TakeDamage);
    ts.Add(WorldManipulatorManager.Manipulations.IncreaseEnemyModifiersChance);
    ts.Add(WorldManipulatorManager.Manipulations.SpawnBombs);
    ts.Add(WorldManipulatorManager.Manipulations.DropPoisonOnAttack);
    ts.Add(WorldManipulatorManager.Manipulations.AllEnemiesHaveModifiersInNextRoom);
    if ((double) PlayerFarming.Instance.health.BlackHearts > 0.0 || (double) PlayerFarming.Instance.health.TotalSpiritHearts > 0.0 || (double) PlayerFarming.Instance.health.BlueHearts > 0.0)
      ts.Add(WorldManipulatorManager.Manipulations.LoseAllSpecialHearts);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleBasePositiveManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    ts.Add(WorldManipulatorManager.Manipulations.GainFaith);
    bool flag1 = false;
    for (int index = DataManager.Instance.UpgradeCoolDowns.Count - 1; index >= 0; --index)
    {
      if (!UpgradeSystem.IsRitualActive(DataManager.Instance.UpgradeCoolDowns[index].Type))
      {
        flag1 = true;
        break;
      }
    }
    if (flag1)
      ts.Add(WorldManipulatorManager.Manipulations.ResetTempleCooldowns);
    List<StructureBrain> structureBrainList = new List<StructureBrain>((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BUILD_SITE));
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT));
    if (structureBrainList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.InstantlyBuildStructures);
    List<Structures_Grave> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Grave>(FollowerLocation.Base);
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dead);
    bool flag2 = false;
    for (int index = followerInfoList.Count - 1; index >= 0; --index)
    {
      foreach (StructureBrain structureBrain in structuresOfType1)
      {
        if (structureBrain.Data.FollowerID == followerInfoList[index].ID)
        {
          flag2 = true;
          break;
        }
      }
      if (flag2)
        break;
    }
    if (flag2)
      ts.Add(WorldManipulatorManager.Manipulations.ResurrectBuriedFollower);
    bool flag3 = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.CursedState != Thought.None && allBrain.Info.CursedState != Thought.OldAge && !DataManager.Instance.Followers_Recruit.Contains(allBrain._directInfoAccess))
      {
        flag3 = true;
        break;
      }
    }
    if (flag3)
      ts.Add(WorldManipulatorManager.Manipulations.CureCursedFollowers);
    List<StructureBrain> structuresOfType2 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.VOMIT);
    List<StructureBrain> structuresOfType3 = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.POOP);
    if (structuresOfType2.Count > 0 || structuresOfType3.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.ClearAllWaste);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public static List<WorldManipulatorManager.Manipulations> GetPossibleBaseNegativeManipulations()
  {
    List<WorldManipulatorManager.Manipulations> ts = new List<WorldManipulatorManager.Manipulations>();
    if (TimeManager.CurrentPhase == DayPhase.Morning || TimeManager.CurrentPhase == DayPhase.Dawn)
      ts.Add(WorldManipulatorManager.Manipulations.SkipTime);
    bool flag = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
      {
        flag = true;
        break;
      }
    }
    if (flag)
    {
      ts.Add(WorldManipulatorManager.Manipulations.AllFollowersPoopOrVomit);
      ts.Add(WorldManipulatorManager.Manipulations.KillRandomFollower);
      if (TimeManager.CurrentPhase != DayPhase.Night || TimeManager.CurrentPhase != DayPhase.Dusk)
        ts.Add(WorldManipulatorManager.Manipulations.SleepFollowers);
    }
    if (FollowerBrain.RandomAvailableBrainNoCurseState() != null)
      ts.Add(WorldManipulatorManager.Manipulations.RandomCursedState);
    List<Structures_Bed> structuresBedList = new List<Structures_Bed>((IEnumerable<Structures_Bed>) StructureManager.GetAllStructuresOfType<Structures_Bed>());
    for (int index = structuresBedList.Count - 1; index >= 0; --index)
    {
      if (structuresBedList[index].IsCollapsed)
        structuresBedList.Remove(structuresBedList[index]);
    }
    if (structuresBedList.Count > 0)
      ts.Add(WorldManipulatorManager.Manipulations.BreakAllBeds);
    ts.Shuffle<WorldManipulatorManager.Manipulations>();
    return ts;
  }

  public enum Manipulations
  {
    GainRandomHeart = 0,
    HealHearts = 1,
    GainTarot = 2,
    DealDamageToAllEnemies = 3,
    ReceiveDemon = 4,
    InvincibleForTime = 5,
    TakeDamage = 100, // 0x00000064
    IncreaseEnemyModifiersChance = 101, // 0x00000065
    SpawnBombs = 102, // 0x00000066
    LoseAllSpecialHearts = 103, // 0x00000067
    DropPoisonOnAttack = 104, // 0x00000068
    AllEnemiesHaveModifiersInNextRoom = 105, // 0x00000069
    ResetTempleCooldowns = 200, // 0x000000C8
    InstantlyBuildStructures = 201, // 0x000000C9
    GainFaith = 202, // 0x000000CA
    ResurrectBuriedFollower = 203, // 0x000000CB
    CureCursedFollowers = 204, // 0x000000CC
    ClearAllWaste = 205, // 0x000000CD
    AllFollowersPoopOrVomit = 300, // 0x0000012C
    BreakAllBeds = 301, // 0x0000012D
    SkipTime = 302, // 0x0000012E
    SleepFollowers = 303, // 0x0000012F
    RandomCursedState = 304, // 0x00000130
    KillRandomFollower = 305, // 0x00000131
  }
}
