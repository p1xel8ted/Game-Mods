// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AlwaysEnableChunk
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Always Enable Chunk On", 0)]
public class Flow_AlwaysEnableChunk : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<bool> in_alw_enable = this.AddValueInput<bool>("Enable?");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      try
      {
        worldGameObject.GetComponent<ChunkedGameObject>().always_active = in_alw_enable.value;
      }
      catch
      {
        Debug.LogError((object) "Error in Flow_AlwayEnableChunk");
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => !this.GetInputValuePort<bool>("Enable?").value ? "Always Enable Chunk Off" : base.name;
    set => base.name = value;
  }
}
