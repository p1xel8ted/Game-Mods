// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UITutorialMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UI.InfoCards;
using src.UI.Items;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UITutorialMenuController : UIMenuBase
{
  public string kSelectedTutorialAnimationState = "Selected";
  public string kCancelSelectionAnimationState = "Cancelled";
  [SerializeField]
  public TutorialMenuItem[] _menuItems;
  [SerializeField]
  public TutorialInfoCardController _infoCardController;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public RectTransform _cardContainer;
  [SerializeField]
  public RectTransform _rootContainer;
  [SerializeField]
  public GameObject _previousPagePrompt;
  [SerializeField]
  public GameObject _nextPagePrompt;
  [SerializeField]
  public UINavigatorFollowElement _followElement;
  [SerializeField]
  public GameObject _contentContainer;

  public override void Awake()
  {
    base.Awake();
    foreach (TutorialMenuItem menuItem in this._menuItems)
    {
      menuItem.gameObject.SetActive(DataManager.Instance.RevealedTutorialTopics.Contains(menuItem.Topic));
      menuItem.OnTopicChosen += new Action<TutorialMenuItem>(this.OnTutorialTopicChosen);
    }
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    this._previousPagePrompt.SetActive(false);
    this._nextPagePrompt.SetActive(false);
  }

  public void OnTutorialTopicChosen(TutorialMenuItem tutorialMenuItem)
  {
    this._infoCardController.enabled = false;
    this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.SetActiveStateForMenu(false);
    this.StartCoroutine((IEnumerator) this.FocusTutorialCard(tutorialMenuItem));
  }

  public IEnumerator FocusTutorialCard(TutorialMenuItem item)
  {
    UITutorialMenuController tutorialMenuController = this;
    tutorialMenuController._controlPrompts.HideAcceptButton();
    TutorialInfoCard currentCard = tutorialMenuController._infoCardController.CurrentCard;
    currentCard.RectTransform.DOKill();
    currentCard.RectTransform.SetParent((Transform) tutorialMenuController._rootContainer, true);
    currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) tutorialMenuController._animator.YieldForAnimation(tutorialMenuController.kSelectedTutorialAnimationState);
    currentCard.Animator.enabled = false;
    tutorialMenuController._previousPagePrompt.SetActive(true);
    tutorialMenuController._nextPagePrompt.SetActive(true);
    bool inputBuffer = false;
    while (!InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
    {
      if ((double) InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) < -0.20000000298023224)
      {
        if (!inputBuffer && currentCard.Page > 0)
        {
          inputBuffer = true;
          UIManager.PlayAudio("event:/ui/arrow_change_selection");
          yield return (object) currentCard.PreviousPage();
        }
        yield return (object) null;
      }
      else if ((double) InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) > 0.20000000298023224)
      {
        if (!inputBuffer && currentCard.Page < currentCard.NumPages)
        {
          inputBuffer = true;
          UIManager.PlayAudio("event:/ui/arrow_change_selection");
          yield return (object) currentCard.NextPage();
        }
        yield return (object) null;
      }
      else
      {
        inputBuffer = false;
        yield return (object) null;
      }
    }
    tutorialMenuController._previousPagePrompt.SetActive(false);
    tutorialMenuController._nextPagePrompt.SetActive(false);
    currentCard.RectTransform.DOLocalMove((Vector3) (Vector2) tutorialMenuController._rootContainer.InverseTransformPoint(tutorialMenuController._cardContainer.TransformPoint(Vector3.zero)), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => currentCard.RectTransform.SetParent((Transform) this._cardContainer, true)));
    yield return (object) tutorialMenuController._animator.YieldForAnimation(tutorialMenuController.kCancelSelectionAnimationState);
    currentCard.Animator.enabled = true;
    tutorialMenuController._controlPrompts.ShowAcceptButton();
    tutorialMenuController.SetActiveStateForMenu(true);
    tutorialMenuController._infoCardController.enabled = true;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    UIManager.PlayAudio("event:/ui/close_menu");
    this._followElement.enabled = false;
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
