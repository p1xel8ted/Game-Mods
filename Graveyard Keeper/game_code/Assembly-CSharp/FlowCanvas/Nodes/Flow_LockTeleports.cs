// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LockTeleports
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Lock teleports")]
[Category("Game Actions")]
[Name("Lock teleports", 0)]
public class Flow_LockTeleports : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> par_do_lock = this.AddValueInput<bool>("Lock");
    ValueInput<int> par_lock_param = this.AddValueInput<int>("Lock Param");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player.data.SetParam("lock_tp", par_do_lock.value ? 1f : 0.0f);
      MainGame.me.player.data.SetParam("lock_tp_param", (float) par_lock_param.value);
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("Lock").value ? "<color=#FFFF50>Lock Teleports</color>" : "<color=#30FF30>Unlock Teleports</color>";
    }
    set => base.name = value;
  }
}
