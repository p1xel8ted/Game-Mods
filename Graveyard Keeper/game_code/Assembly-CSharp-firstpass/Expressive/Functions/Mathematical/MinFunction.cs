// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Mathematical.MinFunction
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

public class MinFunction : FunctionBase
{
  public override string Name => "Min";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 1);
    object a = parameters[0].Evaluate(this.Variables);
    if (a is IEnumerable)
      a = this.Min((IEnumerable) a);
    if (a == null)
      return (object) null;
    foreach (IExpression expression in ((IEnumerable<IExpression>) parameters).Skip<IExpression>(1))
    {
      object b = expression.Evaluate(this.Variables);
      if (b is IEnumerable enumerable)
        b = this.Min(enumerable);
      a = Numbers.Min(a, b);
      if (a == null)
        return (object) null;
    }
    return a;
  }

  public object Min(IEnumerable enumerable)
  {
    object a = (object) null;
    foreach (object b in enumerable)
    {
      if (b == null)
        return (object) null;
      a = a != null ? Numbers.Min(a, b) : b;
    }
    return a;
  }
}
