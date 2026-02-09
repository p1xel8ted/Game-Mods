// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetRandomString
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Get random string")]
[Name("Get random string", 0)]
[Category("Game Actions")]
public class Flow_GetRandomString : MyFlowNode
{
  public List<string> strings = new List<string>();
  public string random_string = string.Empty;

  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("out");
    this.AddValueOutput<string>("random string", (ValueHandler<string>) (() => this.random_string));
    ValueInput<string> in_exeption = this.AddValueInput<string>("exeption");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      bool flag = false;
      int num = -1;
      if (!string.IsNullOrEmpty(in_exeption.value))
      {
        for (int index = 0; index < this.strings.Count; ++index)
        {
          if (this.strings[index] == in_exeption.value)
          {
            flag = true;
            num = index;
            break;
          }
        }
      }
      int index1 = UnityEngine.Random.Range(0, this.strings.Count - (flag ? 1 : 0));
      if (num >= 0 && index1 >= num && index1 < this.strings.Count - 1)
        ++index1;
      this.random_string = this.strings[index1];
      flow_out.Call(f);
    }));
  }
}
