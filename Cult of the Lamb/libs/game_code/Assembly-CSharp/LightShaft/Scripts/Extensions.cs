// Decompiled with JetBrains decompiler
// Type: LightShaft.Scripts.Extensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LightShaft.Scripts;

public static class Extensions
{
  public static T[] Splice<T>(this T[] source, int start)
  {
    return ((IEnumerable<T>) source).ToList<T>().Skip<T>(start).ToList<T>().ToArray<T>();
  }
}
