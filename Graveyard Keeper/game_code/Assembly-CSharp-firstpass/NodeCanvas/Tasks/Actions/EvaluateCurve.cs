// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.EvaluateCurve
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
public class EvaluateCurve : ActionTask
{
  [RequiredField]
  public BBParameter<AnimationCurve> curve;
  public BBParameter<float> from;
  public BBParameter<float> to = (BBParameter<float>) 1f;
  public BBParameter<float> time = (BBParameter<float>) 1f;
  [BlackboardOnly]
  public BBParameter<float> saveAs;

  public override void OnUpdate()
  {
    this.saveAs.value = this.curve.value.Evaluate(Mathf.Lerp(this.from.value, this.to.value, this.elapsedTime / this.time.value));
    if ((double) this.elapsedTime <= (double) this.time.value)
      return;
    this.EndAction();
  }
}
