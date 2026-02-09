// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.String.LengthFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;

#nullable disable
namespace Expressive.Functions.String;

public class LengthFunction : FunctionBase
{
  public override string Name => "Length";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 1, 1);
    object obj = parameters[0].Evaluate(this.Variables);
    if (obj == null)
      return (object) null;
    return obj is string str ? (object) str.Length : (object) obj.ToString().Length;
  }
}
