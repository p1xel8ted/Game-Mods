// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FromIListToList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("From IList To List", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
public class Flow_FromIListToList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<IList<string>> in_IList = this.AddValueInput<IList<string>>("IList");
    List<string> _out_list = (List<string>) null;
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<List<string>>("List", (ValueHandler<List<string>>) (() => _out_list));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_IList.value == null)
      {
        Debug.LogError((object) "Flow_FromIListToList error: IList is null!");
        _out_list = new List<string>();
      }
      else
        _out_list = new List<string>((IEnumerable<string>) in_IList.value);
      flow_out.Call(f);
    }));
  }
}
