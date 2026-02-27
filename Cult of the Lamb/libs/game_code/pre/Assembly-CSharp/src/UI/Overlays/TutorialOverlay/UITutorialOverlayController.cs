// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.TutorialOverlay.UITutorialOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.InfoCards;
using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI.Overlays.TutorialOverlay;

public class UITutorialOverlayController : UIMenuBase
{
  [SerializeField]
  private TutorialInfoCard _tutorialInfoCard;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  private TutorialTopic _topic;
  private float _delay;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Show(TutorialTopic topic, float delay = 0.0f, bool instant = false)
  {
    this._topic = topic;
    this._delay = delay;
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    if (!SettingsManager.Settings.Game.ShowTutorials)
    {
      this.Hide(true);
    }
    else
    {
      UIManager.PlayAudio("event:/ui/open_menu");
      UIManager.PlayAudio("event:/Stings/church_bell");
      this._controlPrompts.HideAcceptButton();
      this._controlPrompts.HideCancelButton();
      this._tutorialInfoCard.Show(this._topic, false);
    }
  }

  protected override IEnumerator DoShowAnimation()
  {
    yield return (object) new WaitForSecondsRealtime(this._delay);
    yield return (object) base.DoShowAnimation();
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  protected override void OnShowCompleted() => this.StartCoroutine((IEnumerator) this.RunOverlay());

  private IEnumerator RunOverlay()
  {
    UITutorialOverlayController overlayController = this;
    bool seenAllPages = false;
    bool inputBuffer = false;
    overlayController._tutorialInfoCard.Animator.enabled = false;
    do
    {
      if ((double) InputManager.UI.GetHorizontalAxis() < -0.20000000298023224)
      {
        if (!inputBuffer)
        {
          inputBuffer = true;
          UIManager.PlayAudio("event:/ui/arrow_change_selection");
          yield return (object) overlayController._tutorialInfoCard.PreviousPage();
        }
        yield return (object) null;
      }
      else if ((double) InputManager.UI.GetHorizontalAxis() > 0.20000000298023224)
      {
        if (!inputBuffer)
        {
          inputBuffer = true;
          UIManager.PlayAudio("event:/ui/arrow_change_selection");
          yield return (object) overlayController._tutorialInfoCard.NextPage();
          if (!seenAllPages && overlayController._tutorialInfoCard.Page == overlayController._tutorialInfoCard.NumPages)
          {
            seenAllPages = true;
            overlayController._controlPrompts.ShowAcceptButton();
          }
        }
        yield return (object) null;
      }
      else
      {
        inputBuffer = false;
        yield return (object) null;
      }
    }
    while (!seenAllPages || !InputManager.UI.GetCancelButtonDown() && !InputManager.UI.GetAcceptButtonDown());
    overlayController.Hide();
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  protected override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
