// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RunScript
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Run script", 0)]
public class Flow_RunScript : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_script_name = this.AddValueInput<string>("Script name");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_on_finished = this.AddFlowOutput("On finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string uscript_name = in_script_name.value;
      if (!string.IsNullOrEmpty(uscript_name))
      {
        if (uscript_name[0] == ':')
          FlowScriptEngine.SendEvent(uscript_name.Substring(1));
        else
          GS.RunFlowScript(uscript_name, (CustomFlowScript.OnFinishedDelegate) (finished_script => flow_on_finished.Call(f)));
      }
      flow_out.Call(f);
    }));
  }
}
