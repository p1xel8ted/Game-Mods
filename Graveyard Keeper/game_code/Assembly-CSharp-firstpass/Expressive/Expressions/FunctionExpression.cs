// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.FunctionExpression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Expressions;

public class FunctionExpression : IExpression
{
  public Func<IExpression[], IDictionary<string, object>, object> _function;
  public string _name;
  public IExpression[] _parameters;

  public FunctionExpression(
    string name,
    Func<IExpression[], IDictionary<string, object>, object> function,
    IExpression[] parameters)
  {
    this._name = name;
    this._function = function;
    this._parameters = parameters;
  }

  public object Evaluate(IDictionary<string, object> variables)
  {
    return this._function(this._parameters, variables);
  }
}
