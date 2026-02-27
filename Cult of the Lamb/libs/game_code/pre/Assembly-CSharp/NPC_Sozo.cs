// Decompiled with JetBrains decompiler
// Type: NPC_Sozo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class NPC_Sozo : Interaction
{
  [SerializeField]
  private GameObject Shrine;
  [Space]
  [SerializeField]
  private Interaction_SimpleConversation convo0;
  [SerializeField]
  private Interaction_SimpleConversation convo1;
  [SerializeField]
  private Interaction_SimpleConversation convo2;
  [SerializeField]
  private Interaction_SimpleConversation convo3;
  [SerializeField]
  private Interaction_SimpleConversation convo4;
  [SerializeField]
  private Interaction_SimpleConversation convo5;
  [SerializeField]
  private Interaction_SimpleConversation convo6;
  [SerializeField]
  private Interaction_SimpleConversation convo7;
  [SerializeField]
  private Interaction_SimpleConversation convo8;
  [SerializeField]
  private Interaction_SimpleConversation convo9;
  [SerializeField]
  private Interaction_SimpleConversation convo10;
  [SerializeField]
  private Interaction_SimpleConversation convoBeforeDeath;
  [SerializeField]
  private SimpleBarkRepeating buyingShroomsBark;
  [Space]
  [SerializeField]
  private SkeletonAnimation SozoSpine;
  [SerializeField]
  private GameObject deadParticles;
  [Space]
  [SerializeField]
  private SkeletonAnimation worshipper1;
  [SerializeField]
  private SkeletonAnimation worshipper2;
  [SerializeField]
  private SkeletonAnimation worshipper3;
  [SerializeField]
  private SkeletonAnimation worshipper4;
  [Space]
  [SerializeField]
  private GameObject pos1;
  [SerializeField]
  private GameObject pos2;
  [SerializeField]
  private GameObject pos3;
  [SerializeField]
  private GameObject pos4;
  [SerializeField]
  private GameObject mushrooms;
  [Space]
  [SerializeField]
  private SimpleSetCamera simpleSetCamera;
  private string sLabel;
  private Coroutine acceptRoutine;
  private const int pricePerMushroom = 5;
  private const int requiredMushrooms1 = 10;
  private const int requiredMushrooms2 = 20;
  public GameObject uINewCard;
  private bool Activated;
  public Interaction_KeyPiece KeyPiecePrefab;

  private int StoryProgress
  {
    get => DataManager.Instance.SozoStoryProgress;
    set => DataManager.Instance.SozoStoryProgress = value;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.Start();
    AudioManager.Instance.SetMusicRoomID(1, "drum_layer");
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.SetMusicRoomID(0, "drum_layer");
  }

  private void Start()
  {
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
      this.SozoSpine.AnimationState.SetAnimation(0, "dead", true);
      this.deadParticles.SetActive(true);
      this.Interactable = false;
    }
    this.UpdateLocalisation();
  }

  public override void OnInteract(StateMachine state)
  {
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
      if (DataManager.Instance.PerformedMushroomRitual)
        this.StartCoroutine((IEnumerator) this.GiveDecorationRoutine());
    }
    else if (this.StoryProgress == 4)
    {
      if (DataManager.Instance.BuiltMushroomDecoration)
        this.StartCoroutine((IEnumerator) this.EndQuestRoutine());
    }
    else if (this.StoryProgress >= 5)
      this.convoBeforeDeath.Play();
    base.OnInteract(state);
  }

  private void TestDecoration()
  {
    if (!DataManager.Instance.PerformedMushroomRitual)
    {
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoPerformRitual);
    }
    DataManager.Instance.PerformedMushroomRitual = true;
    this.StoryProgress = 3;
  }

  private IEnumerator GiveDecorationRoutine()
  {
    NPC_Sozo npcSozo = this;
    npcSozo.StoryProgress = 4;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoReturn);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.GoToAndStop(npcSozo.transform.position + new Vector3(2f, 0.0f), npcSozo.gameObject);
    if (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    npcSozo.convo6.gameObject.SetActive(true);
    npcSozo.convo6.CallOnConversationEnd = false;
    // ISSUE: reference to a compiler-generated method
    npcSozo.convo6.Callback.AddListener(new UnityAction(npcSozo.\u003CGiveDecorationRoutine\u003Eb__40_0));
    npcSozo.convo6.Play();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!DataManager.Instance.SozoBeforeDeath)
      return;
    DataManager.Instance.SozoDead = true;
  }

  private void TestEndQuest()
  {
    this.convo0.gameObject.SetActive(false);
    if (!DataManager.Instance.BuiltMushroomDecoration)
    {
      DataManager.Instance.BuiltMushroomDecoration = true;
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Objectives.CustomQuestTypes.SozoReturn));
    }
    this.StoryProgress = 4;
  }

  private IEnumerator EndQuestRoutine()
  {
    NPC_Sozo npcSozo = this;
    npcSozo.Activated = true;
    npcSozo.StoryProgress = 5;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoReturn);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.GoToAndStop(npcSozo.transform.position + new Vector3(2f, 0.0f), npcSozo.gameObject);
    if (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    npcSozo.convo9.gameObject.SetActive(true);
    npcSozo.convo9.CallOnConversationEnd = false;
    npcSozo.convo9.Play();
    // ISSUE: reference to a compiler-generated method
    npcSozo.convo9.Callback.AddListener(new UnityAction(npcSozo.\u003CEndQuestRoutine\u003Eb__43_0));
  }

  public void RevealShrine()
  {
    this.Shrine.gameObject.SetActive(true);
    Interaction_RatauShrine r = this.Shrine.GetComponentInChildren<Interaction_RatauShrine>();
    Vector3 localPosition = this.Shrine.transform.localPosition;
    this.Shrine.transform.localPosition = localPosition + Vector3.forward;
    this.Shrine.transform.DOLocalMove(localPosition, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      Vector3 localScale = r.transform.localScale;
      r.transform.localScale = Vector3.zero;
      r.transform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
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

  private IEnumerator NeedMoreMushroomsIE()
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

  private IEnumerator SellMushrooms()
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
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.transform.position);
          ResourceCustomTarget.Create(npcSozo.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, (System.Action) null);
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
      ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, npcSozo.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
    Inventory.AddItem(20, count);
  }

  private IEnumerator GiveMushrooms(int requiredAmount)
  {
    NPC_Sozo npcSozo = this;
    if (npcSozo.acceptRoutine != null)
      npcSozo.StopCoroutine(npcSozo.acceptRoutine);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BringSozoMushrooms);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BringSozoMushrooms2);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SozoReturn);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.GoToAndStop(npcSozo.transform.position + new Vector3(2f, 0.0f), npcSozo.gameObject);
    if (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    Inventory.ChangeItemQuantity(29, -requiredAmount);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    GameManager.GetInstance().AddToCamera(npcSozo.gameObject);
    yield return (object) new WaitForSeconds(1f);
    for (int i = 0; i < requiredAmount; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.transform.position);
      ResourceCustomTarget.Create(npcSozo.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, (System.Action) null);
      yield return (object) new WaitForSeconds((float) (0.20000000298023224 - 0.20000000298023224 * (double) (i / requiredAmount)));
    }
    yield return (object) new WaitForSeconds(1f);
    npcSozo.StoryProgress++;
    if (npcSozo.StoryProgress == 2)
    {
      npcSozo.convo2.gameObject.SetActive(true);
      npcSozo.convo2.CallOnConversationEnd = false;
      // ISSUE: reference to a compiler-generated method
      npcSozo.convo2.Callback.AddListener(new UnityAction(npcSozo.\u003CGiveMushrooms\u003Eb__50_0));
      npcSozo.convo2.Play();
    }
    else if (npcSozo.StoryProgress == 3)
    {
      npcSozo.convo3.gameObject.SetActive(true);
      npcSozo.convo3.Play();
    }
  }

  public void Ritual() => this.StartCoroutine((IEnumerator) this.RitualIE());

  private IEnumerator RitualIE()
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
    // ISSUE: reference to a compiler-generated method
    npcSozo.mushrooms.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(npcSozo.\u003CRitualIE\u003Eb__52_0));
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
    // ISSUE: reference to a compiler-generated method
    npcSozo.GiveKeyPiece(new System.Action(npcSozo.\u003CRitualIE\u003Eb__52_1));
  }

  private IEnumerator DelayGiveObjective(Objectives.CustomQuestTypes Quest, int Story)
  {
    this.Activated = true;
    yield return (object) new WaitForSeconds(1.5f);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitSozo", Quest));
    this.StoryProgress = Story;
    this.Start();
    this.Activated = false;
  }

  private IEnumerator GiveShrooms(GameObject follower, float totalTime, float delay)
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

  private IEnumerator GiveKeyPieceRoutine(System.Action Callback)
  {
    NPC_Sozo npcSozo = this;
    yield return (object) null;
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(npcSozo.KeyPiecePrefab, npcSozo.transform.position + Vector3.back * 0.5f, Quaternion.identity, npcSozo.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    KeyPiece.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    KeyPiece.transform.DOMove(PlayerFarming.Instance.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd(false);
    KeyPiece.OnInteract(PlayerFarming.Instance.state);
    KeyPiece.Callback = Callback;
  }
}
