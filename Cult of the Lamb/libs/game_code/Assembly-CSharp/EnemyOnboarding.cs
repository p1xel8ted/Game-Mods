// Decompiled with JetBrains decompiler
// Type: EnemyOnboarding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Map;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyOnboarding : BaseMonoBehaviour
{
  public UnitObject[] enemies;
  public Coroutine waitingRoutine;
  public GameObject chef;
  public bool redFadedIn;
  public static int dungeonRoomsEncounteredThisFloor;
  public GenerateRoom lastPossessedEnemySpawnRoom;
  public EventInstance possessedLoop;
  public EventInstance receiveLoop;

  public void OnEnable()
  {
    if (this.waitingRoutine != null)
      this.StopCoroutine(this.waitingRoutine);
    this.waitingRoutine = this.StartCoroutine((IEnumerator) this.WaitForTileToLoad());
  }

  public IEnumerator WaitForTileToLoad()
  {
    EnemyOnboarding enemyOnboarding = this;
    while ((UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom == (UnityEngine.Object) null || !BiomeGenerator.Instance.CurrentRoom.generateRoom.GeneratedDecorations || (UnityEngine.Object) Interaction_Chest.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    enemyOnboarding.waitingRoutine = (Coroutine) null;
    enemyOnboarding.OnboardEnemy();
    enemyOnboarding.SpawnShopKeeperChef();
    if ((UnityEngine.Object) enemyOnboarding.lastPossessedEnemySpawnRoom != (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom)
      ++EnemyOnboarding.dungeonRoomsEncounteredThisFloor;
    if (DataManager.Instance.Followers_Possessed.Count > 0 & (DataManager.Instance.BossesCompleted.Count >= 4 || DataManager.Instance.QuickStartActive) && !DungeonSandboxManager.Active && !BiomeGenerator.Instance.CurrentRoom.Completed && (UnityEngine.Object) enemyOnboarding.lastPossessedEnemySpawnRoom != (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom && (UnityEngine.Object) DungeonLeaderMechanics.Instance == (UnityEngine.Object) null && !BiomeGenerator.Instance.OverrideRandomWalk && (DataManager.Instance.dungeonRun == DataManager.Instance.EncounteredPossessedEnemyRun || DataManager.Instance.dungeonRun - DataManager.Instance.EncounteredPossessedEnemyRun > 1) && (BiomeGenerator.Instance.TargetPossessedEnemy == -1 || DataManager.Instance.Followers_Possessed[0].ID == BiomeGenerator.Instance.TargetPossessedEnemy))
    {
      DataManager.Instance.EncounteredPossessedEnemyRun = DataManager.Instance.dungeonRun;
      float num1 = 0.33f;
      float num2;
      if (!BiomeGenerator.Instance.EncounteredPossessedEnemyThisFloor)
      {
        num2 = num1 * (float) EnemyOnboarding.dungeonRoomsEncounteredThisFloor;
      }
      else
      {
        EnemyOnboarding.dungeonRoomsEncounteredThisFloor = 0;
        num2 = 0.0f;
      }
      if ((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentMap != null && MapManager.Instance.CurrentMap.GetFinalBossNode() == MapManager.Instance.CurrentNode)
        num2 = 1f;
      if (BiomeGenerator.Instance.PossessedEnemyEncounterCount < 3 && (double) UnityEngine.Random.value < (double) num2 && (UnityEngine.Object) Interaction_Chest.Instance != (UnityEngine.Object) null)
        enemyOnboarding.StartCoroutine((IEnumerator) enemyOnboarding.SpawnPossessedFollower());
    }
  }

  public void OnboardEnemy()
  {
    if (DataManager.Instance == null || DungeonSandboxManager.Active)
      return;
    UnitObject unitObject = (UnitObject) null;
    this.enemies = this.GetComponentsInChildren<UnitObject>(true);
    foreach (UnitObject enemy in this.enemies)
    {
      if (!DataManager.Instance.HasEncounteredEnemy(enemy.name) && (bool) (UnityEngine.Object) enemy.GetComponent<EnemyRequiresOnboarding>() && enemy.gameObject.activeSelf)
        unitObject = enemy;
    }
    if (!((UnityEngine.Object) unitObject != (UnityEngine.Object) null))
      return;
    Interaction_Chest instance = Interaction_Chest.Instance;
    foreach (UnitObject enemy in this.enemies)
    {
      if ((UnityEngine.Object) enemy != (UnityEngine.Object) unitObject)
      {
        Health.team2.Remove(enemy.health);
        instance.Enemies.Remove(enemy.health);
        UnityEngine.Object.Destroy((UnityEngine.Object) enemy.gameObject);
      }
    }
    foreach (RaycastHit2D raycastHit2D in Physics2D.CircleCastAll((Vector2) instance.transform.position, 2f, Vector2.zero))
    {
      Health component = raycastHit2D.collider.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team != Health.Team.PlayerTeam && component.team != Health.Team.Team2 && (UnityEngine.Object) component != (UnityEngine.Object) unitObject.health)
        component.DealDamage((float) int.MaxValue, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    }
    BreakableSpiderNest[] componentsInChildren1 = this.GetComponentsInChildren<BreakableSpiderNest>();
    if (componentsInChildren1.Length != 0)
    {
      for (int index = componentsInChildren1.Length - 1; index >= 0; --index)
        componentsInChildren1[index].GetComponent<Health>().DealDamage(float.MaxValue, this.gameObject, this.transform.position);
    }
    SpiderNest[] componentsInChildren2 = this.GetComponentsInChildren<SpiderNest>();
    if (componentsInChildren2.Length != 0)
    {
      for (int index = componentsInChildren2.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren2[index].gameObject);
    }
    TrapCharger[] componentsInChildren3 = this.GetComponentsInChildren<TrapCharger>();
    if (componentsInChildren3.Length != 0)
    {
      for (int index = componentsInChildren3.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren3[index].gameObject);
    }
    TrapSpikes[] componentsInChildren4 = this.GetComponentsInChildren<TrapSpikes>();
    if (componentsInChildren4.Length != 0)
    {
      for (int index = componentsInChildren4.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren4[index].ParentToDestroy);
    }
    TrapProjectileCross[] componentsInChildren5 = this.GetComponentsInChildren<TrapProjectileCross>();
    if (componentsInChildren5.Length != 0)
    {
      for (int index = componentsInChildren5.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren5[index].gameObject);
    }
    TrapRockFall[] componentsInChildren6 = this.GetComponentsInChildren<TrapRockFall>();
    if (componentsInChildren6.Length != 0)
    {
      for (int index = componentsInChildren6.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren6[index].gameObject);
    }
    unitObject.transform.position = Interaction_Chest.Instance.transform.position;
    unitObject.RemoveModifier();
    Health.team2.Clear();
    Health.team2.Add(unitObject.GetComponent<Health>());
    DataManager.Instance.AddEncounteredEnemy(unitObject.name);
  }

  public void SpawnShopKeeperChef()
  {
    bool flag = (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom.GetComponentInChildren<DungeonLeaderMechanics>(true) != (UnityEngine.Object) null;
    if (DataManager.Instance.ShopKeeperChefState != 1 || flag || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1) || DataManager.Instance.playerDeathsInARow != 0 || (double) UnityEngine.Random.Range(0.0f, 1f) >= 0.05000000074505806)
      return;
    Interaction_Chest instance = Interaction_Chest.Instance;
    this.enemies = this.GetComponentsInChildren<UnitObject>(true);
    if (this.enemies.Length <= 1)
      return;
    int index = 0;
    if (index >= this.enemies.Length)
      return;
    Vector3 position = this.enemies[index].transform.position;
    Transform parent = this.enemies[index].transform.parent;
    instance.Enemies.Remove(this.enemies[index].health);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.enemies[index].gameObject);
    this.chef = UnityEngine.Object.Instantiate<GameObject>(BiomeConstants.Instance.ShopKeeperChef, position, Quaternion.identity, parent);
    this.chef.GetComponent<Health>().OnDie += new Health.DieAction(this.ChefDied);
    instance.AddEnemy(this.chef.GetComponent<Health>());
    if (RoomLockController.DoorsOpen)
      this.chef.GetComponentInChildren<Interaction_SimpleConversation>(true).Interactable = true;
    else
      RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.OnRoomCompleted);
  }

  public void ForceSpawnPossessedFollower()
  {
  }

  public void ForceTestIndoctrinationSequence()
  {
  }

  public IEnumerator SpawnAndTestIndoctrination()
  {
    EnemyOnboarding enemyOnboarding = this;
    yield return (object) enemyOnboarding.StartCoroutine((IEnumerator) enemyOnboarding.SpawnPossessedFollower());
    yield return (object) new WaitForSeconds(1f);
    EnemyFollowerPossessed[] objectsOfType = UnityEngine.Object.FindObjectsOfType<EnemyFollowerPossessed>();
    if (objectsOfType.Length != 0 && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      Follower component = objectsOfType[0].GetComponent<Follower>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.transform.position = PlayerFarming.Instance.transform.position;
    }
    enemyOnboarding.ForceTestIndoctrinationSequence();
  }

  public IEnumerator SpawnPossessedFollower()
  {
    EnemyOnboarding enemyOnboarding = this;
    enemyOnboarding.lastPossessedEnemySpawnRoom = BiomeGenerator.Instance.CurrentRoom.generateRoom;
    bool firstEncounter = BiomeGenerator.Instance.PossessedEnemyEncounterCount == 0;
    DataManager.Instance.Followers_Possessed = DataManager.Instance.Followers_Possessed.OrderByDescending<FollowerInfo, int>((Func<FollowerInfo, int>) (x => x.LeftCultDay)).ToList<FollowerInfo>();
    FollowerInfo followerInfo = DataManager.Instance.Followers_Possessed[0];
    if (TimeManager.CurrentDay - DataManager.Instance.Followers_Possessed[0].LeftCultDay > 5)
    {
      DataManager.Instance.Followers_Possessed = DataManager.Instance.Followers_Possessed.OrderByDescending<FollowerInfo, bool>((Func<FollowerInfo, bool>) (x => FollowerManager.UniqueFollowerIDs.Contains(x.ID))).ToList<FollowerInfo>();
      followerInfo = DataManager.Instance.Followers_Possessed[0];
      if (!FollowerManager.UniqueFollowerIDs.Contains(followerInfo.ID))
      {
        DataManager.Instance.Followers_Possessed = DataManager.Instance.Followers_Possessed.OrderByDescending<FollowerInfo, int>((Func<FollowerInfo, int>) (x => x.XPLevel)).ToList<FollowerInfo>();
        followerInfo = DataManager.Instance.Followers_Possessed[0];
      }
    }
    BiomeGenerator.Instance.TargetPossessedEnemy = followerInfo.ID;
    bool waiting = true;
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.CombatFollowerPrefab, followerInfo, Vector3.zero, enemyOnboarding.transform.parent, BiomeGenerator.Instance.DungeonLocation);
    spawnedFollower.Follower.Health.untouchable = true;
    spawnedFollower.Follower.Health.CanBeTurnedIntoCritter = false;
    spawnedFollower.Follower.Spine.transform.localPosition = new Vector3(0.0f, 0.0f, -5f);
    if (followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      spawnedFollower.FollowerBrain.AddTrait(FollowerTrait.TraitType.Mutated);
      if (!spawnedFollower.FollowerFakeInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
        spawnedFollower.FollowerFakeInfo.Traits.Add(FollowerTrait.TraitType.Mutated);
      FollowerBrain.SetFollowerCostume(spawnedFollower.Follower.Spine.Skeleton, spawnedFollower.FollowerFakeInfo, forceUpdate: true);
    }
    Interaction_Chest.Instance.AddEnemy(spawnedFollower.Follower.Health);
    spawnedFollower.Follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Possessed/idle-possessed");
    spawnedFollower.Follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Possessed/walk-possessed");
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.inOverride = true;
    LightingManager.Instance.overrideSettings = LightingManager.Instance.redSettings;
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    LightingManager.Instance.UpdateLighting(true);
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(enemyOnboarding.OnRoomCompleted);
    enemyOnboarding.redFadedIn = true;
    spawnedFollower.Follower.gameObject.SetActive(false);
    while (PlayerFarming.AnyPlayerGotoAndStopping() || !GameManager.RoomActive)
      yield return (object) null;
    if (firstEncounter)
    {
      foreach (Health health in Health.team2)
      {
        if ((UnityEngine.Object) health != (UnityEngine.Object) null && (UnityEngine.Object) health != (UnityEngine.Object) spawnedFollower.Follower.Health)
          health.AddFreezeTime();
      }
      foreach (Health health in Health.playerTeam)
      {
        if ((UnityEngine.Object) health != (UnityEngine.Object) null && (UnityEngine.Object) health != (UnityEngine.Object) PlayerFarming.Instance.health)
          health.AddFreezeTime();
      }
      Health.isGlobalTimeFreeze = true;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(Interaction_Chest.Instance.gameObject, 8f);
      yield return (object) new WaitForSeconds(0.5f);
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
      AudioManager.Instance.PlayOneShot("event:/Stings/thenight_sacrifice_followers");
      spawnedFollower.Follower.gameObject.SetActive(true);
      spawnedFollower.Follower.State.facingAngle = Utils.GetAngle(enemyOnboarding.transform.position, PlayerFarming.Instance.transform.position);
      double num = (double) spawnedFollower.Follower.SetBodyAnimation("Possessed/spawn-possessed", false);
      spawnedFollower.Follower.AddBodyAnimation("Possessed/idle-possessed", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/possessed/spawn", spawnedFollower.Follower.gameObject);
      yield return (object) new WaitForSeconds(0.5f);
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    }
    spawnedFollower.Follower.gameObject.SetActive(true);
    spawnedFollower.Follower.Spine.transform.DOLocalMove(Vector3.zero, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      waiting = false;
      if (firstEncounter)
        GameManager.GetInstance().OnConversationNext(Interaction_Chest.Instance.gameObject, 5f);
      for (int index = Health.team2.Count - 1; index >= 0; --index)
      {
        if ((double) Vector3.Distance(spawnedFollower.Follower.transform.position, Health.team2[index].transform.position) < 1.0)
          Health.team2[index].DealDamage(Health.team2[index].HP, Health.team2[index].gameObject, Health.team2[index].transform.position);
      }
    }));
    while (waiting)
      yield return (object) null;
    if (firstEncounter && DataManager.Instance.damnedFightConversation <= 2)
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>();
      switch (DataManager.Instance.damnedFightConversation)
      {
        case 0:
          Entries.Add(new ConversationEntry(spawnedFollower.Follower.gameObject, "FollowerInteractions/FollowerDamnFight0/0"));
          break;
        case 1:
          Entries.Add(new ConversationEntry(spawnedFollower.Follower.gameObject, "FollowerInteractions/FollowerDamnFight0/0"));
          break;
        case 2:
          Entries.Add(new ConversationEntry(spawnedFollower.Follower.gameObject, "FollowerInteractions/FollowerDamnFight0/0"));
          break;
      }
      ++DataManager.Instance.damnedFightConversation;
      if (DataManager.Instance.damnedFightConversation >= 3)
        DataManager.Instance.damnedFightConversation = 0;
      foreach (ConversationEntry conversationEntry in Entries)
      {
        conversationEntry.CharacterName = spawnedFollower.FollowerBrain.Info.Name;
        conversationEntry.Animation = "Sin/sin-floating-talk";
      }
      EventInstance SinLoop = AudioManager.Instance.CreateLoop("event:/dialogue/followers/dissent_megaphone", true, false);
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => AudioManager.Instance.StopLoop(SinLoop))), false);
      yield return (object) null;
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/possessed/roar", spawnedFollower.Follower.gameObject);
    double num1 = (double) spawnedFollower.Follower.SetBodyAnimation("Possessed/roar-possessed", false);
    spawnedFollower.Follower.AddBodyAnimation("Possessed/idle-possessed", true, 0.0f);
    EnemyFollowerPossessed enemy = spawnedFollower.Follower.GetComponent<EnemyFollowerPossessed>();
    enemy.health.DontCombo = true;
    enemy.health.DestroyOnDeath = false;
    enemy.GetComponent<DropLootOnDeath>().enabled = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) enemy.GetComponent<SpawnDeadBodyOnDeath>());
    enemy.health.OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) => GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      if (BiomeGenerator.Instance.PossessedEnemyEncounterCount >= 3)
      {
        GameManager.GetInstance().StartCoroutine((IEnumerator) this.KilledPossessedEnemyIE(spawnedFollower));
      }
      else
      {
        DOTween.To((DOGetter<float>) (() => spawnedFollower.Follower.Spine.skeleton.A), (DOSetter<float>) (x => spawnedFollower.Follower.Spine.skeleton.A = x), 0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
        this.StartCoroutine((IEnumerator) EnemyOnboarding.\u003CSpawnPossessedFollower\u003Eg__SlowMo\u007C14_10());
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/possessed/defeated", spawnedFollower.Follower.gameObject);
        AudioManager.Instance.StopLoop(this.possessedLoop);
        enemy.enabled = false;
        spawnedFollower.Follower.TimedAnimation("Possessed/defeated-possessed", 1f, (System.Action) (() =>
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) enemy.gameObject);
          FollowerManager.CleanUpCopyFollower(spawnedFollower);
        }));
      }
    })));
    yield return (object) new WaitForSeconds(0.733333349f);
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.6f, 0.5f);
    yield return (object) new WaitForSeconds(1.26666665f);
    ++BiomeGenerator.Instance.PossessedEnemyEncounterCount;
    BiomeGenerator.Instance.EncounteredPossessedEnemyThisFloor = true;
    if (!((UnityEngine.Object) enemy == (UnityEngine.Object) null))
    {
      enemy.enabled = true;
      spawnedFollower.Follower.Health.untouchable = false;
      spawnedFollower.Follower.Health.CanBeTurnedIntoCritter = true;
      enemyOnboarding.possessedLoop = AudioManager.Instance.CreateLoop("event:/dialogue/followers/possessed/idle", spawnedFollower.Follower.gameObject);
      if (firstEncounter)
      {
        for (int index = 0; index < PlayerFarming.playersCount; ++index)
          PlayerFarming.players[index].health.untouchable = true;
        GameManager.GetInstance().OnConversationEnd();
        foreach (Health health in Health.team2)
        {
          if ((UnityEngine.Object) health != (UnityEngine.Object) null && (UnityEngine.Object) health != (UnityEngine.Object) spawnedFollower.Follower.Health)
            health.ClearFreezeTime();
        }
        foreach (Health health in Health.playerTeam)
        {
          if ((UnityEngine.Object) health != (UnityEngine.Object) null && (UnityEngine.Object) health != (UnityEngine.Object) PlayerFarming.Instance.health)
            health.ClearFreezeTime();
        }
        Health.isGlobalTimeFreeze = false;
        yield return (object) new WaitForSeconds(0.5f);
        for (int index = 0; index < PlayerFarming.playersCount; ++index)
          PlayerFarming.players[index].health.untouchable = false;
      }
    }
  }

  public IEnumerator KilledPossessedEnemyIE(FollowerManager.SpawnedFollower spawnedFollower)
  {
    EnemyOnboarding enemyOnboarding = this;
    bool waiting = true;
    while (waiting)
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        if ((double) PlayerFarming.players[index].health.CurrentHP > 0.0)
          waiting = false;
      }
      if (LetterBox.IsPlaying || !enemyOnboarding.gameObject.activeInHierarchy)
        waiting = true;
      yield return (object) new WaitForEndOfFrame();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) spawnedFollower.Follower.GetComponent<EnemyFollowerPossessed>());
    Vector3 right = Vector3.right;
    Vector3 vector3_1 = spawnedFollower.Follower.transform.position + right * 2f;
    int mask = LayerMask.GetMask("Island");
    if ((bool) Physics2D.Raycast((Vector2) spawnedFollower.Follower.transform.position, (Vector2) right, 2f, mask))
      vector3_1 = spawnedFollower.Follower.transform.position + -right * 2f;
    Vector3 closestPoint1;
    if (!BiomeGenerator.PointWithinIsland(vector3_1, out closestPoint1))
    {
      vector3_1 = closestPoint1;
      Vector3 closestPoint2;
      if (!BiomeGenerator.PointWithinIsland(vector3_1, out closestPoint2))
      {
        Vector3[] vector3Array = new Vector3[3]
        {
          Vector3.left,
          Vector3.up,
          Vector3.down
        };
        foreach (Vector3 vector3_2 in vector3Array)
        {
          Vector3 closestPoint3;
          if (BiomeGenerator.PointWithinIsland(spawnedFollower.Follower.transform.position + vector3_2 * 2f, out closestPoint3))
          {
            vector3_1 = closestPoint3;
            break;
          }
        }
        if (!BiomeGenerator.PointWithinIsland(vector3_1, out closestPoint2))
          vector3_1 = spawnedFollower.Follower.transform.position;
      }
    }
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
      PlayerFarming.players[index].GoToAndStop(vector3_1, forcePositionOnTimeout: true);
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) spawnedFollower.Follower.Health)
      {
        Health.team2[index].HasShield = false;
        Health.team2[index].invincible = false;
        Health.team2[index].DealDamage(Health.team2[index].HP, enemyOnboarding.gameObject, Health.team2[index].transform.position);
      }
    }
    DataManager.Instance.Followers_Possessed.Remove(spawnedFollower.FollowerBrain._directInfoAccess);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject, 6f);
    spawnedFollower.Follower.State.LockStateChanges = false;
    double num1 = (double) spawnedFollower.Follower.SetBodyAnimation("Possessed/die-possessed", false);
    spawnedFollower.Follower.AddBodyAnimation("Possessed/dead-possessed", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/possessed/die", spawnedFollower.Follower.gameObject);
    AudioManager.Instance.StopLoop(enemyOnboarding.possessedLoop);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 0.699999988079071)
    {
      GameManager.SetTimeScale(0.2f);
      yield return (object) null;
    }
    GameManager.SetTimeScale(1f);
    while (PlayerFarming.AnyPlayerGotoAndStopping())
      yield return (object) null;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      player.state.facingAngle = Utils.GetAngle(player.transform.position, spawnedFollower.Follower.transform.position);
      player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      player.simpleSpineAnimator.Animate("Sin/collect", 0, false);
      player.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    }
    double num2 = (double) spawnedFollower.Follower.SetBodyAnimation("Sin/sin-collect", false);
    GameObject godTear = (GameObject) null;
    godTear = UnityEngine.Object.Instantiate<GameObject>(spawnedFollower.Follower.rewardPrefab, spawnedFollower.Follower.Spine.transform.position + new Vector3(0.0f, -0.1f, -1f), Quaternion.identity, enemyOnboarding.transform.parent);
    godTear.transform.localScale = Vector3.zero;
    godTear.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up", enemyOnboarding.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", enemyOnboarding.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", enemyOnboarding.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.1f);
    PlayerSimpleInventory simpleInventory = PlayerFarming.Instance.simpleInventory;
    Vector3 endValue = new Vector3(simpleInventory.ItemImage.transform.position.x, simpleInventory.ItemImage.transform.position.y, -1f);
    GameManager.GetInstance().OnConversationNext(godTear, 6f);
    AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", PlayerFarming.Instance.gameObject);
    godTear.transform.DOMove(endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    spawnedFollower.FollowerBrain.Location = FollowerLocation.Base;
    DataManager.Instance.Followers_Recruit.Add(spawnedFollower.FollowerBrain._directInfoAccess);
    ++DataManager.Instance.FollowersRecruitedThisNode;
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, false);
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
      PlayerFarming.players[index].simpleSpineAnimator.Animate("idle", 0, false);
    godTear.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) godTear.gameObject)));
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/followers/rescue", enemyOnboarding.gameObject);
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "convert-short", false);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", enemyOnboarding.gameObject);
    spawnedFollower.Follower.Portal.gameObject.SetActive(true);
    spawnedFollower.Follower.Portal.AnimationState.SetAnimation(0, "convert-short", false);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    enemyOnboarding.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    float num3 = 0.0f;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming.players[index].state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      num3 = PlayerFarming.Instance.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration;
    }
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(num3 - 1f);
    spawnedFollower.Follower.Portal.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num4 = (int) enemyOnboarding.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    UnityEngine.Object.Destroy((UnityEngine.Object) spawnedFollower.Follower.gameObject);
    FollowerManager.CleanUpCopyFollower(spawnedFollower);
    Inventory.AddItem(154, 1);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void OnRoomCompleted()
  {
    if ((UnityEngine.Object) this.chef != (UnityEngine.Object) null)
      this.chef.GetComponentInChildren<Interaction_SimpleConversation>(true).Interactable = true;
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.OnRoomCompleted);
    this.FadeOut();
  }

  public void ChefDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    DataManager.Instance.ShopKeeperChefState = 2;
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) this.chef != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.chef.gameObject);
  }

  public void FadeOut()
  {
    if (!this.redFadedIn)
      return;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    LightingManager.Instance.UpdateLighting(true);
    LightingManager.Instance.PrepareLightingSettings();
    this.redFadedIn = false;
  }

  public void OnDestroy()
  {
    this.FadeOut();
    AudioManager.Instance.StopLoop(this.possessedLoop);
    AudioManager.Instance.StopLoop(this.receiveLoop);
  }

  [CompilerGenerated]
  public static IEnumerator \u003CSpawnPossessedFollower\u003Eg__SlowMo\u007C14_10(bool setZoom = true)
  {
    if (setZoom)
      GameManager.GetInstance().CameraSetZoom(6f);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 0.699999988079071)
    {
      GameManager.SetTimeScale(0.2f);
      yield return (object) null;
    }
    GameManager.SetTimeScale(1f);
    GameManager.GetInstance().CameraResetTargetZoom();
  }
}
