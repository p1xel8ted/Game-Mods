// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CameraFade
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Camera Fade", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
public class Flow_CameraFade : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<float> par_time = this.AddValueInput<float>("Time");
    ValueInput<bool> par_immediate = this.AddValueInput<bool>("immediate");
    ValueInput<Color> par_color = this.AddValueInput<Color>("Color fade");
    ValueInput<Flow_CameraFade.FadeType> par_fade_type = this.AddValueInput<Flow_CameraFade.FadeType>("Fade type");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_middle = this.AddFlowOutput("Middle");
    FlowOutput flow_finished = this.AddFlowOutput("On Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      float time = par_time.value;
      GUIElements.ChangeBubblesVisibility(false);
      float num = par_immediate.value ? 0.0f : time;
      Color fade_color = par_color.isDefaultValue ? Color.black : par_color.value;
      switch (par_fade_type.value)
      {
        case Flow_CameraFade.FadeType.InOut:
          CameraTools.Fade((GJCommons.VoidDelegate) (() =>
          {
            flow_middle.Call(f);
            CameraTools.UnFade((GJCommons.VoidDelegate) (() =>
            {
              GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
              flow_finished.Call(f);
            }), new float?(time), new Color?(fade_color));
          }), new float?(num), new Color?(fade_color));
          break;
        case Flow_CameraFade.FadeType.In:
          CameraTools.Fade((GJCommons.VoidDelegate) (() => flow_finished.Call(f)), new float?(num), new Color?(fade_color));
          GJTimer.AddTimer(num / 2f, (GJTimer.VoidDelegate) (() => flow_middle.Call(f)));
          break;
        case Flow_CameraFade.FadeType.Out:
          CameraTools.UnFade((GJCommons.VoidDelegate) (() =>
          {
            GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
            flow_finished.Call(f);
          }), new float?(num), new Color?(fade_color));
          GJTimer.AddTimer(num / 2f, (GJTimer.VoidDelegate) (() => flow_middle.Call(f)));
          break;
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      Flow_CameraFade.FadeType fadeType = this.GetInputValuePort<Flow_CameraFade.FadeType>("Fade type").value;
      switch (fadeType)
      {
        case Flow_CameraFade.FadeType.In:
          return $"<color=#0D3C1A>Camera Fade {fadeType}</color>";
        case Flow_CameraFade.FadeType.Out:
          return $"<color=#700B25>Camera Fade {fadeType}</color>";
        default:
          return base.name;
      }
    }
    set => base.name = value;
  }

  public enum FadeType
  {
    InOut,
    In,
    Out,
  }
}
