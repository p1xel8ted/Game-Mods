// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Macros.MacroInputNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Macros;

[Icon("MacroIn", false, "")]
[Description("Defines the Input ports of the Macro.\nTo quickly create ports, you can also Drag&Drop a connection on top of this node!")]
[Protected]
[DoNotList]
public class MacroInputNode : FlowNode
{
  public override Alignment2x2 iconAlignment => Alignment2x2.Default;

  public Macro macro => (Macro) this.graph;

  public override void RegisterPorts()
  {
    for (int index = 0; index < this.macro.inputDefinitions.Count; ++index)
    {
      DynamicPortDefinition def = this.macro.inputDefinitions[index];
      if (Type.op_Equality(def.type, typeof (Flow)))
        this.macro.entryActionMap[def.ID] = new FlowHandler(this.AddFlowOutput(def.name, def.ID).Call);
      else
        this.AddValueOutput(def.name, def.type, (ValueHandlerObject) (() => this.macro.entryFunctionMap[def.ID]()), def.ID);
    }
  }
}
