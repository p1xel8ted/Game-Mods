// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ShowIntro
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Show Intro", 0)]
[Category("Game Actions")]
[Description("Shows game's intro playing from the start of game")]
[Color("79aa3d")]
public class Flow_ShowIntro : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> in_no_words = this.AddValueInput<bool>("no words");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_on_finished = this.AddFlowOutput("On finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      Intro.need_show_first_intro = true;
      Intro.ShowIntro((System.Action) (() =>
      {
        Intro.need_show_first_intro = false;
        flow_on_finished.Call(f);
      }), in_no_words.value, false);
      flow_out.Call(f);
    }));
  }
}
