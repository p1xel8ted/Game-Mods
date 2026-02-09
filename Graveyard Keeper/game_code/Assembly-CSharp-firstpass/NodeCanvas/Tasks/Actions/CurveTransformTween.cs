// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.CurveTransformTween
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Movement/Direct")]
[Name("Curve Tween", 0)]
public class CurveTransformTween : ActionTask<Transform>
{
  public CurveTransformTween.TransformMode transformMode;
  public CurveTransformTween.TweenMode mode;
  public CurveTransformTween.PlayMode playMode;
  public BBParameter<Vector3> targetPosition;
  public BBParameter<AnimationCurve> curve = (BBParameter<AnimationCurve>) AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  public BBParameter<float> time = (BBParameter<float>) 0.5f;
  public Vector3 original;
  public Vector3 final;
  public bool ponging;

  public override void OnExecute()
  {
    if (this.ponging)
      this.final = this.original;
    if (this.transformMode == CurveTransformTween.TransformMode.Position)
      this.original = this.agent.localPosition;
    if (this.transformMode == CurveTransformTween.TransformMode.Rotation)
      this.original = this.agent.localEulerAngles;
    if (this.transformMode == CurveTransformTween.TransformMode.Scale)
      this.original = this.agent.localScale;
    if (!this.ponging)
      this.final = this.targetPosition.value + (this.mode == CurveTransformTween.TweenMode.Additive ? this.original : Vector3.zero);
    this.ponging = this.playMode == CurveTransformTween.PlayMode.PingPong;
    if ((double) (this.original - this.final).magnitude >= 0.10000000149011612)
      return;
    this.EndAction();
  }

  public override void OnUpdate()
  {
    Vector3 vector3 = Vector3.Lerp(this.original, this.final, this.curve.value.Evaluate(this.elapsedTime / this.time.value));
    if (this.transformMode == CurveTransformTween.TransformMode.Position)
      this.agent.localPosition = vector3;
    if (this.transformMode == CurveTransformTween.TransformMode.Rotation)
      this.agent.localEulerAngles = vector3;
    if (this.transformMode == CurveTransformTween.TransformMode.Scale)
      this.agent.localScale = vector3;
    if ((double) this.elapsedTime < (double) this.time.value)
      return;
    this.EndAction(true);
  }

  public enum TransformMode
  {
    Position,
    Rotation,
    Scale,
  }

  public enum TweenMode
  {
    Absolute,
    Additive,
  }

  public enum PlayMode
  {
    Normal,
    PingPong,
  }
}
