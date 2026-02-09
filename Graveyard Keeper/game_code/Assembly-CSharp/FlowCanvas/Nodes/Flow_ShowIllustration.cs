// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ShowIllustration
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Show Illustration", 0)]
public class Flow_ShowIllustration : MyFlowNode
{
  public float _hold_time;

  public override void RegisterPorts()
  {
    ValueInput<string> in_illustration = this.AddValueInput<string>("illustration name");
    ValueInput<string> in_text = this.AddValueInput<string>("text");
    this.AddValueOutput<float>("hold time", (ValueHandler<float>) (() => this._hold_time));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.illustrations_gui.ShowIllustration(in_illustration.value);
      GUIElements.me.illustrations_gui.SetText(in_text.value, out this._hold_time);
      flow_out.Call(f);
    }));
  }
}
