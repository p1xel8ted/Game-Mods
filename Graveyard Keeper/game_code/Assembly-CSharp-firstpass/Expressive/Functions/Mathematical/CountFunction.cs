// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Mathematical.CountFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Functions.Mathematical;

public class CountFunction : FunctionBase
{
  public override string Name => "Count";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 1);
    int num1 = 0;
    foreach (IExpression parameter in parameters)
    {
      int num2 = 1;
      IDictionary<string, object> variables = this.Variables;
      if (parameter.Evaluate(variables) is IEnumerable enumerable)
      {
        int num3 = 0;
        foreach (object obj in enumerable)
          ++num3;
        num2 = num3;
      }
      num1 += num2;
    }
    return (object) num1;
  }
}
