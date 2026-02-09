// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetSpawnedRefugeeWGOList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Spawned Refugee WGO List", 0)]
[Category("Game Actions")]
public class Flow_GetSpawnedRefugeeWGOList : MyFlowNode
{
  public List<WorldGameObject> _refugee_list;

  public override void RegisterPorts()
  {
    this.AddValueOutput<List<WorldGameObject>>("WGO List", (ValueHandler<List<WorldGameObject>>) (() => this._refugee_list));
    FlowOutput @out = this.AddFlowOutput("Out");
    this.AddValueOutput<int>("Refugees Count", (ValueHandler<int>) (() => RefugeesCampEngine.instance.GetSpawnedRefugeeList().Count));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this._refugee_list = RefugeesCampEngine.instance.GetSpawnedRefugeeList();
      @out.Call(f);
    }));
  }
}
