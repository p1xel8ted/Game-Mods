// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TriggerSinShardCharge
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Trigger Sin Shard Charge", 0)]
[Category("Game Actions")]
[Description("Triggers sin shard animation, duration: 5sec.")]
public class Flow_TriggerSinShardCharge : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<Flow_TriggerSinShardCharge.PersonType> person_type = this.AddValueInput<Flow_TriggerSinShardCharge.PersonType>("Person Type");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player.TriggerSmartAnimation("sin_shard_charge");
      MainGame.me.player.wop.GetComponent<Animator>()?.SetFloat("sin_shard_color", (float) person_type.value + 1f);
      flow_out.Call(f);
    }));
  }

  public enum PersonType
  {
    Astrologer,
    Inquisitor,
    Snake,
    Merchant,
    Actress,
    Bishop,
  }
}
