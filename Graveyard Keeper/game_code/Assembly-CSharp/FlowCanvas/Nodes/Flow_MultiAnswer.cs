// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_MultiAnswer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("00FF00")]
[Icon("Dialogue", false, "")]
[Name("Multi answer", 0)]
[Category("Game Actions")]
public class Flow_MultiAnswer : MyFlowNode
{
  public List<string> answers = new List<string>();

  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    List<FlowOutput> outs = new List<FlowOutput>();
    List<ValueInput<AnswerData>> ins = new List<ValueInput<AnswerData>>();
    int num = -1;
    this.SyncLists();
    foreach (string answer in this.answers)
    {
      ++num;
      string name = answer;
      StringBuilder stringBuilder = new StringBuilder("□□□");
      if (false)
        name = $"<color=#4040FF>{stringBuilder?.ToString()}</color> {name}";
      string ID = "out_" + num.ToString();
      outs.Add(this.AddFlowOutput(name, ID));
      ins.Add(this.AddValueInput<AnswerData>($"<color=#A08030>#{num.ToString()}</color>"));
    }
    ValueInput<WorldGameObject> par_wgo_talker = this.AddValueInput<WorldGameObject>("Talker");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      List<AnswerVisualData> answers = new List<AnswerVisualData>();
      this.SyncLists();
      for (int index = 0; index < this.answers.Count; ++index)
      {
        MultipleAnswerVisualData vis_data1 = new MultipleAnswerVisualData()
        {
          id = this.answers[index]
        };
        if (ins[index].value is MultipleAnswerData)
        {
          MultipleAnswerData multipleAnswerData = (MultipleAnswerData) ins[index].value;
          if (multipleAnswerData != null)
          {
            vis_data1.answer_visual_datas = new List<AnswerVisualData>();
            multipleAnswerData.FillVisualData(ref vis_data1, this.wgo);
          }
          answers.Add((AnswerVisualData) vis_data1);
        }
        else
        {
          AnswerVisualData vis_data2 = new AnswerVisualData()
          {
            id = this.answers[index]
          };
          ins[index].value?.FillVisualData(ref vis_data2, this.wgo);
          answers.Add(vis_data2);
        }
      }
      WorldGameObject talker = par_wgo_talker.HasValue<WorldGameObject>() ? par_wgo_talker.value : MainGame.me.player;
      MainGame.me.player.ShowMultianswer(answers, (MultiAnswerGUI.MultiAnswerResult) (chosen => outs[this.answers.IndexOf(chosen)].Call(f)), talker: talker);
      flow_out.Call(f);
    }));
  }

  public bool SyncLists() => false;

  public override string name
  {
    get
    {
      foreach (Port outputPort in this.GetOutputPorts())
      {
        if (!(outputPort.name == "Out") && !outputPort.isConnected)
          return base.name + "\n<color=#FF2020>!!!EMPTY OUT!!!</color>";
      }
      return base.name;
    }
    set => base.name = value;
  }
}
