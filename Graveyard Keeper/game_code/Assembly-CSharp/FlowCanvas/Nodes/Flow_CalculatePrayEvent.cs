// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CalculatePrayEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Calculate Pray Event", 0)]
public class Flow_CalculatePrayEvent : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_id = this.AddValueInput<string>("custom id");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    int _people = 0;
    int _faith = 0;
    int _faith_bonus = 0;
    float _money = 0.0f;
    bool _success = false;
    this.AddValueOutput<int>("people", (ValueHandler<int>) (() => _people));
    this.AddValueOutput<int>("faith", (ValueHandler<int>) (() => _faith));
    this.AddValueOutput<int>("faith bonus", (ValueHandler<int>) (() => _faith_bonus));
    this.AddValueOutput<float>("money", (ValueHandler<float>) (() => _money));
    this.AddValueOutput<bool>("success", (ValueHandler<bool>) (() => _success));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      PrayLogics.PrayResult pray = PrayLogics.CalculatePray(string.IsNullOrEmpty(par_id.value) ? GUIElements.me.pray_craft.pray_craft.linked_sub_id : par_id.value);
      _people = pray.people;
      _faith = pray.faith;
      _faith_bonus = pray.faith_bonus;
      _money = pray.money + pray.money_bonus;
      _success = pray.success;
      flow_out.Call(f);
    }));
  }
}
