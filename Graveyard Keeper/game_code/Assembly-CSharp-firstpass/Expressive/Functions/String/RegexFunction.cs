// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.String.RegexFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System.Text.RegularExpressions;

#nullable disable
namespace Expressive.Functions.String;

public class RegexFunction : FunctionBase
{
  public override string Name => "Regex";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 2, 2);
    return (object) new Regex(parameters[1].Evaluate(this.Variables) as string).IsMatch(parameters[0].Evaluate(this.Variables) as string);
  }
}
