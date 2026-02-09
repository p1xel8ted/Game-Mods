// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.FunctionBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Exceptions;
using Expressive.Expressions;
using LinqTools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Expressive.Functions;

public abstract class FunctionBase : IFunction
{
  [CompilerGenerated]
  public IDictionary<string, object> \u003CVariables\u003Ek__BackingField;

  public IDictionary<string, object> Variables
  {
    get => this.\u003CVariables\u003Ek__BackingField;
    set => this.\u003CVariables\u003Ek__BackingField = value;
  }

  public abstract string Name { get; }

  public abstract object Evaluate(IExpression[] parameters);

  public bool ValidateParameterCount(IExpression[] parameters, int expectedCount, int minimumCount)
  {
    if (expectedCount != -1 && (parameters == null || !((IEnumerable<IExpression>) parameters).Any<IExpression>() || parameters.Length != expectedCount))
      throw new ParameterCountMismatchException($"{this.Name}() takes only {expectedCount.ToString()} argument(s)");
    if (minimumCount > 0 && (parameters == null || !((IEnumerable<IExpression>) parameters).Any<IExpression>() || parameters.Length < minimumCount))
      throw new ParameterCountMismatchException($"{this.Name}() expects at least {minimumCount.ToString()} argument(s)");
    return true;
  }
}
