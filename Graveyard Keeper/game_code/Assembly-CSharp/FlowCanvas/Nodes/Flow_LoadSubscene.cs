// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LoadSubscene
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Load Subscene", 0)]
[Category("Game Actions")]
[Description("Load scene additive to the current scene")]
[Icon("CubePlus", false, "")]
public class Flow_LoadSubscene : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_custom_tag = this.AddValueInput<string>("Scene name");
    FlowOutput flow_out_on_scene_loaded = this.AddFlowOutput("On loaded");
    FlowOutput flow_out_on_unfaded = this.AddFlowOutput("On unfaded");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.ChangeBubblesVisibility(false);
      CameraTools.Fade((GJCommons.VoidDelegate) (() => SubsceneLoadManager.Load(par_custom_tag.value, (System.Action) (() =>
      {
        flow_out_on_scene_loaded.Call(f);
        SubsceneLoadManager.CameraFlyToLastScene((GJCommons.VoidDelegate) (() => CameraTools.UnFade((GJCommons.VoidDelegate) (() =>
        {
          GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
          flow_out_on_unfaded.Call(f);
        }), new float?(2f))));
      }))), new float?(2f));
    }));
  }
}
