// Decompiled with JetBrains decompiler
// Type: TweenPosition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
  public Vector3 from;
  public Vector3 to;
  [HideInInspector]
  public bool worldSpace;
  public Transform mTrans;
  public UIRect mRect;

  public Transform cachedTransform
  {
    get
    {
      if ((UnityEngine.Object) this.mTrans == (UnityEngine.Object) null)
        this.mTrans = this.transform;
      return this.mTrans;
    }
  }

  [Obsolete("Use 'value' instead")]
  public Vector3 position
  {
    get => this.value;
    set => this.value = value;
  }

  public Vector3 value
  {
    get => !this.worldSpace ? this.cachedTransform.localPosition : this.cachedTransform.position;
    set
    {
      if ((UnityEngine.Object) this.mRect == (UnityEngine.Object) null || !this.mRect.isAnchored || this.worldSpace)
      {
        if (this.worldSpace)
          this.cachedTransform.position = value;
        else
          this.cachedTransform.localPosition = value;
      }
      else
      {
        value -= this.cachedTransform.localPosition;
        NGUIMath.MoveRect(this.mRect, value.x, value.y);
      }
    }
  }

  public void Awake() => this.mRect = this.GetComponent<UIRect>();

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = this.from * (1f - factor) + this.to * factor;
  }

  public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
  {
    TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
    tweenPosition.from = tweenPosition.value;
    tweenPosition.to = pos;
    if ((double) duration <= 0.0)
    {
      tweenPosition.Sample(1f, true);
      tweenPosition.enabled = false;
    }
    return tweenPosition;
  }

  public static TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
  {
    TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
    tweenPosition.worldSpace = worldSpace;
    tweenPosition.from = tweenPosition.value;
    tweenPosition.to = pos;
    if ((double) duration <= 0.0)
    {
      tweenPosition.Sample(1f, true);
      tweenPosition.enabled = false;
    }
    return tweenPosition;
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
