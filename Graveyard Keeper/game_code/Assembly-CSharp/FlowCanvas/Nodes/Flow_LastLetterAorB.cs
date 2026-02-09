// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LastLetterAorB
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("A -> inside, B -> outside")]
[Category("Flow Controllers")]
[Name("Last letter A or B", 0)]
public class Flow_LastLetterAorB : MyFlowNode
{
  public bool last_letter_is_A;
  public string changed_str = "";

  public override void RegisterPorts()
  {
    ValueInput<string> in_custom_tag = this.AddValueInput<string>("Custom Tag");
    this.AddValueOutput<bool>("Last Letter Is A Bool", (ValueHandler<bool>) (() => this.last_letter_is_A));
    this.AddValueOutput<string>("Changed out string", (ValueHandler<string>) (() => this.changed_str));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput out_last_letter_is_A = this.AddFlowOutput("Last Letter Is A");
    FlowOutput out_last_letter_is_B = this.AddFlowOutput("Last Letter Is B");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.changed_str = in_custom_tag.value.Trim('_');
      this.last_letter_is_A = this.changed_str[this.changed_str.Length - 1] == 'a' || this.changed_str[this.changed_str.Length - 1] == 'A';
      if (this.last_letter_is_A)
        out_last_letter_is_A.Call(f);
      else
        out_last_letter_is_B.Call(f);
      flow_out.Call(f);
    }));
  }
}
