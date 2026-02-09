// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetAnimatorParam
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Animator Param", 0)]
[Category("Game Actions")]
[Color("989BA4")]
[Description("If WGO is null, then self")]
public class Flow_SetAnimatorParam : MyFlowNode
{
  public override void RegisterPorts()
  {
    WorldGameObject out_wgo = (WorldGameObject) null;
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_state_name = this.AddValueInput<string>("Parameter name");
    ValueInput<AnimatorStateOverriderAtom.AnimatorStates> param_type = this.AddValueInput<AnimatorStateOverriderAtom.AnimatorStates>("Param type");
    ValueInput<float> param_value = this.AddValueInput<float>("Value");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => out_wgo));
    FlowOutput flow_output = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      out_wgo = this.WGOParamOrSelf(par_wgo);
      string name = par_state_name.value;
      float a = param_value.value;
      if ((Object) out_wgo != (Object) null)
      {
        foreach (Animator animator in ((IEnumerable<Animator>) out_wgo.GetComponentsInChildren<Animator>(true)).ToList<Animator>())
        {
          foreach (AnimatorControllerParameter parameter in animator.parameters)
          {
            if (parameter.name == name)
            {
              switch (param_type.value)
              {
                case AnimatorStateOverriderAtom.AnimatorStates.FLOAT:
                  animator.SetFloat(name, a);
                  continue;
                case AnimatorStateOverriderAtom.AnimatorStates.INT:
                  animator.SetInteger(name, (int) a);
                  continue;
                case AnimatorStateOverriderAtom.AnimatorStates.BOOL:
                  animator.SetBool(name, !Mathf.Approximately(a, 0.0f));
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      else
        Debug.LogError((object) "Flow_SetAnimatorParam, WGO is null");
      flow_output.Call(f);
    }));
  }
}
