// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SampleCurve
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
public class SampleCurve : ActionTask
{
  [RequiredField]
  public BBParameter<AnimationCurve> curve;
  [SliderField(0, 1)]
  public BBParameter<float> sampleAt;
  [BlackboardOnly]
  public BBParameter<float> saveAs;

  public override void OnExecute()
  {
    this.saveAs.value = this.curve.value.Evaluate(this.sampleAt.value);
    this.EndAction();
  }
}
