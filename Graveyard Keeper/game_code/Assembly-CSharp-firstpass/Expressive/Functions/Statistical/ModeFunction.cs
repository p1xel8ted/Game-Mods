// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Statistical.ModeFunction
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

public class ModeFunction : FunctionBase
{
  public override string Name => "Mode";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 1);
    IList<object> source1 = (IList<object>) new List<object>();
    foreach (IExpression parameter in parameters)
    {
      object obj1 = parameter.Evaluate(this.Variables);
      if (obj1 is IEnumerable enumerable)
      {
        foreach (object obj2 in enumerable)
          source1.Add(obj2);
      }
      else
        source1.Add(obj1);
    }
    IEnumerable<IGrouping<object, object>> source2 = source1.GroupBy<object, object>((Func<object, object>) (v => v));
    int maxCount = source2.Max<IGrouping<object, object>>((Func<IGrouping<object, object>, int>) (g => g.Count<object>()));
    return source2.First<IGrouping<object, object>>((Func<IGrouping<object, object>, bool>) (g => g.Count<object>() == maxCount)).Key;
  }
}
