// Decompiled with JetBrains decompiler
// Type: SmartConditionsList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class SmartConditionsList
{
  public List<SmartCondition> conditions = new List<SmartCondition>();
  public bool _cached;
  public bool _single;
  public SmartCondition _single_cnd;

  public bool CheckCondition()
  {
    if (!this._cached)
    {
      this._cached = true;
      this._single = this.conditions.Count == 1;
      if (this._single)
        this._single_cnd = this.conditions[0];
    }
    if (this._single)
      return this._single_cnd.CheckCondition();
    for (int index = 0; index < this.conditions.Count; ++index)
    {
      if (!this.conditions[index].CheckCondition())
        return false;
    }
    return true;
  }
}
