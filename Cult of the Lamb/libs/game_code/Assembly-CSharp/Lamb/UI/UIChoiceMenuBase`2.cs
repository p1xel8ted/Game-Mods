// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIChoiceMenuBase`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using Spine.Unity;
using src.UINavigator;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIChoiceMenuBase<T, U> : UIMenuBase where T : UIChoiceInfoCard<U>
{
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public T _infoBox1;
  [SerializeField]
  public T _infoBox2;
  [SerializeField]
  public SkeletonGraphic _crownSpine;
  [SerializeField]
  public RectTransform _crownEye;
  [SerializeField]
  public RectTransform _eyePosLeft;
  [SerializeField]
  public RectTransform _eyePosRight;
  [SerializeField]
  public RectTransform _eyePosDown;
  [SerializeField]
  public RectTransform _crownMovePos;
  [SerializeField]
  public CanvasGroup _bwBackground;

  public void Start()
  {
    this._bwBackground.alpha = 0.0f;
    RectTransform rectTransform = this._crownSpine.rectTransform;
    rectTransform.position = rectTransform.position - new Vector3(0.0f, -1000f);
    this._crownSpine.AnimationState.SetAnimation(0, Interaction_TempleAltar.Instance.SermonCategory == SermonCategory.Pleasure ? "snake-loop" : "animation", true);
    this._controlPrompts.HideCancelButton();
    this.Configure();
    this._infoBox1.Button.OnSelected += new System.Action(this.OnInfoBoxLeftSelected);
    this._infoBox1.Button.onClick.AddListener(new UnityAction(this.OnLeftChoice));
    this._infoBox2.Button.OnSelected += new System.Action(this.OnInfoBoxRightSelected);
    this._infoBox2.Button.onClick.AddListener(new UnityAction(this.OnRightChoice));
  }

  public abstract void Configure();

  public override void OnShowStarted() => this.OverrideDefault((Selectable) this._infoBox1.Button);

  public override IEnumerator DoShowAnimation()
  {
    UIChoiceMenuBase<T, U> uiChoiceMenuBase = this;
    uiChoiceMenuBase._bwBackground.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count == 1)
    {
      uiChoiceMenuBase._infoBox1.ShowSelection(false);
      uiChoiceMenuBase._infoBox1.RectTransform.DOLocalMove((Vector3) new Vector2(0.0f, 0.0f), 0.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      uiChoiceMenuBase._crownSpine.gameObject.SetActive(false);
    }
    else
    {
      uiChoiceMenuBase._controlPrompts.ShowAcceptButton();
      uiChoiceMenuBase._infoBox1.BringOnscreen();
      uiChoiceMenuBase._infoBox2.BringOnscreen();
      uiChoiceMenuBase._crownSpine.rectTransform.DOLocalMoveY(0.0f, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCirc).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    yield return (object) new WaitForSecondsRealtime(0.8f);
    UIManager.PlayAudio("event:/sermon/sermon_menu_appear");
    uiChoiceMenuBase._infoBox1.Button.enabled = true;
    uiChoiceMenuBase._infoBox1.Button.interactable = true;
    uiChoiceMenuBase._infoBox2.Button.enabled = true;
    uiChoiceMenuBase._infoBox2.Button.interactable = true;
    if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count == 1)
    {
      uiChoiceMenuBase.OnInfoBoxLeftSelected();
      uiChoiceMenuBase.OnLeftChoice();
    }
    else
      uiChoiceMenuBase.ActivateNavigation();
    yield return (object) new WaitForSecondsRealtime(1f);
  }

  public virtual void OnInfoBoxLeftSelected()
  {
    UIManager.PlayAudio("event:/sermon/scroll_sermon_menu");
    this._crownEye.DOKill();
    this._crownEye.DOLocalMove(this._eyePosLeft.localPosition, 0.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public virtual void OnInfoBoxRightSelected()
  {
    UIManager.PlayAudio("event:/sermon/scroll_sermon_menu");
    this._crownEye.DOKill();
    this._crownEye.DOLocalMove(this._eyePosRight.localPosition, 0.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public virtual void OnLeftChoice()
  {
    this.StartCoroutine((IEnumerator) this.DoHoldToUnlock(this._infoBox1));
  }

  public virtual void OnRightChoice()
  {
    this.StartCoroutine((IEnumerator) this.DoHoldToUnlock(this._infoBox2));
  }

  public T OppositeChoice(T choice)
  {
    return !((UnityEngine.Object) choice == (UnityEngine.Object) this._infoBox1) ? this._infoBox1 : this._infoBox2;
  }

  public IEnumerator DoHoldToUnlock(T choice)
  {
    UIChoiceMenuBase<T, U> uiChoiceMenuBase = this;
    uiChoiceMenuBase._controlPrompts.HideAcceptButton();
    T obj = uiChoiceMenuBase.OppositeChoice(choice);
    uiChoiceMenuBase._crownSpine.rectTransform.DOKill();
    uiChoiceMenuBase._crownSpine.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
    uiChoiceMenuBase._crownSpine.rectTransform.DOLocalMoveY(Interaction_TempleAltar.Instance.SermonCategory == SermonCategory.Pleasure ? 590f : uiChoiceMenuBase._crownMovePos.localPosition.y, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCirc).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    UIManager.PlayAudio("event:/sermon/sermon_menu_appear");
    UIManager.PlayAudio("event:/sermon/select_upgrade");
    choice.Button.enabled = false;
    choice.Button.interactable = false;
    obj.Button.enabled = false;
    obj.Button.interactable = false;
    choice.ResetTweens();
    obj.ResetTweens();
    choice.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    obj.SendOffscreen();
    choice.ShowSelection(false);
    uiChoiceMenuBase._crownEye.DOKill();
    uiChoiceMenuBase._crownEye.DOLocalMove(uiChoiceMenuBase._eyePosDown.localPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if ((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
      MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(false);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1)
      yield return (object) new WaitForSecondsRealtime(1f);
    choice.RectTransform.DOScale(Vector3.one * 1.1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    if (MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1)
      uiChoiceMenuBase._controlPrompts.ShowCancelButton();
    bool confirmed = true;
    yield return (object) choice.PerformHoldAction((System.Action) (() =>
    {
      confirmed = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E4__this.StartCoroutine((IEnumerator) this.\u003C\u003E4__this.DoCancelChoice(choice));
    }));
    if (confirmed)
    {
      UIManager.PlayAudio("event:/upgrade_statue/upgrade_unlock");
      UIManager.PlayAudio("event:/sermon/select_upgrade");
      uiChoiceMenuBase._crownEye.DOLocalMove(Vector3.zero, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      choice.CardContainer.localPosition = Vector3.zero;
      choice.RectTransform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      choice.WhiteFlash.color = Color.white;
      DOTweenModuleUI.DOColor(choice.WhiteFlash, new Color(1f, 1f, 1f, 0.0f), 0.3f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      if ((UnityEngine.Object) CameraManager.instance != (UnityEngine.Object) null)
        CameraManager.instance.ShakeCameraForDuration(1.2f, 1.5f, 0.3f);
      if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null)
        GameManager.GetInstance().HitStop();
      yield return (object) new WaitForSecondsRealtime(0.4f);
      uiChoiceMenuBase._crownSpine.transform.DOMove(uiChoiceMenuBase._crownSpine.rectTransform.position - new Vector3(0.0f, -800f), 1.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      uiChoiceMenuBase._bwBackground.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      choice.WhiteFlash.color = Color.white;
      yield return (object) new WaitForSecondsRealtime(0.25f);
      DOTweenModuleUI.DOColor(choice.WhiteFlash, new Color(1f, 1f, 1f, 0.0f), 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      choice.RectTransform.DOShakePosition(0.5f, new Vector3(10f, 10f, 0.0f)).SetUpdate<Tweener>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      choice.RectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      uiChoiceMenuBase.MadeChoice((UIChoiceInfoCard<U>) choice);
      yield return (object) null;
      uiChoiceMenuBase.Hide(true);
    }
  }

  public abstract void MadeChoice(UIChoiceInfoCard<U> infoCard);

  public IEnumerator DoCancelChoice(T choice)
  {
    UIChoiceMenuBase<T, U> uiChoiceMenuBase = this;
    uiChoiceMenuBase._controlPrompts.HideCancelButton();
    T otherChoice = uiChoiceMenuBase.OppositeChoice(choice);
    uiChoiceMenuBase.OverrideDefault((Selectable) choice.Button);
    UIManager.PlayAudio("event:/ui/go_back");
    choice.CardContainer.localPosition = Vector3.zero;
    choice.RectTransform.DOKill();
    choice.RectTransform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    uiChoiceMenuBase._crownEye.DOKill();
    uiChoiceMenuBase._crownEye.DOLocalMove(Vector3.zero, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.6f);
    choice.ShowSelection(true);
    choice.BringOnscreen();
    yield return (object) new WaitForSecondsRealtime(0.2f);
    otherChoice.BringOnscreen();
    uiChoiceMenuBase.StartCoroutine((IEnumerator) uiChoiceMenuBase.DoShowAnimation());
    yield return (object) new WaitForSecondsRealtime(0.8f);
    if ((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null && MMConversation.CURRENT_CONVERSATION.DoctrineResponses.Count > 1 && MMConversation.CURRENT_CONVERSATION.DoctrineResponses[0].RewardLevel < 5)
      MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(true);
  }
}
