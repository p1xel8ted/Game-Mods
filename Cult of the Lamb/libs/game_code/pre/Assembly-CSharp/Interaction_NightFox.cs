// Decompiled with JetBrains decompiler
// Type: Interaction_NightFox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

#nullable disable
public class Interaction_NightFox : Interaction
{
  public Interaction_KeyPiece KeyPiecePrefab;
  [SerializeField]
  private GameObject goopFloorParticle;
  private SkeletonAnimation goopSkeleton;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject MoonIcon;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  public Vector3 RatauPosition;
  public Vector3 RatauEndPosition;
  public int RatauScale = 1;
  public SkeletonAnimation Spine;
  private bool Available;
  private string sPeerInToTheDarkness;
  private bool Activating;
  public GameObject PlayerPosition;
  private List<MMTools.Response> responses;
  private List<ConversationEntry> conversationEntries;
  private List<ConversationEntry> conversationOnlyQuestionEntries;
  private int StoneCount;
  public bool ChooseFollowerOverHeart = true;
  private int FollowerCount;
  private List<FollowerManager.SpawnedFollower> SpawnedFollowers = new List<FollowerManager.SpawnedFollower>();
  public GameObject RatauPrefab;
  private ColorGrading colorGrading;

  public Interaction_NightFox.EncounterTypes EncounterType
  {
    get => (Interaction_NightFox.EncounterTypes) DataManager.Instance.CurrentFoxEncounter;
  }

  public static FollowerLocation CurrentFoxLocation
  {
    get => DataManager.Instance.CurrentFoxLocation;
    set => DataManager.Instance.CurrentFoxLocation = value;
  }

  private void Start()
  {
    BiomeConstants.Instance.ppv.profile.TryGetSettings<ColorGrading>(out this.colorGrading);
    this.UpdateLocalisation();
    this.goopSkeleton = this.goopFloorParticle.GetComponent<SkeletonAnimation>();
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnPlayerLocationSet);
  }

  private void OnPlayerLocationSet()
  {
    if ((Interaction_NightFox.CurrentFoxLocation == FollowerLocation.None || Interaction_NightFox.CurrentFoxLocation == PlayerFarming.Location) && !DataManager.Instance.FoxCompleted.Contains(PlayerFarming.Location))
    {
      this.Spine.gameObject.SetActive(false);
      TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
      this.OnNewPhaseStarted();
    }
    else
      this.Deactivate(false);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
  }

  private void Deactivate(bool DelayMoonIcon)
  {
    this.gameObject.SetActive(false);
    this.SimpleSetCamera.AutomaticallyActivate = false;
    int num = (UnityEngine.Object) this.MoonIcon != (UnityEngine.Object) null ? 1 : 0;
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

  private void OnNewPhaseStarted()
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
    PlayerFarming.Instance.GoToAndStop(this.PlayerPosition.transform.position, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.AppearRoutine())));
  }

  private bool CanAfford()
  {
    switch (this.EncounterType)
    {
      case Interaction_NightFox.EncounterTypes.Fish:
        int num = 0;
        foreach (InventoryItem inventoryItem in Inventory.items)
        {
          if (InventoryItem.IsFish((InventoryItem.ITEM_TYPE) inventoryItem.type) && inventoryItem.type != 33)
            num += inventoryItem.quantity;
        }
        return num >= 1;
      case Interaction_NightFox.EncounterTypes.Follower:
        return FollowerBrain.AllAvailableFollowerBrains().Count > 0;
      case Interaction_NightFox.EncounterTypes.FollowerOrHeart:
        return FollowerBrain.AllAvailableFollowerBrains().Count >= 2;
      case Interaction_NightFox.EncounterTypes.Ratau:
        return true;
      default:
        return false;
    }
  }

  private IEnumerator AppearRoutine()
  {
    Interaction_NightFox interactionNightFox = this;
    Interaction_NightFox.CurrentFoxLocation = PlayerFarming.Location;
    AudioManager.Instance.SetMusicRoomID(1, "shore_id");
    interactionNightFox.state.CURRENT_STATE = StateMachine.State.InActive;
    interactionNightFox.state.facingAngle = Utils.GetAngle(interactionNightFox.state.transform.position, interactionNightFox.Spine.transform.position);
    SimulationManager.Pause();
    interactionNightFox.SimpleSetCamera.AutomaticallyActivate = false;
    interactionNightFox.SimpleSetCamera.DectivateDistance = 0.5f;
    interactionNightFox.SimpleSetCamera.Reset();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionNightFox.goopFloorParticle.gameObject, 6f);
    interactionNightFox.ShowGoop();
    yield return (object) new WaitForSeconds(1f);
    interactionNightFox.Spine.AnimationState.SetAnimation(0, "enter", false);
    interactionNightFox.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    interactionNightFox.Spine.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds(2.66666675f);
    interactionNightFox.GetQuestionAndResponses();
    if (DataManager.Instance.FoxIntroductions.Contains(PlayerFarming.Location))
      interactionNightFox.PoseQuestion();
    else
      interactionNightFox.IntroductionConversation();
  }

  private void GetQuestionAndResponses()
  {
    this.responses = new List<MMTools.Response>();
    int encounterType = (int) this.EncounterType;
    string str1 = this.CanAfford() ? "Conversation_NPC/Fox/Response_Yes" : "Conversation_NPC/Fox/Meeting/Response_CantAfford";
    this.responses.Add(new MMTools.Response(str1, (System.Action) (() => this.StartCoroutine(this.CanAfford() ? (IEnumerator) this.Agree() : (IEnumerator) this.CantAfford())), str1));
    string str2 = "Conversation_NPC/Fox/Response_No";
    this.responses.Add(new MMTools.Response(str2, (System.Action) (() => this.StartCoroutine((IEnumerator) this.Disagree())), str2));
    this.conversationEntries = new List<ConversationEntry>();
    int num = -1;
    string str3 = $"Conversation_NPC/Fox/Meeting{encounterType.ToString()}/";
    while (LocalizationManager.GetTermData(str3 + (object) ++num) != null)
    {
      Debug.Log((object) $"{str3}  {(object) num}");
      ConversationEntry conversationEntry = new ConversationEntry(this.Spine.gameObject, str3 + num.ToString());
      this.conversationEntries.Add(conversationEntry);
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    }
    this.conversationOnlyQuestionEntries = new List<ConversationEntry>();
    this.conversationOnlyQuestionEntries.Add(this.conversationEntries[this.conversationEntries.Count - 1]);
  }

  private void IntroductionConversation()
  {
    Debug.Log((object) nameof (IntroductionConversation));
    DataManager.Instance.FoxIntroductions.Add(PlayerFarming.Location);
    ConversationObject ConversationObject = new ConversationObject(this.conversationEntries, this.responses, (System.Action) null);
    foreach (ConversationEntry conversationEntry in this.conversationEntries)
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    MMConversation.Play(ConversationObject, false, SetPlayerIdleOnComplete: false);
    this.CameraIncludePlayer();
  }

  private void PoseQuestion()
  {
    ConversationObject ConversationObject = new ConversationObject(this.conversationOnlyQuestionEntries, this.responses, (System.Action) null);
    foreach (ConversationEntry onlyQuestionEntry in this.conversationOnlyQuestionEntries)
      onlyQuestionEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    MMConversation.Play(ConversationObject, false, SetPlayerIdleOnComplete: false);
    this.CameraIncludePlayer();
  }

  private IEnumerator Agree()
  {
    Interaction_NightFox interactionNightFox = this;
    yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.GiveItem());
    yield return (object) new WaitForEndOfFrame();
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int encounterType = (int) interactionNightFox.EncounterType;
    int num = -1;
    string str = $"Conversation_NPC/Fox/Meeting{encounterType.ToString()}/Answer_A/";
    while (LocalizationManager.GetTermData(str + (object) ++num) != null)
    {
      Debug.Log((object) $"{str}  {(object) num}");
      ConversationEntry conversationEntry = new ConversationEntry(interactionNightFox.Spine.gameObject, str + num.ToString());
      if (encounterType == 3)
      {
        if (num == 0 || num == 1 || num == 2 || num == 3)
        {
          conversationEntry.soundPath = "event:/dialogue/the_night/laugh_the_night";
          conversationEntry.Animation = "talk-laugh";
        }
        else
          conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
      }
      else
        conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
      Entries.Add(conversationEntry);
    }
    // ISSUE: reference to a compiler-generated method
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, new System.Action(interactionNightFox.\u003CAgree\u003Eb__37_0)), false, SetPlayerIdleOnComplete: false, SnapLetterBox: true);
    interactionNightFox.CameraIncludePlayer();
  }

  private IEnumerator AgreeCallback()
  {
    Interaction_NightFox interactionNightFox = this;
    yield return (object) new WaitForEndOfFrame();
    interactionNightFox.SimpleSetCamera.Reset();
    Debug.Log((object) "AGREE!");
    Debug.Log((object) ("EncounterType: " + (object) interactionNightFox.EncounterType));
    if (interactionNightFox.EncounterType == Interaction_NightFox.EncounterTypes.Ratau)
      yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.GiveFinalRewards());
    GameManager.GetInstance().RemoveAllFromCamera();
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(interactionNightFox.KeyPiecePrefab, interactionNightFox.Spine.transform.position + new Vector3(0.0f, -0.2f, -1.25f), Quaternion.identity, interactionNightFox.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: true);
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", interactionNightFox.Spine.transform.position);
    interactionNightFox.Spine.AnimationState.SetAnimation(0, "give-key", false);
    interactionNightFox.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    KeyPiece.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => KeyPiece.transform.DOMove(this.state.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      GameManager.GetInstance().OnConversationEnd(false);
      KeyPiece.OnInteract(this.state);
      this.StartCoroutine((IEnumerator) this.ExitRoutine());
    }))));
    SimulationManager.UnPause();
    DataManager.Instance.FoxCompleted.Add(PlayerFarming.Location);
    Interaction_NightFox.CurrentFoxLocation = FollowerLocation.None;
    ++DataManager.Instance.CurrentFoxEncounter;
  }

  private IEnumerator GiveFinalRewards()
  {
    Debug.Log((object) "Give final rewards!");
    bool Waiting = true;
    FollowerSkinCustomTarget.Create(this.Spine.transform.position, PlayerFarming.Instance.transform.position, 0.5f, "Nightwolf", (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
  }

  private IEnumerator ExitRoutine()
  {
    Interaction_NightFox interactionNightFox = this;
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
    interactionNightFox.Spine.AnimationState.SetAnimation(0, "exit", false);
    yield return (object) new WaitForSeconds(0.1f);
    Vector3 position = interactionNightFox.transform.position;
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", position);
    yield return (object) new WaitForSeconds(0.1f);
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", position);
    yield return (object) new WaitForSeconds(0.1f);
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", position);
    yield return (object) new WaitForSeconds(0.7f);
    interactionNightFox.ShowGoop();
    yield return (object) new WaitForSeconds(1f);
    interactionNightFox.Activating = false;
    if (interactionNightFox.EncounterType >= Interaction_NightFox.EncounterTypes.Ratau)
    {
      yield return (object) new WaitForSeconds(1f);
      if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
      {
        int i = 3;
        while (--i >= 0)
        {
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, interactionNightFox.Spine.transform.position).GetComponent<Interaction_DoctrineStone>().MagnetToPlayer();
          yield return (object) new WaitForSeconds(0.2f);
        }
      }
    }
    interactionNightFox.Deactivate(true);
  }

  private IEnumerator CantAfford()
  {
    Interaction_NightFox interactionNightFox = this;
    yield return (object) new WaitForEndOfFrame();
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int encounterType = (int) interactionNightFox.EncounterType;
    int num = -1;
    string str = "Conversation_NPC/Fox/Answer_CantAfford/";
    while (LocalizationManager.GetTermData(str + (object) ++num) != null)
    {
      Debug.Log((object) $"{str}  {(object) num}");
      ConversationEntry conversationEntry = new ConversationEntry(interactionNightFox.Spine.gameObject, str + num.ToString());
      Entries.Add(conversationEntry);
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    }
    // ISSUE: reference to a compiler-generated method
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, new System.Action(interactionNightFox.\u003CCantAfford\u003Eb__42_0)), SnapLetterBox: true);
    interactionNightFox.CameraIncludePlayer();
  }

  private IEnumerator Disagree()
  {
    Interaction_NightFox interactionNightFox = this;
    yield return (object) new WaitForEndOfFrame();
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int encounterType = (int) interactionNightFox.EncounterType;
    int num = -1;
    string str = $"Conversation_NPC/Fox/Meeting{encounterType.ToString()}/Answer_B/";
    while (LocalizationManager.GetTermData(str + (object) ++num) != null)
    {
      Debug.Log((object) $"{str}  {(object) num}");
      ConversationEntry conversationEntry = new ConversationEntry(interactionNightFox.Spine.gameObject, str + num.ToString());
      Entries.Add(conversationEntry);
      conversationEntry.soundPath = "event:/dialogue/the_night/standard_the_night";
    }
    // ISSUE: reference to a compiler-generated method
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, new System.Action(interactionNightFox.\u003CDisagree\u003Eb__43_0)), SnapLetterBox: true);
    interactionNightFox.CameraIncludePlayer();
  }

  private IEnumerator DisagreeCallback()
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
    Debug.Log((object) "DISAGREE!");
    this.Activating = false;
  }

  private void CameraIncludePlayer()
  {
    MMConversation.ControlCamera = false;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(this.Spine.gameObject);
    GameManager.GetInstance().AddPlayerToCamera();
  }

  private IEnumerator GiveItem()
  {
    Interaction_NightFox interactionNightFox = this;
    switch (interactionNightFox.EncounterType)
    {
      case Interaction_NightFox.EncounterTypes.Fish:
        yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.GiveFishRoutine());
        break;
      case Interaction_NightFox.EncounterTypes.Follower:
        yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.GiveFollowerRoutine());
        break;
      case Interaction_NightFox.EncounterTypes.FollowerOrHeart:
        yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.AskFollowerOrHeart());
        if (interactionNightFox.ChooseFollowerOverHeart)
        {
          yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.GiveFollowerRoutine());
          break;
        }
        yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.GiveHalfHeartRoutine());
        break;
      case Interaction_NightFox.EncounterTypes.Ratau:
        yield return (object) interactionNightFox.StartCoroutine((IEnumerator) interactionNightFox.GiveRatauRoutine());
        break;
    }
  }

  private IEnumerator AskFollowerOrHeart()
  {
    Interaction_NightFox interactionNightFox = this;
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
    choice.Offset = new Vector3(0.0f, -350f);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    choice.Show("<sprite name=\"icon_UIHeartHalf\">", "Inventory/HALF_HEART", "<sprite name=\"icon_Followers\">x " + (object) interactionNightFox.RequiredFollowers(), "Inventory/FOLLOWERS", new System.Action(interactionNightFox.\u003CAskFollowerOrHeart\u003Eb__48_0), new System.Action(interactionNightFox.\u003CAskFollowerOrHeart\u003Eb__48_1), interactionNightFox.state.transform.position);
    while ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      choice.UpdatePosition(interactionNightFox.state.transform.position);
      yield return (object) null;
    }
  }

  private IEnumerator GiveFishRoutine()
  {
    Interaction_NightFox interactionNightFox = this;
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem inventoryItem in Inventory.items)
    {
      if (InventoryItem.IsFish((InventoryItem.ITEM_TYPE) inventoryItem.type) && inventoryItem.type != 33)
        items.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
    }
    InventoryItem.ITEM_TYPE chosenFish = InventoryItem.ITEM_TYPE.NONE;
    interactionNightFox.ShowItemSelector(items, "thenight_fish").OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem => chosenFish = chosenItem);
    while (chosenFish == InventoryItem.ITEM_TYPE.NONE)
      yield return (object) null;
    bool waiting = true;
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionNightFox.Spine.gameObject);
    ResourceCustomTarget.Create(interactionNightFox.Spine.gameObject, interactionNightFox.state.transform.position, chosenFish, (System.Action) (() => waiting = false));
    Inventory.ChangeItemQuantity((int) chosenFish, -1);
    while (waiting)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", interactionNightFox.gameObject);
  }

  private IEnumerator GiveHeartRoutine()
  {
    Interaction_NightFox interactionNightFox = this;
    InventoryItem.ITEM_TYPE chosenHeart = InventoryItem.ITEM_TYPE.NONE;
    interactionNightFox.ShowItemSelector(new List<InventoryItem.ITEM_TYPE>()
    {
      InventoryItem.ITEM_TYPE.MONSTER_HEART
    }, "thenight_heart").OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem => chosenHeart = chosenItem);
    while (chosenHeart == InventoryItem.ITEM_TYPE.NONE)
      yield return (object) null;
    bool waiting = true;
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionNightFox.Spine.gameObject);
    ResourceCustomTarget.Create(interactionNightFox.Spine.gameObject, interactionNightFox.state.transform.position, chosenHeart, (System.Action) (() => waiting = false));
    Inventory.ChangeItemQuantity((int) chosenHeart, -1);
    while (waiting)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/player/collect_heart", interactionNightFox.gameObject);
  }

  private void FollowerTest()
  {
    this.Spine.gameObject.SetActive(true);
    this.GetQuestionAndResponses();
    this.StartCoroutine((IEnumerator) this.Agree());
  }

  private float RequiredFollowers()
  {
    switch (this.EncounterType)
    {
      case Interaction_NightFox.EncounterTypes.Follower:
        return 1f;
      case Interaction_NightFox.EncounterTypes.FollowerOrHeart:
        return 2f;
      default:
        return 0.0f;
    }
  }

  private IEnumerator GiveFollowerRoutine()
  {
    this.SpawnedFollowers = new List<FollowerManager.SpawnedFollower>();
    this.FollowerCount = 0;
    this.FollowerSelect();
    while ((double) this.FollowerCount < (double) this.RequiredFollowers())
      yield return (object) null;
    yield return (object) new WaitForSeconds(1f);
    this.FadeRedIn();
    AudioManager.Instance.PlayOneShot("event:/Stings/thenight_sacrifice_followers", PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNext(this.Spine.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.7f, 1.2f, 2.13333344f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    foreach (FollowerManager.SpawnedFollower spawnedFollower in this.SpawnedFollowers)
    {
      spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "fox-sacrifice", false);
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(2.13333344f);
    this.FadeRedAway();
    foreach (FollowerManager.SpawnedFollower spawnedFollower in this.SpawnedFollowers)
      FollowerManager.CleanUpCopyFollower(spawnedFollower);
    yield return (object) new WaitForSeconds(1f);
  }

  private void FollowerSelect()
  {
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.Show(FollowerBrain.AllAvailableFollowerBrains(), (List<FollowerBrain>) null, false, UpgradeSystem.Type.Count, true, false, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.StartCoroutine((IEnumerator) this.SpawnFollower(followerInfo.ID));
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
  }

  private IEnumerator SpawnFollower(int ID)
  {
    Interaction_NightFox interactionNightFox1 = this;
    yield return (object) new WaitForSeconds(0.5f);
    float f = (float) (360.0 / (double) interactionNightFox1.RequiredFollowers() * (double) interactionNightFox1.FollowerCount * (Math.PI / 180.0));
    float num1 = 1.5f;
    Vector3 Position = new Vector3(num1 * Mathf.Cos(f), num1 * Mathf.Sin(f));
    Debug.Log((object) ("(ID) " + (object) ID));
    Debug.Log((object) ("FollowerManager.FindFollowerInfo(ID) " + (object) FollowerManager.FindFollowerInfo(ID)));
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.FindFollowerInfo(ID), interactionNightFox1.state.transform.position + Vector3.down, interactionNightFox1.Spine.transform, PlayerFarming.Location);
    interactionNightFox1.SpawnedFollowers.Add(spawnedFollower);
    CameraManager.shakeCamera(1f);
    AudioManager.Instance.PlayOneShot("event:/player/standard_jump_spin_float", spawnedFollower.Follower.gameObject);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "fox-spawn", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "fox-floating", true, 0.0f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CureDissenter, ID);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillFollower, ID);
    FollowerManager.RemoveFollowerBrain(ID);
    yield return (object) new WaitForSeconds(1f);
    spawnedFollower.Follower.transform.DOMove(interactionNightFox1.Spine.transform.position + Position, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", spawnedFollower.Follower.gameObject);
    yield return (object) new WaitForSeconds(2.5f);
    Interaction_NightFox interactionNightFox2 = interactionNightFox1;
    int num2 = interactionNightFox1.FollowerCount + 1;
    int num3 = num2;
    interactionNightFox2.FollowerCount = num3;
    if ((double) num2 < (double) interactionNightFox1.RequiredFollowers())
      interactionNightFox1.FollowerSelect();
  }

  private void TestHalfHeart() => this.StartCoroutine((IEnumerator) this.GiveHalfHeartRoutine());

  private IEnumerator GiveHalfHeartRoutine()
  {
    Interaction_NightFox interactionNightFox = this;
    yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "gameover", false);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/select_hearts", PlayerFarming.Instance.transform.position);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", PlayerFarming.Instance.transform.position);
    yield return (object) new WaitForSeconds(3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    CameraManager.shakeCamera(2f);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", PlayerFarming.Instance.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "red", "burst_big");
    bool Waiting = true;
    ResourceCustomTarget.Create(interactionNightFox.Spine.gameObject, interactionNightFox.state.transform.position, InventoryItem.ITEM_TYPE.HALF_HEART, (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/player/collect_heart", interactionNightFox.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "idle", true);
    HealthPlayer objectOfType = UnityEngine.Object.FindObjectOfType<HealthPlayer>();
    --objectOfType.totalHP;
    objectOfType.HP = objectOfType.totalHP;
    --DataManager.Instance.PLAYER_HEALTH_MODIFIED;
  }

  private void TestRat() => this.StartCoroutine((IEnumerator) this.GiveRatauRoutine());

  private IEnumerator GiveRatauRoutine()
  {
    yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/Stings/sacrifice_ratau", PlayerFarming.Instance.transform.position);
    DataManager.Instance.RatauKilled = true;
    ObjectiveManager.FailDefeatKnucklebones("NAMES/Ratau");
    AudioManager.Instance.PlayOneShot("event:/player/standard_jump_spin_float", PlayerFarming.Instance.transform.position);
    SkeletonAnimation rat = UnityEngine.Object.Instantiate<GameObject>(this.RatauPrefab, this.RatauPosition, Quaternion.identity).GetComponentInChildren<SkeletonAnimation>();
    rat.AnimationState.SetAnimation(0, "ratau-sacrifice/start", false);
    rat.AnimationState.AddAnimation(0, "ratau-sacrifice/loop", true, 0.0f);
    rat.skeleton.ScaleX = (float) this.RatauScale;
    yield return (object) new WaitForSeconds(2.25f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(rat.gameObject, "Conversation_NPC/Fox/SeeRatau/1")
    }, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.isPlaying)
      yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/dialogue/ratau/ratau_sacrificed", rat.gameObject);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "ratau-sacrifice/start", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "ratau-sacrifice/loop", true, 0.0f);
    Debug.Log((object) ("a PlayerFarming.Instance.Spine.AnimationState: " + (object) PlayerFarming.Instance.Spine.AnimationState));
    this.Spine.AnimationState.SetAnimation(0, "ratau-sacrifice/start", false);
    this.Spine.AnimationState.AddAnimation(0, "ratau-sacrifice/loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1.56666672f);
    rat.transform.DOMove(this.RatauEndPosition, 3f);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", rat.gameObject);
    yield return (object) new WaitForSeconds(3f);
    this.FadeRedIn();
    GameManager.GetInstance().AddToCamera(this.Spine.gameObject);
    GameManager.GetInstance().CameraSetTargetZoom(5f);
    CameraManager.instance.ShakeCameraForDuration(0.7f, 1.2f, 1.5f);
    AudioManager.Instance.PlayOneShot("event:/small_portal/close", PlayerFarming.Instance.transform.position);
    rat.AnimationState.SetAnimation(0, "ratau-sacrifice/end", false);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "ratau-sacrifice/end", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    this.Spine.AnimationState.SetAnimation(0, "ratau-sacrifice/end", false);
    this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(2.33333325f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.3f);
    this.FadeRedAway();
    yield return (object) new WaitForSeconds(2f);
  }

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    Gizmos.DrawWireSphere(this.RatauPosition, 0.5f);
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.RatauEndPosition, 0.5f);
  }

  public void FadeRedAway()
  {
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.UpdateLighting(true);
  }

  private void FadeRedIn()
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
      AudioManager.Instance.PlayOneShot("event:/enemy/summoned", this.transform.position);
      this.goopFloorParticle.GetComponent<SimpleSpineDeactivateAfterPlay>().enabled = false;
    }
    else
      this.StartCoroutine((IEnumerator) this.HideGoopDelayIE());
  }

  private IEnumerator HideGoopDelayIE()
  {
    yield return (object) new WaitForSeconds(1f);
    this.goopFloorParticle.GetComponent<SimpleSpineDeactivateAfterPlay>().enabled = true;
  }

  private UIItemSelectorOverlayController ShowItemSelector(
    List<InventoryItem.ITEM_TYPE> items,
    string key)
  {
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(items, new ItemSelector.Params()
    {
      Key = key,
      Context = ItemSelector.Context.Give,
      Offset = new Vector2(0.0f, 200f),
      HideOnSelection = true,
      PreventCancellation = true
    });
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => itemSelector = (UIItemSelectorOverlayController) null);
    return itemSelector;
  }

  public enum EncounterTypes
  {
    Fish,
    Follower,
    FollowerOrHeart,
    Ratau,
  }
}
