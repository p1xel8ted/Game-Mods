// Decompiled with JetBrains decompiler
// Type: Interaction_BaseDungeonDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_BaseDungeonDoor : Interaction
{
  public int FollowerCount = 5;
  public int SacrificeFollowerLevel = -1;
  public GameObject RitualPosition;
  public GameObject RitualReceiveDevotionPosition;
  public FollowerLocation Location;
  public string SceneName;
  public BoxCollider2D CollideForDoor;
  public BoxCollider2D BlockingCollider;
  public SimpleSetCamera SimpleSetCamera;
  public SkeletonAnimation DoorSpine;
  public GameObject Lights;
  public GameObject portal;
  public GameObject DoorLights;
  public SkeletonAnimation Crown;
  public GameObject DoorToMove;
  public Vector3 OpenDoorPosition = new Vector3(0.0f, -2.5f, 4f);
  [SerializeField]
  public ParticleSystem doorSmokeParticleSystem;
  [SerializeField]
  public SkeletonRendererCustomMaterials spineMaterialOverride;
  public GameObject RitualLighting;
  [SerializeField]
  public GameObject BeholderEyeStatues;
  [SerializeField]
  public GameObject BeholderEyeStatues_2;
  public bool Used;
  [CompilerGenerated]
  public bool \u003CUnlocked\u003Ek__BackingField;
  public GameObject doorLightSource;
  public SpriteRenderer doorInnerBlack;
  public string SRequires;
  public string SOpenDoor;
  [TermsPopup("")]
  public string PlaceName;
  public Color PlaceColor;
  public string PlaceString;
  public List<FollowerBrain> brains;
  public bool HaveFollowers;
  public bool Blocking;
  public List<FollowerManager.SpawnedFollower> spawnedFollowers = new List<FollowerManager.SpawnedFollower>();
  public int NumGivingDevotion;
  public EventInstance LoopedSound;
  public EventInstance loopedInstanceOutro;

  public bool Unlocked
  {
    get => this.\u003CUnlocked\u003Ek__BackingField;
    set => this.\u003CUnlocked\u003Ek__BackingField = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 2f;
    this.Used = false;
    this.Lights.SetActive(false);
    this.spineMaterialOverride.enabled = false;
    if (this.FollowerCount <= 0 && !DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location))
      DataManager.Instance.UnlockedDungeonDoor.Add(this.Location);
    if (this.Location == FollowerLocation.Dungeon1_5 && !DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location) || this.Location == FollowerLocation.Dungeon1_6 && !DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location))
      DataManager.Instance.UnlockedDungeonDoor.Add(this.Location);
    this.Unlocked = DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location);
    if (this.Unlocked && !this.Blocking)
    {
      this.DoorToMove.transform.localPosition = this.OpenDoorPosition;
      this.DoorToMove.SetActive(false);
    }
    else
      this.DoorToMove.transform.localPosition = Vector3.zero;
    this.OpenDoor();
    this.DoorSpine.AnimationState.SetAnimation(0, Mathf.Clamp(DataManager.Instance.GetDungeonLayer(this.Location) - 1, 0, 3).ToString(), false);
    if (DataManager.Instance.DeathCatBeaten && DataManager.Instance.OnboardedLayer2 && !DataManager.Instance.DungeonCompleted(this.Location, true))
      this.DoorSpine.AnimationState.SetAnimation(0, DataManager.GetGodTearNotches(this.Location).ToString(), true);
    else if (DataManager.Instance.BossesCompleted.Contains(this.Location))
      this.DoorSpine.AnimationState.SetAnimation(0, "beaten", true);
    else if (DataManager.Instance.BossesEncountered.Contains(this.Location))
      this.DoorSpine.AnimationState.SetAnimation(0, "4", true);
    this.DoorLights.SetActive(this.GetFollowerCount());
    if (this.Location == FollowerLocation.Dungeon1_1 && !DataManager.Instance.IntroDoor1)
      this.DoorLights.SetActive(false);
    if (this.Location == FollowerLocation.Dungeon1_5 || this.Location == FollowerLocation.Dungeon1_6)
      this.DoorLights.SetActive(false);
    this.CheckBeholders();
  }

  public void CheckBeholders()
  {
    this.BeholderEyeStatues.SetActive(false);
    this.BeholderEyeStatues_2.SetActive(false);
    switch (this.Location)
    {
      case FollowerLocation.Dungeon1_1:
        if (DataManager.Instance.CheckKilledBosses("Boss Beholder 1_P2"))
        {
          this.BeholderEyeStatues_2.SetActive(true);
          break;
        }
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 1"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
      case FollowerLocation.Dungeon1_2:
        if (DataManager.Instance.CheckKilledBosses("Boss Beholder 2_P2"))
        {
          this.BeholderEyeStatues_2.SetActive(true);
          break;
        }
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 2"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
      case FollowerLocation.Dungeon1_3:
        if (DataManager.Instance.CheckKilledBosses("Boss Beholder 3_P2"))
        {
          this.BeholderEyeStatues_2.SetActive(true);
          break;
        }
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 3"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
      case FollowerLocation.Dungeon1_4:
        if (DataManager.Instance.CheckKilledBosses("Boss Beholder 4_P2"))
        {
          this.BeholderEyeStatues_2.SetActive(true);
          break;
        }
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 4"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
    }
  }

  public void Start()
  {
    this.OnEnableInteraction();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.SRequires = ScriptLocalization.Interactions.Requires;
    this.SOpenDoor = ScriptLocalization.Interactions.OpenDoor;
    this.PlaceString = LocalizationManager.GetTranslation(this.PlaceName);
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    if (this.Unlocked)
      return;
    playerFarming.indicator.ShowTopInfo($"<sprite name=\"img_SwirleyLeft\"> {this.PlaceString.Colour(this.PlaceColor)} <sprite name=\"img_SwirleyRight\">");
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public bool GetFollowerCount()
  {
    this.brains = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = this.brains.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(this.brains[index]._directInfoAccess) || DataManager.Instance.Followers_Recruit.Contains(this.brains[index]._directInfoAccess))
        this.brains.RemoveAt(index);
    }
    return this.brains.Count >= this.FollowerCount;
  }

  public override void GetLabel()
  {
    if (this.Unlocked)
    {
      this.Label = $"<sprite name=\"img_SwirleyLeft\"> {this.PlaceString.Colour(this.PlaceColor)} <sprite name=\"img_SwirleyRight\">";
      this.Interactable = false;
    }
    else if (!this.Unlocked && (this.Location == FollowerLocation.Dungeon1_5 || this.Location == FollowerLocation.Dungeon1_6))
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
    {
      this.Interactable = true;
      this.HaveFollowers = this.GetFollowerCount();
      string str1 = LocalizeIntegration.ReverseText(DataManager.Instance.Followers.Count.ToString());
      string str2 = LocalizeIntegration.ReverseText(this.FollowerCount.ToString());
      if (LocalizeIntegration.IsArabic())
      {
        string str3;
        if (!this.HaveFollowers)
          str3 = $"{this.SRequires} {str2} / <color=red> {str1}</color> {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        else
          str3 = $"{this.SOpenDoor} | {str2} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        this.Label = str3;
      }
      else
      {
        string str4;
        if (!this.HaveFollowers)
          str4 = $"{this.SRequires}<color=red> {str1}</color> / {str2} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        else
          str4 = $"{this.SOpenDoor} | {str2} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        this.Label = str4;
      }
      if (this.SacrificeFollowerLevel == -1)
        return;
      if (this.HaveFollowers)
      {
        if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.OpenedDoorTimestamp >= 1200.0)
        {
          string str5 = $"<color=#FFD201>{ScriptLocalization.Interactions.Level} {this.SacrificeFollowerLevel.ToString()}</color><sprite name=\"icon_Ascend\">";
          string str6 = $" ({LocalizationManager.GetTranslation("Interactions/CantBeResurrected")})";
          if (LocalizeIntegration.IsArabic())
            this.Label = $"{string.Format(LocalizationManager.GetTranslation("Interactions/SacrificeFollowerToOpen"), (object) str5)} | ({str2}/{str1} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}) {str6.Colour(StaticColors.GreyColor)}";
          else
            this.Label = $"{string.Format(LocalizationManager.GetTranslation("Interactions/SacrificeFollowerToOpen"), (object) str5)} | ({str1}/{str2} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}) {str6.Colour(StaticColors.GreyColor)}";
        }
        else
        {
          this.Interactable = false;
          this.Label = ScriptLocalization.Interactions.Recharging;
        }
      }
      else
        this.Interactable = false;
    }
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    if (this.Unlocked)
      return;
    base.IndicateHighlighted(playerFarming);
  }

  public void OpenDoor()
  {
    if (this.Unlocked && !this.Blocking)
    {
      this.CollideForDoor.enabled = true;
      this.BlockingCollider.enabled = false;
      this.Lights.SetActive(true);
    }
    else
    {
      this.CollideForDoor.enabled = false;
      this.BlockingCollider.enabled = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.SacrificeFollowerLevel != -1)
    {
      if (FollowerBrain.AllBrains.Count > 0)
      {
        base.OnInteract(state);
        this.playerFarming.GoToAndStop(this.transform.position + new Vector3(-1f, -2.5f), this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine())));
      }
      else
      {
        this.IndicateHighlighted(this.playerFarming);
        AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
        this.playerFarming.indicator.PlayShake();
      }
    }
    else if (this.HaveFollowers)
    {
      this.StartCoroutine((IEnumerator) this.DoRitualRoutine());
    }
    else
    {
      this.IndicateHighlighted(this.playerFarming);
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
      this.playerFarming.indicator.PlayShake();
    }
  }

  public IEnumerator SacrificeFollowerRoutine()
  {
    Interaction_BaseDungeonDoor interactionBaseDungeonDoor = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBaseDungeonDoor.state.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.XPLevel >= interactionBaseDungeonDoor.SacrificeFollowerLevel)
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower)));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.UnavailableLowLevel));
    }
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.SACRIFICE_TO_DOOR;
    followerSelectInstance.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, true, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower");
      this.StartCoroutine((IEnumerator) this.SpawnFollower(followerInfo.ID));
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
  }

  public IEnumerator SpawnFollower(int ID)
  {
    Interaction_BaseDungeonDoor interactionBaseDungeonDoor = this;
    while (MMConversation.isPlaying)
      yield return (object) null;
    DataManager.Instance.OpenedDoorTimestamp = TimeManager.TotalElapsedGameTime;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBaseDungeonDoor.state.gameObject);
    yield return (object) new WaitForSeconds(1f);
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.FindFollowerInfo(ID), interactionBaseDungeonDoor.transform.position + Vector3.down / 3f + Vector3.right / 1.8f, interactionBaseDungeonDoor.transform.parent, PlayerFarming.Location);
    spawnedFollower.Follower.gameObject.SetActive(false);
    interactionBaseDungeonDoor.portal.gameObject.SetActive(true);
    GameManager.GetInstance().OnConversationNext(interactionBaseDungeonDoor.portal.gameObject, 7f);
    yield return (object) new WaitForSeconds(0.5f);
    spawnedFollower.Follower.gameObject.SetActive(true);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(0, "sacrifice-door", false);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower.Follower.gameObject);
    spawnedFollower.Follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseDungeonDoor.HandleAnimationStateEvent);
    FollowerManager.RemoveFollowerBrain(ID);
    ObjectiveManager.FailUniqueFollowerObjectives(ID);
    yield return (object) new WaitForSeconds(2f);
    UIManager.PlayAudio("event:/Stings/thenight_sacrifice_followers");
    float Progress = 0.0f;
    float Duration = 6.75f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      GameManager.GetInstance().CameraSetTargetZoom(Mathf.Lerp(9f, 4f, Mathf.SmoothStep(0.0f, 1f, Progress / Duration)));
      CameraManager.shakeCamera((float) (0.30000001192092896 + 0.5 * ((double) Progress / (double) Duration)));
      yield return (object) null;
    }
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.3f);
    spawnedFollower.Follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseDungeonDoor.HandleAnimationStateEvent);
    FollowerManager.CleanUpCopyFollower(spawnedFollower);
    interactionBaseDungeonDoor.SimpleSetCamera.Play();
    yield return (object) interactionBaseDungeonDoor.StartCoroutine((IEnumerator) interactionBaseDungeonDoor.OpenDoorRoutine());
    GameManager.GetInstance().OnConversationEnd();
  }

  public void HandleAnimationStateEvent(TrackEntry trackentry, Spine.Event e)
  {
    Debug.Log((object) e.Data.Name.Colour(Color.yellow));
    if (!(e.Data.Name == "door-sacrifice"))
      return;
    UIManager.PlayAudio("event:/rituals/door_sacrifice");
  }

  public override void OnDisable()
  {
    base.OnDisable();
    for (int index = this.spawnedFollowers.Count - 1; index >= 0; --index)
      FollowerManager.CleanUpCopyFollower(this.spawnedFollowers[index]);
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.OpenDoorRoutine());

  public void Block()
  {
    Debug.Log((object) "BLOCK ME!");
    this.Blocking = true;
    this.DoorToMove.transform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.Unlocked = false;
    this.OpenDoor();
  }

  public void Unblock()
  {
    this.Blocking = false;
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location))
      return;
    this.DoorToMove.transform.DOLocalMove(this.OpenDoorPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.Unlocked = true;
    this.OpenDoor();
  }

  public IEnumerator DoRitualRoutine()
  {
    Interaction_BaseDungeonDoor interactionBaseDungeonDoor = this;
    interactionBaseDungeonDoor.spineMaterialOverride.enabled = true;
    yield return (object) null;
    SimulationManager.Pause();
    bool Waiting = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBaseDungeonDoor.state.gameObject);
    interactionBaseDungeonDoor.playerFarming.GoToAndStop(interactionBaseDungeonDoor.RitualPosition.transform.position + new Vector3(0.0f, -2f), interactionBaseDungeonDoor.RitualPosition, GoToCallback: (System.Action) (() =>
    {
      this.playerFarming.transform.position = this.RitualPosition.transform.position + new Vector3(0.0f, -2f);
      Waiting = false;
    }));
    yield return (object) new WaitForSeconds(1f);
    List<FollowerBrain> brains = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = brains.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(brains[index]._directInfoAccess) || FollowerManager.FollowerLocked(brains[index].Info.ID))
        brains.RemoveAt(index);
    }
    yield return (object) new WaitForSeconds(1f);
    for (int i = 0; i < brains.Count; ++i)
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(brains[i]._directInfoAccess, interactionBaseDungeonDoor.RitualPosition.transform.position, interactionBaseDungeonDoor.transform.parent, PlayerFarming.Location);
      interactionBaseDungeonDoor.spawnedFollowers.Add(spawnedFollower);
      spawnedFollower.Follower.transform.position = interactionBaseDungeonDoor.GetFollowerPosition(i, brains.Count);
      spawnedFollower.Follower.State.facingAngle = (double) spawnedFollower.Follower.transform.position.x > 0.0 ? 180f : 0.0f;
      spawnedFollower.Follower.State.LookAngle = spawnedFollower.Follower.State.facingAngle;
      spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      spawnedFollower.Follower.SetFaceAnimation("Emotions/emotion-happy", true);
      if ((bool) (UnityEngine.Object) spawnedFollower.Follower.GetComponentInChildren<ShadowLockToGround>())
        spawnedFollower.Follower.GetComponentInChildren<ShadowLockToGround>().enabled = false;
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower.Follower.gameObject);
      spawnedFollower.Follower.TimedAnimation("spawn-in", 0.466666669f, (System.Action) (() =>
      {
        spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) spawnedFollower.Follower.SetBodyAnimation("dance", true);
      }));
      yield return (object) new WaitForSeconds(0.05f);
    }
    while (Waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(CrownStatueController.Instance.CameraPosition, 10f);
    interactionBaseDungeonDoor.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionBaseDungeonDoor.playerFarming.simpleSpineAnimator.Animate("rituals/door-ritual", 0, false);
    AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", interactionBaseDungeonDoor.playerFarming.gameObject);
    interactionBaseDungeonDoor.RitualLighting.SetActive(true);
    BiomeConstants.Instance.ImpactFrameForDuration();
    interactionBaseDungeonDoor.LoopedSound = AudioManager.Instance.CreateLoop("event:/door/eye_beam_door_open", true);
    MMVibrate.RumbleContinuous(0.0f, 2f, interactionBaseDungeonDoor.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.6f, 0.7f, 2f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 0.75f);
    interactionBaseDungeonDoor.playerFarming.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseDungeonDoor.HandleEvent);
    yield return (object) new WaitForSeconds(1f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    interactionBaseDungeonDoor.NumGivingDevotion = 0;
    foreach (FollowerManager.SpawnedFollower spawnedFollower in interactionBaseDungeonDoor.spawnedFollowers)
    {
      ++interactionBaseDungeonDoor.NumGivingDevotion;
      spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) spawnedFollower.Follower.SetBodyAnimation("worship", true);
      interactionBaseDungeonDoor.StartCoroutine((IEnumerator) interactionBaseDungeonDoor.SpawnSouls(spawnedFollower.Follower.Spine.transform.position));
      yield return (object) new WaitForSeconds(0.1f);
    }
    while (interactionBaseDungeonDoor.NumGivingDevotion > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionBaseDungeonDoor.playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseDungeonDoor.HandleEvent);
    MMVibrate.StopRumble(interactionBaseDungeonDoor.playerFarming);
    interactionBaseDungeonDoor.RitualLighting.SetActive(false);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    AudioManager.Instance.StopLoop(interactionBaseDungeonDoor.LoopedSound);
    yield return (object) interactionBaseDungeonDoor.StartCoroutine((IEnumerator) interactionBaseDungeonDoor.OpenDoorRoutine());
    yield return (object) new WaitForSeconds(1f);
    foreach (FollowerManager.SpawnedFollower spawnedFollower1 in interactionBaseDungeonDoor.spawnedFollowers)
    {
      FollowerManager.SpawnedFollower spawnedFollower = spawnedFollower1;
      interactionBaseDungeonDoor.StartCoroutine((IEnumerator) interactionBaseDungeonDoor.PlaySoundDelay(spawnedFollower.Follower.gameObject));
      spawnedFollower.Follower.TimedAnimation("spawn-out", 0.8666667f, (System.Action) (() => FollowerManager.CleanUpCopyFollower(spawnedFollower)), false);
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionBaseDungeonDoor.spawnedFollowers.Clear();
  }

  public IEnumerator PlaySoundDelay(GameObject spawnedFollower)
  {
    yield return (object) new WaitForSeconds(0.566666663f);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower);
  }

  public IEnumerator SpawnSouls(Vector3 Position)
  {
    float delay = 0.5f;
    float Count = 8f;
    for (int i = 0; (double) i < (double) Count; ++i)
    {
      float num = (float) i / Count;
      SoulCustomTargetLerp.Create(this.RitualReceiveDevotionPosition.gameObject, Position + Vector3.forward * 2f + Vector3.up, 0.5f, Color.red);
      yield return (object) new WaitForSeconds(delay - 0.2f * num);
    }
    --this.NumGivingDevotion;
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "Spin")
    {
      Debug.Log((object) "Spin sfx");
      CameraManager.shakeCamera(0.1f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
      MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) this);
      this.loopedInstanceOutro = AudioManager.Instance.CreateLoop("event:/player/jump_spin_float", this.playerFarming.gameObject, true);
    }
    else
    {
      if (!(e.Data.Name == "sfxTrigger"))
        return;
      AudioManager.Instance.CreateLoop("event:/Stings/lamb_ascension", this.playerFarming.gameObject, true);
    }
  }

  public Vector3 GetFollowerPosition(int index, int total)
  {
    if (total <= 12)
    {
      float num = 3f;
      float f = (float) ((double) index * (360.0 / (double) total) * (Math.PI / 180.0));
      return this.RitualPosition.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    }
    int b = 8;
    float num1;
    float f1;
    if (index < b)
    {
      num1 = 3f;
      f1 = (float) ((double) index * (360.0 / (double) Mathf.Min(total, b)) * (Math.PI / 180.0));
    }
    else
    {
      num1 = 4f;
      f1 = (float) ((double) (index - b) * (360.0 / (double) (total - b)) * (Math.PI / 180.0));
    }
    return this.RitualPosition.transform.position + new Vector3(num1 * Mathf.Cos(f1), num1 * Mathf.Sin(f1));
  }

  public void FadeDoorLight()
  {
    if (this.DoorLights.gameObject.activeSelf)
      return;
    this.DoorLights.SetActive(true);
    SpriteRenderer component = this.DoorLights.GetComponent<SpriteRenderer>();
    component.color = component.color with { a = 0.0f };
    component.DOFade(1f, 2f);
    DataManager.Instance.IntroDoor1 = true;
  }

  public IEnumerator OpenDoorRoutine()
  {
    Interaction_BaseDungeonDoor interactionBaseDungeonDoor = this;
    if (!LetterBox.IsPlaying)
    {
      GameManager.GetInstance().OnConversationNew();
      interactionBaseDungeonDoor.SimpleSetCamera.Play();
    }
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(interactionBaseDungeonDoor.Location))
      DataManager.Instance.UnlockedDungeonDoor.Add(interactionBaseDungeonDoor.Location);
    AudioManager.Instance.PlayOneShot("event:/door/door_unlock", interactionBaseDungeonDoor.gameObject);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/door/door_lower", interactionBaseDungeonDoor.gameObject);
    interactionBaseDungeonDoor.doorSmokeParticleSystem.Play();
    float Progress = 0.0f;
    float Duration = 3f;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, Duration);
    Vector3 StartingPosition = interactionBaseDungeonDoor.DoorToMove.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      interactionBaseDungeonDoor.DoorToMove.transform.position = Vector3.Lerp(StartingPosition, StartingPosition + interactionBaseDungeonDoor.OpenDoorPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/door/door_done", interactionBaseDungeonDoor.gameObject);
    interactionBaseDungeonDoor.doorSmokeParticleSystem.Stop();
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    interactionBaseDungeonDoor.SimpleSetCamera.Reset();
    interactionBaseDungeonDoor.spineMaterialOverride.enabled = false;
    interactionBaseDungeonDoor.Unlocked = true;
    interactionBaseDungeonDoor.OpenDoor();
    interactionBaseDungeonDoor.Lights.SetActive(true);
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player") || this.Used)
      return;
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.PlayOneShot("event:/Stings/boss_door_complete");
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_appear");
    PlayerFarming.ReloadAllFaith();
    this.Used = true;
    MMTransition.StopCurrentTransition();
    if (this.Location == FollowerLocation.Dungeon1_5 || this.Location == FollowerLocation.Dungeon1_6)
      DataManager.Instance.CurrentDLCNodeType = DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss;
    Interaction_BaseDungeonDoor.GetFloor(this.Location);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.SceneName, 1f, "", new System.Action(this.FadeSave));
    GameManager.GetInstance().OnConversationNew();
  }

  public static void GetFloor(FollowerLocation Location)
  {
    DataManager.LocationAndLayer locationAndLayer = DataManager.LocationAndLayer.ContainsLocation(Location, DataManager.Instance.CachePreviousRun);
    int num1 = 4;
    int num2 = DataManager.Instance.GetDungeonLayer(Location);
    bool flag = num2 >= num1 || DataManager.Instance.DungeonCompleted(Location);
    if (GameManager.Layer2 && DataManager.Instance.DungeonCompleted(Location))
      num2 = DataManager.GetGodTearNotches(Location) + 1;
    DataManager.Instance.DungeonBossFight = num2 >= num1 && !DataManager.Instance.DungeonCompleted(Location, GameManager.Layer2);
    if (DataManager.Instance.DungeonCompleted(Location, GameManager.Layer2) && ((DataManager.Instance.GaveLeshyHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Leshy)) && !DataManager.Instance.LeshyHealed && DataManager.Instance.Followers_Demons_Types.Contains(8) && Location == FollowerLocation.Dungeon1_1 || (DataManager.Instance.GaveHeketHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Heket)) && !DataManager.Instance.HeketHealed && DataManager.Instance.Followers_Demons_Types.Contains(9) && Location == FollowerLocation.Dungeon1_2 || (DataManager.Instance.GaveKallamarHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Kallamar)) && !DataManager.Instance.KallamarHealed && DataManager.Instance.Followers_Demons_Types.Contains(10) && Location == FollowerLocation.Dungeon1_3 || (DataManager.Instance.GaveShamuraHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Shamura)) && !DataManager.Instance.ShamuraHealed && DataManager.Instance.Followers_Demons_Types.Contains(11) && Location == FollowerLocation.Dungeon1_4))
      DataManager.Instance.DungeonBossFight = true;
    if (flag)
    {
      num2 = DataManager.RandomSeed.Next(1, num1 + 1);
      if (locationAndLayer != null)
      {
        while (num2 == locationAndLayer.Layer)
          num2 = DataManager.RandomSeed.Next(1, num1 + 1);
      }
    }
    GameManager.DungeonUseAllLayers = flag;
    if (flag)
      GameManager.CurrentDungeonLayer = 4;
    else
      GameManager.NextDungeonLayer(num2);
    GameManager.NewRun("", false, Location);
    if (locationAndLayer != null)
    {
      locationAndLayer.Layer = num2;
      Debug.Log((object) ("Now set cached layer to: " + locationAndLayer.Layer.ToString()));
    }
    else
      DataManager.Instance.CachePreviousRun.Add(new DataManager.LocationAndLayer(Location, num2));
  }

  public void FadeSave() => SaveAndLoad.Save();

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__45_0()
  {
    this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine());
  }
}
