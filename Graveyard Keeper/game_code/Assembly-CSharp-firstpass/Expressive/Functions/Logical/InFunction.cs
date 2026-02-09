// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Logical.InFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using Expressive.Helpers;

#nullable disable
namespace Expressive.Functions.Logical;

public class InFunction : FunctionBase
{
  public override string Name => "In";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, -1, 2);
    bool flag = false;
    object a = parameters[0].Evaluate(this.Variables);
    for (int index = 1; index < parameters.Length; ++index)
    {
      if (Comparison.CompareUsingMostPreciseType(a, parameters[index].Evaluate(this.Variables)) == 0)
      {
        flag = true;
        break;
      }
    }
    return (object) flag;
  }
}
