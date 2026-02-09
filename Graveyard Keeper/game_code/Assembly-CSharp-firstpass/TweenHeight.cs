// Decompiled with JetBrains decompiler
// Type: TweenHeight
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Height")]
[RequireComponent(typeof (UIWidget))]
public class TweenHeight : UITweener
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
  public int height
  {
    get => this.value;
    set => this.value = value;
  }

  public int value
  {
    get => this.cachedWidget.height;
    set => this.cachedWidget.height = value;
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

  public static TweenHeight Begin(UIWidget widget, float duration, int height)
  {
    TweenHeight tweenHeight = UITweener.Begin<TweenHeight>(widget.gameObject, duration);
    tweenHeight.from = widget.height;
    tweenHeight.to = height;
    if ((double) duration <= 0.0)
    {
      tweenHeight.Sample(1f, true);
      tweenHeight.enabled = false;
    }
    return tweenHeight;
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
