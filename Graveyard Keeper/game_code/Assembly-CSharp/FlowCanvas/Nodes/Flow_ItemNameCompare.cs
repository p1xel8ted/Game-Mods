// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ItemNameCompare
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Compare Item name with string", 0)]
public class Flow_ItemNameCompare : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<string> par_param = this.AddValueInput<string>("param");
    FlowOutput flow_same = this.AddFlowOutput("==");
    FlowOutput flow_not_the_same = this.AddFlowOutput("!=");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      Item obj = in_item.value;
      if (obj == null)
      {
        if (par_param.isDefaultValue)
          flow_same.Call(f);
        else
          flow_not_the_same.Call(f);
      }
      else if (obj.id == par_param.value)
        flow_same.Call(f);
      else
        flow_not_the_same.Call(f);
    }));
  }
}
