// Decompiled with JetBrains decompiler
// Type: Interaction_DepositWitnessEyes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_DepositWitnessEyes : Interaction_SimpleInteraction
{
  public bool forceActiveForTesting;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string HiddenAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BounceAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string WalkAnimation;
  [HideInInspector]
  public bool hidden = true;
  public GameObject HidingProp;
  public float HidingPropWiggleMultiplierRotation = 8f;
  public float HidingPropWiggleMultiplierTime = 0.2f;
  public Tweener wiggleTween;
  public SkeletonAnimation spine;
  public GameObject BeholderEyeGraphic;
  public bool confirmGiveEye;

  public void Awake()
  {
    if (!this.forceActiveForTesting && (!SeasonsManager.Active || DataManager.Instance.DepositedWitnessEyesForRelics >= 2))
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      this.spine.AnimationState.SetAnimation(0, this.HiddenAnimation, true);
  }

  public override void Start()
  {
    base.Start();
    this.UpdateHiddenState();
  }

  public new void Update() => this.UpdateHiddenState();

  public void UpdateHiddenState()
  {
    if (this.hidden && (this.wiggleTween == null || !this.wiggleTween.IsActive()))
    {
      this.StartWiggle();
    }
    else
    {
      if (this.hidden || this.wiggleTween == null || !this.wiggleTween.IsActive())
        return;
      this.StopWiggle();
    }
  }

  public void StartWiggle()
  {
    Tweener wiggleTween = this.wiggleTween;
    if (wiggleTween != null)
      wiggleTween.Kill();
    this.wiggleTween = (Tweener) this.HidingProp.transform.DOLocalRotate(new Vector3(0.0f, UnityEngine.Random.Range(-this.HidingPropWiggleMultiplierRotation, this.HidingPropWiggleMultiplierRotation), 0.0f), UnityEngine.Random.Range(this.HidingPropWiggleMultiplierTime, this.HidingPropWiggleMultiplierTime * 3f)).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InOutSine).SetLoops<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(-1, DG.Tweening.LoopType.Yoyo).OnStepComplete<TweenerCore<Quaternion, Vector3, QuaternionOptions>>((TweenCallback) (() => this.wiggleTween.ChangeEndValue((object) new Vector3(0.0f, UnityEngine.Random.Range(-this.HidingPropWiggleMultiplierRotation, this.HidingPropWiggleMultiplierRotation), 0.0f), UnityEngine.Random.Range(this.HidingPropWiggleMultiplierTime, this.HidingPropWiggleMultiplierTime * 3f), true)));
  }

  public void StopWiggle()
  {
    Tweener wiggleTween = this.wiggleTween;
    if (wiggleTween != null)
      wiggleTween.Kill();
    AudioManager.Instance.PlayOneShot("event:/dlc/env/witnesseyenpc/mushroom_open");
    this.HidingProp.transform.DOLocalRotate(new Vector3(0.0f, -130f, 0.0f), 0.75f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutSine);
    this.spine.AnimationState.SetAnimation(0, this.BounceAnimation, false);
    this.spine.AnimationState.AddAnimation(0, this.IdleAnimation, true, 0.0f);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (DataManager.Instance.DepositedWitnessEyesForRelics == 0 || DataManager.Instance.DepositedWitnessEyesForRelics == 1)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT) == 0)
        this.playerFarming.indicator.PlayShake();
      else
        this.StartCoroutine((IEnumerator) this.OpenIE(state));
    }
    else
      this.StartCoroutine((IEnumerator) this.OpenIE(state));
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (DataManager.Instance.DepositedWitnessEyesForRelics == 0 || DataManager.Instance.DepositedWitnessEyesForRelics == 1)
      this.Label = $"{string.Format(LocalizationManager.GetTranslation("UI/ItemSelector/Context/Give"), (object) InventoryItem.LocalizedName(InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT).Colour(StaticColors.YellowColorHex))} {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT, 1)}";
    else
      this.Label = this.Interactable ? LocalizationManager.GetTranslation("UI/Settings/Controls/Interact") : "";
  }

  public IEnumerator OpenIE(StateMachine state)
  {
    Interaction_DepositWitnessEyes depositWitnessEyes = this;
    PlayerFarming playerFarming = state.GetComponent<PlayerFarming>();
    playerFarming.GoToAndStop(depositWitnessEyes.transform.position + new Vector3(0.5f, -1.5f, 0.0f));
    depositWitnessEyes.hidden = false;
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    state.facingAngle = Utils.GetAngle(state.transform.position, depositWitnessEyes.transform.position);
    GameManager.GetInstance().OnConversationNew();
    depositWitnessEyes.Interactable = false;
    depositWitnessEyes.HasChanged = true;
    if (depositWitnessEyes.forceActiveForTesting)
    {
      DataManager.Instance.DepositedWitnessEyesForRelics = -1;
      depositWitnessEyes.forceActiveForTesting = false;
    }
    string charName = LocalizationManager.GetTranslation("NAMES/WitnessEyeTrader");
    int witnessEyesForRelics = DataManager.Instance.DepositedWitnessEyesForRelics;
    int itemQuantity = Inventory.GetItemQuantity(232);
    if (DataManager.Instance.DepositedWitnessEyesForRelics == -1)
    {
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/1", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
        new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/2", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
        new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/3", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
        new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/4", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName)
      }, (List<MMTools.Response>) null, (System.Action) (() =>
      {
        if (ObjectiveManager.HasObjectiveOfGroupID("Objectives/GroupTitles/RottenWitnessEye"))
          return;
        ObjectiveManager.Add((ObjectivesData) new Objectives_GiveItem("Objectives/GroupTitles/RottenWitnessEye", charName, 2, InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT), isDLCObjective: true);
      })));
      DataManager.Instance.DepositedWitnessEyesForRelics = 0;
    }
    else if (itemQuantity == 0)
    {
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/4", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName)
      }, (List<MMTools.Response>) null, (System.Action) null));
    }
    else
    {
      Debug.Log((object) "Not first encounter and already ready to give, move to next no convo");
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/4", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName)
      }, (List<MMTools.Response>) null, (System.Action) null));
    }
    while (MMConversation.isPlaying)
      yield return (object) null;
    List<RelicType> relics = new List<RelicType>();
    if (DataManager.Instance.DepositedWitnessEyesForRelics == 0 && Inventory.GetItemQuantity(232) > 0)
    {
      GameManager.GetInstance().OnConversationNew();
      yield return (object) depositWitnessEyes.GiveEyeConfirm(depositWitnessEyes.transform.position);
      if (!depositWitnessEyes.confirmGiveEye)
      {
        MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/4", CharacterName: charName)
        }, (List<MMTools.Response>) null, (System.Action) null));
        while (MMConversation.isPlaying)
          yield return (object) null;
        GameManager.GetInstance().OnConversationEnd();
        depositWitnessEyes.Interactable = true;
        depositWitnessEyes.HasChanged = true;
      }
      else
      {
        yield return (object) depositWitnessEyes.GiveEye(state.transform.position + new Vector3(0.0f, -0.5f, -1f), depositWitnessEyes.gameObject.transform.position + new Vector3(0.0f, 0.0f, -0.5f));
        ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT);
        Inventory.ChangeItemQuantity(232, -1);
        MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/GiveFirstRelic/1", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/GiveFirstRelic/2", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/GiveFirstRelic/3", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName)
        }, (List<MMTools.Response>) null, (System.Action) null));
        while (MMConversation.isPlaying)
          yield return (object) null;
        GameManager.GetInstance().OnConversationNew();
        DataManager.UnlockRelic(RelicType.FieryBlood);
        DataManager.Instance.DepositedWitnessEyesForRelics = 1;
        relics.Add(RelicType.FieryBlood);
        UIRelicMenuController menu = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
        menu.Show(relics);
        AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu");
        relics.Remove(RelicType.FieryBlood);
        yield return (object) menu.YieldUntilHidden();
        MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/ByeForNow/1", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/ByeForNow/2", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName)
        }, (List<MMTools.Response>) null, (System.Action) null));
        yield return (object) null;
        while (MMConversation.isPlaying)
          yield return (object) null;
        yield return (object) new WaitForSeconds(1f);
        GameManager.GetInstance().OnConversationEnd();
        depositWitnessEyes.Interactable = true;
        depositWitnessEyes.HasChanged = true;
      }
    }
    else
    {
      depositWitnessEyes.Interactable = true;
      depositWitnessEyes.HasChanged = true;
      if (DataManager.Instance.DepositedWitnessEyesForRelics == 1 && Inventory.GetItemQuantity(232) > 0)
      {
        depositWitnessEyes.Interactable = false;
        depositWitnessEyes.HasChanged = true;
        GameManager.GetInstance().OnConversationNew();
        yield return (object) depositWitnessEyes.GiveEyeConfirm(depositWitnessEyes.transform.position);
        if (!depositWitnessEyes.confirmGiveEye)
        {
          MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
          {
            new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/Intro/4", CharacterName: charName)
          }, (List<MMTools.Response>) null, (System.Action) null));
          while (MMConversation.isPlaying)
            yield return (object) null;
          GameManager.GetInstance().OnConversationEnd();
          depositWitnessEyes.Interactable = true;
          depositWitnessEyes.HasChanged = true;
          yield break;
        }
        yield return (object) depositWitnessEyes.GiveEye(state.transform.position + new Vector3(0.0f, -0.5f, -1f), depositWitnessEyes.gameObject.transform.position + new Vector3(0.0f, 0.0f, -0.5f));
        ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT);
        Inventory.ChangeItemQuantity(232, -1);
        MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/GiveSecondRelic/1", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/GiveSecondRelic/2", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName)
        }, (List<MMTools.Response>) null, (System.Action) null));
        while (MMConversation.isPlaying)
          yield return (object) null;
        GameManager.GetInstance().OnConversationNew();
        DataManager.UnlockRelic(RelicType.IceyBlood);
        DataManager.Instance.DepositedWitnessEyesForRelics = 2;
        relics.Add(RelicType.IceyBlood);
        UIRelicMenuController menu = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
        menu.Show(relics);
        AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu");
        relics.Remove(RelicType.IceyBlood);
        yield return (object) menu.YieldUntilHidden();
        yield return (object) new WaitForSeconds(1f);
        MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/GoodbyeForever/1", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName),
          new ConversationEntry(depositWitnessEyes.gameObject, "Conversation_NPC/WitnessEyeTrader/GoodbyeForever/2", soundPath: "event:/dialogue/followers/talk_short_hate", CharacterName: charName)
        }, (List<MMTools.Response>) null, (System.Action) null));
        yield return (object) null;
        while (MMConversation.isPlaying)
          yield return (object) null;
        depositWitnessEyes.HidingProp.transform.DOLocalRotate(new Vector3(0.0f, -90f, 0.0f), 3f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutSine);
        depositWitnessEyes.HidingProp.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
        yield return (object) new WaitForSeconds(2f);
        GameManager.GetInstance().OnConversationEnd();
        depositWitnessEyes.spine.AnimationState.SetAnimation(0, depositWitnessEyes.WalkAnimation, true);
        AudioManager.Instance.PlayOneShot("event:/Stings/goat_spawn");
        AudioManager.Instance.PlayOneShot("event:/dlc/env/witnesseyenpc/walk_away");
        yield return (object) depositWitnessEyes.transform.DOMove(depositWitnessEyes.transform.position + new Vector3(20f, 0.0f, 0.0f), 7f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
      }
      GameManager.GetInstance().OnConversationEnd();
    }
  }

  public IEnumerator GiveEye(Vector3 startPos, Vector3 endPos)
  {
    yield return (object) new WaitForSeconds(0.75f);
    AudioManager.Instance.PlayOneShot("event:/Stings/goat_spawn");
    this.BeholderEyeGraphic.transform.position = startPos;
    this.BeholderEyeGraphic.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/witnesseyenpc/eye_give");
    bool animationCompleted = false;
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.Append((Tween) this.BeholderEyeGraphic.transform.DOMove(startPos + new Vector3(0.0f, 0.0f, -2f), 0.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
    sequence.AppendInterval(0.3f);
    sequence.Append((Tween) this.BeholderEyeGraphic.transform.DOMove(endPos, 0.8f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad));
    sequence.OnComplete<DG.Tweening.Sequence>((TweenCallback) (() => animationCompleted = true));
    yield return (object) new WaitForSeconds(1.75f);
    this.spine.gameObject.transform.DOPunchScale(Vector3.one * 0.125f, 0.6f, 6, 0.8f).SetEase<Tweener>(Ease.OutQuad);
    while (!animationCompleted)
      yield return (object) null;
    this.BeholderEyeGraphic.SetActive(false);
    yield return (object) new WaitForSeconds(0.75f);
  }

  public IEnumerator GiveEyeConfirm(Vector3 anchorWorldPos)
  {
    Interaction_DepositWitnessEyes depositWitnessEyes = this;
    GameObject withTag = GameObject.FindWithTag("Canvas");
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), (bool) (UnityEngine.Object) withTag ? withTag.transform : depositWitnessEyes.transform) as GameObject;
    if ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
      if ((bool) (UnityEngine.Object) choice)
      {
        choice.Offset = new Vector3(0.0f, 250f);
        choice.Show("UI/Generic/Decline", string.Format(ScriptLocalization.UI_ItemSelector_Context.Give, (object) InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT, 1)), (System.Action) (() =>
        {
          this.confirmGiveEye = false;
          g = (GameObject) null;
        }), (System.Action) (() =>
        {
          this.confirmGiveEye = true;
          g = (GameObject) null;
        }), anchorWorldPos, true);
        while ((UnityEngine.Object) g != (UnityEngine.Object) null)
        {
          choice.UpdatePosition(anchorWorldPos);
          yield return (object) null;
        }
      }
      choice = (ChoiceIndicator) null;
    }
  }

  [CompilerGenerated]
  public void \u003CStartWiggle\u003Eb__14_0()
  {
    this.wiggleTween.ChangeEndValue((object) new Vector3(0.0f, UnityEngine.Random.Range(-this.HidingPropWiggleMultiplierRotation, this.HidingPropWiggleMultiplierRotation), 0.0f), UnityEngine.Random.Range(this.HidingPropWiggleMultiplierTime, this.HidingPropWiggleMultiplierTime * 3f), true);
  }
}
