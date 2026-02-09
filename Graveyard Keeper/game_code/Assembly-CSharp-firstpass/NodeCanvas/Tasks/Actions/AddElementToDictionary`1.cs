// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.AddElementToDictionary`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Dictionaries")]
public class AddElementToDictionary<T> : ActionTask
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<Dictionary<string, T>> dictionary;
  public BBParameter<string> key;
  public BBParameter<T> value;

  public override string info => $"{this.dictionary}[{this.key}] = {this.value}";

  public override void OnExecute()
  {
    if (this.dictionary.value == null)
    {
      this.EndAction(false);
    }
    else
    {
      this.dictionary.value[this.key.value] = this.value.value;
      this.EndAction();
    }
  }
}
