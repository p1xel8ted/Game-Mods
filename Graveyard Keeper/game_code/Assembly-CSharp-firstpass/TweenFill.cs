// Decompiled with JetBrains decompiler
// Type: TweenFill
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (UIBasicSprite))]
[AddComponentMenu("NGUI/Tween/Tween Fill")]
public class TweenFill : UITweener
{
  [Range(0.0f, 1f)]
  public float from = 1f;
  [Range(0.0f, 1f)]
  public float to = 1f;
  public bool mCached;
  public UIBasicSprite mSprite;

  public void Cache()
  {
    this.mCached = true;
    this.mSprite = (UIBasicSprite) this.GetComponent<UISprite>();
  }

  public float value
  {
    get
    {
      if (!this.mCached)
        this.Cache();
      return (Object) this.mSprite != (Object) null ? this.mSprite.fillAmount : 0.0f;
    }
    set
    {
      if (!this.mCached)
        this.Cache();
      if (!((Object) this.mSprite != (Object) null))
        return;
      this.mSprite.fillAmount = value;
    }
  }

  public override void OnUpdate(float factor, bool isFinished)
  {
    this.value = Mathf.Lerp(this.from, this.to, factor);
  }

  public static TweenFill Begin(GameObject go, float duration, float fill)
  {
    TweenFill tweenFill = UITweener.Begin<TweenFill>(go, duration);
    tweenFill.from = tweenFill.value;
    tweenFill.to = fill;
    if ((double) duration <= 0.0)
    {
      tweenFill.Sample(1f, true);
      tweenFill.enabled = false;
    }
    return tweenFill;
  }

  public override void SetStartToCurrentValue() => this.from = this.value;

  public override void SetEndToCurrentValue() => this.to = this.value;
}
