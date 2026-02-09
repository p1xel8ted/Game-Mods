// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Mathematical.SignFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using Expressive.Helpers;
using System;

#nullable disable
namespace Expressive.Functions.Mathematical;

public class SignFunction : FunctionBase
{
  public override string Name => "Sign";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 1, 1);
    object obj = parameters[0].Evaluate(this.Variables);
    if (obj != null)
    {
      switch (ReflectionTools.GetTypeCode(obj))
      {
        case TypeCode.SByte:
          return (object) Math.Sign(Convert.ToSByte(obj));
        case TypeCode.Int16:
          return (object) Math.Sign(Convert.ToInt16(obj));
        case TypeCode.UInt16:
          return (object) Math.Sign((int) Convert.ToUInt16(obj));
        case TypeCode.Int32:
          return (object) Math.Sign(Convert.ToInt32(obj));
        case TypeCode.UInt32:
          return (object) Math.Sign((long) Convert.ToUInt32(obj));
        case TypeCode.Int64:
          return (object) Math.Sign(Convert.ToInt64(obj));
        case TypeCode.Single:
          return (object) Math.Sign(Convert.ToSingle(obj));
        case TypeCode.Double:
          return (object) Math.Sign(Convert.ToDouble(obj));
        case TypeCode.Decimal:
          return (object) Math.Sign(Convert.ToDecimal(obj));
      }
    }
    return (object) null;
  }
}
