// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetDictionaryElement`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Dictionaries")]
public class GetDictionaryElement<T> : ActionTask
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<Dictionary<string, T>> dictionary;
  public BBParameter<string> key;
  [BlackboardOnly]
  public BBParameter<T> saveAs;

  public override string info => $"{this.saveAs} = {this.dictionary}[{this.key}]";

  public override void OnExecute()
  {
    if (this.dictionary.value == null)
    {
      this.EndAction(false);
    }
    else
    {
      this.saveAs.value = this.dictionary.value[this.key.value];
      this.EndAction();
    }
  }
}
