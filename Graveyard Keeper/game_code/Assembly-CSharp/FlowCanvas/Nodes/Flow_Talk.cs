// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_Talk
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Talk", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("Dialogue", false, "")]
public class Flow_Talk : MyFlowNode
{
  public bool say_as_player;
  public bool override_pos;
  public SmartSpeechEngine.VoiceID force_voice_id;

  public override void RegisterPorts()
  {
    WorldGameObject out_wgo = (WorldGameObject) null;
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_txt = this.AddValueInput<string>("Text");
    ValueInput<string> par_anim = this.AddValueInput<string>("Anim id", "Anim");
    ValueInput<Flow_Talk.AnimPlayOrder> par_anim_t = this.AddValueInput<Flow_Talk.AnimPlayOrder>("Anim play");
    ValueInput<SpeechBubbleGUI.SpeechBubbleType> par_type = this.AddValueInput<SpeechBubbleGUI.SpeechBubbleType>("Bubble", "Type");
    ValueInput<bool> par_player = this.AddValueInput<bool>("Player?", "Player talk");
    ValueInput<Flow_Talk.SpeechBubbleForceOrientation> par_orientation = this.AddValueInput<Flow_Talk.SpeechBubbleForceOrientation>("Orientation", "OrientationType");
    FlowOutput flow_on_finished = this.AddFlowOutput("On Finished");
    FlowOutput flow_out = this.AddFlowOutput("Immediate", "Out");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => out_wgo));
    ValueInput<Transform> par_pos = this.AddValueInput<Transform>("OverrodePosition");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject obj_wgo = par_player.value ? MainGame.me.player : this.WGOParamOrSelf(par_wgo);
      if ((UnityEngine.Object) obj_wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Trying to talk with a null WGO: " + par_txt.value));
        flow_out.Call(f);
      }
      else
      {
        Flow_Talk.SpeechBubbleForceOrientation forceOrientation = par_orientation.value;
        bool? to_left = new bool?();
        if (forceOrientation != Flow_Talk.SpeechBubbleForceOrientation.Auto)
          to_left = new bool?(forceOrientation == Flow_Talk.SpeechBubbleForceOrientation.Left);
        MainGame.me.save.known_npcs.GetOrCreateNPC(obj_wgo.obj_id);
        out_wgo = obj_wgo;
        System.Action on_said = (System.Action) (() => flow_on_finished.Call(f));
        System.Action on_anim_finished = (System.Action) (() =>
        {
          WorldGameObject worldGameObject = obj_wgo;
          string text = par_txt.value;
          SpeechBubbleGUI.SpeechBubbleType speechBubbleType = par_type.value;
          bool? to_left1 = to_left;
          int type = (int) speechBubbleType;
          int forceVoiceId = (int) this.force_voice_id;
          int num = this.say_as_player ? 1 : 0;
          Transform overrode_pos = this.override_pos ? par_pos.value : (Transform) null;
          worldGameObject.Say(text, (GJCommons.VoidDelegate) (() =>
          {
            out_wgo = obj_wgo;
            on_said();
          }), to_left1, (SpeechBubbleGUI.SpeechBubbleType) type, (SmartSpeechEngine.VoiceID) forceVoiceId, num != 0, overrode_pos);
          flow_out.Call(f);
        });
        if (string.IsNullOrEmpty(par_anim.value))
        {
          on_anim_finished();
        }
        else
        {
          string[] strArray = new string[6]
          {
            "Triggering animation ",
            par_anim.value,
            " and waiting. WGO = ",
            obj_wgo.name,
            ", ",
            null
          };
          Flow_Talk.AnimPlayOrder animPlayOrder = par_anim_t.value;
          strArray[5] = animPlayOrder.ToString();
          Debug.Log((object) string.Concat(strArray));
          animPlayOrder = par_anim_t.value;
          switch (animPlayOrder)
          {
            case Flow_Talk.AnimPlayOrder.Before:
              obj_wgo.TriggerSmartAnimation(par_anim.value, on_anim_finished);
              break;
            case Flow_Talk.AnimPlayOrder.Together:
              obj_wgo.TriggerSmartAnimation(par_anim.value);
              on_anim_finished();
              break;
            case Flow_Talk.AnimPlayOrder.After:
              on_said = (System.Action) (() => obj_wgo.TriggerSmartAnimation(par_anim.value, (System.Action) (() => flow_on_finished.Call(f))));
              on_anim_finished();
              break;
          }
        }
      }
    }));
  }

  public override string name
  {
    get
    {
      SpeechBubbleGUI.SpeechBubbleType speechBubbleType = this.GetInputValuePort<SpeechBubbleGUI.SpeechBubbleType>("Type").value;
      if (this.GetInputValuePort<bool>("Player talk").value)
        return $"<color=#FFFF50>Player {speechBubbleType}</color>";
      return !this.GetInputValuePort("WGO").isConnected ? $"<color=#30FF30>Self {speechBubbleType}</color>" : base.name;
    }
    set => base.name = value;
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    if (!GUILayout.Button("Refresh"))
      return;
    this.GatherPorts();
  }

  public enum AnimPlayOrder
  {
    Before,
    Together,
    After,
  }

  public enum SpeechBubbleForceOrientation
  {
    Auto,
    Left,
    Right,
  }
}
