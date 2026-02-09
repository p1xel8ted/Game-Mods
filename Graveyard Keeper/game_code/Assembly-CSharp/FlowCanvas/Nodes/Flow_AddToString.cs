// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddToString
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Add smthng to string")]
[Category("Game Actions")]
[Name("Add to string", 0)]
public class Flow_AddToString : MyFlowNode
{
  public string sum = "";

  public override void RegisterPorts()
  {
    ValueInput<string> in_str_1 = this.AddValueInput<string>("String 1");
    ValueInput<string> in_str_2 = this.AddValueInput<string>("String 2");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<string>("Sum", (ValueHandler<string>) (() => this.sum));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.sum = in_str_1.value + in_str_2.value;
      flow_out.Call(f);
    }));
  }
}
