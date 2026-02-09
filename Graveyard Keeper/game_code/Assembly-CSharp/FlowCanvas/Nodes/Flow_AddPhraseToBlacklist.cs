// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddPhraseToBlacklist
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If Character is null, then Player")]
[Category("Game Actions")]
[Name("Add Phrase To Blacklist", 0)]
public class Flow_AddPhraseToBlacklist : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_phrase = this.AddValueInput<string>("Phrase ID");
    ValueInput<bool> in_remove = this.AddValueInput<bool>("remove");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (!string.IsNullOrEmpty(in_phrase.value))
      {
        if (in_remove.value)
          MainGame.me.save.black_list_of_phrases.Remove(in_phrase.value);
        else
          MainGame.me.save.AddPhraseToBlackList(in_phrase.value);
      }
      else
        Debug.LogError((object) "Phrase ID is null or empty!");
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("remove").value ? "<color=#FFFF50>Remove Phrase From Blacklist</color>" : "<color=#30FF30>Add Phrase To Blacklist</color>";
    }
    set => base.name = value;
  }
}
