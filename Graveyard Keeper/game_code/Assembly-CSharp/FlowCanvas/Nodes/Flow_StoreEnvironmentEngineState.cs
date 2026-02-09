// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StoreEnvironmentEngineState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Store Environment Engine State", 0)]
[Category("Game Actions")]
[Description("State can be 'Inside' xor 'Realtime'")]
public class Flow_StoreEnvironmentEngineState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> to_store_flag = this.AddValueInput<bool>("Store state?");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (!to_store_flag.value)
        EnvironmentEngine.me.RestoreEnvironmentEngineState();
      else
        EnvironmentEngine.me.StoreEnvironmentEngineState();
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return !this.GetInputValuePort<bool>("Store state?").value ? "Restore Environment Engine State" : base.name;
    }
    set => base.name = value;
  }
}
