// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.IFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System.Collections.Generic;

#nullable disable
namespace Expressive.Functions;

public interface IFunction
{
  IDictionary<string, object> Variables { get; set; }

  string Name { get; }

  object Evaluate(IExpression[] parameters);
}
