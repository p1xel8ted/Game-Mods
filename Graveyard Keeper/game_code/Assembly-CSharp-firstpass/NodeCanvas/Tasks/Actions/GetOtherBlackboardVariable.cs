// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetOtherBlackboardVariable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Use this to get a variable on any blackboard by overriding the agent")]
[Category("✫ Blackboard")]
public class GetOtherBlackboardVariable : ActionTask<Blackboard>
{
  [RequiredField]
  public BBParameter<string> targetVariableName;
  [BlackboardOnly]
  public BBObjectParameter saveAs;

  public override string info => $"{this.saveAs} = {this.targetVariableName}";

  public override void OnExecute()
  {
    Variable variable = this.agent.GetVariable(this.targetVariableName.value, (Type) null);
    if (variable == null)
    {
      this.EndAction(false);
    }
    else
    {
      this.saveAs.value = variable.value;
      this.EndAction(true);
    }
  }
}
