// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Mathematical.TanFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System;

#nullable disable
namespace Expressive.Functions.Mathematical;

public class TanFunction : FunctionBase
{
  public override string Name => "Tan";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 1, 1);
    return (object) Math.Tan(Convert.ToDouble(parameters[0].Evaluate(this.Variables)));
  }
}
