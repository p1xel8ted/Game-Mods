// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.EvaluateAs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Expressions;

public static class EvaluateAs
{
  public static string EvaluateAsString(this IExpression e, IDictionary<string, object> variables)
  {
    return Convert.ToString(e.Evaluate(variables));
  }

  public static int EvaluateAsInt(this IExpression e, IDictionary<string, object> variables)
  {
    return Convert.ToInt32(e.Evaluate(variables));
  }

  public static bool EvaluateAsBoolean(this IExpression e, IDictionary<string, object> variables)
  {
    return Convert.ToBoolean(e.Evaluate(variables));
  }

  public static float EvaluateAsFloat(this IExpression e, IDictionary<string, object> variables)
  {
    return Convert.ToSingle(e.Evaluate(variables));
  }
}
