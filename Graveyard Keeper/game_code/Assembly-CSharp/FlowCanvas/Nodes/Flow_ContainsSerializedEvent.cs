// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ContainsSerializedEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Contains Serialized Event", 0)]
[Category("Game Actions")]
public class Flow_ContainsSerializedEvent : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<string> event_id = this.AddValueInput<string>("Event Id");
    bool contains = false;
    ValueInput<WorldGameObject> wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(wgo);
      if ((Object) worldGameObject != (Object) null)
      {
        contains = worldGameObject.ContainsSerializedEvent(event_id.value);
        if (contains)
          Debug.Log((object) $"Already has {event_id.value} Event");
      }
      flow_out.Call(f);
    }));
    this.AddValueOutput<bool>("Contains", (ValueHandler<bool>) (() => contains));
  }
}
