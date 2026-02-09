// Decompiled with JetBrains decompiler
// Type: Expressive.Operators.Relational.LessThanOrEqualOperator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;

#nullable disable
namespace Expressive.Operators.Relational;

public class LessThanOrEqualOperator : OperatorBase
{
  public override string[] Tags => new string[1]{ "<=" };

  public override IExpression BuildExpression(Token previousToken, IExpression[] expressions)
  {
    return (IExpression) new BinaryExpression(BinaryExpressionType.LessThanOrEqual, expressions[0], expressions[1]);
  }

  public override OperatorPrecedence GetPrecedence(Token previousToken)
  {
    return OperatorPrecedence.LessThanOrEqual;
  }
}
