// Decompiled with JetBrains decompiler
// Type: TweenColor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
  public Color from = Color.white;
  public Color to = Color.white;
  public bool mCached;
  public UIWidget mWidget;
  public Material mMat;
  public Light mLight;
  public SpriteRenderer mSr;

  public void Cache()
  {
    this.mCached = true;
    this.mWidget = this.GetComponent<UIWidget>();
    if ((UnityEngine.Object) this.mWidget != (UnityEngine.Object) null)
      return;
    this.mSr = this.GetComponent<SpriteRenderer>();
    if ((UnityEngine.Object) this.mSr != (UnityEngine.Object) null)
      return;
    Renderer component = this.GetComponent<Renderer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      this.mMat = component.material;
    }
    else
    {
      this.mLight = this.GetComponent<Light>();
      if (!((UnityEngine.Object) this.mLight == (UnityEngine.Object) null))
        return;
      this.mWidget = this.GetComponentInChildren<UIWidget>();
    }
  }

  [Obsolete("Use 'value' instead")]
  public Color color
  {
    get => this.value;
    set => this.value = value;
  }

  public Color value
  {
    get
    {
      if (!this.mCached)
        this.Cache();
      if ((UnityEngine.Object) this.mWidget != (UnityEngine.Object) null)
        return this.mWidget.color;
      if ((UnityEngine.Object) this.mMat != (UnityEngine.Object) null)
        return this.mMat.color;
      if ((UnityEngine.Object) this.mSr != (UnityEngine.Object) null)
        return this.mSr.color;
      return (UnityEngine.Object) this.mLight != (UnityEngine.Object) null ? this.mLight.color : Color.black;
    }
    set
    {
      if (!this.mCached)
        this.Cache();
      if ((UnityEngine.Object) this.mWidget != (UnityEngine.Object) null)
        this.mWidget.color = value;
      else if ((UnityEngine.Object) this.mMat != (UnityEngine.Object) null)
        this.mMat.color = value;
      else if ((UnityEngine.Object) this.mSr != (UnityEngine.Object) null)
      {
        this.mSr.color = value;
      }
      else
      {
        if (!((UnityEngine.Object) this.mLight != (UnityEngine.Object) null))
          return;
        this.mLight.color = value;
        this.mLight.enabled = (double) value.r + (double) value.g + (double) value.b > 0.0099999997764825821;
      }
    }
  }

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = Color.Lerp(this.from, this.to, factor);
  }

  public static TweenColor Begin(GameObject go, float duration, Color color)
  {
    TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
    tweenColor.from = tweenColor.value;
    tweenColor.to = color;
    if ((double) duration <= 0.0)
    {
      tweenColor.Sample(1f, true);
      tweenColor.enabled = false;
    }
    return tweenColor;
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
