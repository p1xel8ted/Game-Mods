// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetVariable`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
public class SetVariable<T> : ActionTask
{
  [BlackboardOnly]
  public BBParameter<T> valueA;
  public BBParameter<T> valueB;

  public override string info => $"{this.valueA?.ToString()} = {this.valueB?.ToString()}";

  public override void OnExecute()
  {
    this.valueA.value = this.valueB.value;
    this.EndAction();
  }
}
