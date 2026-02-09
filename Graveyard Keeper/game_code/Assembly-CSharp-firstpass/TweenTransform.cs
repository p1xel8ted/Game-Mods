// Decompiled with JetBrains decompiler
// Type: TweenTransform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener
{
  public Transform from;
  public Transform to;
  public bool parentWhenFinished;
  public Transform mTrans;
  public Vector3 mPos;
  public Quaternion mRot;
  public Vector3 mScale;

  public override void OnUpdate(float factor, bool isFinished)
  {
    if (!((Object) this.to != (Object) null))
      return;
    if ((Object) this.mTrans == (Object) null)
    {
      this.mTrans = this.transform;
      this.mPos = this.mTrans.position;
      this.mRot = this.mTrans.rotation;
      this.mScale = this.mTrans.localScale;
    }
    if ((Object) this.from != (Object) null)
    {
      this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
      this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
      this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
    }
    else
    {
      this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
      this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
      this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
    }
    if (!(this.parentWhenFinished & isFinished))
      return;
    this.mTrans.parent = this.to;
  }

  public static TweenTransform Begin(GameObject go, float duration, Transform to)
  {
    return TweenTransform.Begin(go, duration, (Transform) null, to);
  }

  public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
  {
    TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
    tweenTransform.from = from;
    tweenTransform.to = to;
    if ((double) duration <= 0.0)
    {
      tweenTransform.Sample(1f, true);
      tweenTransform.enabled = false;
    }
    return tweenTransform;
  }
}
