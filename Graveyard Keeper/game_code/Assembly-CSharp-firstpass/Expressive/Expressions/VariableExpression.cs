// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.VariableExpression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Expressions;

public class VariableExpression : IExpression
{
  public string _variableName;

  public VariableExpression(string variableName) => this._variableName = variableName;

  public object Evaluate(IDictionary<string, object> variables)
  {
    if (variables == null || !variables.ContainsKey(this._variableName))
      throw new ArgumentException($"The variable '{this._variableName}' has not been supplied.");
    return variables[this._variableName] is Expression variable ? variable.Evaluate(variables) : variables[this._variableName];
  }
}
