// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PlayMusic
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Play Music", 0)]
[Category("Game Actions")]
[Description("Play Music by name")]
public class Flow_PlayMusic : MyFlowNode
{
  public override void RegisterPorts()
  {
    this.AddValueInput<string>("sound");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      Debug.LogError((object) "Use of a depricated 'PlayMusic' Flow block");
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return !this.GetInputValuePort("sound").isConnected && this.GetInputValuePort("sound").isDefaultValue ? "Stop Music" : base.name;
    }
    set => base.name = value;
  }
}
