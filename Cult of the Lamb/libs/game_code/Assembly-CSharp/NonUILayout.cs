// Decompiled with JetBrains decompiler
// Type: NonUILayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class NonUILayout : MonoBehaviour
{
  [SerializeField]
  public float elementSize = 2f;
  [SerializeField]
  public bool clearCache;
  [SerializeField]
  public bool isVertical;
  [SerializeField]
  public NonUILayout.Alignment alignment = NonUILayout.Alignment.Center;
  [SerializeField]
  public bool _animated;
  public int ActiveChildrenCount;
  public HashSet<NonUILayoutElement> elements = new HashSet<NonUILayoutElement>();
  public NonUILayoutElement thisElement;
  public bool firstcall = true;

  public void Start()
  {
    this.thisElement = this.GetComponent<NonUILayoutElement>();
    this.RefreshElements();
  }

  public void AdjustPositions()
  {
    this.ActiveChildrenCount = this.elements.Where<NonUILayoutElement>((Func<NonUILayoutElement, bool>) (e => e.gameObject.activeSelf && e.enabled && !e.IgnoreLayout)).Count<NonUILayoutElement>();
    if ((UnityEngine.Object) this.thisElement != (UnityEngine.Object) null && this.ActiveChildrenCount == 0)
    {
      this.thisElement.IgnoreLayout = true;
      this.thisElement.ParentLayout.RefreshElements();
    }
    else if ((UnityEngine.Object) this.thisElement != (UnityEngine.Object) null && this.thisElement.IgnoreLayout && this.ActiveChildrenCount > 0)
    {
      this.thisElement.IgnoreLayout = false;
      this.thisElement.ParentLayout.RefreshElements();
    }
    int num1 = 0;
    bool flag = this.ActiveChildrenCount % 2 != 0;
    Vector3 endValue;
    foreach (NonUILayoutElement element in this.elements)
    {
      if (element.gameObject.activeSelf && element.enabled && !element.IgnoreLayout)
      {
        switch (this.alignment)
        {
          case NonUILayout.Alignment.Beginning:
            int num2 = num1;
            endValue = !this.isVertical ? new Vector3(this.elementSize * (float) num2, 0.0f, 0.0f) : new Vector3(0.0f, -this.elementSize * (float) num2, 0.0f);
            if (!this.firstcall && this._animated && element.transform.localPosition != endValue)
            {
              element.transform.DOKill();
              element.transform.DOLocalMove(endValue, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
            }
            else
              element.transform.localPosition = endValue;
            ++num1;
            continue;
          case NonUILayout.Alignment.Center:
            int num3 = num1 - this.ActiveChildrenCount / 2;
            if (num3 < 0 && !flag)
              ++num3;
            float num4 = flag ? 0.0f : (num1 - this.ActiveChildrenCount / 2 >= 0 ? 0.5f : -0.5f);
            endValue = !this.isVertical ? new Vector3(this.elementSize * ((float) num3 + num4), 0.0f, 0.0f) : new Vector3(0.0f, (float) (-(double) this.elementSize * ((double) num3 + (double) num4)), 0.0f);
            if (!this.firstcall && this._animated && element.transform.localPosition != endValue)
            {
              element.transform.DOKill();
              element.transform.DOLocalMove(endValue, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
            }
            else
              element.transform.localPosition = endValue;
            ++num1;
            continue;
          case NonUILayout.Alignment.End:
            int num5 = num1;
            endValue = !this.isVertical ? new Vector3(-this.elementSize * (float) (this.ActiveChildrenCount - 1 - num5), 0.0f, 0.0f) : new Vector3(0.0f, this.elementSize * (float) (this.ActiveChildrenCount - 1 - num5), 0.0f);
            if (!this.firstcall && this._animated && element.transform.localPosition != endValue)
            {
              element.transform.DOKill();
              element.transform.DOLocalMove(endValue, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
            }
            else
              element.transform.localPosition = endValue;
            ++num1;
            continue;
          default:
            continue;
        }
      }
    }
    this.firstcall = false;
  }

  public void RefreshElements()
  {
    this.elements.Clear();
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      NonUILayoutElement component;
      this.transform.GetChild(index).TryGetComponent<NonUILayoutElement>(out component);
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.ParentLayout = this;
        this.elements.Add(component);
      }
    }
    this.AdjustPositions();
  }

  public void OnElementEnabled(NonUILayoutElement element)
  {
    if (!this.elements.Contains(element))
      return;
    this.AdjustPositions();
  }

  public void OnElementDisabled(NonUILayoutElement element)
  {
    if (!this.elements.Contains(element))
      return;
    this.AdjustPositions();
  }

  public enum Alignment
  {
    Beginning,
    Center,
    End,
  }
}
