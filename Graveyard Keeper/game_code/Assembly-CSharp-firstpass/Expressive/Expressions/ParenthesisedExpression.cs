// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.ParenthesisedExpression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Exceptions;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Expressions;

public class ParenthesisedExpression : IExpression
{
  public IExpression _innerExpression;

  public ParenthesisedExpression(IExpression innerExpression)
  {
    this._innerExpression = innerExpression;
  }

  public object Evaluate(IDictionary<string, object> variables)
  {
    return this._innerExpression != null ? this._innerExpression.Evaluate(variables) : throw new MissingParticipantException("Missing contents inside ().");
  }
}
