// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.TryCatch
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Similar to Try/Catch/Finally in code")]
public class TryCatch : FlowControlNode
{
  public override void RegisterPorts()
  {
    FlowOutput fTry = this.AddFlowOutput("Try");
    FlowOutput fCatch = this.AddFlowOutput("Catch");
    FlowOutput fFinally = this.AddFlowOutput("Finally");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      try
      {
        fTry.Call(f);
      }
      catch
      {
        fCatch.Call(f);
      }
      finally
      {
        fFinally.Call(f);
      }
    }));
  }
}
