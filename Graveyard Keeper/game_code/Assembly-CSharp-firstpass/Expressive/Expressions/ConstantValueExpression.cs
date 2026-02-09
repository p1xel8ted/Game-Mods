// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.ConstantValueExpression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

#nullable disable
namespace Expressive.Expressions;

public class ConstantValueExpression : IExpression
{
  public ConstantValueExpressionType _expressionType;
  public object _value;

  public ConstantValueExpression(ConstantValueExpressionType type, object value)
  {
    this._expressionType = type;
    this._value = value;
  }

  public object Evaluate(IDictionary<string, object> variables) => this._value;
}
