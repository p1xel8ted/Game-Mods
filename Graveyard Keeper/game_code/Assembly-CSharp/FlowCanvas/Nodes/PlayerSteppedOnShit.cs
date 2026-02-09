// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PlayerSteppedOnShit
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Player stepped on shit")]
[Name("Player on shit", 0)]
public class PlayerSteppedOnShit : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<float> in_speed = this.AddValueInput<float>("speed");
    ValueInput<float> in_acceleration = this.AddValueInput<float>("acceleration");
    ValueInput<float> in_friction = this.AddValueInput<float>("friction");
    ValueInput<float> in_dec_speed = this.AddValueInput<float>("decrease speed");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player.components.character.SetMovementModifiers(new MovementComponent.Modifier(in_acceleration.value - 0.98f, in_dec_speed.value), new MovementComponent.Modifier(0.38f + in_friction.value, in_dec_speed.value), new MovementComponent.Modifier(2f, in_dec_speed.value), new MovementComponent.Modifier(in_speed.value, in_dec_speed.value));
      MainGame.me.player_component.ForceTrailChange(Ground.GroudType.Shit);
      flow_out.Call(f);
    }));
  }
}
