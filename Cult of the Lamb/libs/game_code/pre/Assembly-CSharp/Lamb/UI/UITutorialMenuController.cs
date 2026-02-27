// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UITutorialMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private string kSelectedTutorialAnimationState = "Selected";
  private string kCancelSelectionAnimationState = "Cancelled";
  [SerializeField]
  private TutorialMenuItem[] _menuItems;
  [SerializeField]
  private TutorialInfoCardController _infoCardController;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  private RectTransform _cardContainer;
  [SerializeField]
  private RectTransform _rootContainer;
  [SerializeField]
  private GameObject _previousPagePrompt;
  [SerializeField]
  private GameObject _nextPagePrompt;
  [SerializeField]
  private UINavigatorFollowElement _followElement;

  public override void Awake()
  {
    base.Awake();
    foreach (TutorialMenuItem menuItem in this._menuItems)
    {
      menuItem.gameObject.SetActive(DataManager.Instance.RevealedTutorialTopics.Contains(menuItem.Topic));
      menuItem.OnTopicChosen += new Action<TutorialMenuItem>(this.OnTutorialTopicChosen);
    }
  }

  protected override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    this._previousPagePrompt.SetActive(false);
    this._nextPagePrompt.SetActive(false);
  }

  private void OnTutorialTopicChosen(TutorialMenuItem tutorialMenuItem)
  {
    this._infoCardController.enabled = false;
    this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.SetActiveStateForMenu(false);
    this.StartCoroutine((IEnumerator) this.FocusTutorialCard(tutorialMenuItem));
  }

  private IEnumerator FocusTutorialCard(TutorialMenuItem item)
  {
    UITutorialMenuController tutorialMenuController = this;
    tutorialMenuController._controlPrompts.HideAcceptButton();
    TutorialInfoCard currentCard = tutorialMenuController._infoCardController.CurrentCard;
    currentCard.RectTransform.SetParent((Transform) tutorialMenuController._rootContainer, true);
    currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) tutorialMenuController._animator.YieldForAnimation(tutorialMenuController.kSelectedTutorialAnimationState);
    currentCard.Animator.enabled = false;
    bool inputBuffer = false;
    while (!InputManager.UI.GetCancelButtonDown())
    {
      if ((double) InputManager.UI.GetHorizontalAxis() < -0.20000000298023224)
      {
        if (!inputBuffer)
        {
          inputBuffer = true;
          UIManager.PlayAudio("event:/ui/arrow_change_selection");
          yield return (object) currentCard.PreviousPage();
        }
        yield return (object) null;
      }
      else if ((double) InputManager.UI.GetHorizontalAxis() > 0.20000000298023224)
      {
        if (!inputBuffer)
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

  protected override void OnHideStarted()
  {
    UIManager.PlayAudio("event:/ui/close_menu");
    this._followElement.enabled = false;
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
