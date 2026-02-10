// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDoctrineMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Lamb.UI;

public class UIDoctrineMenuController : UIMenuBase
{
  [FormerlySerializedAs("_tabNavigator")]
  [Header("Doctrine Menu")]
  [SerializeField]
  public DoctrineTabNavigatorBase tabNavigatorBase;
  [SerializeField]
  public SkeletonGraphic _book;
  [SerializeField]
  public GameObject _leftTab;
  [SerializeField]
  public GameObject _rightTab;
  public EventInstance LoopedSound;

  public override void Awake()
  {
    base.Awake();
    this.tabNavigatorBase.CanvasGroup.alpha = 0.0f;
    this._book.startingAnimation = "open";
    this._leftTab.gameObject.SetActive(false);
    this._rightTab.gameObject.SetActive(false);
  }

  public override IEnumerator DoShowAnimation()
  {
    UIDoctrineMenuController doctrineMenuController = this;
    doctrineMenuController._canvasGroup.interactable = false;
    yield return (object) doctrineMenuController._book.YieldForAnimation("open");
    doctrineMenuController._animator.Play("Show");
    if (!doctrineMenuController.LoopedSound.isValid())
      doctrineMenuController.LoopedSound = UIManager.CreateLoop("event:/player/new_item_pages_loop");
    yield return (object) doctrineMenuController._book.YieldForAnimation("flicking");
    AudioManager.Instance.StopLoop(doctrineMenuController.LoopedSound);
    doctrineMenuController.tabNavigatorBase.CanvasGroup.alpha = 1f;
    doctrineMenuController.tabNavigatorBase.ShowDefault();
    doctrineMenuController._leftTab.gameObject.SetActive(doctrineMenuController.tabNavigatorBase.CanNavigateLeft());
    doctrineMenuController._rightTab.gameObject.SetActive(doctrineMenuController.tabNavigatorBase.CanNavigateRight());
    UIManager.PlayAudio("event:/ui/open_menu");
    yield return (object) doctrineMenuController._book.YieldForAnimation("page_settle");
    doctrineMenuController._book.AnimationState.SetAnimation(0, "openpage", false);
    doctrineMenuController._canvasGroup.interactable = true;
  }

  public override IEnumerator DoHideAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIDoctrineMenuController doctrineMenuController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    UIManager.PlayAudio("event:/player/new_item_book_close");
    doctrineMenuController._animator.Play("Hide");
    doctrineMenuController.tabNavigatorBase.gameObject.SetActive(false);
    doctrineMenuController.tabNavigatorBase.CurrentMenu.Hide(true);
    doctrineMenuController._leftTab.gameObject.SetActive(false);
    doctrineMenuController._rightTab.gameObject.SetActive(false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) doctrineMenuController._book.YieldForAnimation("close");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnHideCompleted()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    Object.Destroy((Object) this.gameObject);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }
}
