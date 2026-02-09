// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Logical.IfFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System;

#nullable disable
namespace Expressive.Functions.Logical;

public class IfFunction : FunctionBase
{
  public override string Name => "If3";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 3, 3);
    return !Convert.ToBoolean(parameters[0].Evaluate(this.Variables)) ? parameters[2].Evaluate(this.Variables) : parameters[1].Evaluate(this.Variables);
  }
}
