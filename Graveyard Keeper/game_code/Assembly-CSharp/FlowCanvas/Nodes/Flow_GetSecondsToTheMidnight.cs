// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetSecondsToTheMidnight
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Get Seconds To The Midnight", 0)]
public class Flow_GetSecondsToTheMidnight : MyFlowNode
{
  public float seconds_value;

  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<float>("Seconds", (ValueHandler<float>) (() => this.seconds_value));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.seconds_value = TimeOfDay.me.GetSecondsToTheMidnight();
      flow_out.Call(f);
    }));
  }
}
