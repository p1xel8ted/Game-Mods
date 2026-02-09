// Decompiled with JetBrains decompiler
// Type: Expressive.Operators.Additive.PlusOperator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using LinqTools;
using System;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Operators.Additive;

public class PlusOperator : OperatorBase
{
  public override string[] Tags => new string[1]{ "+" };

  public override IExpression BuildExpression(Token previousToken, IExpression[] expressions)
  {
    return this.IsUnary(previousToken) ? (IExpression) new UnaryExpression(UnaryExpressionType.Plus, expressions[0] ?? expressions[1]) : (IExpression) new BinaryExpression(BinaryExpressionType.Add, expressions[0], expressions[1]);
  }

  public override bool CanGetCaptiveTokens(
    Token previousToken,
    Token token,
    Queue<Token> remainingTokens)
  {
    Queue<Token> remainingTokens1 = new Queue<Token>((IEnumerable<Token>) remainingTokens.ToArray());
    return ((IEnumerable<Token>) this.GetCaptiveTokens(previousToken, token, remainingTokens1)).Any<Token>();
  }

  public override Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
  {
    return ((IEnumerable<Token>) allCaptiveTokens).Skip<Token>(1).ToArray<Token>();
  }

  public override OperatorPrecedence GetPrecedence(Token previousToken)
  {
    return this.IsUnary(previousToken) ? OperatorPrecedence.UnaryPlus : OperatorPrecedence.Add;
  }

  public bool IsUnary(Token previousToken)
  {
    return string.IsNullOrEmpty(previousToken == null ? (string) null : previousToken.CurrentToken) || string.Equals(previousToken.CurrentToken, "(", StringComparison.Ordinal) || previousToken.CurrentToken.IsArithmeticOperator();
  }
}
