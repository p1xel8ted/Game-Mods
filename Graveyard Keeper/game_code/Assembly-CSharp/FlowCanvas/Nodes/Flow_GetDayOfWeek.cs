// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetDayOfWeek
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Day of Week")]
[Name("Day of Week", 0)]
public class Flow_GetDayOfWeek : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_error = this.AddFlowOutput("Error");
    FlowOutput flow_Sloth = this.AddFlowOutput("Sloth - Astrologer");
    FlowOutput flow_Wrath = this.AddFlowOutput("Wrath - Inquisitor");
    FlowOutput flow_Envy = this.AddFlowOutput("Envy - Cultist");
    FlowOutput flow_Gluttony = this.AddFlowOutput("Gluttony - Merchant");
    FlowOutput flow_Lust = this.AddFlowOutput("Lust - Actress");
    FlowOutput flow_Pride = this.AddFlowOutput("Pride - Bishop");
    Sins.SinType out_sin = Sins.SinType.Greed;
    this.AddValueOutput<Sins.SinType>("sin type", (ValueHandler<Sins.SinType>) (() => out_sin));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      out_sin = (Sins.SinType) ((12 - MainGame.me.save.day_of_week) % 6 + 1);
      switch (out_sin)
      {
        case Sins.SinType.Greed:
          flow_error.Call(f);
          break;
        case Sins.SinType.Sloth:
          flow_Sloth.Call(f);
          break;
        case Sins.SinType.Wrath:
          flow_Wrath.Call(f);
          break;
        case Sins.SinType.Envy:
          flow_Envy.Call(f);
          break;
        case Sins.SinType.Gluttony:
          flow_Gluttony.Call(f);
          break;
        case Sins.SinType.Lust:
          flow_Lust.Call(f);
          break;
        case Sins.SinType.Pride:
          flow_Pride.Call(f);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }));
  }
}
