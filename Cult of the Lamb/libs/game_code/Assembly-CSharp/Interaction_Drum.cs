// Decompiled with JetBrains decompiler
// Type: Interaction_Drum
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Drum : Interaction
{
  public bool Activating;
  public GameObject FollowerPosition;
  public GameObject PlayerPosition;
  public GameObject CameraBone;
  public Structure Structure;
  [SerializeField]
  public GameObject rewardPrefab;
  [SerializeField]
  public GameObject lightingVolume;
  [SerializeField]
  public GameObject fireOn;
  [SerializeField]
  public GameObject fireOff;
  [SerializeField]
  public GameObject fireRecharging;
  [SerializeField]
  public GameObject[] ghosts;
  [SerializeField]
  public SpriteRenderer radialProgress;
  [SerializeField]
  public GameObject radialProgressObj;
  public float TimeUsed = -1f;
  public float currentDayProgress = -1f;
  public float materialValue = -1f;
  public string SacrificeFollower;
  public string NotEnoughFollowers;
  public string NoFollowers;
  public string AlreadyHeardConfession;
  public List<Follower> followers = new List<Follower>();
  public float cachedVOVolume = -1f;
  public FollowerTask_ManualControl Task;
  public EventInstance sound;
  public List<EventInstance> loops = new List<EventInstance>();

  public float normalisedDrumCircleTime
  {
    get
    {
      return (float) (((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastDrumCircleTime) / 2400.0);
    }
  }

  public bool drumCircleReady => (double) this.normalisedDrumCircleTime >= 1.0 && !this.Activating;

  public void Start()
  {
    this.UpdateLocalisation();
    this.fireOn.gameObject.SetActive(false);
    this.fireOff.gameObject.SetActive(true);
    this.fireRecharging.gameObject.SetActive(false);
    foreach (GameObject ghost in this.ghosts)
      ghost.gameObject.SetActive(false);
    if (!this.drumCircleReady)
    {
      this.fireRecharging.gameObject.SetActive(true);
      this.fireOff.gameObject.SetActive(false);
    }
    this.radialProgress.material = new Material(this.radialProgress.material);
    this.radialProgress.material.SetFloat("_Angle", 90f);
    this.SetSpriteAtlasArcCenterOffset();
  }

  public void GetLastTimeUsed()
  {
    if ((double) this.normalisedDrumCircleTime < 1.0)
    {
      this.radialProgressObj.gameObject.SetActive(true);
      float normalisedDrumCircleTime = this.normalisedDrumCircleTime;
      this.radialProgress.material.SetFloat("_Arc2", (float) ((double) normalisedDrumCircleTime * 360.0 * -1.0 + 360.0));
      this.TimeUsed = normalisedDrumCircleTime;
      this.currentDayProgress = TimeManager.CurrentDayProgress;
      this.materialValue = (float) ((double) normalisedDrumCircleTime * 360.0 * -1.0 + 360.0);
    }
    else
    {
      this.radialProgressObj.gameObject.SetActive(false);
      this.TimeUsed = -1f;
    }
  }

  public List<Follower> Followers => this.followers;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.SacrificeFollower = LocalizationManager.GetTranslation("Interactions/Play");
    this.NotEnoughFollowers = ScriptLocalization.Interactions.RequiresMoreFollowers;
    this.NoFollowers = ScriptLocalization.Interactions.NoFollowers;
    this.AlreadyHeardConfession = LocalizationManager.GetTranslation("UI/Generic/Cooldown");
  }

  public override void GetLabel()
  {
    if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastDrumCircleTime > 2400.0)
    {
      if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0)
      {
        this.Label = DataManager.Instance.Followers.Count <= 0 ? this.NoFollowers : this.NotEnoughFollowers;
        this.Interactable = false;
      }
      else
      {
        this.Interactable = true;
        this.Label = this.Activating ? "" : this.SacrificeFollower;
      }
    }
    else
    {
      this.Interactable = false;
      this.Label = this.AlreadyHeardConfession;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.DrumRoutine());
  }

  public override void Update()
  {
    base.Update();
    this.GetLastTimeUsed();
    if (!this.fireRecharging.gameObject.activeSelf || !this.drumCircleReady)
      return;
    this.fireRecharging.gameObject.SetActive(false);
    this.fireOff.gameObject.SetActive(true);
  }

  public IEnumerator DrumRoutine()
  {
    Interaction_Drum drum = this;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    Interaction_Drum.\u003C\u003Ec__DisplayClass37_0 cDisplayClass370 = new Interaction_Drum.\u003C\u003Ec__DisplayClass37_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.\u003C\u003E4__this = this;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(drum.playerFarming.CameraBone, 8f);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.waiting = true;
    // ISSUE: reference to a compiler-generated method
    drum.playerFarming.GoToAndStop(drum.PlayerPosition, drum.FollowerPosition, GoToCallback: new System.Action(cDisplayClass370.\u003CDrumRoutine\u003Eb__0));
    // ISSUE: reference to a compiler-generated field
    while (cDisplayClass370.waiting)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    drum.playerFarming.playerController.speed = 0.0f;
    drum.playerFarming.transform.position = drum.PlayerPosition.transform.position;
    drum.playerFarming.state.facingAngle = drum.playerFarming.state.LookAngle = 270f;
    drum.playerFarming.Spine.transform.position -= Vector3.forward * 0.38f;
    drum.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    drum.playerFarming.Spine.AnimationState.SetAnimation(0, "Drumming/drum-idle", true);
    yield return (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.winningFollower = (Follower) null;
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain._directInfoAccess.IsSnowman)
        followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain, FollowerSelectEntry.Status.Unavailable));
      else if (follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
        followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain, FollowerSelectEntry.Status.UnavailableZombie));
      else if (follower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
        followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain, FollowerSelectEntry.Status.UnavailableExistentialDread));
      else if (follower.Brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
        followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain, FollowerSelectEntry.Status.UnavailableMissionaryTerrified));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain)));
    }
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.waiting = true;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.followerSelectMenu.VotingType = TwitchVoting.VotingType.DRUM_CIRCLE;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.followerSelectMenu.Show(followerSelectEntries, followerSelectionType: UpgradeSystem.Type.Building_Drum);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass370.followerSelectMenu.SetBackgroundState(false);
    // ISSUE: reference to a compiler-generated field
    UIFollowerSelectMenuController followerSelectMenu1 = cDisplayClass370.followerSelectMenu;
    // ISSUE: reference to a compiler-generated method
    followerSelectMenu1.OnFollowerSelected = followerSelectMenu1.OnFollowerSelected + new System.Action<FollowerInfo>(cDisplayClass370.\u003CDrumRoutine\u003Eb__13);
    // ISSUE: reference to a compiler-generated field
    UIFollowerSelectMenuController followerSelectMenu2 = cDisplayClass370.followerSelectMenu;
    // ISSUE: reference to a compiler-generated method
    followerSelectMenu2.OnShow = followerSelectMenu2.OnShow + new System.Action(cDisplayClass370.\u003CDrumRoutine\u003Eb__14);
    // ISSUE: reference to a compiler-generated field
    UIFollowerSelectMenuController followerSelectMenu3 = cDisplayClass370.followerSelectMenu;
    // ISSUE: reference to a compiler-generated method
    followerSelectMenu3.OnShownCompleted = followerSelectMenu3.OnShownCompleted + new System.Action(cDisplayClass370.\u003CDrumRoutine\u003Eb__15);
    // ISSUE: reference to a compiler-generated field
    UIFollowerSelectMenuController followerSelectMenu4 = cDisplayClass370.followerSelectMenu;
    // ISSUE: reference to a compiler-generated method
    followerSelectMenu4.OnCancel = followerSelectMenu4.OnCancel + new System.Action(cDisplayClass370.\u003CDrumRoutine\u003Eb__16);
    // ISSUE: reference to a compiler-generated field
    UIFollowerSelectMenuController followerSelectMenu5 = cDisplayClass370.followerSelectMenu;
    // ISSUE: reference to a compiler-generated method
    followerSelectMenu5.OnHidden = followerSelectMenu5.OnHidden + new System.Action(cDisplayClass370.\u003CDrumRoutine\u003Eb__17);
    // ISSUE: reference to a compiler-generated field
    while (cDisplayClass370.waiting)
      yield return (object) null;
    // ISSUE: reference to a compiler-generated field
    if ((UnityEngine.Object) cDisplayClass370.winningFollower == (UnityEngine.Object) null)
    {
      GameManager.GetInstance().OnConversationEnd();
      drum.Activating = false;
      drum.Interactable = true;
    }
    else
    {
      if (TimeManager.CurrentPhase == DayPhase.Night && FollowerBrainStats.ShouldSleep)
      {
        DataManager.Instance.WokeUpEveryoneDay = TimeManager.CurrentDay;
        CultFaithManager.AddThought(Thought.Cult_WokeUpEveryone);
      }
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) null);
      yield return (object) new WaitForSecondsRealtime(1f);
      drum.playerFarming.transform.localScale = new Vector3(-1f, 1f, 1f);
      drum.lightingVolume.gameObject.SetActive(true);
      drum.fireOn.gameObject.SetActive(true);
      drum.fireOff.gameObject.SetActive(false);
      foreach (GameObject ghost in drum.ghosts)
        ghost.gameObject.SetActive(false);
      drum.followers = new List<Follower>();
      foreach (Follower follower in Follower.Followers)
      {
        if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
          drum.followers.Add(follower);
      }
      LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
      for (int index = 0; index < drum.followers.Count; ++index)
      {
        Follower follower = drum.followers[index];
        follower.Brain.CurrentTask?.Abort();
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        Vector3 vector3 = drum.GetCirclePos(drum.PlayerPosition.transform.position, UnityEngine.Random.value + 1f, index, drum.followers.Count) + (Vector3) (UnityEngine.Random.insideUnitCircle / 2f);
        Vector3 normalized = (vector3 - drum.PlayerPosition.transform.position).normalized;
        follower.transform.position = vector3 + normalized;
        follower.HideAllFollowerIcons();
        drum.loops.Add(follower.PlayLoopedVO("event:/followers/Speech/mumbling_drumcircle", follower.gameObject));
      }
      yield return (object) new WaitForEndOfFrame();
      yield return (object) new WaitForEndOfFrame();
      for (int index = 0; index < drum.followers.Count; ++index)
      {
        TrackEntry trackEntry = drum.followers[index].Spine.AnimationState.SetAnimation(1, "Drumming/Drumming-dance", true);
        if ((double) UnityEngine.Random.value < 0.5)
          trackEntry.TrackTime = 0.8666667f;
      }
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.winningFollower.transform.position = drum.FollowerPosition.transform.position;
      Time.timeScale = 1f;
      GameManager.GetInstance().OnConversationNext(drum.playerFarming.gameObject, 2.5f);
      yield return (object) new WaitForSeconds(0.5f);
      MMTransition.ResumePlay();
      Time.timeScale = 1f;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.cam = GameManager.GetInstance().CamFollowTarget;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      DOTween.To(new DOGetter<float>(cDisplayClass370.\u003CDrumRoutine\u003Eb__1), new DOSetter<float>(cDisplayClass370.\u003CDrumRoutine\u003Eb__2), 5f, 4f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      BiomeConstants.Instance.VignetteTween(6f, BiomeConstants.Instance.VignetteDefaultValue, 0.6f);
      BiomeConstants.Instance.DepthOfFieldTween(0.0f, 8.7f, 26f, 0.0f, 0.0f);
      yield return (object) new WaitForEndOfFrame();
      BiomeConstants.Instance.DepthOfFieldTween(6f, 8.7f, 26f, 1f, 200f);
      yield return (object) new WaitForSeconds(2f);
      AudioManager.Instance.StopCurrentMusic();
      yield return (object) new WaitForSeconds(1.5f);
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
      drum.sound = new EventInstance();
      int song = UnityEngine.Random.Range(1, 6);
      switch (song)
      {
        case 1:
          drum.sound = AudioManager.Instance.PlayOneShotWithInstance("event:/building/drum_circle/mini_game_one");
          break;
        case 2:
          drum.sound = AudioManager.Instance.PlayOneShotWithInstance("event:/building/drum_circle/mini_game_two");
          break;
        case 3:
          drum.sound = AudioManager.Instance.PlayOneShotWithInstance("event:/building/drum_circle/mini_game_three");
          break;
        case 4:
          drum.sound = AudioManager.Instance.PlayOneShotWithInstance("event:/building/drum_circle/mini_game_four");
          break;
        case 5:
          drum.sound = AudioManager.Instance.PlayOneShotWithInstance("event:/building/drum_circle/mini_game_five");
          break;
      }
      // ISSUE: reference to a compiler-generated method
      drum.StartCoroutine((IEnumerator) cDisplayClass370.\u003CDrumRoutine\u003Eg__InitialAnimsIE\u007C3());
      yield return (object) new WaitForSeconds(0.5f);
      GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.from = GameManager.GetInstance().CamFollowTarget.transform.position;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.to = new Vector3(GameManager.GetInstance().CamFollowTarget.transform.position.x - 4f, GameManager.GetInstance().CamFollowTarget.transform.position.y, GameManager.GetInstance().CamFollowTarget.transform.position.z);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.time = 0.0f;
      float ogX = GameManager.GetInstance().CamFollowTarget.transform.position.x;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      DOTween.To(new DOGetter<float>(cDisplayClass370.\u003CDrumRoutine\u003Eb__4), new DOSetter<float>(cDisplayClass370.\u003CDrumRoutine\u003Eb__5), 1f, 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>(new TweenCallback(cDisplayClass370.\u003CDrumRoutine\u003Eb__6)).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      yield return (object) new WaitForSeconds(0.5f);
      AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", drum.gameObject.transform.position);
      yield return (object) new WaitForSeconds(0.65714f);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.loadTask = MonoSingleton<UIManager>.Instance.LoadDrumMinigameAssets();
      // ISSUE: reference to a compiler-generated method
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(cDisplayClass370.\u003CDrumRoutine\u003Eb__7));
      UIDrumMinigameOverlayController _uiDrumMinigameOverlayController = MonoSingleton<UIManager>.Instance.DrumMinigameOverlayControllerTemplate.Instantiate<UIDrumMinigameOverlayController>();
      _uiDrumMinigameOverlayController.Initialise(drum, song - 1);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.happinessLevel = 0;
      float sinProgress = 0.0f;
      // ISSUE: reference to a compiler-generated method
      _uiDrumMinigameOverlayController.OnSuccessfulPress += new UIDrumMinigameOverlayController.NormalEvent(cDisplayClass370.\u003CDrumRoutine\u003Eb__8);
      // ISSUE: reference to a compiler-generated method
      _uiDrumMinigameOverlayController.OnFailedPress += new UIDrumMinigameOverlayController.NormalEvent(cDisplayClass370.\u003CDrumRoutine\u003Eb__9);
      float timer = 1f;
      while ((UnityEngine.Object) _uiDrumMinigameOverlayController != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass370.happinessLevel = _uiDrumMinigameOverlayController.HappinessLevel;
        sinProgress = _uiDrumMinigameOverlayController.SinProgress;
        timer += Time.deltaTime;
        if ((double) timer > 1.0)
        {
          timer = 0.0f;
          GameObject ghost1;
          do
          {
            ghost1 = drum.ghosts[UnityEngine.Random.Range(0, drum.ghosts.Length)];
          }
          while (ghost1.activeSelf);
          foreach (GameObject ghost2 in drum.ghosts)
            ghost2.gameObject.SetActive(false);
          ghost1.gameObject.SetActive(true);
        }
        yield return (object) null;
      }
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.from = GameManager.GetInstance().CamFollowTarget.transform.position;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.to = new Vector3(ogX, GameManager.GetInstance().CamFollowTarget.transform.position.y, GameManager.GetInstance().CamFollowTarget.transform.position.z);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.time = 0.0f;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      DOTween.To(new DOGetter<float>(cDisplayClass370.\u003CDrumRoutine\u003Eb__10), new DOSetter<float>(cDisplayClass370.\u003CDrumRoutine\u003Eb__11), 1f, 1.5f).OnUpdate<TweenerCore<float, float, FloatOptions>>(new TweenCallback(cDisplayClass370.\u003CDrumRoutine\u003Eb__12)).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      yield return (object) new WaitForSeconds(1.5f);
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
      drum.fireRecharging.gameObject.SetActive(true);
      drum.fireOn.gameObject.SetActive(false);
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      for (int index = drum.loops.Count - 1; index >= 0; --index)
        AudioManager.Instance.StopLoop(drum.loops[index]);
      drum.loops.Clear();
      AudioManager.Instance.StopOneShotInstanceEarly(drum.sound, STOP_MODE.IMMEDIATE);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.winningFollower.Brain.AddPleasureInt(Mathf.RoundToInt(sinProgress * 65f));
      // ISSUE: reference to a compiler-generated field
      bool flag = cDisplayClass370.winningFollower.Brain.CanGiveSin();
      foreach (Follower follower in drum.followers)
      {
        if (flag)
        {
          double num1 = (double) follower.SetBodyAnimation("Reactions/react-enlightened" + UnityEngine.Random.Range(1, 3).ToString(), false);
        }
        else
        {
          double num2 = (double) follower.SetBodyAnimation("Conversations/react-hate" + UnityEngine.Random.Range(1, 4).ToString(), false);
        }
        follower.AddBodyAnimation("idle", true, 0.0f);
      }
      AudioManager.Instance.PlayMusic("event:/music/base/base_main");
      drum.playerFarming.transform.localScale = Vector3.one;
      if (flag)
      {
        UIManager.PlayAudio("event:/Stings/gamble_win");
        UIManager.PlayAudio("event:/building/building_bell_ring");
      }
      yield return (object) new WaitForSeconds(2.5f);
      DataManager.Instance.LastDrumCircleTime = TimeManager.TotalElapsedGameTime;
      WarmthBar.ModifyWarmth("Notifications/DrumCircle/Warmth", (float) StructureManager.GetBuildingWarmth(StructureBrain.TYPES.DRUM_CIRCLE));
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass370.winningFollower.Brain.CanGiveSin())
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass370.winningFollower.GiveSinToPlayer((System.Action) null);
      }
      // ISSUE: reference to a compiler-generated field
      while (cDisplayClass370.winningFollower.InGiveSinSequence)
        yield return (object) null;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass370.winningFollower.Brain.HasTrait(FollowerTrait.TraitType.MusicLover))
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass370.winningFollower.GiveSinToPlayer((System.Action) null);
      }
      // ISSUE: reference to a compiler-generated field
      while (cDisplayClass370.winningFollower.InGiveSinSequence)
        yield return (object) null;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass370.winningFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      drum.playerFarming.Spine.transform.DOLocalMove(Vector3.zero, 0.5f);
      yield return (object) new WaitForSeconds(1f);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.DrumCircle);
      // ISSUE: reference to a compiler-generated field
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.DrumCircle, cDisplayClass370.winningFollower.Brain.Info.ID);
      GameManager.GetInstance().OnConversationEnd();
      BiomeConstants.Instance.DepthOfFieldTween(1f, 8.7f, 26f, 1f, 200f);
      BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
      foreach (Follower follower in drum.followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
        {
          follower.ShowAllFollowerIcons();
          follower.Brain.CompleteCurrentTask();
        }
      }
      drum.Activating = false;
      drum.lightingVolume.gameObject.SetActive(false);
      LocationManager._Instance.ReEnableRitualEffects();
      BiomeBaseManager.Instance.InitMusic();
    }
  }

  public Vector3 GetCirclePos(Vector3 center, float distance, int index, int count)
  {
    float f = (float) ((double) index * (360.0 / (double) count) * (Math.PI / 180.0));
    return center + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
  }

  public void FollowerQuit()
  {
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((double) this.cachedVOVolume != -1.0)
    {
      AudioManager.Instance.SetVOBusVolume(this.cachedVOVolume);
      this.cachedVOVolume = -1f;
    }
    for (int index = this.loops.Count - 1; index >= 0; --index)
      AudioManager.Instance.StopLoop(this.loops[index]);
    this.loops.Clear();
  }

  public void SetSpriteAtlasArcCenterOffset()
  {
    Vector2 center = this.radialProgress.sprite.textureRect.center;
    this.radialProgress.material.SetVector("_ArcCenterOffset", (Vector4) (new Vector2(center.x / (float) this.radialProgress.sprite.texture.width, center.y / (float) this.radialProgress.sprite.texture.height) - Vector2.one * 0.5f));
  }
}
