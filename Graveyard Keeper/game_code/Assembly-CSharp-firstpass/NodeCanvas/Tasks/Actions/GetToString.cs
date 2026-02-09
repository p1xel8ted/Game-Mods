// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetToString
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Name("Get Variable To String", 0)]
public class GetToString : ActionTask
{
  [BlackboardOnly]
  public BBParameter<object> variable;
  [BlackboardOnly]
  public BBParameter<string> toString;

  public override string info => $"{this.toString} = {this.variable}.ToString()";

  public override void OnExecute()
  {
    this.toString.value = !this.variable.isNull ? this.variable.value.ToString() : "NULL";
    this.EndAction();
  }
}
