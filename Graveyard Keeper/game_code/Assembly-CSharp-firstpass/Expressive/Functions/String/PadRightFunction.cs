// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.String.PadRightFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using System;

#nullable disable
namespace Expressive.Functions.String;

public class PadRightFunction : FunctionBase
{
  public override string Name => "PadRight";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 3, 3);
    object obj1 = parameters[0].Evaluate(this.Variables);
    object obj2 = parameters[1].Evaluate(this.Variables);
    if (obj1 == null || obj2 == null)
      return (object) null;
    string str = !(obj1 is string) ? obj1.ToString() : (string) obj1;
    int int32 = Convert.ToInt32(obj2);
    object obj3 = parameters[2].Evaluate(this.Variables);
    paddingChar = ' ';
    if (!(obj3 is char paddingChar) && obj3 is string)
      paddingChar = ((string) obj3)[0];
    return (object) str.PadRight(int32, paddingChar);
  }
}
