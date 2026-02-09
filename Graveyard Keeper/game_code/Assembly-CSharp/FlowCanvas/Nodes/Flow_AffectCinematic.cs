// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AffectCinematic
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("4155be")]
[Category("Game Actions")]
[Name("Affect Cinematic", 0)]
public class Flow_AffectCinematic : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> par_cinematic = this.AddValueInput<bool>("Enable cinematic?");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GS.AffectCinematic(par_cinematic.value);
      flow_out.Call(f);
    }));
  }
}
