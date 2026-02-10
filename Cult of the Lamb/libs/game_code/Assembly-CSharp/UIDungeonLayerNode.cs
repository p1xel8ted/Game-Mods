// Decompiled with JetBrains decompiler
// Type: UIDungeonLayerNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDungeonLayerNode : BaseMonoBehaviour
{
  [SerializeField]
  public Image vistedIcon;
  [SerializeField]
  public Image selectedIcon;
  [SerializeField]
  public Image connectionBar;

  public void SetState(UIDungeonLayerNode.State state)
  {
    this.vistedIcon.gameObject.SetActive(state == UIDungeonLayerNode.State.Visted);
    this.selectedIcon.gameObject.SetActive(state == UIDungeonLayerNode.State.Selected || state == UIDungeonLayerNode.State.Visted);
    switch (state)
    {
      case UIDungeonLayerNode.State.Visted:
        this.vistedIcon.fillAmount = 1f;
        this.selectedIcon.fillAmount = 1f;
        if (!(bool) (Object) this.connectionBar)
          break;
        this.connectionBar.fillAmount = 1f;
        break;
      case UIDungeonLayerNode.State.Selected:
        this.StartCoroutine((IEnumerator) this.SelectedIE());
        break;
    }
  }

  public IEnumerator SelectedIE()
  {
    UIDungeonLayerNode dungeonLayerNode = this;
    dungeonLayerNode.selectedIcon.fillAmount = 0.0f;
    yield return (object) new WaitForSeconds(0.25f);
    if ((bool) (Object) dungeonLayerNode.connectionBar)
    {
      dungeonLayerNode.connectionBar.DOFillAmount(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
      yield return (object) new WaitForSeconds(0.5f);
    }
    dungeonLayerNode.selectedIcon.DOFillAmount(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(0.75f);
    dungeonLayerNode.vistedIcon.gameObject.SetActive(true);
    dungeonLayerNode.vistedIcon.color = Color.white;
    dungeonLayerNode.transform.localScale = Vector3.one * 1.25f;
    dungeonLayerNode.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f, 1);
  }

  public enum State
  {
    None,
    Visted,
    Selected,
  }
}
