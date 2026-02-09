// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.UnaryExpression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Helpers;
using System;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Expressions;

public class UnaryExpression : IExpression
{
  public IExpression _expression;
  public UnaryExpressionType _expressionType;

  public UnaryExpression(UnaryExpressionType type, IExpression expression)
  {
    this._expressionType = type;
    this._expression = expression;
  }

  public object Evaluate(IDictionary<string, object> variables)
  {
    switch (this._expressionType)
    {
      case UnaryExpressionType.Minus:
        return Numbers.Subtract((object) 0, this._expression.Evaluate(variables));
      case UnaryExpressionType.Not:
        object obj = this._expression.Evaluate(variables);
        if (obj != null)
        {
          int typeCode = (int) ReflectionTools.GetTypeCode(obj);
          return obj is bool flag ? (object) !flag : (object) !Convert.ToBoolean(obj);
        }
        break;
      case UnaryExpressionType.Plus:
        return Numbers.Add((object) 0, this._expression.Evaluate(variables));
    }
    return (object) null;
  }
}
