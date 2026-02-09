// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_UnloadLastSubsceneWithoutFade
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Icon("CubeArrowStraight", false, "")]
[Description("Removes last additive added subscene without fade and camera moving")]
[Category("Game Actions")]
[Name("Just Unload Last Subscene", 0)]
public class Flow_UnloadLastSubsceneWithoutFade : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      SubsceneLoadManager.UnloadLastScene();
      flow_out.Call(f);
    }));
  }
}
