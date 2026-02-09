// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.DecomposeVector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Description("Create up to 3 floats from a Vector and save them to blackboard")]
public class DecomposeVector : ActionTask
{
  public BBParameter<Vector3> targetVector;
  [BlackboardOnly]
  public BBParameter<float> x;
  [BlackboardOnly]
  public BBParameter<float> y;
  [BlackboardOnly]
  public BBParameter<float> z;

  public override string info => "Decompose Vector " + this.targetVector?.ToString();

  public override void OnExecute()
  {
    this.x.value = this.targetVector.value.x;
    this.y.value = this.targetVector.value.y;
    this.z.value = this.targetVector.value.z;
    this.EndAction();
  }
}
