// Decompiled with JetBrains decompiler
// Type: WarriorTrioMinibossController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using Unify;
using UnityEngine;

#nullable disable
public class WarriorTrioMinibossController : MiniBossController
{
  public MiniBossManager miniBossManager;
  public WarriorTrioManager warriorTrioManager;
  public const string swordFollowerSkin = "Boss Dog 6";
  public const string staffFollowerSkin = "Boss Dog 4";
  public const string axeFollowerSkin = "Boss Dog 5";
  public const string swordDisplayName = "NAMES/MiniBoss/Dungeon5/MiniBoss4";
  public const string axeDisplayName = "NAMES/MiniBoss/Dungeon5/MiniBoss4_2";
  public const string staffDisplayName = "NAMES/MiniBoss/Dungeon5/MiniBoss4_3";
  public Dictionary<string, string> skinDisplayNameDictionary = new Dictionary<string, string>()
  {
    {
      "Boss Dog 6",
      "NAMES/MiniBoss/Dungeon5/MiniBoss4"
    },
    {
      "Boss Dog 4",
      "NAMES/MiniBoss/Dungeon5/MiniBoss4_3"
    },
    {
      "Boss Dog 5",
      "NAMES/MiniBoss/Dungeon5/MiniBoss4_2"
    }
  };

  public override void OnDeathLogicCustom(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.miniBossManager.IncrementDeathCount();
    Victim.OnDie -= new Health.DieAction(((MiniBossController) this).OnDeathLogicCustom);
    TrapPoison.RemoveAllPoison();
    Projectile.ClearProjectiles();
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_5:
        ++DataManager.Instance.DLCDungeon5MiniBossIndex;
        break;
      case FollowerLocation.Dungeon1_6:
        ++DataManager.Instance.DLCDungeon6MiniBossIndex;
        break;
    }
    if (this.miniBossManager.GetForceReward() == InventoryItem.ITEM_TYPE.GOD_TEAR)
    {
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, Vector3.ClampMagnitude(Victim.transform.position + Vector3.back, 4f), 0.0f);
      pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      pickUp.MagnetToPlayer = false;
      PlayerReturnToBase.Disabled = true;
    }
    if (!this.miniBossManager.Completed && (Victim.gameObject.CompareTag("Boss") || Health.team2.Count <= 1))
    {
      DOTween.Kill((object) this);
      DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
      this.miniBossManager.Completed = true;
      if (!DataManager.Instance.CheckKilledBosses(this.name) && !DungeonSandboxManager.Active)
      {
        PlayerReturnToBase.Disabled = true;
        AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_FIRST_BOSS"));
        AudioManager.Instance.PlayOneShot("event:/Stings/boss_kill", PlayerFarming.Instance.gameObject);
        this.SpawnNewFollowers(Victim.transform.position, Victim.transform.parent);
      }
      string name = this.name;
      if (GameManager.Layer2)
      {
        string str = name + "_P2";
      }
    }
    if (GameManager.CurrentDungeonLayer >= 4)
      return;
    DataManager.Instance.SetDungeonLayer(BiomeGenerator.Instance.DungeonLocation, GameManager.CurrentDungeonLayer + 1);
  }

  public void SpawnNewFollowers(Vector3 centrePosition, Transform parent)
  {
    string[] lastWarriorKilled = this.GetSortedSkinNamesByLastWarriorKilled();
    string skinName1 = lastWarriorKilled[0];
    string skinName2 = lastWarriorKilled[1];
    string skinName3 = lastWarriorKilled[2];
    string skinDisplayName1 = this.skinDisplayNameDictionary[lastWarriorKilled[0]];
    string skinDisplayName2 = this.skinDisplayNameDictionary[lastWarriorKilled[1]];
    string skinDisplayName3 = this.skinDisplayNameDictionary[lastWarriorKilled[2]];
    GameObject gameObject1 = new GameObject("WarriorTrioParent");
    gameObject1.transform.parent = parent;
    Vector3 zero = Vector3.zero;
    Vector3 relativePosition1 = new Vector3(-1.7f, 1.7f, 0.0f);
    Vector3 relativePosition2 = new Vector3(1.7f, 1.7f, 0.0f);
    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.miniBossManager.FollowerToSpawn, centrePosition + zero, Quaternion.identity, gameObject1.transform);
    GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.miniBossManager.FollowerToSpawn, centrePosition + relativePosition1, Quaternion.identity, gameObject1.transform);
    GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.miniBossManager.FollowerToSpawn, centrePosition + relativePosition2, Quaternion.identity, gameObject1.transform);
    this.miniBossManager.SetFollowerToWaitFor(gameObject2);
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> dictionary = new Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata>()
    {
      {
        gameObject2,
        new Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata(skinName1, skinDisplayName1, zero)
      },
      {
        gameObject3,
        new Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata(skinName2, skinDisplayName2, relativePosition1)
      },
      {
        gameObject4,
        new Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata(skinName3, skinDisplayName3, relativePosition2)
      }
    };
    this.DisableFollowerSpawnInteractions(dictionary);
    Interaction_FollowerSpawnWarriorTrio spawnWarriorTrio = gameObject2.AddComponent<Interaction_FollowerSpawnWarriorTrio>();
    spawnWarriorTrio.OutlineTarget = gameObject1;
    spawnWarriorTrio.Play(dictionary, gameObject2);
    DataManager.SetFollowerSkinUnlocked("Boss Dog 6");
    DataManager.SetFollowerSkinUnlocked("Boss Dog 5");
    DataManager.SetFollowerSkinUnlocked("Boss Dog 4");
  }

  public void DisableFollowerSpawnInteractions(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> followersData)
  {
    foreach (GameObject key in followersData.Keys)
      key.GetComponent<Interaction_FollowerSpawn>().enabled = false;
  }

  public string[] GetSortedSkinNamesByLastWarriorKilled()
  {
    return this.RotateToFront(new string[3]
    {
      "Boss Dog 6",
      "Boss Dog 5",
      "Boss Dog 4"
    }, this.GetLastWarriorKilled());
  }

  public string GetLastWarriorKilled()
  {
    string lastWarriorKilled = "Boss Dog 6";
    if ((UnityEngine.Object) this.warriorTrioManager.LastWarriorHit != (UnityEngine.Object) null)
    {
      if (this.warriorTrioManager.LastWarriorHit is EnemyWolfGuardian_Sword)
        lastWarriorKilled = "Boss Dog 6";
      else if (this.warriorTrioManager.LastWarriorHit is EnemyWolfGuardian_Staff)
        lastWarriorKilled = "Boss Dog 4";
      else if (this.warriorTrioManager.LastWarriorHit is EnemyWolfGuardian_Axe)
        lastWarriorKilled = "Boss Dog 5";
    }
    return lastWarriorKilled;
  }

  public string[] RotateToFront(string[] array, string target)
  {
    int num = Array.IndexOf<string>(array, target);
    if (num == -1 || array.Length <= 1)
      return array;
    string[] front = new string[array.Length];
    for (int index = 0; index < array.Length; ++index)
      front[index] = array[(num + index) % array.Length];
    return front;
  }
}
