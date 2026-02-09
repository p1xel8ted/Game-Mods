// Decompiled with JetBrains decompiler
// Type: UniversalStorage
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class UniversalStorage
{
  [SerializeField]
  public List<string> _keys = new List<string>();
  [SerializeField]
  public List<object> _vals = new List<object>();

  public T Get<T>(string name)
  {
    int index = this._keys.IndexOf(name);
    return index != -1 ? (T) this._vals[index] : throw new Exception("Variable not found: " + name);
  }

  public void Set<T>(string name, T val)
  {
    int index = this._keys.IndexOf(name);
    if (index == -1)
    {
      this._keys.Add(name);
      this._vals.Add((object) val);
    }
    else
      this._vals[index] = (object) val;
  }
}
