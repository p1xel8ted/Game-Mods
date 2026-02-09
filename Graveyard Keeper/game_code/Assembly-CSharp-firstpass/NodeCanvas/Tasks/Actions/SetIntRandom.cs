// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetIntRandom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Name("Set Integer Random", 0)]
[Description("Set a blackboard integer variable at random between min and max value")]
public class SetIntRandom : ActionTask
{
  public BBParameter<int> minValue;
  public BBParameter<int> maxValue;
  [BlackboardOnly]
  public BBParameter<int> intVariable;

  public override string info
  {
    get
    {
      return $"Set {this.intVariable?.ToString()} Random({this.minValue?.ToString()}, {this.maxValue?.ToString()})";
    }
  }

  public override void OnExecute()
  {
    this.intVariable.value = Random.Range(this.minValue.value, this.maxValue.value + 1);
    this.EndAction();
  }
}
