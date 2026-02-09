// Decompiled with JetBrains decompiler
// Type: Expressive.Operators.Grouping.ParenthesisOpenOperator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using LinqTools;
using System;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Operators.Grouping;

public class ParenthesisOpenOperator : IOperator
{
  public string[] Tags => new string[1]{ "(" };

  public IExpression BuildExpression(Token previousToken, IExpression[] expressions)
  {
    return (IExpression) new ParenthesisedExpression(expressions[0] ?? expressions[1]);
  }

  public bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
  {
    Queue<Token> remainingTokens1 = new Queue<Token>((IEnumerable<Token>) remainingTokens.ToArray());
    return ((IEnumerable<Token>) this.GetCaptiveTokens(previousToken, token, remainingTokens1)).Any<Token>();
  }

  public Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
  {
    IList<Token> source = (IList<Token>) new List<Token>();
    source.Add(token);
    int num = 1;
    while (remainingTokens.Any<Token>())
    {
      Token token1 = remainingTokens.Dequeue();
      source.Add(token1);
      if (string.Equals(token1.CurrentToken, "(", StringComparison.Ordinal))
        ++num;
      else if (string.Equals(token1.CurrentToken, ")", StringComparison.Ordinal))
        --num;
      if (num <= 0)
        break;
    }
    return source.ToArray<Token>();
  }

  public Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
  {
    return ((IEnumerable<Token>) allCaptiveTokens).Skip<Token>(1).Take<Token>(allCaptiveTokens.Length - 2).ToArray<Token>();
  }

  public OperatorPrecedence GetPrecedence(Token previousToken)
  {
    return OperatorPrecedence.ParenthesisOpen;
  }
}
