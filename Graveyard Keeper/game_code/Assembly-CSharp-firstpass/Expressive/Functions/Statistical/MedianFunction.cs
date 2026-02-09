// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Statistical.MedianFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using LinqTools;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Functions.Statistical;

public class MedianFunction : FunctionBase
{
  public override string Name => "Median";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 1);
    IList<Decimal> source = (IList<Decimal>) new List<Decimal>();
    foreach (IExpression parameter in parameters)
    {
      object obj1 = parameter.Evaluate(this.Variables);
      if (obj1 is IEnumerable enumerable)
      {
        foreach (object obj2 in enumerable)
          source.Add(Convert.ToDecimal(obj2));
      }
      else
        source.Add(Convert.ToDecimal(obj1));
    }
    return (object) this.Median(source.ToArray<Decimal>());
  }

  public Decimal Median(Decimal[] xs)
  {
    List<Decimal> list = ((IEnumerable<Decimal>) xs).OrderBy<Decimal, Decimal>((Func<Decimal, Decimal>) (x => x)).ToList<Decimal>();
    double index = (double) (list.Count - 1) / 2.0;
    return (list[(int) index] + list[(int) (index + 0.5)]) / 2M;
  }
}
