// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetBooleanRandom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Description("Set a blackboard boolean variable at random between min and max value")]
public class SetBooleanRandom : ActionTask
{
  [BlackboardOnly]
  public BBParameter<bool> boolVariable;

  public override string info => $"Set {this.boolVariable?.ToString()} Random";

  public override void OnExecute()
  {
    this.boolVariable.value = Random.Range(0, 2) != 0;
    this.EndAction();
  }
}
