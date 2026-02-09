// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SwitchTag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (GameObject)})]
[Description("Branch the Flow based on the tag of a GameObject value")]
[Category("Flow Controllers/Switchers")]
public class SwitchTag : FlowControlNode
{
  [SerializeField]
  public string[] _tagNames;

  public override void RegisterPorts()
  {
    ValueInput<GameObject> go = this.AddValueInput<GameObject>("Value");
    List<FlowOutput> outs = new List<FlowOutput>();
    for (int index = 0; index < this._tagNames.Length; ++index)
      outs.Add(this.AddFlowOutput(this._tagNames[index], index.ToString()));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      for (int index = 0; index < this._tagNames.Length; ++index)
      {
        if (this._tagNames[index] == go.value.tag)
          outs[index].Call(f);
      }
    }));
  }
}
