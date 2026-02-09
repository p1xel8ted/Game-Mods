// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetTaskState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("ffd200")]
[ParadoxNotion.Design.Icon("Task", false, "")]
[Category("Game Actions")]
[Name("Set Task", 0)]
public class Flow_SetTaskState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_npc = this.AddValueInput<string>("NPC id (\"\" if self)", "NPC id");
    ValueInput<string> par_task = this.AddValueInput<string>("Task");
    ValueInput<KnownNPC.TaskState.State> par_state = this.AddValueInput<KnownNPC.TaskState.State>("State");
    ValueInput<float> par_story_br = this.AddValueInput<float>("Story brnz");
    ValueInput<float> par_story_silv = this.AddValueInput<float>("Story silv");
    ValueInput<float> par_story_gold = this.AddValueInput<float>("Story gold");
    FlowOutput flow_out = this.AddFlowOutput("Immediate", "Out");
    FlowOutput flow_finished = this.AddFlowOutput("On Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((UnityEngine.Object) this.wgo != (UnityEngine.Object) null && (double) MainGame.me.player.GetParam("p_journalist") > 0.0)
        this.wgo.DropStory(par_story_br.value, par_story_silv.value, par_story_gold.value);
      string objId = par_npc.value;
      if (string.IsNullOrEmpty(objId))
        objId = this.wgo.obj_id;
      MainGame.me.save.SetTaskState(objId, par_task.value, par_state.value, (System.Action) (() => flow_finished.Call(f)));
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return !string.IsNullOrEmpty(this.GetInputValuePort<string>("NPC id").value) ? "Set [npc] Task" : "Set [self] Task";
    }
    set => base.name = value;
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("NPC id");
  }
}
