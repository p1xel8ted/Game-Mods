// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Return
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Should always be used to return out of a Custom Function. The return value is only required if the Custom Function returns a value as well.")]
[Color("d86b13")]
[Category("Functions/Custom")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (object)})]
public class Return : FlowControlNode
{
  public override void RegisterPorts()
  {
    ValueInput<object> returnPort = this.AddValueInput<object>("Value");
    this.AddFlowInput(" ", (FlowHandler) (f => f.Return(returnPort.value)));
  }
}
