// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Mathematical.MaxFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using Expressive.Helpers;
using LinqTools;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Functions.Mathematical;

public class MaxFunction : FunctionBase
{
  public override string Name => "Max";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 1);
    object a = parameters[0].Evaluate(this.Variables);
    if (a is IEnumerable)
      a = this.Max((IEnumerable) a);
    if (a == null)
      return (object) null;
    foreach (IExpression expression in ((IEnumerable<IExpression>) parameters).Skip<IExpression>(1))
    {
      object b = expression.Evaluate(this.Variables);
      if (b is IEnumerable enumerable)
        b = this.Max(enumerable);
      a = Numbers.Max(a, b);
      if (a == null)
        return (object) null;
    }
    return a;
  }

  public object Max(IEnumerable enumerable)
  {
    object a = (object) null;
    foreach (object b in enumerable)
    {
      if (b == null)
        return (object) null;
      a = a != null ? Numbers.Max(a, b) : b;
    }
    return a;
  }
}
