// Decompiled with JetBrains decompiler
// Type: TweenScale
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
  public Vector3 from = Vector3.one;
  public Vector3 to = Vector3.one;
  public bool updateTable;
  public Transform mTrans;
  public UITable mTable;

  public Transform cachedTransform
  {
    get
    {
      if ((UnityEngine.Object) this.mTrans == (UnityEngine.Object) null)
        this.mTrans = this.transform;
      return this.mTrans;
    }
  }

  public Vector3 value
  {
    get => this.cachedTransform.localScale;
    set => this.cachedTransform.localScale = value;
  }

  [Obsolete("Use 'value' instead")]
  public Vector3 scale
  {
    get => this.value;
    set => this.value = value;
  }

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = this.from * (1f - factor) + this.to * factor;
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

  public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
  {
    TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
    tweenScale.from = tweenScale.value;
    tweenScale.to = scale;
    if ((double) duration <= 0.0)
    {
      tweenScale.Sample(1f, true);
      tweenScale.enabled = false;
    }
    return tweenScale;
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
