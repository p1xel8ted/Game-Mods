// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ShowSubtitle
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Show Subtitle", 0)]
[Category("Game Actions")]
public class Flow_ShowSubtitle : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_subtitle_text = this.AddValueInput<string>("Subtitle text");
    ValueInput<bool> in_autoclose = this.AddValueInput<bool>("autoclose");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_autoclosed = this.AddFlowOutput("Autoclosed");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(in_subtitle_text.value))
      {
        if (GUIElements.me.illustrations_gui.is_open)
          GUIElements.me.illustrations_gui.Hide();
      }
      else
      {
        if (!GUIElements.me.illustrations_gui.is_open)
          GUIElements.me.illustrations_gui.Open(false);
        float hold_time;
        GUIElements.me.illustrations_gui.SetText(in_subtitle_text.value, out hold_time);
        if (in_autoclose.value)
          GJTimer.AddTimer(hold_time, (GJTimer.VoidDelegate) (() =>
          {
            GUIElements.me.illustrations_gui.Hide();
            GJTimer.AddTimer(0.5f, (GJTimer.VoidDelegate) (() => flow_autoclosed.Call(f)));
          }));
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return string.IsNullOrEmpty(this.GetInputValuePort<string>("Subtitle text").value) ? "Remove subtitle" : "Show subtitle";
    }
  }
}
