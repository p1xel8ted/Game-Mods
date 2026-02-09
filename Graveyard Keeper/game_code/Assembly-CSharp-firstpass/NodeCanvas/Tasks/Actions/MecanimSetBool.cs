// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MecanimSetBool
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Animator")]
[Description("You can either use a parameter name OR hashID. Leave the parameter name empty or none to use hashID instead.")]
[Name("Set Parameter Bool", 0)]
public class MecanimSetBool : ActionTask<Animator>
{
  public BBParameter<string> parameter;
  public BBParameter<int> parameterHashID;
  public BBParameter<bool> setTo;

  public override string info
  {
    get
    {
      return $"Mec.SetBool {(string.IsNullOrEmpty(this.parameter.value) ? (object) this.parameterHashID.ToString() : (object) this.parameter.ToString())} to {this.setTo}";
    }
  }

  public override void OnExecute()
  {
    if (!string.IsNullOrEmpty(this.parameter.value))
      this.agent.SetBool(this.parameter.value, this.setTo.value);
    else
      this.agent.SetBool(this.parameterHashID.value, this.setTo.value);
    this.EndAction(true);
  }
}
