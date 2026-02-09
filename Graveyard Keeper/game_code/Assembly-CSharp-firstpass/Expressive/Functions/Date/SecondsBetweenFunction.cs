// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Date.SecondsBetweenFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System;

#nullable disable
namespace Expressive.Functions.Date;

public sealed class SecondsBetweenFunction : FunctionBase
{
  public override string Name => "SecondsBetween";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 2, 2);
    object obj1 = parameters[0].Evaluate(this.Variables);
    object obj2 = parameters[1].Evaluate(this.Variables);
    if (obj1 == null || obj2 == null)
      return (object) null;
    DateTime dateTime = Convert.ToDateTime(obj1);
    return (object) (Convert.ToDateTime(obj2) - dateTime).TotalSeconds;
  }
}
