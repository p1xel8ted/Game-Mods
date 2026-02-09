// Decompiled with JetBrains decompiler
// Type: TweenWidth
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Width")]
[RequireComponent(typeof (UIWidget))]
public class TweenWidth : UITweener
{
  public int from = 100;
  public int to = 100;
  public bool updateTable;
  public UIWidget mWidget;
  public UITable mTable;

  public UIWidget cachedWidget
  {
    get
    {
      if ((UnityEngine.Object) this.mWidget == (UnityEngine.Object) null)
        this.mWidget = this.GetComponent<UIWidget>();
      return this.mWidget;
    }
  }

  [Obsolete("Use 'value' instead")]
  public int width
  {
    get => this.value;
    set => this.value = value;
  }

  public int value
  {
    get => this.cachedWidget.width;
    set => this.cachedWidget.width = value;
  }

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = Mathf.RoundToInt((float) ((double) this.from * (1.0 - (double) factor) + (double) this.to * (double) factor));
    if (!this.updateTable)
      return;
    if ((UnityEngine.Object) this.mTable == (UnityEngine.Object) null)
    {
      this.mTable = NGUITools.FindInParents<UITable>(this.gameObject);
      if ((UnityEngine.Object) this.mTable == (UnityEngine.Object) null)
      {
        this.updateTable = false;
        return;
      }
    }
    this.mTable.repositionNow = true;
  }

  public static TweenWidth Begin(UIWidget widget, float duration, int width)
  {
    TweenWidth tweenWidth = UITweener.Begin<TweenWidth>(widget.gameObject, duration);
    tweenWidth.from = widget.width;
    tweenWidth.to = width;
    if ((double) duration <= 0.0)
    {
      tweenWidth.Sample(1f, true);
      tweenWidth.enabled = false;
    }
    return tweenWidth;
  }

  [ContextMenu("Set 'From' to current value")]
  public override void SetStartToCurrentValue() => this.from = this.value;

  [ContextMenu("Set 'To' to current value")]
  public override void SetEndToCurrentValue() => this.to = this.value;

  [ContextMenu("Assume value of 'From'")]
  public void SetCurrentValueToStart() => this.value = this.from;

  [ContextMenu("Assume value of 'To'")]
  public void SetCurrentValueToEnd() => this.value = this.to;
}
