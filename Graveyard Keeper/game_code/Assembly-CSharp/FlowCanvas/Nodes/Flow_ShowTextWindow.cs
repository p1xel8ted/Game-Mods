// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ShowTextWindow
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Icon("TextWindow", false, "")]
[Name("Show Text Window", 0)]
public class Flow_ShowTextWindow : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> txt = this.AddValueInput<string>("Text");
    FlowOutput o = this.AddFlowOutput("Out");
    ValueInput<bool> is_cinematic = this.AddValueInput<bool>("Cinematic");
    FlowOutput on_closed = this.AddFlowOutput("On Closed");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (is_cinematic.value)
        GUIElements.me.cinematic_text.Open(txt.value, (GJCommons.VoidDelegate) (() => on_closed.Call(f)));
      else
        GUIElements.me.text_window.Open(txt.value, (GJCommons.VoidDelegate) (() => on_closed.Call(f)));
      o.Call(f);
    }));
  }
}
