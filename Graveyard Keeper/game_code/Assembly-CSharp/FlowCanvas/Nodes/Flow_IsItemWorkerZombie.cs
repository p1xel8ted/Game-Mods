// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsItemWorkerZombie
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Returns bool")]
[Name("Is Item The Working Zombie", 0)]
public class Flow_IsItemWorkerZombie : MyFlowNode
{
  public override void RegisterPorts()
  {
    bool is_worker = false;
    this.AddValueOutput<bool>("Is worker", (ValueHandler<bool>) (() => is_worker));
    ValueInput<Item> item = this.AddValueInput<Item>("Item");
    FlowOutput flow_yes = this.AddFlowOutput("True");
    FlowOutput flow_no = this.AddFlowOutput("False");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (item.value == null || item.value.IsEmpty())
      {
        Debug.LogError((object) "The item is null or empty!(Flow_IsItemWorkerZombie)");
        flow_no.Call(f);
      }
      else
      {
        is_worker = item.value.is_worker;
        if (is_worker)
          flow_yes.Call(f);
        else
          flow_no.Call(f);
      }
    }));
  }
}
