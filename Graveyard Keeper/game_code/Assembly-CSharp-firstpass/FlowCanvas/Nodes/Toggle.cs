// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Toggle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("When In is called, calls On or Off depending on the current toggle state. Whenever Toggle input is called the state changes.")]
[Category("Flow Controllers/Togglers")]
public class Toggle : FlowControlNode
{
  public bool open = true;
  public bool original;

  public override string name => $"{base.name} {(this.open ? "[ON]" : "[OFF]")}";

  public override void OnGraphStarted() => this.original = this.open;

  public override void OnGraphStoped() => this.open = this.original;

  public override void RegisterPorts()
  {
    FlowOutput tOut = this.AddFlowOutput("On");
    FlowOutput fOut = this.AddFlowOutput("Off");
    this.AddFlowInput("In", (FlowHandler) (f => this.Call(this.open ? tOut : fOut, f)));
    this.AddFlowInput(nameof (Toggle), (FlowHandler) (f => this.open = !this.open));
  }
}
