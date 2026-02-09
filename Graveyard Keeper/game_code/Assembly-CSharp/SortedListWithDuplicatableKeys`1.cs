// Decompiled with JetBrains decompiler
// Type: SortedListWithDuplicatableKeys`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SortedListWithDuplicatableKeys<T>
{
  public List<int> keys = new List<int>();
  public List<T> values = new List<T>();

  public int Count => this.keys.Count;

  public void Insert(int key, T value)
  {
    if (this.keys.Count == 0)
    {
      this.keys.Add(key);
      this.values.Add(value);
    }
    else
    {
      int index = 0;
      while (index < this.keys.Count && this.keys[index] <= key)
        ++index;
      if (index == this.keys.Count)
      {
        this.keys.Add(key);
        this.values.Add(value);
      }
      else
      {
        this.keys.Insert(index, key);
        this.values.Insert(index, value);
      }
    }
  }
}
