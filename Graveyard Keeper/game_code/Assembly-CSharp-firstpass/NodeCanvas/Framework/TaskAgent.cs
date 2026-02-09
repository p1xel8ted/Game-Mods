// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.TaskAgent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[Serializable]
public class TaskAgent : BBParameter<UnityEngine.Object>
{
  public new UnityEngine.Object value
  {
    get
    {
      if (!this.useBlackboard)
        return (UnityEngine.Object) (this._value as Component);
      UnityEngine.Object @object = base.value;
      if (@object == (UnityEngine.Object) null)
        return (UnityEngine.Object) null;
      switch (@object)
      {
        case GameObject _:
          return (UnityEngine.Object) (@object as GameObject).transform;
        case Component _:
          return @object;
        default:
          return (UnityEngine.Object) null;
      }
    }
    set => this._value = value;
  }

  public override object objectValue
  {
    get => (object) this.value;
    set => this.value = (UnityEngine.Object) value;
  }
}
