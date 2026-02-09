// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.GetOtherVariable`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Other Of Type", 0)]
[Category("Variables/Blackboard")]
[Description("Use this to get a variable value from blackboards other than the one this flowscript is using")]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (Wild)})]
public class GetOtherVariable<T> : VariableNode
{
  public override string name => "Get Variable";

  public override void RegisterPorts()
  {
    ValueInput<Blackboard> bb = this.AddValueInput<Blackboard>("Blackboard");
    ValueInput<string> varName = this.AddValueInput<string>("Variable");
    this.AddValueOutput<T>("Value", (ValueHandler<T>) (() => bb.value.GetValue<T>(varName.value)));
  }

  public override void SetVariable(object o)
  {
  }
}
