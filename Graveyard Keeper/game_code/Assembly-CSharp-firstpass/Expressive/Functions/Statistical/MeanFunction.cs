// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Statistical.MeanFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using Expressive.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Functions.Statistical;

public class MeanFunction : FunctionBase
{
  public override string Name => "Mean";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 1);
    int num1 = 0;
    object a1 = (object) 0;
    foreach (IExpression parameter in parameters)
    {
      int num2 = 1;
      IDictionary<string, object> variables = this.Variables;
      object b1 = parameter.Evaluate(variables);
      if (b1 is IEnumerable enumerable)
      {
        int num3 = 0;
        object a2 = (object) 0;
        foreach (object b2 in enumerable)
        {
          ++num3;
          a2 = Numbers.Add(a2, b2);
        }
        num2 = num3;
        b1 = a2;
      }
      a1 = Numbers.Add(a1, b1);
      num1 += num2;
    }
    return (object) (Convert.ToDouble(a1) / (double) num1);
  }
}
