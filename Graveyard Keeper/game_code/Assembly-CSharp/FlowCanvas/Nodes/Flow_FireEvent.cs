// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FireEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If WGO is null, then self")]
[Category("Game Actions")]
[Name("Fire Event", 0)]
public class Flow_FireEvent : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_event = this.AddValueInput<string>("event");
    ValueInput<float> par_delay = this.AddValueInput<float>("delay (s)");
    WorldGameObject _wgo = (WorldGameObject) null;
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => _wgo));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject o = this.WGOParamOrSelf(par_wgo);
      _wgo = o;
      if ((Object) o == (Object) null)
      {
        Debug.LogError((object) "WGO GoTo error: WGO #1 is null");
        flow_out.Call(f);
      }
      else
      {
        float delay = par_delay.value;
        if (!string.IsNullOrEmpty(par_event.value))
          GJTimer.AddTimer(0.03f, (GJTimer.VoidDelegate) (() =>
          {
            _wgo = o;
            o.FireEvent(par_event.value, delay);
          }));
        flow_out.Call(f);
      }
    }));
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("event");
  }
}
