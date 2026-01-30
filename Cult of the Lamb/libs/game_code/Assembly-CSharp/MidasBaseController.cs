// Decompiled with JetBrains decompiler
// Type: MidasBaseController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using MMTools;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MidasBaseController : MonoBehaviour
{
  public static MidasBaseController Instance;
  [SerializeField]
  public GameObject[] peakingPositions;
  public bool fleeing;
  public bool busy;
  public bool insideBase;
  public float stealingTimer = -1f;
  public FollowerManager.SpawnedFollower fakeMidas;
  public FollowerLocation location;
  public List<InventoryItem> inventory = new List<InventoryItem>();
  public static bool EncounteredMidasInTemple;
  public EventInstance chokeLoop;

  public void Awake()
  {
    MidasBaseController.Instance = this;
    LocationManager.OnFollowersSpawned += new System.Action(this.OnFollowersSpawned);
  }

  public void OnDestroy()
  {
    LocationManager.OnFollowersSpawned -= new System.Action(this.OnFollowersSpawned);
    if (this.fakeMidas != null)
      FollowerManager.CleanUpCopyFollower(this.fakeMidas);
    MidasBaseController.EncounteredMidasInTemple = false;
    AudioManager.Instance.StopLoop(this.chokeLoop);
  }

  public void OnFollowersSpawned() => this.SpawnFakeMidas();

  public void SpawnFakeMidas()
  {
    if (!DataManager.Instance.HasMidasHiding || this.fakeMidas != null)
      return;
    this.fakeMidas = FollowerManager.SpawnCopyFollower(DataManager.Instance.MidasFollowerInfo, Vector3.zero, BaseLocationManager.Instance.transform, FollowerLocation.Base);
    this.fakeMidas.FollowerBrain.ResetStats();
    this.fakeMidas.Follower.UpdateOutfit();
    this.SetMidasPeakingPosition();
  }

  public bool TryStealingGold()
  {
    if (DataManager.Instance.HasMidasHiding)
    {
      if ((double) DataManager.Instance.TimeSinceMidasStoleGold == -1.0)
        DataManager.Instance.TimeSinceMidasStoleGold = TimeManager.TotalElapsedGameTime + 240f;
      else if ((double) TimeManager.TotalElapsedGameTime >= (double) DataManager.Instance.TimeSinceMidasStoleGold)
      {
        this.SetMidasStealingGoldPosition();
        return true;
      }
    }
    return false;
  }

  public void Update()
  {
    if (this.fakeMidas != null && DataManager.Instance.HasMidasHiding && !this.fleeing && !this.busy)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((double) Vector3.Distance(player.transform.position, this.fakeMidas.Follower.transform.position) < 5.0)
        {
          this.fleeing = true;
          Vector3 dir = (player.transform.position - this.transform.position).normalized * (this.insideBase ? 100f : 10f);
          this.fakeMidas.Follower.SpeedMultiplier = 3f;
          if (this.inventory.Count > 0)
          {
            for (int index = 0; index < this.inventory.Count; ++index)
              InventoryItem.Spawn((InventoryItem.ITEM_TYPE) this.inventory[index].type, this.inventory[index].quantity, this.fakeMidas.Follower.transform.position);
            this.inventory.Clear();
          }
          this.fakeMidas.Follower.TimedAnimation("Reactions/react-spooked", 0.6666667f, (System.Action) (() => this.Flee(dir)));
          break;
        }
        if ((double) Vector3.Distance(player.transform.position, this.fakeMidas.Follower.transform.position) < 10.0 && this.insideBase && (double) this.stealingTimer == -1.0)
          this.stealingTimer = 0.0f;
      }
    }
    if ((double) this.stealingTimer != -1.0 && !this.busy)
    {
      this.stealingTimer += Time.deltaTime;
      if ((double) this.stealingTimer >= 5.0)
      {
        this.Flee((PlayerFarming.Instance.transform.position - this.transform.position).normalized * 100f);
        this.stealingTimer = -1f;
      }
    }
    if (PlayerFarming.Location == this.location)
      return;
    this.location = PlayerFarming.Location;
    if (this.location != FollowerLocation.Base || this.TryCapturingMidas())
      return;
    this.TryStealingGold();
  }

  public void Flee(Vector3 dir)
  {
    AudioManager.Instance.PlayOneShot("event:/dialogue/midas/laugh_adult", this.gameObject);
    this.fakeMidas.Follower.SpeedMultiplier = 3f;
    this.fleeing = true;
    this.fakeMidas.Follower.Brain.CurrentState = (FollowerState) new FollowerState_Midas();
    this.fakeMidas.Follower.GoTo(this.fakeMidas.Follower.transform.position + dir, (System.Action) (() =>
    {
      DataManager.Instance.MidasStolenGold.AddRange((IEnumerable<InventoryItem>) this.inventory);
      this.inventory.Clear();
      if (this.insideBase)
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/jump_launch", this.fakeMidas.Follower.gameObject);
        this.fakeMidas.Follower.Spine.AnimationState.SetAnimation(1, "Midas/jump", false);
        this.fakeMidas.Follower.transform.DOMoveX(this.fakeMidas.Follower.transform.position.x + ((double) dir.x > 0.0 ? 2f : -2f), 0.72f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.36f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
        {
          AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/jump_land", this.fakeMidas.Follower.gameObject);
          this.fleeing = false;
          this.SetMidasPeakingPosition();
        }));
        this.fakeMidas.Follower.Spine.transform.DOLocalMoveY(2f, 0.36f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.36f);
        this.fakeMidas.Follower.Spine.transform.DOLocalMoveY(0.0f, 0.36f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.72f);
      }
      else
      {
        this.fleeing = false;
        this.SetMidasPeakingPosition();
      }
    }));
  }

  public void SetMidasPeakingPosition()
  {
    bool flag;
    do
    {
      this.fakeMidas.Follower.transform.position = this.peakingPositions[UnityEngine.Random.Range(0, this.peakingPositions.Length)].transform.position;
      flag = true;
      foreach (Component player in PlayerFarming.players)
      {
        if ((double) Vector3.Distance(player.transform.position, this.fakeMidas.Follower.transform.position) < 15.0)
        {
          flag = false;
          break;
        }
      }
    }
    while (!flag);
    this.fakeMidas.Follower.Brain.LastPosition = this.fakeMidas.Follower.transform.position;
    this.fakeMidas.Follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    this.fakeMidas.Follower.IgnoreBounds = true;
    this.fakeMidas.Follower.FacePosition((Vector3) Vector2.zero);
    this.fakeMidas.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) this.fakeMidas.Follower.SetBodyAnimation("Prison/Unlawful/pray-suspicious-look", true);
  }

  public void SetMidasStealingGoldPosition()
  {
    DataManager.Instance.TimeSinceMidasStoleGold = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(240 /*0xF0*/, 1200);
    List<Structures_Refinery> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Refinery>();
    List<Structures_OfferingShrine> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_OfferingShrine>();
    List<StructureBrain> structureBrainList = new List<StructureBrain>();
    for (int index1 = 0; index1 < structuresOfType1.Count; ++index1)
    {
      for (int index2 = 0; index2 < structuresOfType1[index1].Data.Inventory.Count; ++index2)
      {
        if (structuresOfType1[index1].Data.Inventory[index2].type == 86 || structuresOfType1[index1].Data.Inventory[index2].type == 20)
        {
          structureBrainList.Add((StructureBrain) structuresOfType1[index1]);
          break;
        }
      }
    }
    for (int index3 = 0; index3 < structuresOfType2.Count; ++index3)
    {
      for (int index4 = 0; index4 < structuresOfType2[index3].Data.Inventory.Count; ++index4)
      {
        if (structuresOfType2[index3].Data.Inventory[index4].type == 86 || structuresOfType2[index3].Data.Inventory[index4].type == 20)
        {
          structureBrainList.Add((StructureBrain) structuresOfType2[index3]);
          break;
        }
      }
    }
    if (structureBrainList.Count > 0)
    {
      this.inventory.Clear();
      StructureBrain structureBrain = structureBrainList[UnityEngine.Random.Range(0, structureBrainList.Count)];
      for (int index = structureBrain.Data.Inventory.Count - 1; index >= 0; --index)
      {
        if (structureBrain.Data.Inventory[index].type == 86 || structureBrain.Data.Inventory[index].type == 20)
        {
          this.inventory.Add(structureBrain.Data.Inventory[index]);
          structureBrain.Data.Inventory.RemoveAt(index);
        }
      }
      foreach (Interaction_CollectResourceChest chest in Interaction_CollectResourceChest.Chests)
        chest.UpdateChest();
      foreach (Interaction_OfferingShrine shrine in Interaction_OfferingShrine.Shrines)
        shrine.ShowItem();
      this.stealingTimer = -1f;
      this.fakeMidas.Follower.transform.position = structureBrain.Data.Position;
      this.fakeMidas.Follower.Brain.LastPosition = this.fakeMidas.Follower.transform.position;
      this.fakeMidas.Follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      this.fakeMidas.Follower.IgnoreBounds = true;
      this.fakeMidas.Follower.FacePosition((Vector3) Vector2.zero);
      this.fakeMidas.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) this.fakeMidas.Follower.SetBodyAnimation("Jeer/jeer-plotting", true);
      this.insideBase = true;
    }
    else
      this.SetMidasPeakingPosition();
  }

  public bool TryCapturingMidas()
  {
    if (MidasBaseController.EncounteredMidasInTemple)
    {
      MidasBaseController.EncounteredMidasInTemple = false;
      foreach (Interaction_WolfTrap trap in Interaction_WolfTrap.Traps)
      {
        if (trap.Structure.Brain.Data.Inventory.Count > 0 && trap.Structure.Brain.Data.Inventory[0].type == 86)
        {
          this.StartCoroutine((IEnumerator) this.CapturedMidasIE(trap));
          return true;
        }
      }
    }
    return false;
  }

  public IEnumerator CapturedMidasIE(Interaction_WolfTrap trap)
  {
    MidasBaseController midasBaseController = this;
    midasBaseController.busy = true;
    midasBaseController.fakeMidas.Follower.ClearPath();
    midasBaseController.fakeMidas.FollowerFakeBrain.ResetStats();
    midasBaseController.fakeMidas.FollowerBrain.ResetStats();
    midasBaseController.fakeMidas.Follower.transform.position = trap.transform.position + Vector3.down * 0.1f;
    midasBaseController.fakeMidas.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    FollowerBrain.SetFollowerCostume(midasBaseController.fakeMidas.Follower.Spine.Skeleton, midasBaseController.fakeMidas.FollowerFakeInfo, true, true);
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.Instance;
    LayerMask islandMask = new LayerMask();
    islandMask = (LayerMask) ((int) islandMask | 1 << LayerMask.NameToLayer("Island"));
    while (MMTransition.IsPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(midasBaseController.fakeMidas.Follower.gameObject, 7f);
    List<FollowerBrain> availableFollowers = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
        availableFollowers.Add(allBrain);
    }
    for (int index = 0; index < availableFollowers.Count; ++index)
      midasBaseController.StartCoroutine((IEnumerator) midasBaseController.WaitForFollower(availableFollowers[index], availableFollowers, islandMask));
    yield return (object) null;
    trap.TrappedMidas();
    double num1 = (double) midasBaseController.fakeMidas.Follower.SetBodyAnimation("Midas/trapped", true);
    bool waiting = true;
    PlayerFarming.Instance.GoToAndStop(midasBaseController.fakeMidas.Follower.transform.position + Vector3.left, midasBaseController.fakeMidas.Follower.gameObject, GoToCallback: (System.Action) (() => waiting = false));
    float targetDistance = GameManager.GetInstance().CamFollowTarget.targetDistance;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().CamFollowTarget.targetDistance = targetDistance;
    while (waiting)
      yield return (object) null;
    List<ConversationEntry> Entries1 = new List<ConversationEntry>()
    {
      new ConversationEntry(midasBaseController.fakeMidas.Follower.gameObject, "Conversation_NPC/Midas_Caught/0")
    };
    foreach (ConversationEntry conversationEntry in Entries1)
    {
      conversationEntry.CharacterName = ScriptLocalization.NAMES.Midas;
      conversationEntry.Animation = "Midas/trapped-talk";
      conversationEntry.DefaultAnimation = conversationEntry.Animation;
      conversationEntry.LoopAnimation = false;
      conversationEntry.followerID = 100006;
      conversationEntry.soundPath = "event:/dialogue/midas/scared_short";
    }
    MMConversation.Play(new ConversationObject(Entries1, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    yield return (object) null;
    SimulationManager.Pause();
    while (MMConversation.isPlaying)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    SimulationManager.Pause();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("murder-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("murder-loop", 0, true);
    midasBaseController.fakeMidas.Follower.FacePosition(PlayerFarming.Instance.transform.position);
    double num2 = (double) midasBaseController.fakeMidas.Follower.SetBodyAnimation("Midas/choke-start", false);
    midasBaseController.fakeMidas.Follower.AddBodyAnimation("Midas/choke-loop", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/choke_start", midasBaseController.fakeMidas.Follower.transform.position);
    midasBaseController.fakeMidas.Follower.Spine.CustomMaterialOverride.Clear();
    midasBaseController.fakeMidas.Follower.Spine.CustomMaterialOverride.Add(midasBaseController.fakeMidas.Follower.NormalMaterial, midasBaseController.fakeMidas.Follower.BW_Material);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    PlayerFarming.Instance.indicator.ForceShown = true;
    PlayerFarming.Instance.indicator.SetForceShownPosition(Vector3.down * 100f);
    PlayerFarming.Instance.indicator.HoldToInteract = SettingsManager.Settings.Accessibility.HoldActions;
    PlayerFarming.Instance.indicator.gameObject.SetActive(true);
    PlayerFarming.Instance.indicator.text.text = ScriptLocalization.FollowerInteractions.Reeducate;
    PlayerFarming.Instance.indicator.Reset();
    trap.Structure.Brain.Data.Inventory.Clear();
    trap.UpdateVisuals();
    midasBaseController.chokeLoop = AudioManager.Instance.CreateLoop("event:/dlc/env/midas/choke_hold", true);
    float Progress = 0.0f;
    bool accepted = false;
    if (!SettingsManager.Settings.Accessibility.HoldActions)
      yield return (object) new WaitForSecondsRealtime(0.5f);
    while (true)
    {
      if (SettingsManager.Settings.Accessibility.HoldActions)
      {
        if (InputManager.Gameplay.GetInteractButtonHeld(PlayerFarming.Instance))
          Progress += Time.deltaTime / 3f;
        else
          Progress = Mathf.Clamp01(PlayerFarming.Instance.indicator.Progress - Time.deltaTime);
        PlayerFarming.Instance.indicator.Progress = Progress;
      }
      else if (!accepted)
      {
        accepted = InputManager.Gameplay.GetInteractButtonHeld(PlayerFarming.Instance);
        if (accepted)
        {
          PlayerFarming.Instance.indicator.ForceShown = false;
          PlayerFarming.Instance.indicator.gameObject.SetActive(false);
        }
      }
      else
        Progress += Time.deltaTime / 3f;
      int num3 = (int) midasBaseController.chokeLoop.setParameterByName("hold_time", Mathf.Clamp01(Progress));
      midasBaseController.fakeMidas.Follower.Head.bone.ScaleX = Mathf.Lerp(1f, 1.75f, Progress);
      midasBaseController.fakeMidas.Follower.Head.bone.ScaleY = Mathf.Lerp(1f, 1.75f, Progress);
      if ((double) Progress < 1.0)
        yield return (object) null;
      else
        break;
    }
    AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/choke_complete", midasBaseController.fakeMidas.Follower.transform.position);
    AudioManager.Instance.StopLoop(midasBaseController.chokeLoop);
    PlayerFarming.Instance.indicator.ForceShown = false;
    PlayerFarming.Instance.indicator.gameObject.SetActive(false);
    List<ConversationEntry> Entries2 = new List<ConversationEntry>()
    {
      new ConversationEntry(midasBaseController.fakeMidas.Follower.gameObject, "Conversation_NPC/Midas_Caught/1")
    };
    foreach (ConversationEntry conversationEntry in Entries2)
    {
      conversationEntry.CharacterName = ScriptLocalization.NAMES.Midas;
      conversationEntry.Animation = "Midas/choke-loop";
      conversationEntry.DefaultAnimation = conversationEntry.Animation;
      conversationEntry.LoopAnimation = false;
      conversationEntry.followerID = 100006;
      conversationEntry.soundPath = "event:/dialogue/midas/scared_short";
    }
    MMConversation.Play(new ConversationObject(Entries2, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    midasBaseController.fakeMidas.Follower.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("murder-start3", 0, false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/choke_falldown", midasBaseController.fakeMidas.Follower.transform.position);
    double num4 = (double) midasBaseController.fakeMidas.Follower.SetBodyAnimation("Midas/choke-end", false);
    midasBaseController.fakeMidas.Follower.AddBodyAnimation("Injured/idle", true, 0.0f);
    midasBaseController.fakeMidas.Follower.Head.bone.ScaleX = 1f;
    midasBaseController.fakeMidas.Follower.Head.bone.ScaleY = 1f;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("BEAT_UP_MIDAS"));
    yield return (object) new WaitForSeconds(0.5f);
    List<PickUp> gold = new List<PickUp>();
    for (int index = 0; index < 5; ++index)
    {
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, midasBaseController.fakeMidas.Follower.transform.position, 0.0f);
      gold.Add(pickUp);
      pickUp.SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(0, 360));
      pickUp.Player = PlayerFarming.Instance.gameObject;
      pickUp.MagnetToPlayer = false;
      pickUp.AddToInventory = false;
    }
    yield return (object) new WaitForSeconds(0.76f);
    for (int index = 0; index < gold.Count; ++index)
      gold[index].MagnetToPlayer = true;
    for (int index = 0; index < DataManager.Instance.MidasStolenGold.Count; ++index)
      Inventory.AddItem(DataManager.Instance.MidasStolenGold[index].type, DataManager.Instance.MidasStolenGold[index].quantity);
    DataManager.Instance.MidasStolenGold.Clear();
    DataManager.Instance.CompletedMidasFollowerQuest = true;
    DataManager.Instance.HasMidasHiding = false;
    Follower realMidas = FollowerManager.CreateNewFollower(DataManager.Instance.MidasFollowerInfo, midasBaseController.fakeMidas.Follower.transform.position);
    realMidas.Brain.ResetStats();
    realMidas.Brain.Info.Name = ScriptLocalization.NAMES.Midas;
    realMidas.transform.position = midasBaseController.fakeMidas.Follower.transform.position;
    realMidas.gameObject.SetActive(true);
    realMidas.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    realMidas.Spine.transform.localScale = new Vector3(-1f, 1f, 1f);
    realMidas.HideAllFollowerIcons();
    realMidas.Brain._directInfoAccess.Traits.Clear();
    realMidas.Brain._directInfoAccess.TraitsSet = true;
    realMidas.Brain._directInfoAccess.Traits.Add(FollowerTrait.TraitType.Bastard);
    realMidas.Brain._directInfoAccess.Traits.Add(FollowerTrait.TraitType.RoyalPooper);
    realMidas.Brain._directInfoAccess.StartingCursedState = Thought.None;
    FollowerManager.CleanUpCopyFollower(midasBaseController.fakeMidas);
    realMidas.Interaction_FollowerInteraction.state = PlayerFarming.Instance.state;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    yield return (object) new WaitForSeconds(1f);
    realMidas.Brain.RemoveTrait(FollowerTrait.TraitType.Bastard, true);
    realMidas.Brain.AddTrait(FollowerTrait.TraitType.Scared, true);
    realMidas.Brain.MakeInjured();
    SimulationManager.UnPause();
    yield return (object) realMidas.StartCoroutine((IEnumerator) realMidas.Interaction_FollowerInteraction.SimpleNewRecruitRoutine());
    Debug.Log((object) "Achievement: Indoctrinated Midas!");
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.INDOCTRINATE_MIDAS));
    while (UIMenuBase.ActiveMenus.Count > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(4.5f);
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
      {
        follower.State.LockStateChanges = false;
        follower.OverridingEmotions = false;
        follower.Brain.CompleteCurrentTask();
        follower.ShowAllFollowerIcons();
      }
    }
    realMidas.Spine.transform.localScale = Vector3.one;
    realMidas.ShowAllFollowerIcons();
    realMidas.Brain.CompleteCurrentTask();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForEndOfFrame();
    realMidas.Brain.CheckChangeState();
    realMidas.ResetStateAnimations();
  }

  public IEnumerator WaitForFollower(
    FollowerBrain brain,
    List<FollowerBrain> availableFollowers,
    LayerMask layerMask)
  {
    while (brain.Location != FollowerLocation.Base)
    {
      if (brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
        brain.CurrentTask.Arrive();
      yield return (object) null;
    }
    yield return (object) new WaitForEndOfFrame();
    Follower follower = FollowerManager.FindFollowerByID(brain.Info.ID);
    follower.Brain.CompleteCurrentTask();
    if (brain.CurrentTaskType == FollowerTaskType.Sleep)
      follower.StopAllCoroutines();
    Vector3 position = follower.transform.position;
    follower.transform.position = this.GetCirclePosition(availableFollowers, follower);
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) follower.transform.position, (Vector2) (this.fakeMidas.Follower.transform.position - follower.transform.position).normalized, Vector3.Distance(this.fakeMidas.Follower.transform.position, follower.transform.position), (int) layerMask).collider != (UnityEngine.Object) null)
    {
      follower.transform.position = position;
    }
    else
    {
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      follower.FacePosition(this.fakeMidas.Follower.transform.position);
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      follower.State.LockStateChanges = true;
      follower.OverridingEmotions = true;
      follower.SetFaceAnimation((double) UnityEngine.Random.value < 0.5 ? "Emotions/emotion-unhappy" : "Emotions/emotion-angry", true);
      double num1 = (double) follower.SetBodyAnimation("cheer", true);
      follower.HideAllFollowerIcons();
      yield return (object) new WaitForEndOfFrame();
      double num2 = (double) follower.SetBodyAnimation("cheer", true);
    }
  }

  public Vector3 GetCirclePosition(List<FollowerBrain> availableFollowers, Follower follower)
  {
    int num1 = availableFollowers.IndexOf(follower.Brain);
    if (availableFollowers.Count <= 12)
    {
      float num2 = 1.25f;
      float f = (float) ((double) num1 * (360.0 / (double) availableFollowers.Count) * (Math.PI / 180.0));
      return this.fakeMidas.Follower.transform.position + new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
    }
    int b = 8;
    float num3;
    float f1;
    if (num1 < b)
    {
      num3 = 1.5f;
      f1 = (float) ((double) num1 * (360.0 / (double) Mathf.Min(availableFollowers.Count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num3 = 2.5f;
      f1 = (float) ((double) (num1 - b) * (360.0 / (double) (availableFollowers.Count - b)) * (Math.PI / 180.0));
    }
    return this.fakeMidas.Follower.transform.position + new Vector3(num3 * Mathf.Cos(f1), num3 * Mathf.Sin(f1));
  }
}
