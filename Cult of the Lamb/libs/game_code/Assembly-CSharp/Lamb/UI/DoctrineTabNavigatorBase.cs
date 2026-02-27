// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DoctrineTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class DoctrineTabNavigatorBase : MMTabNavigatorBase<DoctrineBookmark>
{
  [SerializeField]
  public SkeletonGraphic _book;
  [SerializeField]
  public CanvasGroup _canvasgroup;
  [SerializeField]
  public GameObject _leftTab;
  [SerializeField]
  public GameObject _rightTab;
  public EventInstance LoopedSound;

  public CanvasGroup CanvasGroup => this._canvasgroup;

  public override void OnMenuShow()
  {
  }

  public override void OnMenuHide() => AudioManager.Instance.StopLoop(this.LoopedSound);

  public override void PerformTransitionTo(DoctrineBookmark from, DoctrineBookmark to)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.DoTransitionTo((MMTab) from, (MMTab) to));
  }

  public IEnumerator DoTransitionTo(MMTab from, MMTab to)
  {
    DoctrineTabNavigatorBase tabNavigatorBase = this;
    from.Menu.Hide(true);
    tabNavigatorBase._canvasgroup.alpha = 0.0f;
    if (!tabNavigatorBase.LoopedSound.isValid())
      tabNavigatorBase.LoopedSound = UIManager.CreateLoop("event:/player/new_item_pages_loop");
    yield return (object) tabNavigatorBase._book.YieldForAnimation("flicking");
    AudioManager.Instance.StopLoop(tabNavigatorBase.LoopedSound);
    to.Menu.Show(true);
    tabNavigatorBase._canvasgroup.alpha = 1f;
    UIManager.PlayAudio("event:/ui/open_menu");
    tabNavigatorBase._leftTab.SetActive(tabNavigatorBase.CanNavigateLeft());
    tabNavigatorBase._rightTab.SetActive(tabNavigatorBase.CanNavigateRight());
    yield return (object) tabNavigatorBase._book.YieldForAnimation("page_settle");
    tabNavigatorBase._book.AnimationState.SetAnimation(0, "openpage", false);
  }
}
