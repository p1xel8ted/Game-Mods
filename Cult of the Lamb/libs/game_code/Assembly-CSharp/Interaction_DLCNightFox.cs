// Decompiled with JetBrains decompiler
// Type: Interaction_DLCNightFox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class Interaction_DLCNightFox : Interaction
{
  [SerializeField]
  public GameObject goopFloorParticle;
  public SkeletonAnimation goopSkeleton;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject MoonIcon;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  public bool Activating;
  public Coroutine exitRoutine;
  public SkeletonAnimation Spine;
  public bool Available;
  public string sPeerInToTheDarkness;
  public GameObject PlayerPosition;
  public List<MMTools.Response> responses;
  public List<ConversationEntry> conversationEntries;
  public List<ConversationEntry> conversationOnlyQuestionEntries;
  public int FollowerCount;
  public List<FollowerManager.SpawnedFollower> SpawnedFollowers = new List<FollowerManager.SpawnedFollower>();

  public int convoNumber
  {
    get
    {
      return DataManager.Instance.CurrentDLCFoxEncounter + Enum.GetNames(typeof (Interaction_NightFox.EncounterTypes)).Length;
    }
  }

  public Interaction_DLCNightFox.EncounterTypes EncounterType
  {
    get => (Interaction_DLCNightFox.EncounterTypes) DataManager.Instance.CurrentDLCFoxEncounter;
  }

  public static FollowerLocation CurrentFoxLocation
  {
    get => DataManager.Instance.CurrentFoxLocation;
    set => DataManager.Instance.CurrentFoxLocation = value;
  }

  public void Start() => this.UpdateLocalisation();

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.EncounterType != Interaction_DLCNightFox.EncounterTypes.MutatedFollower || PlayerFarming.Location != FollowerLocation.Base || DataManager.Instance.FoxCompleted.Contains(FollowerLocation.DLC_ShrineRoom))
      return;
    this.StartCoroutine((IEnumerator) this.AwaitPlayerInShrineRoom());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.Activating = false;
    if (this.exitRoutine == null)
      return;
    this.Deactivate(false);
    this.exitRoutine = (Coroutine) null;
  }

  public void Awake()
  {
    if (this.EncounterType == Interaction_DLCNightFox.EncounterTypes.LambFollower)
      LocationManager.OnPlayerLocationSet += new System.Action(this.OnRatauHubLocationSet);
    else if (this.EncounterType == Interaction_DLCNightFox.EncounterTypes.MutatedFollower)
      LocationManager.OnPlayerLocationSet += new System.Action(this.OnDLCShrineRoomLocationSet);
    else
      this.Deactivate(false);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnDLCShrineRoomLocationSet);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnRatauHubLocationSet);
  }

  public IEnumerator AwaitPlayerInShrineRoom()
  {
    while (PlayerFarming.Location == FollowerLocation.Base)
      yield return (object) null;
    if (this.EncounterType == Interaction_DLCNightFox.EncounterTypes.MutatedFollower && PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom)
      this.OnDLCShrineRoomLocationSet();
  }

  public void OnDLCShrineRoomLocationSet()
  {
    if (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom && PlayerFarming.Location != FollowerLocation.Base)
    {
      this.Deactivate(false);
      LocationManager.OnPlayerLocationSet -= new System.Action(this.OnDLCShrineRoomLocationSet);
    }
    else
    {
      if ((Interaction_DLCNightFox.CurrentFoxLocation == FollowerLocation.None || Interaction_DLCNightFox.CurrentFoxLocation == FollowerLocation.DLC_ShrineRoom) && !DataManager.Instance.FoxCompleted.Contains(FollowerLocation.DLC_ShrineRoom))
      {
        this.Spine.gameObject.SetActive(false);
        TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
        this.OnNewPhaseStarted();
      }
      else
        this.Deactivate(false);
      LocationManager.OnPlayerLocationSet -= new System.Action(this.OnDLCShrineRoomLocationSet);
    }
  }

  public void OnRatauHubLocationSet()
  {
    if (PlayerFarming.Location != FollowerLocation.Hub1_RatauOutside)
    {
      this.Deactivate(false);
      LocationManager.OnPlayerLocationSet -= new System.Action(this.OnRatauHubLocationSet);
    }
    else if (!DataManager.Instance.FoxCompleted.Contains(FollowerLocation.DLC_ShrineRoom))
    {
      this.Deactivate(false);
      LocationManager.OnPlayerLocationSet -= new System.Action(this.OnRatauHubLocationSet);
    }
    else if (!DataManager.Instance.RatauStaffQuestAliveDead)
    {
      this.Deactivate(false);
      LocationManager.OnPlayerLocationSet -= new System.Action(this.OnRatauHubLocationSet);
    }
    else
    {
      if ((Interaction_DLCNightFox.CurrentFoxLocation == FollowerLocation.None || Interaction_DLCNightFox.CurrentFoxLocation == PlayerFarming.Location) && !DataManager.Instance.FoxCompleted.Contains(PlayerFarming.Location))
      {
        this.Spine.gameObject.SetActive(false);
        TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
        this.OnNewPhaseStarted();
      }
      else
        this.Deactivate(false);
      LocationManager.OnPlayerLocationSet -= new System.Action(this.OnRatauHubLocationSet);
    }
  }

  public void Deactivate(bool DelayMoonIcon)
  {
    this.gameObject.SetActive(false);
    this.SimpleSetCamera.AutomaticallyActivate = false;
    if (!((UnityEngine.Object) this.MoonIcon != (UnityEngine.Object) null))
      return;
    if (DelayMoonIcon)
    {
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.AppendInterval(5f);
      sequence.Append((Tween) this.MoonIcon.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.MoonIcon.SetActive(false))));
      sequence.Play<DG.Tweening.Sequence>();
    }
    else
      this.MoonIcon.SetActive(false);
  }

  public void OnNewPhaseStarted()
  {
    if (TimeManager.CurrentPhase == DayPhase.Night)
    {
      this.Available = true;
    }
    else
    {
      this.Available = false;
      if ((UnityEngine.Object) this.MoonIcon != (UnityEngine.Object) null)
      {
        this.MoonIcon.transform.localScale = Vector3.one * 1.15f;
        this.MoonIcon.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
      }
    }
    this.HasChanged = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sPeerInToTheDarkness = ScriptLocalization.Interactions.PeerInToTheDarkness;
  }

  public override void GetLabel()
  {
    if (this.Available && !this.Activating)
      this.Label = this.sPeerInToTheDarkness;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activating = true;
    this.playerFarming.GoToAndStop(this.PlayerPosition.transform.position, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.AppearRoutine())));
  }

  public bool CanAfford()
  {
    switch (this.EncounterType)
    {
      case Interaction_DLCNightFox.EncounterTypes.MutatedFollower:
        return this.GetAvaiableMutatedFollowers().Count > 0;
      case Interaction_DLCNightFox.EncounterTypes.LambFollower:
        return this.GetAvaiableLambFollowers().Count > 0;
      default:
        return false;
    }
  }

  public IEnumerator AppearRoutine()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    if (interactionDlcNightFox.EncounterType == Interaction_DLCNightFox.EncounterTypes.LambFollower && (UnityEngine.Object) MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate == (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.LoadPlayerUpgradesMenu();
    Interaction_DLCNightFox.CurrentFoxLocation = PlayerFarming.Location;
    AudioManager.Instance.SetMusicRoomID(1, "shore_id");
    interactionDlcNightFox.state.CURRENT_STATE = StateMachine.State.InActive;
    interactionDlcNightFox.state.facingAngle = Utils.GetAngle(interactionDlcNightFox.state.transform.position, interactionDlcNightFox.Spine.transform.position);
    SimulationManager.Pause();
    interactionDlcNightFox.SimpleSetCamera.AutomaticallyActivate = false;
    interactionDlcNightFox.SimpleSetCamera.DectivateDistance = 0.5f;
    interactionDlcNightFox.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcNightFox.goopFloorParticle.gameObject, 6f);
    interactionDlcNightFox.ShowGoop();
    yield return (object) new WaitForSeconds(1f);
    interactionDlcNightFox.Spine.AnimationState.SetAnimation(0, "enter", false);
    interactionDlcNightFox.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    interactionDlcNightFox.Spine.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds(2.66666675f);
    interactionDlcNightFox.GetQuestionAndResponses();
    if (DataManager.Instance.FoxIntroductions.Contains(PlayerFarming.Location))
      interactionDlcNightFox.PoseQuestion();
    else
      interactionDlcNightFox.IntroductionConversation();
  }

  public void GetQuestionAndResponses()
  {
    this.responses = new List<MMTools.Response>();
    int convoNumber = this.convoNumber;
    string str1 = this.CanAfford() ? "UI/Generic/Accept" : "Conversation_NPC/Fox/Meeting/Response_CantAfford";
    this.responses.Add(new MMTools.Response(str1, (System.Action) (() => this.StartCoroutine(this.CanAfford() ? (IEnumerator) this.Agree() : (IEnumerator) this.CantAfford())), str1));
    string str2 = "UI/Generic/Decline";
    this.responses.Add(new MMTools.Response(str2, (System.Action) (() => this.StartCoroutine((IEnumerator) this.Disagree())), str2));
    this.conversationEntries = new List<ConversationEntry>();
    int num = -1;
    string str3 = $"Conversation_NPC/Fox/Meeting{convoNumber.ToString()}/";
    if (DataManager.Instance.FoxIntroductions.Contains(PlayerFarming.Location))
      str3 += "Again/";
    while (LocalizationManager.GetTermData(str3 + (++num).ToString()) != null)
    {
      Debug.Log((object) $"{str3}  {num.ToString()}");
      ConversationEntry conversationEntry = new ConversationEntry(this.Spine.gameObject, str3 + num.ToString());
      this.conversationEntries.Add(conversationEntry);
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    }
    this.conversationOnlyQuestionEntries = new List<ConversationEntry>((IEnumerable<ConversationEntry>) this.conversationEntries);
  }

  public void IntroductionConversation()
  {
    Debug.Log((object) nameof (IntroductionConversation));
    DataManager.Instance.FoxIntroductions.Add(PlayerFarming.Location);
    ConversationObject ConversationObject = new ConversationObject(this.conversationEntries, this.responses, (System.Action) null);
    foreach (ConversationEntry conversationEntry in this.conversationEntries)
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    MMConversation.Play(ConversationObject, false, SetPlayerIdleOnComplete: false);
    this.CameraIncludePlayer();
  }

  public void PoseQuestion()
  {
    ConversationObject ConversationObject = new ConversationObject(this.conversationOnlyQuestionEntries, this.responses, (System.Action) null);
    foreach (ConversationEntry onlyQuestionEntry in this.conversationOnlyQuestionEntries)
      onlyQuestionEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    MMConversation.Play(ConversationObject, false, SetPlayerIdleOnComplete: false);
    this.CameraIncludePlayer();
  }

  public IEnumerator Agree()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    yield return (object) interactionDlcNightFox.StartCoroutine((IEnumerator) interactionDlcNightFox.GiveItem());
    yield return (object) new WaitForEndOfFrame();
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int convoNumber = interactionDlcNightFox.convoNumber;
    int num = -1;
    string str = $"Conversation_NPC/Fox/Meeting{convoNumber.ToString()}/Answer_A/";
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
    {
      if (!DataManager.Instance.RatauKilled && num == 3)
        Entries.Add(new ConversationEntry(interactionDlcNightFox.Spine.gameObject, $"{str}RatauAlive/{num.ToString()}")
        {
          soundPath = "event:/dialogue/the_night/standard_the_night"
        });
      else
        Entries.Add(new ConversationEntry(interactionDlcNightFox.Spine.gameObject, str + num.ToString())
        {
          soundPath = "event:/dialogue/the_night/standard_the_night"
        });
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, new System.Action(interactionDlcNightFox.\u003CAgree\u003Eb__41_0)), false, SetPlayerIdleOnComplete: false, SnapLetterBox: true);
    interactionDlcNightFox.CameraIncludePlayer();
  }

  public IEnumerator AgreeCallback()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    yield return (object) new WaitForEndOfFrame();
    interactionDlcNightFox.SimpleSetCamera.Reset();
    bool waiting = true;
    if (interactionDlcNightFox.EncounterType == Interaction_DLCNightFox.EncounterTypes.MutatedFollower && !DataManager.Instance.RatauKilled)
    {
      yield return (object) interactionDlcNightFox.StartCoroutine((IEnumerator) interactionDlcNightFox.ShowWeaponSequence());
      waiting = false;
    }
    else
      interactionDlcNightFox.UnlockRatauFleece((System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().OnConversationEnd();
    interactionDlcNightFox.exitRoutine = interactionDlcNightFox.StartCoroutine((IEnumerator) interactionDlcNightFox.ExitRoutine());
    SimulationManager.UnPause();
    DataManager.Instance.FoxCompleted.Add(PlayerFarming.Location);
    Interaction_DLCNightFox.CurrentFoxLocation = FollowerLocation.None;
    ++DataManager.Instance.CurrentDLCFoxEncounter;
  }

  public IEnumerator ExitRoutine()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    switch (SceneManager.GetActiveScene().name)
    {
      case "Hub-Shore":
        if (DataManager.Instance.Lighthouse_Lit)
        {
          AudioManager.Instance.SetMusicRoomID(6, "shore_id");
          break;
        }
        AudioManager.Instance.SetMusicRoomID(0, "shore_id");
        break;
      case "Mushroom Research Site":
        AudioManager.Instance.SetMusicRoomID(3, "shore_id");
        break;
      case "Midas Cave":
        AudioManager.Instance.SetMusicRoomID(4, "shore_id");
        break;
      case "Dungeon Decoration Shop 1":
        AudioManager.Instance.SetMusicRoomID(5, "shore_id");
        break;
      default:
        AudioManager.Instance.SetMusicRoomID(0, "shore_id");
        break;
    }
    interactionDlcNightFox.Spine.AnimationState.SetAnimation(0, "exit", false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/the_night_fox/teleport_out", interactionDlcNightFox.Spine.transform.position);
    yield return (object) new WaitForSeconds(0.1f);
    Vector3 position = interactionDlcNightFox.transform.position;
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", position);
    yield return (object) new WaitForSeconds(0.1f);
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", position);
    yield return (object) new WaitForSeconds(0.1f);
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", position);
    yield return (object) new WaitForSeconds(0.7f);
    interactionDlcNightFox.ShowGoop();
    yield return (object) new WaitForSeconds(1f);
    interactionDlcNightFox.Activating = false;
    interactionDlcNightFox.Deactivate(true);
    interactionDlcNightFox.exitRoutine = (Coroutine) null;
  }

  public IEnumerator CantAfford()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    yield return (object) new WaitForEndOfFrame();
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int convoNumber = interactionDlcNightFox.convoNumber;
    int num = -1;
    string str = "Conversation_NPC/Fox/Answer_CantAfford/";
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
    {
      Debug.Log((object) $"{str}  {num.ToString()}");
      ConversationEntry conversationEntry = new ConversationEntry(interactionDlcNightFox.Spine.gameObject, str + num.ToString());
      Entries.Add(conversationEntry);
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, new System.Action(interactionDlcNightFox.\u003CCantAfford\u003Eb__44_0)), SnapLetterBox: true);
    interactionDlcNightFox.CameraIncludePlayer();
  }

  public IEnumerator Disagree()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    yield return (object) new WaitForEndOfFrame();
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int convoNumber = interactionDlcNightFox.convoNumber;
    int num = -1;
    string str = $"Conversation_NPC/Fox/Meeting{convoNumber.ToString()}/Answer_B/";
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
    {
      Debug.Log((object) $"{str}  {num.ToString()}");
      ConversationEntry conversationEntry = new ConversationEntry(interactionDlcNightFox.Spine.gameObject, str + num.ToString());
      Entries.Add(conversationEntry);
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, new System.Action(interactionDlcNightFox.\u003CDisagree\u003Eb__45_0)), SnapLetterBox: true);
    interactionDlcNightFox.CameraIncludePlayer();
  }

  public IEnumerator DisagreeCallback()
  {
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(0.5f);
    this.Spine.AnimationState.SetAnimation(0, "exit", false);
    yield return (object) new WaitForSeconds(1f);
    this.ShowGoop();
    yield return (object) new WaitForSeconds(1.66666675f);
    SimulationManager.UnPause();
    this.SimpleSetCamera.DectivateDistance = 1f;
    this.SimpleSetCamera.AutomaticallyActivate = true;
    AudioManager.Instance.SetMusicRoomID(0, "shore_id");
    this.Spine.gameObject.SetActive(false);
    this.Activating = false;
  }

  public void CameraIncludePlayer()
  {
    MMConversation.ControlCamera = false;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(this.Spine.gameObject);
    GameManager.GetInstance().AddPlayerToCamera();
  }

  public IEnumerator GiveItem()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    switch (interactionDlcNightFox.EncounterType)
    {
      case Interaction_DLCNightFox.EncounterTypes.MutatedFollower:
        yield return (object) interactionDlcNightFox.StartCoroutine((IEnumerator) interactionDlcNightFox.GiveFollowerRoutine());
        break;
      case Interaction_DLCNightFox.EncounterTypes.LambFollower:
        yield return (object) interactionDlcNightFox.StartCoroutine((IEnumerator) interactionDlcNightFox.GiveFollowerRoutine());
        break;
    }
  }

  public float RequiredFollowers()
  {
    switch (this.EncounterType)
    {
      case Interaction_DLCNightFox.EncounterTypes.MutatedFollower:
        return 1f;
      case Interaction_DLCNightFox.EncounterTypes.LambFollower:
        return 1f;
      default:
        return 0.0f;
    }
  }

  public IEnumerator GiveFollowerRoutine()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    interactionDlcNightFox.SpawnedFollowers = new List<FollowerManager.SpawnedFollower>();
    interactionDlcNightFox.FollowerCount = 0;
    interactionDlcNightFox.FollowerSelect();
    while ((double) interactionDlcNightFox.FollowerCount < (double) interactionDlcNightFox.RequiredFollowers())
      yield return (object) null;
    yield return (object) new WaitForSeconds(1f);
    interactionDlcNightFox.FadeRedIn();
    AudioManager.Instance.PlayOneShot("event:/Stings/thenight_sacrifice_followers", interactionDlcNightFox.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionDlcNightFox.Spine.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.7f, 1.2f, 2.13333344f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, interactionDlcNightFox.playerFarming);
    foreach (FollowerManager.SpawnedFollower spawnedFollower in interactionDlcNightFox.SpawnedFollowers)
    {
      spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "fox-sacrifice", false);
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(2.13333344f);
    interactionDlcNightFox.FadeRedAway();
    foreach (FollowerManager.SpawnedFollower spawnedFollower in interactionDlcNightFox.SpawnedFollowers)
      FollowerManager.CleanUpCopyFollower(spawnedFollower);
    yield return (object) new WaitForSeconds(1f);
  }

  public void FollowerSelect()
  {
    Time.timeScale = 0.0f;
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      bool flag = false;
      foreach (FollowerManager.SpawnedFollower spawnedFollower in this.SpawnedFollowers)
      {
        if (follower.ID == spawnedFollower.FollowerFakeBrain.Info.ID)
        {
          flag = true;
          break;
        }
      }
      if (this.EncounterType == Interaction_DLCNightFox.EncounterTypes.LambFollower)
      {
        if (follower.ID == 100007 && !flag)
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower)));
      }
      else if (this.EncounterType == Interaction_DLCNightFox.EncounterTypes.MutatedFollower && (follower.Traits.Contains(FollowerTrait.TraitType.Mutated) || follower.Traits.Contains(FollowerTrait.TraitType.MutatedVisual) && !flag))
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower)));
    }
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.VotingType = TwitchVoting.VotingType.SACRIFICE_TO_NIGHT_FOX;
    followerSelectInstance.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, false, true, true, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", this.playerFarming.gameObject);
      this.StartCoroutine((IEnumerator) this.SpawnFollower(followerInfo.ID));
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnHide = selectMenuController2.OnHide + (System.Action) (() => Time.timeScale = 1f);
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnHidden = selectMenuController3.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  public IEnumerator SpawnFollower(int ID)
  {
    Interaction_DLCNightFox interactionDlcNightFox1 = this;
    yield return (object) new WaitForSeconds(0.5f);
    float f = (float) (360.0 / (double) interactionDlcNightFox1.RequiredFollowers() * (double) interactionDlcNightFox1.FollowerCount * (Math.PI / 180.0));
    float num1 = 1.5f;
    Vector3 Position = new Vector3(num1 * Mathf.Cos(f), num1 * Mathf.Sin(f));
    Debug.Log((object) ("(ID) " + ID.ToString()));
    Debug.Log((object) ("FollowerManager.FindFollowerInfo(ID) " + FollowerManager.FindFollowerInfo(ID)?.ToString()));
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.FindFollowerInfo(ID), interactionDlcNightFox1.state.transform.position + Vector3.down, interactionDlcNightFox1.Spine.transform, PlayerFarming.Location);
    interactionDlcNightFox1.SpawnedFollowers.Add(spawnedFollower);
    CameraManager.shakeCamera(1f);
    AudioManager.Instance.PlayOneShot("event:/player/standard_jump_spin_float", spawnedFollower.Follower.gameObject);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "fox-spawn", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "fox-floating", true, 0.0f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CureDissenter, ID);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillFollower, ID);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillFollowersSpouse, ID);
    FollowerManager.RemoveFollowerBrain(ID);
    ObjectiveManager.FailUniqueFollowerObjectives(ID);
    yield return (object) new WaitForSeconds(1f);
    spawnedFollower.Follower.transform.DOMove(interactionDlcNightFox1.Spine.transform.position + Position, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", spawnedFollower.Follower.gameObject);
    yield return (object) new WaitForSeconds(2.5f);
    Interaction_DLCNightFox interactionDlcNightFox2 = interactionDlcNightFox1;
    int num2 = interactionDlcNightFox1.FollowerCount + 1;
    int num3 = num2;
    interactionDlcNightFox2.FollowerCount = num3;
    if ((double) num2 < (double) interactionDlcNightFox1.RequiredFollowers())
      interactionDlcNightFox1.FollowerSelect();
  }

  public void FadeRedAway()
  {
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.UpdateLighting(true);
  }

  public void FadeRedIn()
  {
    LightingManager.Instance.inOverride = true;
    this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = this.LightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = 0.0f;
    LightingManager.Instance.UpdateLighting(true);
  }

  public void ShowGoop()
  {
    if (!this.goopFloorParticle.gameObject.activeSelf)
    {
      this.goopFloorParticle.gameObject.SetActive(true);
      this.goopFloorParticle.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "leader-start", false);
      this.goopFloorParticle.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "leader-loop", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/enemy/summoned", this.goopFloorParticle.transform.position);
      this.goopFloorParticle.GetComponent<SimpleSpineDeactivateAfterPlay>().enabled = false;
    }
    else
      this.StartCoroutine((IEnumerator) this.HideGoopDelayIE());
  }

  public IEnumerator HideGoopDelayIE()
  {
    yield return (object) new WaitForSeconds(1f);
    this.goopFloorParticle.GetComponent<SimpleSpineDeactivateAfterPlay>().enabled = true;
  }

  public List<FollowerBrain> GetAvaiableMutatedFollowers()
  {
    List<FollowerBrain> mutatedFollowers = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && (allBrain.Info.HasTrait(FollowerTrait.TraitType.Mutated) || allBrain.Info.HasTrait(FollowerTrait.TraitType.MutatedVisual)))
        mutatedFollowers.Add(allBrain);
    }
    return mutatedFollowers;
  }

  public List<FollowerBrain> GetAvaiableLambFollowers()
  {
    List<FollowerBrain> avaiableLambFollowers = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.ID == 100007)
        avaiableLambFollowers.Add(allBrain);
    }
    return avaiableLambFollowers;
  }

  public void UnlockRatauFleece(System.Action onHidden)
  {
    this.enabled = true;
    DataManager.Instance.RatauStaffQuestWonGame = true;
    DataManager.Instance.UnlockedFleeces.Add(12);
    UIPlayerUpgradesMenuController upgradesMenuController = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
    upgradesMenuController.OnHidden = upgradesMenuController.OnHidden + onHidden;
    upgradesMenuController.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
    {
      PlayerFleeceManager.FleeceType.RatauCloakBloody
    }, true);
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.RATAU_END));
  }

  public IEnumerator ShowWeaponSequence()
  {
    Interaction_DLCNightFox interactionDlcNightFox = this;
    GameManager.GetInstance().OnConversationNew();
    PlayerSimpleInventory inventory = interactionDlcNightFox.state.gameObject.GetComponent<PlayerSimpleInventory>();
    PickUp ratauStaff = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RATAU_STAFF, 1, interactionDlcNightFox.Spine.transform.position + new Vector3(0.0f, -0.2f, -1.25f), 0.0f, (System.Action<PickUp>) (pickUp => pickUp.enabled = false));
    while ((UnityEngine.Object) ratauStaff == (UnityEngine.Object) null)
      yield return (object) null;
    Vector3 endValue = new Vector3(inventory.ItemImage.transform.position.x, inventory.ItemImage.transform.position.y, -1f);
    GameManager.GetInstance().OnConversationNext(ratauStaff.gameObject, 7f);
    bool isMoving = true;
    ratauStaff.transform.DOMove(endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => isMoving = false));
    while (isMoving)
      yield return (object) null;
    interactionDlcNightFox.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionDlcNightFox.transform.position);
    yield return (object) new WaitForSeconds(1.5f);
    UnityEngine.Object.Destroy((UnityEngine.Object) ratauStaff.gameObject);
    DataManager.Instance.RatauIntroWoolhaven = true;
    DataManager.Instance.ForcedStartingWeapon = EquipmentType.Sword_Ratau;
    DataManager.Instance.AddWeapon(EquipmentType.Sword_Ratau);
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/LegendaryWeaponAddedToPool", EquipmentManager.GetWeaponData(EquipmentType.Sword_Ratau).GetLocalisedTitle());
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003CDeactivate\u003Eb__25_0() => this.MoonIcon.SetActive(false);

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__32_0()
  {
    this.StartCoroutine((IEnumerator) this.AppearRoutine());
  }

  [CompilerGenerated]
  public void \u003CGetQuestionAndResponses\u003Eb__38_0()
  {
    this.StartCoroutine(this.CanAfford() ? (IEnumerator) this.Agree() : (IEnumerator) this.CantAfford());
  }

  [CompilerGenerated]
  public void \u003CGetQuestionAndResponses\u003Eb__38_1()
  {
    this.StartCoroutine((IEnumerator) this.Disagree());
  }

  [CompilerGenerated]
  public void \u003CAgree\u003Eb__41_0() => this.StartCoroutine((IEnumerator) this.AgreeCallback());

  [CompilerGenerated]
  public void \u003CCantAfford\u003Eb__44_0()
  {
    this.StartCoroutine((IEnumerator) this.DisagreeCallback());
  }

  [CompilerGenerated]
  public void \u003CDisagree\u003Eb__45_0()
  {
    this.StartCoroutine((IEnumerator) this.DisagreeCallback());
  }

  public enum EncounterTypes
  {
    MutatedFollower,
    LambFollower,
  }
}
