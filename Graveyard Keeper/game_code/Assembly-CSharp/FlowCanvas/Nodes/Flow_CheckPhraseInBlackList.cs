// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CheckPhraseInBlackList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Check Phrase In BlackList", 0)]
[Category("Game Actions")]
public class Flow_CheckPhraseInBlackList : MyFlowNode
{
  public override void RegisterPorts()
  {
    bool phrase_contains = false;
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<string> phrase = this.AddValueInput<string>("Phrase");
    this.AddValueOutput<bool>("Contains", (ValueHandler<bool>) (() => phrase_contains));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string str = phrase.value;
      phrase_contains = MainGame.me.save.black_list_of_phrases.Contains(str);
      flow_out.Call(f);
    }));
  }
}
