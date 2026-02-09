// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LockCraftList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Lock Craft List", 0)]
[Category("Game Actions")]
public class Flow_LockCraftList : MyFlowNode
{
  public List<string> craft_ids = new List<string>();

  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      foreach (string craftId in this.craft_ids)
        MainGame.me.save.LockCraftForever(craftId);
      flow_out.Call(f);
    }));
  }
}
