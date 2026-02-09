// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetTimeOfDay
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Get Time Of Day")]
[Category("Game Actions")]
[Name("Get Time Of Day", 0)]
public class Flow_GetTimeOfDay : MyFlowNode
{
  public override void RegisterPorts()
  {
    float time_of_day = 0.0f;
    int day = 0;
    this.AddValueOutput<float>("Time Of Day", (ValueHandler<float>) (() => time_of_day));
    this.AddValueOutput<int>("Day", (ValueHandler<int>) (() => day));
    FlowOutput flow_night = this.AddFlowOutput("Night");
    FlowOutput flow_morning = this.AddFlowOutput("Morning");
    FlowOutput flow_daytime = this.AddFlowOutput("Daytime");
    FlowOutput flow_evening = this.AddFlowOutput("Evening");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      time_of_day = TimeOfDay.me.GetTimeK();
      day = MainGame.me.save.day;
      if ((double) time_of_day < 0.15000000596046448)
        flow_night.Call(f);
      else if ((double) time_of_day < 0.34999999403953552)
        flow_morning.Call(f);
      else if ((double) time_of_day < 0.699999988079071)
        flow_daytime.Call(f);
      else
        flow_evening.Call(f);
    }));
  }
}
