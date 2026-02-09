// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AnswersArray
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Answers List", 0)]
[Category("Game Actions")]
public class Flow_AnswersArray : MyFlowNode
{
  public int count;
  public ValueOutput<List<AnswerData>> datasList;
  public List<ValueInput<AnswerData>> ins = new List<ValueInput<AnswerData>>();

  public override void RegisterPorts()
  {
    this.datasList = this.AddValueOutput<List<AnswerData>>("Datas", new ValueHandler<List<AnswerData>>(this.GetData));
    for (int index = 0; index < this.count; ++index)
      this.ins.Add(this.AddValueInput<AnswerData>($"<color=#A08030>#{index.ToString()}</color>"));
  }

  public List<AnswerData> GetData()
  {
    List<AnswerData> data = new List<AnswerData>();
    for (int index = 0; index < this.ins.Count; ++index)
      data.Add(this.ins[index].value);
    return data;
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    if (!GUILayout.Button("Refresh"))
      return;
    this.GatherPorts();
  }
}
