// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Break
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Iterators")]
[Description("Can be used within a For Loop or For Each node, to Break the iteration.")]
public class Break : FlowControlNode
{
  public override void RegisterPorts()
  {
    this.AddFlowInput(nameof (Break), (FlowHandler) (f =>
    {
      if (f.Break == null)
        return;
      f.Break();
    }));
  }
}
