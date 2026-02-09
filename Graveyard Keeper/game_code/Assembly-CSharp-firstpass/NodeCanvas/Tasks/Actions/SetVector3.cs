// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetVector3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Description("Set a blackboard Vector3 variable")]
public class SetVector3 : ActionTask
{
  [BlackboardOnly]
  public BBParameter<Vector3> valueA;
  public OperationMethod operation;
  public BBParameter<Vector3> valueB;
  public bool perSecond;

  public override string info
  {
    get
    {
      return $"{this.valueA} {OperationTools.GetOperationString(this.operation)} {this.valueB}{(this.perSecond ? (object) " Per Second" : (object) "")}";
    }
  }

  public override void OnExecute()
  {
    this.valueA.value = OperationTools.Operate(this.valueA.value, this.valueB.value, this.operation, this.perSecond ? Time.deltaTime : 1f);
    this.EndAction();
  }
}
