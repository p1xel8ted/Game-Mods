// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.String.SubstringFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;

#nullable disable
namespace Expressive.Functions.String;

public class SubstringFunction : FunctionBase
{
  public override string Name => "Substring";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 3, 3);
    string str = (string) parameters[0].Evaluate(this.Variables);
    int num1 = (int) parameters[1].Evaluate(this.Variables);
    int num2 = (int) parameters[2].Evaluate(this.Variables);
    int startIndex = num1;
    int length = num2;
    return (object) str.Substring(startIndex, length);
  }
}
