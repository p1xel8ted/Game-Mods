// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.MecanimCheckBool
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("Animator")]
[Name("Check Parameter Bool", 0)]
public class MecanimCheckBool : ConditionTask<Animator>
{
  [RequiredField]
  public BBParameter<string> parameter;
  public BBParameter<bool> value;

  public override string info
  {
    get => $"Mec.Bool {this.parameter.ToString()} == {this.value?.ToString()}";
  }

  public override bool OnCheck() => this.agent.GetBool(this.parameter.value) == this.value.value;
}
