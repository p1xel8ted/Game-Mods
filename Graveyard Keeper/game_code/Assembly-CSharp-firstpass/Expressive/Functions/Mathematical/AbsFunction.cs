// Decompiled with JetBrains decompiler
// Type: Expressive.Functions.Mathematical.AbsFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Expressions;
using Expressive.Helpers;
using System;

#nullable disable
namespace Expressive.Functions.Mathematical;

public class AbsFunction : FunctionBase
{
  public override string Name => "Abs";

  public override object Evaluate(IExpression[] parameters)
  {
    this.ValidateParameterCount(parameters, 1, 1);
    object obj = parameters[0].Evaluate(this.Variables);
    if (obj != null)
    {
      switch (ReflectionTools.GetTypeCode(obj))
      {
        case TypeCode.SByte:
          return (object) Math.Abs(Convert.ToSByte(obj));
        case TypeCode.Int16:
          return (object) Math.Abs(Convert.ToInt16(obj));
        case TypeCode.UInt16:
          return (object) Math.Abs((int) Convert.ToUInt16(obj));
        case TypeCode.Int32:
          return (object) Math.Abs(Convert.ToInt32(obj));
        case TypeCode.UInt32:
          return (object) Math.Abs((long) Convert.ToUInt32(obj));
        case TypeCode.Int64:
          return (object) Math.Abs(Convert.ToInt64(obj));
        case TypeCode.Single:
          return (object) Math.Abs(Convert.ToSingle(obj));
        case TypeCode.Double:
          return (object) Math.Abs(Convert.ToDouble(obj));
        case TypeCode.Decimal:
          return (object) Math.Abs(Convert.ToDecimal(obj));
      }
    }
    return (object) null;
  }
}
