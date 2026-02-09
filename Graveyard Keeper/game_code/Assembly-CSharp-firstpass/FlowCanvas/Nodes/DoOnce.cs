// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.DoOnce
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Filters")]
[Description("Filters Out to be called only once. After the first call, Out is no longer called until Reset is called")]
public class DoOnce : FlowControlNode
{
  public bool called;

  public override void OnGraphStarted() => this.called = false;

  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (this.called)
        return;
      this.called = true;
      o.Call(f);
    }));
    this.AddFlowInput("Reset", (FlowHandler) (f => this.called = false));
  }
}
