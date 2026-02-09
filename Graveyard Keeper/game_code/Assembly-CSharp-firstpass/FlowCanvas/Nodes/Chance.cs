// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Chance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Filter the Flow based on a chance of 0 to 1 for 0% - 100%")]
[Category("Flow Controllers/Filters")]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (float)})]
public class Chance : FlowControlNode
{
  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    ValueInput<float> c = this.AddValueInput<float>("Percentage");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((double) UnityEngine.Random.Range(0.0f, 1f) > (double) c.value)
        return;
      o.Call(f);
    }));
  }
}
