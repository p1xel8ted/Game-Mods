// Decompiled with JetBrains decompiler
// Type: TweenAlpha
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
  [Range(0.0f, 1f)]
  public float from = 1f;
  [Range(0.0f, 1f)]
  public float to = 1f;
  public bool mCached;
  public UIRect mRect;
  public Material mMat;
  public Light mLight;
  public SpriteRenderer mSr;
  public float mBaseIntensity = 1f;

  [Obsolete("Use 'value' instead")]
  public float alpha
  {
    get => this.value;
    set => this.value = value;
  }

  public void Cache()
  {
    this.mCached = true;
    this.mRect = this.GetComponent<UIRect>();
    this.mSr = this.GetComponent<SpriteRenderer>();
    if (!((UnityEngine.Object) this.mRect == (UnityEngine.Object) null) || !((UnityEngine.Object) this.mSr == (UnityEngine.Object) null))
      return;
    this.mLight = this.GetComponent<Light>();
    if ((UnityEngine.Object) this.mLight == (UnityEngine.Object) null)
    {
      Renderer component = this.GetComponent<Renderer>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.mMat = component.material;
      if (!((UnityEngine.Object) this.mMat == (UnityEngine.Object) null))
        return;
      this.mRect = this.GetComponentInChildren<UIRect>();
    }
    else
      this.mBaseIntensity = this.mLight.intensity;
  }

  public float value
  {
    get
    {
      if (!this.mCached)
        this.Cache();
      if ((UnityEngine.Object) this.mRect != (UnityEngine.Object) null)
        return this.mRect.alpha;
      if ((UnityEngine.Object) this.mSr != (UnityEngine.Object) null)
        return this.mSr.color.a;
      return !((UnityEngine.Object) this.mMat != (UnityEngine.Object) null) ? 1f : this.mMat.color.a;
    }
    set
    {
      if (!this.mCached)
        this.Cache();
      if ((UnityEngine.Object) this.mRect != (UnityEngine.Object) null)
        this.mRect.alpha = value;
      else if ((UnityEngine.Object) this.mSr != (UnityEngine.Object) null)
        this.mSr.color = this.mSr.color with { a = value };
      else if ((UnityEngine.Object) this.mMat != (UnityEngine.Object) null)
      {
        this.mMat.color = this.mMat.color with { a = value };
      }
      else
      {
        if (!((UnityEngine.Object) this.mLight != (UnityEngine.Object) null))
          return;
        this.mLight.intensity = this.mBaseIntensity * value;
      }
    }
  }

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = Mathf.Lerp(this.from, this.to, factor);
  }

  public static TweenAlpha Begin(GameObject go, float duration, float alpha, float delay = 0.0f)
  {
    TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration, delay);
    tweenAlpha.from = tweenAlpha.value;
    tweenAlpha.to = alpha;
    if ((double) duration <= 0.0)
    {
      tweenAlpha.Sample(1f, true);
      tweenAlpha.enabled = false;
    }
    return tweenAlpha;
  }

  public override void SetStartToCurrentValue() => this.from = this.value;

  public override void SetEndToCurrentValue() => this.to = this.value;
}
