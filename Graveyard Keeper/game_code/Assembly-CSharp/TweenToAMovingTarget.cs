// Decompiled with JetBrains decompiler
// Type: TweenToAMovingTarget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

#nullable disable
public class TweenToAMovingTarget : MonoBehaviour
{
  public TweenToAMovingTarget.GetTransformDelegate _get_target_dlg;
  public float _target_check_period;
  public float _duration;
  public System.Action _on_complete;
  public float _t;
  public float _check_t;
  public Transform _target;
  public Vector3 _real_target_pos;
  public Vector3 _cur_target_pos;
  public Vector3 _origin_pos;

  public static void DoTweenToAMovingTarget(
    GameObject obj,
    TweenToAMovingTarget.GetTransformDelegate get_target,
    float target_check_period,
    float duration,
    System.Action on_complete)
  {
    TweenToAMovingTarget tweenToAmovingTarget = obj.AddComponent<TweenToAMovingTarget>();
    tweenToAmovingTarget._target = get_target();
    tweenToAmovingTarget._duration = duration;
    tweenToAmovingTarget._target_check_period = target_check_period;
    tweenToAmovingTarget._get_target_dlg = get_target;
    tweenToAmovingTarget._on_complete = on_complete;
    tweenToAmovingTarget._cur_target_pos = tweenToAmovingTarget._real_target_pos = (UnityEngine.Object) tweenToAmovingTarget._target == (UnityEngine.Object) null ? Vector3.zero : tweenToAmovingTarget._target.transform.position;
    tweenToAmovingTarget._origin_pos = obj.transform.position;
  }

  public void Update()
  {
    float deltaTime = Time.deltaTime;
    this._t += deltaTime;
    this._check_t += deltaTime;
    if ((double) this._check_t > (double) this._target_check_period)
    {
      this._check_t = 0.0f;
      Transform transform = this._get_target_dlg();
      if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
        this._target = transform;
      if ((UnityEngine.Object) this._target != (UnityEngine.Object) null)
        this._real_target_pos = this._target.transform.position;
    }
    this._cur_target_pos += (this._real_target_pos - this._cur_target_pos) * deltaTime;
    if ((double) this._t > (double) this._duration)
    {
      this.transform.position = this._real_target_pos;
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
      this._on_complete.TryInvoke();
    }
    else
      this.transform.position = Vector3.Lerp(this._origin_pos, this._cur_target_pos, EaseManager.Evaluate(Ease.InOutCubic, (EaseFunction) null, this._t, this._duration, 1f, 1f));
  }

  public delegate Transform GetTransformDelegate();
}
