// Decompiled with JetBrains decompiler
// Type: Expressive.Helpers.Comparison
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;

#nullable disable
namespace Expressive.Helpers;

public static class Comparison
{
  public static Type[] CommonTypes = new Type[6]
  {
    typeof (long),
    typeof (double),
    typeof (bool),
    typeof (DateTime),
    typeof (string),
    typeof (Decimal)
  };

  public static int CompareUsingMostPreciseType(object a, object b)
  {
    Type mostPreciseType = Comparison.GetMostPreciseType(a.GetType(), b.GetType());
    return Comparer.Default.Compare(Convert.ChangeType(a, mostPreciseType), Convert.ChangeType(b, mostPreciseType));
  }

  public static Type GetMostPreciseType(Type a, Type b)
  {
    foreach (Type commonType in Comparison.CommonTypes)
    {
      if (Type.op_Equality(a, commonType) || Type.op_Equality(b, commonType))
        return commonType;
    }
    return a;
  }
}
