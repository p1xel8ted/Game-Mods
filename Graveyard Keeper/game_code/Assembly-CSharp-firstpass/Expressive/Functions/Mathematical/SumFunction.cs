// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Mathematical.SumFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using Expressive.Helpers;
using System.Collections;

#nullable disable
namespace Expressive.Functions.Mathematical;

public class SumFunction : FunctionBase
{
  public override string Name => "Sum";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 1);
    object obj1 = (object) 0;
    foreach (IExpression parameter in parameters)
    {
      object obj2 = parameter.Evaluate(this.Variables);
      if (obj2 is IEnumerable enumerable)
      {
        object obj3 = (object) 0;
        foreach (object obj4 in enumerable)
          obj3 = Numbers.Add(obj3 ?? (object) 0, obj4 ?? (object) 0);
        obj2 = obj3;
      }
      obj1 = Numbers.Add(obj1 ?? (object) 0, obj2 ?? (object) 0);
    }
    return obj1;
  }
}
