// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMDynamicVerticalLayoutGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public float _padding;
  public List<RectTransform> _cachedTransforms = new List<RectTransform>();

  public void OnEnable() => this.FlagForUpdate();

  public void SetLayoutHorizontal()
  {
  }

  public void SetLayoutVertical() => this.FlagForUpdate();

  public void OnTransformChildrenChanged() => this.FlagForUpdate();

  public void FlagForUpdate()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.UpdateLayout();
  }

  public void Update()
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

  public void UpdateLayout()
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
