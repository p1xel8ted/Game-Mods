// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetFloatRandom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Description("Set a blackboard float variable at random between min and max value")]
public class SetFloatRandom : ActionTask
{
  public BBParameter<float> minValue;
  public BBParameter<float> maxValue;
  [BlackboardOnly]
  public BBParameter<float> floatVariable;

  public override string info
  {
    get
    {
      return $"Set {this.floatVariable?.ToString()} Random({this.minValue?.ToString()}, {this.maxValue?.ToString()})";
    }
  }

  public override void OnExecute()
  {
    this.floatVariable.value = Random.Range(this.minValue.value, this.maxValue.value);
    this.EndAction();
  }
}
