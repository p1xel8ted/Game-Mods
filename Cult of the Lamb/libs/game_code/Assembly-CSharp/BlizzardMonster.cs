// Decompiled with JetBrains decompiler
// Type: BlizzardMonster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlizzardMonster : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public LightingManagerVolume lightOverride;
  [SerializeField]
  public GameObject[] followerBones;
  public float targetDistanceOffset;
  public const int SNOWMEN_REQUIRED = 5;
  public List<Follower> snowmen = new List<Follower>();

  public void Configure(bool success, BlizzardMonster.Params info, System.Action callback)
  {
    this.StartCoroutine(this.PerformSequence(success, info, callback));
  }

  public IEnumerator PerformSequence(bool success, BlizzardMonster.Params info, System.Action callback)
  {
    BlizzardMonster blizzardMonster = this;
    Vector3 pos = new Vector3((double) UnityEngine.Random.value < 0.5 ? PlacementRegion.X_Constraints.x : PlacementRegion.X_Constraints.y, (float) UnityEngine.Random.Range(-23, -3));
    blizzardMonster.transform.position = pos;
    Vector3 dir = blizzardMonster.transform.position.normalized;
    blizzardMonster.spine.transform.localScale = new Vector3((double) dir.x > 0.0 ? 1f : -1f, 1f, 1f);
    blizzardMonster.spine.transform.localPosition = dir * 20f;
    blizzardMonster.spine.transform.localPosition += Vector3.forward * 7f;
    yield return (object) blizzardMonster.StartCoroutine(BlizzardMonster.FadeIn());
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PlayOneShot("event:/Stings/blizzard_monster_intro");
    Vector3 vector3 = pos - dir * 7.5f;
    SimulationManager.Pause();
    List<Follower> availableFollowers = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
      {
        availableFollowers.Add(follower);
        follower.Brain.CurrentTask?.Abort();
        follower.StopAllCoroutines();
        follower.State.LockStateChanges = true;
        follower.OverridingEmotions = true;
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        follower.HideAllFollowerIcons();
        follower.transform.position = vector3 + new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));
        follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        follower.FacePosition(blizzardMonster.transform.position);
      }
    }
    if (availableFollowers.Count > 0)
      GameManager.GetInstance().OnConversationNext(availableFollowers[UnityEngine.Random.Range(0, availableFollowers.Count)].gameObject, 3f);
    else
      GameManager.GetInstance().OnConversationNext(blizzardMonster.gameObject);
    yield return (object) new WaitForEndOfFrame();
    foreach (Follower follower in availableFollowers)
    {
      follower.State.LockStateChanges = false;
      follower.OverridingEmotions = false;
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sin/idle-ritual-sin" + UnityEngine.Random.Range(1, 4).ToString());
      double num = (double) follower.SetBodyAnimation("Sin/idle-ritual-sin" + UnityEngine.Random.Range(1, 4).ToString(), true);
      follower.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 1f);
    }
    yield return (object) blizzardMonster.StartCoroutine(BlizzardMonster.FadeOut());
    float duration = 11f;
    float time = 0.0f;
    Vector3 targetOffset = GameManager.GetInstance().CamFollowTarget.TargetOffset;
    float zoom = GameManager.GetInstance().CamFollowTarget.targetDistance;
    DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), duration, duration - 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.Lerp(targetOffset, targetOffset + Vector3.up * 1.5f + dir * 7f, time / (duration - 2f));
      GameManager.GetInstance().CamFollowTarget.targetDistance = Mathf.Lerp(zoom, 9f, time / (duration - 2f)) + this.targetDistanceOffset;
      BiomeConstants.Instance.DepthOfFieldTween(1.5f, GameManager.GetInstance().CamFollowTarget.targetDistance + 1f, 10f, 1f, 145f);
    })).SetDelay<TweenerCore<float, float, FloatOptions>>(3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    if (DataManager.Instance.BlizzardSnowmenGiven >= 5)
    {
      blizzardMonster.spine.gameObject.SetActive(false);
      blizzardMonster.StartCoroutine(blizzardMonster.SecretIE(availableFollowers, blizzardMonster.spine.transform.position, callback));
      blizzardMonster.StartCoroutine(blizzardMonster.EnterMonsterSequence((int) duration, dir, false));
    }
    else
    {
      yield return (object) blizzardMonster.StartCoroutine(blizzardMonster.EnterMonsterSequence((int) duration, dir, true));
      foreach (Follower follower in availableFollowers)
      {
        FollowerTask_RunAway.TimeSinceLastScream = 0.0f;
        follower.Brain.CurrentState = (FollowerState) new FollowerState_Following();
        follower.ResetStateAnimations();
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_RunAway(new Vector3(follower.transform.position.x * -1f, (float) UnityEngine.Random.Range(-23, -3))));
      }
      yield return (object) new WaitForSeconds(2f);
      yield return (object) blizzardMonster.StartCoroutine(BlizzardMonster.FadeIn());
      GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.zero;
      foreach (Follower follower in availableFollowers)
      {
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Terrified());
        follower.Brain.CurrentTask.SetState(FollowerTaskState.Idle);
      }
      List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SACRIFICE_TABLE, newList: true);
      if (structuresOfType.Count > 0 && !structuresOfType[0].Data.IsCollapsed)
      {
        blizzardMonster.transform.position = structuresOfType[0].Data.Position;
        if (success)
        {
          DataManager.Instance.BlizzardOfferingsGiven = new List<DataManager.Offering>();
          DataManager.Instance.BlizzardOfferingRequirements = new List<DataManager.Offering>();
          if (structuresOfType.Count > 0)
          {
            InventoryItem inventoryItem1 = new InventoryItem((InventoryItem.ITEM_TYPE) UnityEngine.Random.Range(180, 185), 1);
            structuresOfType[0].Data.Inventory.Add(inventoryItem1);
            InventoryItem inventoryItem2 = new InventoryItem();
            if ((double) UnityEngine.Random.value < 0.33000001311302185)
            {
              inventoryItem2.quantity = UnityEngine.Random.Range(25, 50);
              inventoryItem2.type = (double) UnityEngine.Random.value < 0.5 ? 172 : 139;
            }
            else if ((double) UnityEngine.Random.value < 0.6600000262260437)
            {
              inventoryItem2.quantity = UnityEngine.Random.Range(4, 7);
              inventoryItem2.type = (double) UnityEngine.Random.value < 0.5 ? 81 : 82;
            }
            else
            {
              inventoryItem2.quantity = UnityEngine.Random.Range(4, 7);
              inventoryItem2.type = 231;
            }
            structuresOfType[0].Data.Inventory.Add(inventoryItem2);
          }
          if ((UnityEngine.Object) Interaction_SacrificeTable.Instance != (UnityEngine.Object) null)
            Interaction_SacrificeTable.Instance.SetSprites();
          yield return (object) new WaitForSecondsRealtime(2f);
          GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.zero;
        }
        else
        {
          DataManager.Instance.BlizzardOfferingsGiven = new List<DataManager.Offering>();
          DataManager.Instance.BlizzardOfferingRequirements = new List<DataManager.Offering>();
          if ((UnityEngine.Object) Interaction_SacrificeTable.Instance != (UnityEngine.Object) null)
            Interaction_SacrificeTable.Instance.SetSprites();
          structuresOfType[0].Collapse();
        }
      }
      if (success)
      {
        if (!info.FollowersToKill[0]._directInfoAccess.IsSnowman)
          info.FollowersToKill.Clear();
      }
      else if (info.StructuresToBreak.Count > 0)
      {
        HUD_Manager.Instance.NotificationCenter.Show(true);
        NotificationCentre.Instance.ClearNotifications();
        NotificationCentre.NotificationsEnabled = false;
        foreach (StructureBrain structureBrain1 in info.StructuresToBreak)
        {
          StructureBrain structureBrain = structureBrain1;
          Structure structure1 = (Structure) null;
          foreach (Structure structure2 in Structure.Structures)
          {
            if (structure2.Brain == structureBrain)
              structure1 = structure2;
          }
          if (!((UnityEngine.Object) structure1 == (UnityEngine.Object) null))
          {
            GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
            GameManager.GetInstance().CamFollowTarget.ForceSnapTo(structure1.transform.position - GameManager.GetInstance().CamFollowTarget.transform.forward * 5f - Vector3.forward / 2f);
            GameManager.GetInstance().CamFollowTarget.targetDistance = zoom;
            BiomeConstants.Instance.DepthOfFieldTween(1.5f, GameManager.GetInstance().CamFollowTarget.targetDistance + 1f, 10f, 1f, 145f);
            yield return (object) blizzardMonster.StartCoroutine(BlizzardMonster.FadeOut());
            yield return (object) new WaitForSeconds(0.5f);
            NotificationCentre.NotificationsEnabled = true;
            if ((double) UnityEngine.Random.value < 0.5)
            {
              BiomeConstants.Instance.EmitSmokeInteractionVFX(structureBrain.Data.Position, Vector3.one * (float) structureBrain.Data.Bounds.x);
              structureBrain.Collapse();
              AudioManager.Instance.PlayOneShot("event:/material/building_collapse", structureBrain.Data.Position);
            }
            else
              structureBrain.SnowedUnder();
            NotificationCentre.NotificationsEnabled = false;
            yield return (object) new WaitForSeconds(1.5f);
            yield return (object) blizzardMonster.StartCoroutine(BlizzardMonster.FadeIn());
            structureBrain = (StructureBrain) null;
          }
        }
        HUD_Manager.Instance.NotificationCenter.Show(true);
        NotificationCentre.Instance.ClearNotifications();
        NotificationCentre.NotificationsEnabled = false;
        info.StructuresToBreak.Clear();
      }
      blizzardMonster.transform.position = new Vector3((double) UnityEngine.Random.value < 0.5 ? PlacementRegion.X_Constraints.x - 3f : PlacementRegion.X_Constraints.y + 3f, (float) UnityEngine.Random.Range(-23, -3));
      dir = blizzardMonster.transform.position.normalized;
      blizzardMonster.spine.transform.localPosition = Vector3.zero;
      blizzardMonster.spine.transform.localScale = new Vector3((double) dir.x > 0.0 ? -1f : 1f, 1f, 1f);
      int index = 0;
      foreach (FollowerBrain followerBrain in info.FollowersToKill)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          followerById.Brain.CompleteCurrentTask();
          followerById.enabled = false;
          followerById.transform.parent = blizzardMonster.followerBones[index].transform;
          followerById.transform.localPosition = new Vector3(0.25f, -0.6f, -0.3f);
          followerById.transform.localScale = Vector3.one;
          followerById.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          double num = (double) followerById.SetBodyAnimation("Sin/sin-floating", true);
          ++index;
        }
      }
      foreach (Follower follower in availableFollowers)
      {
        Follower f = follower;
        if (!info.FollowersToKill.Contains(f.Brain))
        {
          f.ResetStateAnimations();
          f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
          f.HideAllFollowerIcons();
          f.transform.position = blizzardMonster.transform.position + -dir * 12f + new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));
          f.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          f.FacePosition(blizzardMonster.transform.position);
          f.State.LockStateChanges = false;
          f.OverridingEmotions = false;
          double num = (double) f.SetBodyAnimation("Conversations/talk-hate" + UnityEngine.Random.Range(1, 4).ToString(), true);
          f.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 1f);
          if ((double) UnityEngine.Random.value < 0.5)
            GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(2f, 6f), (System.Action) (() => f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_SnowballFight(this.spine.transform.position + Vector3.forward * -2f, true))));
        }
      }
      GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
      GameManager.GetInstance().CamFollowTarget.ForceSnapTo(blizzardMonster.transform.position + Vector3.up * 2f - GameManager.GetInstance().CamFollowTarget.transform.forward * 9f);
      BiomeConstants.Instance.DepthOfFieldTween(1.5f, 10f, 10f, 1f, 145f);
      yield return (object) blizzardMonster.StartCoroutine(BlizzardMonster.FadeOut());
      blizzardMonster.StartCoroutine(blizzardMonster.ExitMonsterSequence(7f, new Vector3((double) dir.x > 0.0 ? -1f : 1f, 0.0f, 0.0f)));
      yield return (object) new WaitForSeconds(1f);
      foreach (FollowerBrain followerBrain in info.FollowersToKill)
      {
        NotificationCentre.NotificationsEnabled = true;
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.KilledByBlizzardMonster, followerBrain.Info, NotificationFollower.Animation.Dead);
        NotificationCentre.NotificationsEnabled = false;
      }
      WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Heavy, 2f);
      yield return (object) new WaitForSeconds(6f);
      GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.zero;
      SimulationManager.UnPause();
      foreach (FollowerBrain followerBrain in info.FollowersToKill)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
          followerById.Die(NotificationCentre.NotificationType.KilledByBlizzardMonster, false, force: true);
      }
      foreach (Follower follower in availableFollowers)
      {
        follower.State.CURRENT_STATE = StateMachine.State.Idle;
        follower.Brain.CheckChangeState();
        follower.Brain.CompleteCurrentTask();
        follower.ShowAllFollowerIcons();
      }
      yield return (object) new WaitForEndOfFrame();
      System.Action action = callback;
      if (action != null)
        action();
      UnityEngine.Object.Destroy((UnityEngine.Object) blizzardMonster.gameObject);
    }
  }

  public IEnumerator DestroyStructuresIE(BlizzardMonster.Params info)
  {
    BlizzardMonster blizzardMonster = this;
    foreach (StructureBrain structureBrain in info.StructuresToBreak)
    {
      blizzardMonster.transform.position = structureBrain.Data.Position;
      yield return (object) new WaitForSecondsRealtime(1f);
      structureBrain.Collapse();
    }
    yield return (object) new WaitForSecondsRealtime(1f);
  }

  public IEnumerator KillFollowersIE(BlizzardMonster.Params info)
  {
    BlizzardMonster blizzardMonster = this;
    foreach (FollowerBrain follower in info.FollowersToKill)
    {
      blizzardMonster.transform.position = follower.LastPosition;
      yield return (object) new WaitForSeconds(1f);
      Follower followerById = FollowerManager.FindFollowerByID(follower.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        followerById.Die(PlayAnimation: false, force: true);
      else
        follower.Die(NotificationCentre.NotificationType.Died);
    }
    yield return (object) new WaitForSeconds(1f);
  }

  public IEnumerator KillAnimalsIE(BlizzardMonster.Params info)
  {
    BlizzardMonster blizzardMonster = this;
    for (int i = 0; i < info.AnimalsToKill.Value; ++i)
    {
      if (info.AnimalsToKill.Key.Data.Animals.Count > i)
      {
        StructuresData.Ranchable_Animal animal = info.AnimalsToKill.Key.Data.Animals[i];
        foreach (Interaction_Ranchable ranchable1 in Interaction_Ranchable.Ranchables)
        {
          Interaction_Ranchable ranchable = ranchable1;
          if (ranchable.Animal.ID == animal.ID)
          {
            blizzardMonster.transform.position = ranchable.transform.position;
            yield return (object) new WaitForSeconds(1f);
            ranchable.Die();
            break;
          }
          ranchable = (Interaction_Ranchable) null;
        }
      }
    }
    yield return (object) new WaitForSeconds(1f);
  }

  public IEnumerator HitPlayerIE()
  {
    BlizzardMonster blizzardMonster = this;
    foreach (PlayerFarming player1 in PlayerFarming.players)
    {
      PlayerFarming player = player1;
      blizzardMonster.transform.position = player.transform.position;
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/player/gethit", player.transform.position);
      BiomeConstants.Instance.EmitHitVFX(player.transform.position, Quaternion.identity.z, "HitFX_Blocked");
      player.simpleSpineAnimator.FlashWhite(false);
      CameraManager.instance.ShakeCameraForDuration(1.5f, 1.7f, 0.2f);
      player.state.facingAngle = Utils.GetAngle(player.transform.position, blizzardMonster.transform.position);
      player.state.CURRENT_STATE = StateMachine.State.HitThrown;
      player.playerController.MakeUntouchable(1f, false);
      player.health.HP -= 2f;
      if (player.isLamb)
        DataManager.Instance.PLAYER_REMOVED_HEARTS += 2f;
      else
        DataManager.Instance.COOP_PLAYER_REMOVED_HEARTS += 2f;
      player.transform.DOMoveX(player.transform.position.x + ((double) player.transform.position.x > (double) blizzardMonster.transform.position.x ? 1.5f : -1.5f), 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => player.state.CURRENT_STATE = StateMachine.State.InActive));
    }
    yield return (object) new WaitForSeconds(1f);
  }

  public IEnumerator TakeDevotionIE()
  {
    this.transform.position = BuildingShrine.Shrines[0].transform.position;
    yield return (object) new WaitForSeconds(1f);
    BuildingShrine.Shrines[0].StructureBrain.Data.SoulCount = 0;
    BuildingShrine.Shrines[0].UpdateBar();
    yield return (object) new WaitForSeconds(1f);
  }

  public void FadeRedIn() => this.lightOverride.gameObject.SetActive(true);

  public void FadeRedAway()
  {
    DeviceLightingManager.StopAll();
    DeviceLightingManager.TransitionLighting(Color.red, Color.white, 1f, DeviceLightingManager.F_KEYS);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => DeviceLightingManager.UpdateLocation()));
    GameManager.GetInstance().SetDitherTween(0.0f);
    this.lightOverride.gameObject.SetActive(false);
  }

  public IEnumerator EnterMonsterSequence(int duration, Vector3 dir, bool roar)
  {
    this.spine.AnimationState.SetAnimation(0, "walk", true);
    float strength = 1f;
    for (int i = 0; i < duration; ++i)
    {
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/enemy/hopper_miniboss/hopper_miniboss_land", this.spine.gameObject);
      Vector3 endValue = this.spine.transform.localPosition - dir * 1.5f;
      if ((double) endValue.z > 0.0)
        --endValue.z;
      this.spine.transform.DOLocalMove(endValue, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      CameraManager.instance.shakeCamera1(2f * strength, (float) UnityEngine.Random.Range(0, 360));
      GameManager.GetInstance().CamFollowTarget.targetDistance += 0.25f;
      this.targetDistanceOffset += 0.25f;
      strength += 0.25f;
    }
    yield return (object) new WaitForSeconds(1f);
    if (roar)
    {
      this.spine.AnimationState.SetAnimation(0, nameof (roar), false);
      this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      yield return (object) new WaitForSeconds(0.5f);
      CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 1f);
      GameManager.GetInstance().CamFollowTarget.targetDistance += 5f;
    }
  }

  public IEnumerator ExitMonsterSequence(float duration, Vector3 dir)
  {
    this.spine.AnimationState.SetAnimation(0, "walk", true);
    float strength = 1f;
    for (int i = 0; (double) i < (double) duration; ++i)
    {
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/enemy/hopper_miniboss/hopper_miniboss_land", this.spine.gameObject);
      Vector3 endValue = this.spine.transform.localPosition - dir * 1.5f;
      if ((double) endValue.z > 0.0)
        --endValue.z;
      this.spine.transform.DOLocalMove(endValue, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      CameraManager.instance.shakeCamera1(2f * strength, (float) UnityEngine.Random.Range(0, 360));
      GameManager.GetInstance().CamFollowTarget.targetDistance += 0.25f;
      strength += 0.25f;
    }
    yield return (object) new WaitForSeconds(1f);
    this.spine.AnimationState.SetAnimation(0, "idle", true);
  }

  public static IEnumerator FadeIn()
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public static IEnumerator FadeOut()
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public IEnumerator SecretIE(
    List<Follower> availableFollowers,
    Vector3 spawnPosition,
    System.Action callback)
  {
    BlizzardMonster blizzardMonster = this;
    yield return (object) new WaitForSeconds(11f);
    foreach (Follower availableFollower in availableFollowers)
    {
      availableFollower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Egg/mating-consider" + UnityEngine.Random.Range(2, 5).ToString());
      double num = (double) availableFollower.SetBodyAnimation("Egg/mating-consider" + UnityEngine.Random.Range(2, 5).ToString(), true);
      availableFollower.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 1f);
    }
    FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Base, "IceGore");
    info.Name = LocalizationManager.GetTranslation("NAMES/Icegor");
    info.ID = 10008;
    info.TraitsSet = true;
    info.Traits.Clear();
    info.Traits.Add(FollowerTrait.TraitType.WarmBlooded);
    info.Traits.Add(FollowerTrait.TraitType.Aestivation);
    FollowerBrain resurrectingFollower = FollowerBrain.GetOrCreateBrain(info);
    Follower revivedFollower = FollowerManager.CreateNewFollower(resurrectingFollower._directInfoAccess, spawnPosition);
    revivedFollower.HideAllFollowerIcons();
    resurrectingFollower.Location = FollowerLocation.Base;
    resurrectingFollower.DesiredLocation = FollowerLocation.Base;
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    resurrectingFollower.ResetStats();
    resurrectingFollower.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
    yield return (object) new WaitForEndOfFrame();
    float moveDuration = 5f;
    Vector3 centerPoint = Vector3.zero;
    foreach (Component availableFollower in availableFollowers)
      centerPoint += availableFollower.transform.position;
    centerPoint /= (float) availableFollowers.Count;
    Vector3 pos = centerPoint;
    Vector3 targetPos = centerPoint + centerPoint.normalized * 5f;
    pos.x = spawnPosition.x;
    revivedFollower.IgnoreBounds = true;
    revivedFollower.transform.position = pos;
    revivedFollower.transform.DOMove(targetPos, moveDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    revivedFollower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "run");
    yield return (object) new WaitForEndOfFrame();
    revivedFollower.FacePosition(revivedFollower.transform.position + new Vector3((double) revivedFollower.transform.position.x > 0.0 ? -1f : 1f, 0.0f));
    blizzardMonster.StartCoroutine(blizzardMonster.SpawnSnowmenIE(pos + new Vector3(0.45f, 1.5f), targetPos + new Vector3(0.45f, 1.5f), moveDuration));
    blizzardMonster.StartCoroutine(blizzardMonster.SpawnSnowmenIE(pos + new Vector3(0.35f, -1.2f), targetPos + new Vector3(0.35f, -1.2f), moveDuration));
    blizzardMonster.StartCoroutine(blizzardMonster.SpawnSnowmenIE(pos + new Vector3(-0.35f, -1.35f), targetPos + new Vector3(-0.35f, -1.35f), moveDuration));
    blizzardMonster.StartCoroutine(blizzardMonster.SpawnSnowmenIE(pos + new Vector3(-0.45f, 1.25f), targetPos + new Vector3(-0.45f, 1.25f), moveDuration));
    PlayerFarming.Instance.GoToAndStop(centerPoint);
    yield return (object) new WaitForSeconds(moveDuration);
    GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.zero;
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 6f);
    double num1 = (double) revivedFollower.SetBodyAnimation("Conversations/greet-nice", false);
    revivedFollower.AddBodyAnimation("Reactions/react-embarrassed", false, 0.0f);
    revivedFollower.AddBodyAnimation("idle", true, 0.0f);
    foreach (Follower availableFollower in availableFollowers)
    {
      availableFollower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Conversations/react-hate" + UnityEngine.Random.Range(1, 4).ToString());
      double num2 = (double) availableFollower.SetBodyAnimation("Conversations/react-hate" + UnityEngine.Random.Range(1, 4).ToString(), true);
      availableFollower.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 1f);
    }
    availableFollowers[0].Brain.HardSwapToTask((FollowerTask) new FollowerTask_SnowballFight(revivedFollower.transform.position, true));
    yield return (object) new WaitForSeconds(5f);
    Vector3 normalized = (revivedFollower.transform.position - PlayerFarming.Instance.transform.position).normalized;
    PlayerFarming.Instance.GoToAndStop(revivedFollower.transform.position - normalized * 1.5f);
    revivedFollower.IgnoreBounds = false;
    revivedFollower.State.CURRENT_STATE = StateMachine.State.Idle;
    revivedFollower.SimpleAnimator.ResetAnimationsToDefaults();
    yield return (object) new WaitForSeconds(1f);
    yield return (object) blizzardMonster.StartCoroutine(revivedFollower.GetComponent<interaction_FollowerInteraction>().SimpleNewRecruitRoutine());
    while (UIMenuBase.ActiveMenus.Count > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(4.5f);
    resurrectingFollower.CompleteCurrentTask();
    revivedFollower.State.CURRENT_STATE = StateMachine.State.Idle;
    revivedFollower.ShowAllFollowerIcons();
    foreach (Follower snowman in blizzardMonster.snowmen)
    {
      snowman.IgnoreBounds = false;
      snowman.State.CURRENT_STATE = StateMachine.State.Idle;
      snowman.Brain.CompleteCurrentTask();
      snowman.ShowAllFollowerIcons();
    }
    GameManager.GetInstance().OnConversationEnd();
    SimulationManager.UnPause();
    foreach (Follower availableFollower in availableFollowers)
    {
      availableFollower.State.CURRENT_STATE = StateMachine.State.Idle;
      availableFollower.Brain.CompleteCurrentTask();
      availableFollower.ShowAllFollowerIcons();
    }
    DataManager.Instance.BlizzardOfferingsGiven = new List<DataManager.Offering>();
    DataManager.Instance.BlizzardOfferingRequirements = new List<DataManager.Offering>();
    List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SACRIFICE_TABLE, newList: true);
    if (structuresOfType.Count > 0)
    {
      InventoryItem inventoryItem = new InventoryItem((InventoryItem.ITEM_TYPE) UnityEngine.Random.Range(180, 185), 1);
      structuresOfType[0].Data.Inventory.Add(inventoryItem);
    }
    if ((UnityEngine.Object) Interaction_SacrificeTable.Instance != (UnityEngine.Object) null)
      Interaction_SacrificeTable.Instance.SetSprites();
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Heavy, 2f);
    DataManager.Instance.CompletedBlizzardSecret = true;
    System.Action action = callback;
    if (action != null)
      action();
    UnityEngine.Object.Destroy((UnityEngine.Object) blizzardMonster.gameObject);
  }

  public IEnumerator SpawnSnowmenIE(Vector3 spawnPosition, Vector3 targetPosition, float duration)
  {
    FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Base, $"Snowman/Good_{UnityEngine.Random.Range(1, 4)}");
    info.Name = LocalizationManager.GetTranslation("NAMES/Snowman");
    info.IsSnowman = true;
    info.Special = FollowerSpecialType.Snowman_Great;
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(info);
    Follower revivedFollower = FollowerManager.CreateNewFollower(brain._directInfoAccess, spawnPosition);
    revivedFollower.HideAllFollowerIcons();
    this.snowmen.Add(revivedFollower);
    brain.Location = FollowerLocation.Base;
    brain.DesiredLocation = FollowerLocation.Base;
    brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    brain.ResetStats();
    brain.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
    yield return (object) new WaitForEndOfFrame();
    revivedFollower.IgnoreBounds = true;
    revivedFollower.transform.position = spawnPosition;
    revivedFollower.transform.DOMove(targetPosition, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    revivedFollower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "run");
    revivedFollower.FacePosition(targetPosition);
    yield return (object) new WaitForSeconds(duration);
    double num = (double) revivedFollower.SetBodyAnimation("idle", true);
    revivedFollower.SimpleAnimator.ResetAnimationsToDefaults();
  }

  public struct Params
  {
    public List<FollowerBrain> FollowersToKill;
    public List<StructureBrain> StructuresToBreak;
    public KeyValuePair<Structures_Ranch, int> AnimalsToKill;
    public bool HitPlayer;
    public bool TakeDevotion;
  }
}
