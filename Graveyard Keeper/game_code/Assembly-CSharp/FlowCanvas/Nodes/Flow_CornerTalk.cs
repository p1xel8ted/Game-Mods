// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CornerTalk
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Corner Talk", 0)]
[Category("Game Actions")]
[Icon("Dialogue", false, "")]
public class Flow_CornerTalk : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_txt = this.AddValueInput<string>("Text");
    ValueInput<string> par_spr = this.AddValueInput<string>("Sprite");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_on_finished = this.AddFlowOutput("On Finished");
    ValueInput<SmartSpeechEngine.VoiceID> par_voice = this.AddValueInput<SmartSpeechEngine.VoiceID>("Voice");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.corner_talk.Say(par_txt.value, (GJCommons.VoidDelegate) (() => flow_on_finished.Call(f)), par_spr.value, par_voice.value);
      flow_out.Call(f);
    }));
  }
}
