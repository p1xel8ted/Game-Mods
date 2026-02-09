// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.NormalizeVector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
public class NormalizeVector : ActionTask
{
  public BBParameter<Vector3> targetVector;
  public BBParameter<float> multiply = (BBParameter<float>) 1f;

  public override void OnExecute()
  {
    this.targetVector.value = this.targetVector.value.normalized * this.multiply.value;
    this.EndAction(true);
  }
}
