// Decompiled with JetBrains decompiler
// Type: NPC_Sozo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class NPC_Sozo : Interaction
{
  [SerializeField]
  public GameObject Shrine;
  [Space]
  [SerializeField]
  public GameObject SozoCrop;
  [SerializeField]
  public Interaction_SimpleConversation convo0;
  [SerializeField]
  public Interaction_SimpleConversation convo1;
  [SerializeField]
  public Interaction_SimpleConversation convo2;
  [SerializeField]
  public Interaction_SimpleConversation convo3;
  [SerializeField]
  public Interaction_SimpleConversation convo4;
  [SerializeField]
  public Interaction_SimpleConversation convo5;
  [SerializeField]
  public Interaction_SimpleConversation convo6;
  [SerializeField]
  public Interaction_SimpleConversation convo7;
  [SerializeField]
  public Interaction_SimpleConversation convo8;
  [SerializeField]
  public Interaction_SimpleConversation convo9;
  [SerializeField]
  public Interaction_SimpleConversation convo10;
  [SerializeField]
  public Interaction_SimpleConversation convoBeforeDeath;
  [SerializeField]
  public SimpleBarkRepeating buyingShroomsBark;
  [Space]
  [SerializeField]
  public SkeletonAnimation SozoSpine;
  [SerializeField]
  public GameObject deadParticles;
  [Space]
  [SerializeField]
  public SkeletonAnimation worshipper1;
  [SerializeField]
  public SkeletonAnimation worshipper2;
  [SerializeField]
  public SkeletonAnimation worshipper3;
  [SerializeField]
  public SkeletonAnimation worshipper4;
  [Space]
  [SerializeField]
  public GameObject pos1;
  [SerializeField]
  public GameObject pos2;
  [SerializeField]
  public GameObject pos3;
  [SerializeField]
  public GameObject pos4;
  [SerializeField]
  public GameObject mushrooms;
  [Space]
  [SerializeField]
  public SimpleSetCamera simpleSetCamera;
  public string sLabel;
  public Coroutine acceptRoutine;
  public const int pricePerMushroom = 5;
  public const int requiredMushrooms1 = 10;
  public const int requiredMushrooms2 = 20;
  public GameObject uINewCard;
  public bool Activated;
  public Interaction_KeyPiece KeyPiecePrefab;

  public int StoryProgress
  {
    get => DataManager.Instance.SozoStoryProgress;
    set => DataManager.Instance.SozoStoryProgress = value;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Start();
    AudioManager.Instance.SetMusicRoomID(1, "drum_layer");
    DoctrineUpgradeSystem.Initialise();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.SetMusicRoomID(0, "drum_layer");
    DoctrineUpgradeSystem.DeInitialise();
  }

  public void Start()
  {
    this.SozoCrop.SetActive(false);
    this.Shrine.SetActive(false);
    this.convo0.gameObject.SetActive(false);
    this.convo1.gameObject.SetActive(false);
    this.convo2.gameObject.SetActive(false);
    this.convo3.gameObject.SetActive(false);
    this.convo4.gameObject.SetActive(false);
    this.convo5.gameObject.SetActive(false);
    this.convo6.gameObject.SetActive(false);
    this.convo7.gameObject.SetActive(false);
    this.convo8.gameObject.SetActive(false);
    this.convo9.gameObject.SetActive(false);
    this.convo10.gameObject.SetActive(false);
    this.convoBeforeDeath.gameObject.SetActive(false);
    this.buyingShroomsBark.gameObject.SetActive(false);
    switch (this.StoryProgress)
    {
      case -1:
      case 0:
        this.convo0.gameObject.SetActive(true);
        break;
      case 5:
        this.Shrine.SetActive(true);
        this.SozoSpine.AnimationState.SetAnimation(0, "tripping-balls", true);
        break;
    }
    if (DataManager.Instance.SozoDead)
    {
      this.SozoSpine.AnimationState.SetAnimation(0, DataManager.Instance.SozoTakenMushroom ? "dead-no-mushroom" : "dead", true);
      this.deadParticles.SetActive(true);
      this.Interactable = false;
    }
    this.UpdateLocalisation();
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (this.StoryProgress == 1)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL) >= 10)
        this.StartCoroutine((IEnumerator) this.GiveMushrooms(10));
      else
        this.StartCoroutine((IEnumerator) this.NeedMoreMushroomsIE());
    }
    else if (this.StoryProgress == 2)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL) >= 20)
        this.StartCoroutine((IEnumerator) this.GiveMushrooms(20));
      else
        this.StartCoroutine((IEnumerator) this.NeedMoreMushroomsIE());
    }
    else if (this.StoryProgress == 3)
    {
      if (!DataManager.Instance.PerformedMushroomRitual)
        return;
      this.StartCoroutine((IEnumerator) this.GiveDecorationRoutine());
    }
    else if (this.StoryProgress == 4)
    {
      if (!DataManager.Instance.BuiltMushroomDecoration)
        return;
      this.StartCoroutine((IEnumerator) this.EndQuestRoutine());
    }
    else if (this.StoryProgress == 5 && DataManager.Instance.SozoDead && !DataManager.Instance.SozoTakenMushroom)
    {
      this.StartCoroutine((IEnumerator) this.TakeSozoMushroom());
    }
    else
    {
      if (this.StoryProgress < 5)
        return;
      this.convoBeforeDeath.Play();
    }
  }

  public IEnumerator TakeSozoMushroom()
  {
    NPC_Sozo npcSozo = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(npcSozo.playerFarming.gameObject, 7f);
    yield return (object) null;
    bool Waiting = true;
    npcSozo.playerFarming.GoToAndStop(npcSozo.transform.position + new Vector3(0.0f, -0.5f), npcSozo.gameObject, GoToCallback: (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
    npcSozo.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) new WaitForSeconds(1f);
    npcSozo.SozoSpine.AnimationState.SetAnimation(0, "dead-no-mushroom", true);
    GameManager.GetInstance().OnConversationNext(npcSozo.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    npcSozo.SozoCrop.SetActive(true);
    npcSozo.SozoCrop.transform.DOMove(npcSozo.playerFarming.transform.position + new Vector3(0.0f, 0.6f, -1.1f), 0.1f);
    npcSozo.SozoCrop.transform.localScale = Vector3.zero;
    npcSozo.SozoCrop.transform.DOScale(Vector3.one * 0.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    npcSozo.playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_pickup", npcSozo.gameObject);
    yield return (object) new WaitForSeconds(2f);
    npcSozo.SozoCrop.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.SozoCrop.SetActive(false)));
    yield return (object) new WaitForSeconds(0.5f);
    npcSozo.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.SozoTakenMushroom = true;
    yield return (object) new WaitForSeconds(0.75f);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.SEED_SOZO, 1);
  }

  public void TestDecoration()
  {
    if (!DataManager.Instance.PerformedMushroomRitual)
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoPerformRitual);
    }
    DataManager.Instance.PerformedMushroomRitual = true;
    this.StoryProgress = 3;
  }

  public IEnumerator GiveDecorationRoutine()
  {
    NPC_Sozo npcSozo = this;
    npcSozo.StoryProgress = 4;
    npcSozo.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoReturn);
    yield return (object) new WaitForSeconds(1f);
    npcSozo.playerFarming.GoToAndStop(npcSozo.transform.position + new Vector3(2f, 0.0f), npcSozo.gameObject);
    if (npcSozo.playerFarming.GoToAndStopping)
      yield return (object) null;
    npcSozo.convo6.gameObject.SetActive(true);
    npcSozo.convo6.CallOnConversationEnd = false;
    npcSozo.convo6.Callback.AddListener(new UnityAction(npcSozo.\u003CGiveDecorationRoutine\u003Eb__42_0));
    npcSozo.convo6.Play();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!DataManager.Instance.SozoBeforeDeath)
      return;
    DataManager.Instance.SozoDead = true;
  }

  public void TestEndQuest()
  {
    this.convo0.gameObject.SetActive(false);
    if (!DataManager.Instance.BuiltMushroomDecoration)
    {
      DataManager.Instance.BuiltMushroomDecoration = true;
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
    }
    this.StoryProgress = 4;
  }

  public IEnumerator EndQuestRoutine()
  {
    NPC_Sozo npcSozo = this;
    npcSozo.Activated = true;
    npcSozo.StoryProgress = 5;
    npcSozo.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoReturn);
    yield return (object) new WaitForSeconds(1f);
    npcSozo.playerFarming.GoToAndStop(npcSozo.transform.position + new Vector3(2f, 0.0f), npcSozo.gameObject);
    if (npcSozo.playerFarming.GoToAndStopping)
      yield return (object) null;
    npcSozo.convo9.gameObject.SetActive(true);
    npcSozo.convo9.CallOnConversationEnd = false;
    npcSozo.convo9.Play();
    npcSozo.convo9.Callback.AddListener(new UnityAction(npcSozo.\u003CEndQuestRoutine\u003Eb__45_0));
  }

  public void RevealShrine()
  {
    this.Shrine.gameObject.SetActive(true);
    Interaction_RatauShrine r = this.Shrine.GetComponentInChildren<Interaction_RatauShrine>();
    r.XPBar.gameObject.SetActive(false);
    Vector3 localPosition = this.Shrine.transform.localPosition;
    this.Shrine.transform.localPosition = localPosition + Vector3.forward;
    this.Shrine.transform.DOLocalMove(localPosition, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      Vector3 localScale = r.transform.localScale;
      r.transform.localScale = Vector3.zero;
      r.transform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      r.XPBar.gameObject.SetActive(true);
      CultFaithManager.AddThought(Thought.Cult_PledgedToYou);
    }));
  }

  public override void GetLabel()
  {
    this.AutomaticallyInteract = false;
    if (this.Activated)
      this.Label = "";
    else if (this.StoryProgress > 0)
    {
      if (this.StoryProgress < 3)
        this.Label = $"{ScriptLocalization.Interactions.Give}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, this.StoryProgress == 1 ? 10 : 20)}";
      else if (this.StoryProgress == 3 && DataManager.Instance.PerformedMushroomRitual)
      {
        this.AutomaticallyInteract = true;
        this.Label = ScriptLocalization.Interactions.Talk;
      }
      else if (this.StoryProgress == 4 && DataManager.Instance.BuiltMushroomDecoration)
      {
        this.AutomaticallyInteract = true;
        this.Label = ScriptLocalization.Interactions.Talk;
      }
      else if (this.StoryProgress == 5 && !DataManager.Instance.SozoDead)
      {
        this.AutomaticallyInteract = true;
        this.Label = ScriptLocalization.Interactions.Talk;
      }
      else if (this.StoryProgress == 5 && DataManager.Instance.SozoDead && !DataManager.Instance.SozoTakenMushroom)
      {
        this.Interactable = true;
        this.Label = LocalizationManager.GetTranslation("Interactions/TakeMushroom");
      }
      else
        this.Label = "";
    }
    else
      this.Label = "";
  }

  public void IntroConvoComplete() => this.StoryProgress = 1;

  public void PlayerHasDoneRitual()
  {
    this.StoryProgress = 4;
    this.GiveKeyPiece((System.Action) null);
  }

  public IEnumerator NeedMoreMushroomsIE()
  {
    NPC_Sozo npcSozo = this;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    ConversationEntry conversationEntry1 = new ConversationEntry(npcSozo.gameObject, "Conversation_NPC/Sozo/StillNotEnoughShrooms/0");
    conversationEntry1.CharacterName = ScriptLocalization.NAMES.Sozo;
    conversationEntry1.soundPath = "event:/dialogue/sozo/sozo_evil";
    conversationEntry1.Animation = "talk-welcome";
    conversationEntry1.DefaultAnimation = "animation";
    Entries.Add(conversationEntry1);
    ConversationEntry conversationEntry2 = ConversationEntry.Clone(conversationEntry1);
    conversationEntry2.TermToSpeak = "Conversation_NPC/Sozo/StillNotEnoughShrooms/1";
    conversationEntry2.Animation = "talk-laugh";
    conversationEntry2.soundPath = "event:/dialogue/sozo/sozo_evil";
    Entries.Add(conversationEntry2);
    ConversationEntry conversationEntry3 = ConversationEntry.Clone(conversationEntry2);
    conversationEntry3.TermToSpeak = "Conversation_NPC/Sozo/StillNotEnoughShrooms/2";
    conversationEntry3.Animation = "talk";
    conversationEntry3.soundPath = "event:/dialogue/sozo/sozo_standard";
    Entries.Add(conversationEntry3);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator SellMushrooms()
  {
    NPC_Sozo npcSozo = this;
    bool activiating = true;
    bool firstPress = true;
    float delay = 0.0f;
    int count = 0;
    while (activiating)
    {
      if ((double) delay <= 0.0)
      {
        if (Inventory.GetItemQuantity(29) > 0)
        {
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", npcSozo.playerFarming.transform.position);
          ResourceCustomTarget.Create(npcSozo.gameObject, npcSozo.playerFarming.transform.position, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, (System.Action) null);
          Inventory.ChangeItemQuantity(29, -1);
          ++count;
        }
        else
          activiating = false;
        delay = firstPress ? 0.5f : 0.1f;
        firstPress = false;
      }
      delay -= Time.deltaTime;
      if (InputManager.Gameplay.GetInteractButtonUp())
        activiating = false;
      else
        yield return (object) null;
    }
    int amount = Mathf.Min(5, count);
    for (int i = 0; i < amount; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", npcSozo.transform.position);
      ResourceCustomTarget.Create(npcSozo.playerFarming.gameObject, npcSozo.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
    Inventory.AddItem(20, count);
  }

  public IEnumerator GiveMushrooms(int requiredAmount)
  {
    NPC_Sozo npcSozo = this;
    if (npcSozo.acceptRoutine != null)
      npcSozo.StopCoroutine(npcSozo.acceptRoutine);
    npcSozo.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BringSozoMushrooms);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BringSozoMushrooms2);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoReturn);
    yield return (object) new WaitForSeconds(1f);
    npcSozo.playerFarming.GoToAndStop(npcSozo.transform.position + new Vector3(2f, 0.0f), npcSozo.gameObject);
    if (npcSozo.playerFarming.GoToAndStopping)
      yield return (object) null;
    Inventory.ChangeItemQuantity(29, -requiredAmount);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(npcSozo.playerFarming.gameObject, 8f);
    GameManager.GetInstance().AddToCamera(npcSozo.gameObject);
    yield return (object) new WaitForSeconds(1f);
    for (int i = 0; i < requiredAmount; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", npcSozo.playerFarming.transform.position);
      ResourceCustomTarget.Create(npcSozo.gameObject, npcSozo.playerFarming.transform.position, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, (System.Action) null);
      yield return (object) new WaitForSeconds((float) (0.20000000298023224 - 0.20000000298023224 * (double) (i / requiredAmount)));
    }
    yield return (object) new WaitForSeconds(1f);
    npcSozo.StoryProgress++;
    if (npcSozo.StoryProgress == 2)
    {
      npcSozo.convo2.gameObject.SetActive(true);
      npcSozo.convo2.CallOnConversationEnd = false;
      npcSozo.convo2.Callback.AddListener(new UnityAction(npcSozo.\u003CGiveMushrooms\u003Eb__52_0));
      npcSozo.convo2.Play();
    }
    else if (npcSozo.StoryProgress == 3)
    {
      npcSozo.convo3.gameObject.SetActive(true);
      npcSozo.convo3.Play();
    }
  }

  public void Ritual() => this.StartCoroutine((IEnumerator) this.RitualIE());

  public IEnumerator RitualIE()
  {
    NPC_Sozo npcSozo = this;
    npcSozo.convo3.gameObject.SetActive(false);
    yield return (object) new WaitForEndOfFrame();
    npcSozo.worshipper1.gameObject.SetActive(true);
    npcSozo.worshipper2.gameObject.SetActive(true);
    npcSozo.worshipper3.gameObject.SetActive(true);
    npcSozo.worshipper4.gameObject.SetActive(true);
    GameManager.GetInstance().OnConversationNext(npcSozo.gameObject, 12f);
    npcSozo.worshipper1.transform.DOMove(npcSozo.pos1.transform.position, 5f);
    npcSozo.worshipper2.transform.DOMove(npcSozo.pos2.transform.position, 5f);
    npcSozo.worshipper3.transform.DOMove(npcSozo.pos3.transform.position, 5f);
    npcSozo.worshipper4.transform.DOMove(npcSozo.pos4.transform.position, 5f);
    yield return (object) new WaitForSeconds(2.5f);
    npcSozo.simpleSetCamera.Play();
    yield return (object) new WaitForSeconds(2.5f);
    npcSozo.mushrooms.SetActive(true);
    npcSozo.mushrooms.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
    npcSozo.SozoSpine.AnimationState.SetAnimation(0, "talk-yes", true);
    yield return (object) new WaitForSeconds(1f);
    npcSozo.worshipper1.AnimationState.SetAnimation(0, "ritual", true);
    npcSozo.worshipper2.AnimationState.SetAnimation(0, "ritual", true);
    npcSozo.worshipper3.AnimationState.SetAnimation(0, "ritual", true);
    npcSozo.worshipper4.AnimationState.SetAnimation(0, "ritual", true);
    float delay1 = 0.0f;
    npcSozo.StartCoroutine((IEnumerator) npcSozo.GiveShrooms(npcSozo.worshipper1.gameObject, 5f, delay1));
    float delay2 = delay1 + 0.1f;
    npcSozo.StartCoroutine((IEnumerator) npcSozo.GiveShrooms(npcSozo.worshipper2.gameObject, 5f, delay2));
    float delay3 = delay2 + 0.1f;
    npcSozo.StartCoroutine((IEnumerator) npcSozo.GiveShrooms(npcSozo.worshipper3.gameObject, 5f, delay3));
    float delay4 = delay3 + 0.1f;
    npcSozo.StartCoroutine((IEnumerator) npcSozo.GiveShrooms(npcSozo.worshipper4.gameObject, 5f, delay4));
    BiomeConstants.Instance.PsychedelicFadeIn(5f);
    AudioManager.Instance.SetMusicPsychedelic(1f);
    yield return (object) new WaitForSeconds(5f);
    npcSozo.mushrooms.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(npcSozo.\u003CRitualIE\u003Eb__54_0));
    yield return (object) new WaitForSeconds(1f);
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, 4, true, (System.Action) null)
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Brainwashing);
    npcSozo.worshipper1.AnimationState.SetAnimation(0, "worship", true);
    npcSozo.worshipper2.AnimationState.SetAnimation(0, "worship", true);
    npcSozo.worshipper3.AnimationState.SetAnimation(0, "worship", true);
    npcSozo.worshipper4.AnimationState.SetAnimation(0, "worship", true);
    npcSozo.worshipper1.transform.DOMove(npcSozo.pos1.transform.position + Vector3.down * 7f, 2.5f);
    npcSozo.worshipper2.transform.DOMove(npcSozo.pos2.transform.position + Vector3.down * 7f, 2.5f);
    npcSozo.worshipper3.transform.DOMove(npcSozo.pos3.transform.position + Vector3.down * 7f, 2.5f);
    npcSozo.worshipper4.transform.DOMove(npcSozo.pos4.transform.position + Vector3.down * 7f, 2.5f);
    BiomeConstants.Instance.PsychedelicFadeOut(1.5f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.SetMusicPsychedelic(0.0f);
    npcSozo.SozoSpine.AnimationState.SetAnimation(0, "animation", true);
    npcSozo.simpleSetCamera.Reset();
    yield return (object) new WaitForSeconds(1.5f);
    npcSozo.worshipper1.gameObject.SetActive(false);
    npcSozo.worshipper2.gameObject.SetActive(false);
    npcSozo.worshipper3.gameObject.SetActive(false);
    npcSozo.worshipper4.gameObject.SetActive(false);
    npcSozo.convo4.gameObject.SetActive(true);
    while (MMConversation.isPlaying)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    npcSozo.convo4.Play();
    while (MMConversation.isPlaying)
      yield return (object) null;
    npcSozo.GiveKeyPiece(new System.Action(npcSozo.\u003CRitualIE\u003Eb__54_1));
  }

  public IEnumerator DelayGiveObjective(Objectives.CustomQuestTypes Quest, int Story)
  {
    this.Activated = true;
    yield return (object) new WaitForSeconds(1.5f);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Quest));
    this.StoryProgress = Story;
    this.Start();
    this.Activated = false;
  }

  public IEnumerator GiveShrooms(GameObject follower, float totalTime, float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    int randomCoins = 10;
    float increment = (totalTime - delay) / (float) randomCoins;
    for (int i = 0; i < randomCoins; ++i)
    {
      this.mushrooms.transform.DOPunchScale(Vector3.one * 0.1f, increment - 0.05f);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.mushrooms.transform.position);
      ResourceCustomTarget.Create(follower.gameObject, this.mushrooms.transform.position + Vector3.forward, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, (System.Action) null);
      yield return (object) new WaitForSeconds(increment);
    }
  }

  public void GiveKeyPiece(System.Action Callback)
  {
    this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine(Callback));
  }

  public IEnumerator GiveKeyPieceRoutine(System.Action Callback)
  {
    NPC_Sozo npcSozo = this;
    yield return (object) null;
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(npcSozo.KeyPiecePrefab, npcSozo.transform.position + Vector3.back * 0.5f, Quaternion.identity, npcSozo.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    KeyPiece.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    KeyPiece.transform.DOMove(npcSozo.playerFarming.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd(false);
    KeyPiece.OnInteract(npcSozo.playerFarming.state);
    KeyPiece.Callback = Callback;
  }

  [CompilerGenerated]
  public void \u003CGiveDecorationRoutine\u003Eb__42_0()
  {
    this.GiveKeyPiece((System.Action) (() =>
    {
      this.convo6.gameObject.SetActive(false);
      this.convo7.gameObject.SetActive(true);
      this.convo7.Play();
      this.convo7.CallOnConversationEnd = false;
      this.convo7.Callback.AddListener((UnityAction) (() =>
      {
        DataManager.Instance.SozoDecorationQuestActive = true;
        StructureBrain.TYPES DecorationType = StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE;
        StructuresData.CompleteResearch(DecorationType);
        StructuresData.SetRevealed(DecorationType);
        UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
        overlayController.pickedBuilding = DecorationType;
        overlayController.Show(UINewItemOverlayController.TypeOfCard.Decoration, this.transform.position, false);
        overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
        {
          this.convo8.gameObject.SetActive(true);
          this.convo8.Play();
          this.convo8.CallOnConversationEnd = true;
          this.convo8.Callback.AddListener((UnityAction) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/VisitSozo", DecorationType))));
        });
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003CGiveDecorationRoutine\u003Eb__42_1()
  {
    this.convo6.gameObject.SetActive(false);
    this.convo7.gameObject.SetActive(true);
    this.convo7.Play();
    this.convo7.CallOnConversationEnd = false;
    this.convo7.Callback.AddListener((UnityAction) (() =>
    {
      DataManager.Instance.SozoDecorationQuestActive = true;
      StructureBrain.TYPES DecorationType = StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE;
      StructuresData.CompleteResearch(DecorationType);
      StructuresData.SetRevealed(DecorationType);
      UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
      overlayController.pickedBuilding = DecorationType;
      overlayController.Show(UINewItemOverlayController.TypeOfCard.Decoration, this.transform.position, false);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        this.convo8.gameObject.SetActive(true);
        this.convo8.Play();
        this.convo8.CallOnConversationEnd = true;
        this.convo8.Callback.AddListener((UnityAction) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/VisitSozo", DecorationType))));
      });
    }));
  }

  [CompilerGenerated]
  public void \u003CGiveDecorationRoutine\u003Eb__42_2()
  {
    DataManager.Instance.SozoDecorationQuestActive = true;
    StructureBrain.TYPES DecorationType = StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE;
    StructuresData.CompleteResearch(DecorationType);
    StructuresData.SetRevealed(DecorationType);
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.pickedBuilding = DecorationType;
    overlayController.Show(UINewItemOverlayController.TypeOfCard.Decoration, this.transform.position, false);
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      this.convo8.gameObject.SetActive(true);
      this.convo8.Play();
      this.convo8.CallOnConversationEnd = true;
      this.convo8.Callback.AddListener((UnityAction) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/VisitSozo", DecorationType))));
    });
  }

  [CompilerGenerated]
  public void \u003CEndQuestRoutine\u003Eb__45_0()
  {
    this.GiveKeyPiece((System.Action) (() =>
    {
      this.convo10.gameObject.SetActive(true);
      this.convo10.CallOnConversationEnd = true;
      this.convo10.Play();
      this.convo10.Callback.AddListener((UnityAction) (() =>
      {
        this.SozoSpine.AnimationState.SetAnimation(0, "talk-mushroom", true);
        this.RevealShrine();
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003CEndQuestRoutine\u003Eb__45_1()
  {
    this.convo10.gameObject.SetActive(true);
    this.convo10.CallOnConversationEnd = true;
    this.convo10.Play();
    this.convo10.Callback.AddListener((UnityAction) (() =>
    {
      this.SozoSpine.AnimationState.SetAnimation(0, "talk-mushroom", true);
      this.RevealShrine();
    }));
  }

  [CompilerGenerated]
  public void \u003CEndQuestRoutine\u003Eb__45_2()
  {
    this.SozoSpine.AnimationState.SetAnimation(0, "talk-mushroom", true);
    this.RevealShrine();
  }

  [CompilerGenerated]
  public void \u003CGiveMushrooms\u003Eb__52_0()
  {
    Debug.Log((object) "CALL BACK 2!");
    this.GiveKeyPiece((System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayGiveObjective(Objectives.CustomQuestTypes.BringSozoMushrooms2, 2))));
  }

  [CompilerGenerated]
  public void \u003CGiveMushrooms\u003Eb__52_1()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayGiveObjective(Objectives.CustomQuestTypes.BringSozoMushrooms2, 2));
  }

  [CompilerGenerated]
  public void \u003CRitualIE\u003Eb__54_0()
  {
    this.mushrooms.SetActive(false);
    this.mushrooms.transform.localScale = Vector3.one;
  }

  [CompilerGenerated]
  public void \u003CRitualIE\u003Eb__54_1()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayGiveObjective(Objectives.CustomQuestTypes.SozoPerformRitual, 3));
  }
}
