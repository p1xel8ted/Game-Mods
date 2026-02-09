// Decompiled with JetBrains decompiler
// Type: Expressive.Operators.IOperator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Operators;

public interface IOperator
{
  string[] Tags { get; }

  IExpression BuildExpression(Token previousToken, IExpression[] expressions);

  bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens);

  Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens);

  Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens);

  OperatorPrecedence GetPrecedence(Token previousToken);
}
