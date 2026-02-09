// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PlayerEnable
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Player Enable", 0)]
[Color("4155be")]
public class Flow_PlayerEnable : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> par_enable = this.AddValueInput<bool>("Enable player");
    ValueInput<bool> par_cinematic = this.AddValueInput<bool>("Affect cinematic");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GS.SetPlayerEnable(par_enable.value, par_cinematic.value);
      ObjectDefinition currentInteractiveNpc = GUIElements.me.relation.GetCurrentInteractiveNPC();
      if (!par_enable.value)
      {
        if (currentInteractiveNpc != null)
          SmartAudioEngine.me.OnStartNPCInteraction(currentInteractiveNpc);
      }
      else
        SmartAudioEngine.me.OnEndNPCInteraction();
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => !this.GetInputValuePort<bool>("Enable player").value ? "Player Disable" : base.name;
    set => base.name = value;
  }
}
