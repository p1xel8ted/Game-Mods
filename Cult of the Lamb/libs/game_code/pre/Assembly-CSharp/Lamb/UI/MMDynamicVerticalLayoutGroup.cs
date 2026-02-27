// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMDynamicVerticalLayoutGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMDynamicVerticalLayoutGroup : MonoBehaviour, ILayoutGroup, ILayoutController
{
  [SerializeField]
  private float _padding;
  private List<RectTransform> _cachedTransforms = new List<RectTransform>();

  private void OnEnable() => this.FlagForUpdate();

  public void SetLayoutHorizontal()
  {
  }

  public void SetLayoutVertical() => this.FlagForUpdate();

  private void OnTransformChildrenChanged() => this.FlagForUpdate();

  private void FlagForUpdate()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.UpdateLayout();
  }

  private void Update()
  {
    Vector2 zero = Vector2.zero;
    for (int index = 0; index < this._cachedTransforms.Count; ++index)
    {
      zero.x = this._cachedTransforms[index].anchoredPosition.x;
      if (index > 0)
      {
        zero.y = this._cachedTransforms[index - 1].anchoredPosition.y;
        zero.y -= this._cachedTransforms[index - 1].sizeDelta.y;
        zero.y -= this._padding;
      }
      this._cachedTransforms[index].anchoredPosition -= (this._cachedTransforms[index].anchoredPosition - zero) / 2f;
    }
  }

  private void UpdateLayout()
  {
    List<RectTransform> rectTransformList = new List<RectTransform>();
    IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        RectTransform current = (RectTransform) enumerator.Current;
        LayoutElement component;
        if (current.gameObject.activeSelf && (!current.TryGetComponent<LayoutElement>(out component) || !component.ignoreLayout))
          rectTransformList.Add(current);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    Vector2 zero = Vector2.zero;
    for (int index = 0; index < rectTransformList.Count; ++index)
    {
      zero.x = rectTransformList[index].anchoredPosition.x;
      if (index > 0)
      {
        zero.y = rectTransformList[index - 1].anchoredPosition.y;
        zero.y -= rectTransformList[index - 1].sizeDelta.y;
        zero.y -= this._padding;
      }
      if (!this._cachedTransforms.Contains(rectTransformList[index]))
      {
        this._cachedTransforms.Add(rectTransformList[index]);
        rectTransformList[index].anchoredPosition = zero;
      }
    }
    this._cachedTransforms = rectTransformList;
  }
}
