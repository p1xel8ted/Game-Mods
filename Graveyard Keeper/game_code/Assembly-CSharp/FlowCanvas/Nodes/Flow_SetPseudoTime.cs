// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetPseudoTime
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Set Pseudo Time", 0)]
[Description("Set Pseudo Time")]
public class Flow_SetPseudoTime : MyFlowNode
{
  public static float remembered_time = -1f;

  public override void RegisterPorts()
  {
    ValueInput<float> in_time = this.AddValueInput<float>("Time");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((double) in_time.value < -1.0)
      {
        TimeOfDay.me.time_of_day = Flow_SetPseudoTime.remembered_time;
        Flow_SetPseudoTime.remembered_time = -1f;
      }
      else
      {
        if ((double) Flow_SetPseudoTime.remembered_time == -1.0)
          Flow_SetPseudoTime.remembered_time = TimeOfDay.me.time_of_day;
        TimeOfDay.me.time_of_day = in_time.value;
      }
      TimeOfDay.me.Update();
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return (double) this.GetInputValuePort<float>("Time").value < -1.0 ? "Restore Pseudo Time" : base.name;
    }
    set => base.name = value;
  }
}
