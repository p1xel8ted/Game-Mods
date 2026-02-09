// Decompiled with JetBrains decompiler
// Type: LevelGrades
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class LevelGrades
{
  [SerializeField]
  public List<int> _values = new List<int>();
  public string grade_id;

  public int GetLevel(float value) => this.GetLevel(Mathf.Floor(value));

  public int GetLevel(int value)
  {
    for (int index = 0; index < this._values.Count; ++index)
    {
      if (value < this._values[index])
        return index;
    }
    return this._values.Count - 1;
  }

  public int GetValueOfLevel(int level)
  {
    if (level >= this._values.Count)
      level = this._values.Count - 1;
    return level < 0 ? 0 : this._values[level];
  }

  public void AddLevel(int value) => this._values.Add(value);

  public int GetValueToNextLevel(int value) => this.GetValueOfLevel(this.GetLevel(value)) - value;
}
