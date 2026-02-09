// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Macros.MacroOutputNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Macros;

[Protected]
[Description("Defines the Output ports of the Macro.\nTo quickly create ports, you can also Drag&Drop a connection on top of this node!")]
[Icon("MacroOut", false, "")]
[DoNotList]
public class MacroOutputNode : FlowNode
{
  public override Alignment2x2 iconAlignment => Alignment2x2.Default;

  public Macro macro => (Macro) this.graph;

  public override void RegisterPorts()
  {
    for (int index = 0; index < this.macro.outputDefinitions.Count; ++index)
    {
      DynamicPortDefinition def = this.macro.outputDefinitions[index];
      if (Type.op_Equality(def.type, typeof (Flow)))
        this.AddFlowInput(def.name, (FlowHandler) (f => this.macro.exitActionMap[def.ID](f)), def.ID);
      else
        this.macro.exitFunctionMap[def.ID] = new ValueHandlerObject(this.AddValueInput(def.name, def.type, def.ID).GetValue);
    }
  }
}
