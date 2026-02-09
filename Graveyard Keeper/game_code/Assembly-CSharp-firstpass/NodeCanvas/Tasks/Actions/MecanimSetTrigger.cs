// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MecanimSetTrigger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Set Parameter Trigger", 0)]
[Category("Animator")]
[Description("You can either use a parameter name OR hashID. Leave the parameter name empty or none to use hashID instead.")]
public class MecanimSetTrigger : ActionTask<Animator>
{
  public BBParameter<string> parameter;
  public BBParameter<int> parameterHashID;

  public override string info
  {
    get
    {
      return $"Mec.SetTrigger {(string.IsNullOrEmpty(this.parameter.value) ? (object) this.parameterHashID.ToString() : (object) this.parameter.ToString())}";
    }
  }

  public override void OnExecute()
  {
    if (!string.IsNullOrEmpty(this.parameter.value))
      this.agent.SetTrigger(this.parameter.value);
    else
      this.agent.SetTrigger(this.parameterHashID.value);
    this.EndAction();
  }
}
