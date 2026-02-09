// Decompiled with JetBrains decompiler
// Type: Enum`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public static class Enum<T>
{
  public static T Parse(string value) => (T) Enum.Parse(typeof (T), value);

  public static IList<T> GetValues()
  {
    IList<T> values = (IList<T>) new List<T>();
    foreach (object obj in Enum.GetValues(typeof (T)))
      values.Add((T) obj);
    return values;
  }
}
