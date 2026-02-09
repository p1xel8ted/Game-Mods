// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_UnloopParticlAndDeleteWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Unloop Particle And Delete WGO", 0)]
public class Flow_UnloopParticlAndDeleteWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<float> in_time = this.AddValueInput<float>("time");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_wgo.value != (Object) null)
      {
        ParticlesMinIntensityAndDelete componentInChildren = in_wgo.value.GetComponentInChildren<ParticlesMinIntensityAndDelete>();
        if ((Object) componentInChildren != (Object) null)
          componentInChildren.DoDelete(in_time.value);
      }
      flow_out.Call(f);
    }));
  }
}
