// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ConditionalUpdateEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Checks the condition boolean input per frame and calls outputs when the value has changed")]
[Category("Events/Other")]
[Name("Conditional Event", 0)]
public class ConditionalUpdateEvent : EventNode, IUpdatable
{
  public FlowOutput becameTrue;
  public FlowOutput becameFalse;
  public ValueInput<bool> condition;
  public bool lastState;

  public override void RegisterPorts()
  {
    this.becameTrue = this.AddFlowOutput("Became True");
    this.becameFalse = this.AddFlowOutput("Became False");
    this.condition = this.AddValueInput<bool>("Condition");
  }

  public void Update()
  {
    if (!this.condition.value)
    {
      if (!this.lastState)
        return;
      this.becameFalse.Call(new Flow());
      this.lastState = false;
    }
    else
    {
      if (this.lastState)
        return;
      this.becameTrue.Call(new Flow());
      this.lastState = true;
    }
  }
}
