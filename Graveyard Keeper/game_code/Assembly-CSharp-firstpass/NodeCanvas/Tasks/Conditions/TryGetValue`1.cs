// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.TryGetValue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard/Dictionaries")]
public class TryGetValue<T> : ConditionTask
{
  [BlackboardOnly]
  [RequiredField]
  public BBParameter<Dictionary<string, T>> targetDictionary;
  [RequiredField]
  public BBParameter<string> key;
  [BlackboardOnly]
  public BBParameter<T> saveValueAs;

  public override string info
  {
    get => $"{this.targetDictionary}.TryGetValue({this.key} as {this.saveValueAs})";
  }

  public override bool OnCheck()
  {
    T obj;
    if (this.targetDictionary.value == null || !this.targetDictionary.value.TryGetValue(this.key.value, out obj))
      return false;
    this.saveValueAs.value = obj;
    return true;
  }
}
