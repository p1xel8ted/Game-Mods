// Decompiled with JetBrains decompiler
// Type: Expressive.Operators.OperatorBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Operators;

public abstract class OperatorBase : IOperator
{
  public abstract string[] Tags { get; }

  public abstract IExpression BuildExpression(Token previousToken, IExpression[] expressions);

  public virtual bool CanGetCaptiveTokens(
    Token previousToken,
    Token token,
    Queue<Token> remainingTokens)
  {
    return true;
  }

  public virtual Token[] GetCaptiveTokens(
    Token previousToken,
    Token token,
    Queue<Token> remainingTokens)
  {
    return new Token[1]{ token };
  }

  public virtual Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens) => new Token[0];

  public abstract OperatorPrecedence GetPrecedence(Token previousToken);
}
