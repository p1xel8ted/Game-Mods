// Decompiled with JetBrains decompiler
// Type: Expressive.Helpers.Numbers
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Expressive.Helpers;

public class Numbers
{
  public static object ConvertIfString(object s)
  {
    switch (s)
    {
      case string _:
      case char _:
        return (object) Decimal.Parse(s.ToString());
      default:
        return s;
    }
  }

  public static object Add(object a, object b)
  {
    if (a == null || b == null)
      return (object) null;
    a = Numbers.ConvertIfString(a);
    b = Numbers.ConvertIfString(b);
    if (a is double d1 && double.IsNaN(d1))
      return a;
    if (b is double d2 && double.IsNaN(d2))
      return b;
    TypeCode typeCode1 = ReflectionTools.GetTypeCode(a);
    TypeCode typeCode2 = ReflectionTools.GetTypeCode(b);
    switch (typeCode1)
    {
      case TypeCode.Boolean:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'bool'");
          case TypeCode.SByte:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Byte:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Int16:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.UInt16:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Int32:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.UInt32:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Int64:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Single:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Double:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Decimal:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
        }
        break;
      case TypeCode.SByte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (sbyte) a + (int) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((int) (sbyte) a + (int) (byte) b);
          case TypeCode.Int16:
            return (object) ((int) (sbyte) a + (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (sbyte) a + (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (sbyte) a + (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (sbyte) a + (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (sbyte) a + (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (sbyte) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (sbyte) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (sbyte) a + (Decimal) b);
        }
        break;
      case TypeCode.Byte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'byte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (byte) a + (int) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((int) (byte) a + (int) (byte) b);
          case TypeCode.Int16:
            return (object) ((int) (byte) a + (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (byte) a + (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (byte) a + (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (byte) a + (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (byte) a + (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (byte) a + (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (byte) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (byte) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (byte) a + (Decimal) b);
        }
        break;
      case TypeCode.Int16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (short) a + (int) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((int) (short) a + (int) (byte) b);
          case TypeCode.Int16:
            return (object) ((int) (short) a + (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (short) a + (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (short) a + (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (short) a + (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (short) a + (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (short) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (short) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (short) a + (Decimal) b);
        }
        break;
      case TypeCode.UInt16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ushort' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (ushort) a + (int) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((int) (ushort) a + (int) (byte) b);
          case TypeCode.Int16:
            return (object) ((int) (ushort) a + (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (ushort) a + (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (ushort) a + (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (ushort) a + (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (ushort) a + (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (ushort) a + (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ushort) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ushort) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ushort) a + (Decimal) b);
        }
        break;
      case TypeCode.Int32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) a + (int) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((int) a + (int) (byte) b);
          case TypeCode.Int16:
            return (object) ((int) a + (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) a + (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) a + (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (int) a + (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (int) a + (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (int) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (int) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (int) a + (Decimal) b);
        }
        break;
      case TypeCode.UInt32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'unit' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) (uint) a + (long) (sbyte) b);
          case TypeCode.Byte:
            return (object) (uint) ((int) (uint) a + (int) (byte) b);
          case TypeCode.Int16:
            return (object) ((long) (uint) a + (long) (short) b);
          case TypeCode.UInt16:
            return (object) (uint) ((int) (uint) a + (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) (uint) a + (long) (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (uint) a + (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (uint) a + (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (uint) a + (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (uint) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (uint) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (uint) a + (Decimal) b);
        }
        break;
      case TypeCode.Int64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) a + (long) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((long) a + (long) (byte) b);
          case TypeCode.Int16:
            return (object) ((long) a + (long) (short) b);
          case TypeCode.UInt16:
            return (object) ((long) a + (long) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) a + (long) (int) b);
          case TypeCode.UInt32:
            return (object) ((long) a + (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) a + (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (long) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (long) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (long) a + (Decimal) b);
        }
        break;
      case TypeCode.UInt64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'bool'");
          case TypeCode.SByte:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'sbyte'");
          case TypeCode.Byte:
            return (object) (ulong) ((long) (ulong) a + (long) (byte) b);
          case TypeCode.Int16:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'short'");
          case TypeCode.UInt16:
            return (object) (ulong) ((long) (ulong) a + (long) (ushort) b);
          case TypeCode.Int32:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'int'");
          case TypeCode.UInt32:
            return (object) (ulong) ((long) (ulong) a + (long) (uint) b);
          case TypeCode.Int64:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'ulong'");
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (ulong) a + (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ulong) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ulong) a + (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ulong) a + (Decimal) b);
        }
        break;
      case TypeCode.Single:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'float' and 'bool'");
          case TypeCode.SByte:
            return (object) (float) ((double) (float) a + (double) (sbyte) b);
          case TypeCode.Byte:
            return (object) (float) ((double) (float) a + (double) (byte) b);
          case TypeCode.Int16:
            return (object) (float) ((double) (float) a + (double) (short) b);
          case TypeCode.UInt16:
            return (object) (float) ((double) (float) a + (double) (ushort) b);
          case TypeCode.Int32:
            return (object) (float) ((double) (float) a + (double) (int) b);
          case TypeCode.UInt32:
            return (object) (float) ((double) (float) a + (double) (uint) b);
          case TypeCode.Int64:
            return (object) (float) ((double) (float) a + (double) (long) b);
          case TypeCode.UInt64:
            return (object) (float) ((double) (float) a + (double) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (float) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (float) a + (double) b);
          case TypeCode.Decimal:
            return (object) (Convert.ToDecimal(a) + (Decimal) b);
        }
        break;
      case TypeCode.Double:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'double' and 'bool'");
          case TypeCode.SByte:
            return (object) ((double) a + (double) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((double) a + (double) (byte) b);
          case TypeCode.Int16:
            return (object) ((double) a + (double) (short) b);
          case TypeCode.UInt16:
            return (object) ((double) a + (double) (ushort) b);
          case TypeCode.Int32:
            return (object) ((double) a + (double) (int) b);
          case TypeCode.UInt32:
            return (object) ((double) a + (double) (uint) b);
          case TypeCode.Int64:
            return (object) ((double) a + (double) (long) b);
          case TypeCode.UInt64:
            return (object) ((double) a + (double) (ulong) b);
          case TypeCode.Single:
            return (object) ((double) a + (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) a + (double) b);
          case TypeCode.Decimal:
            return (object) (Convert.ToDecimal(a) + (Decimal) b);
        }
        break;
      case TypeCode.Decimal:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'decimal' and 'bool'");
          case TypeCode.SByte:
            return (object) ((Decimal) a + (Decimal) (sbyte) b);
          case TypeCode.Byte:
            return (object) ((Decimal) a + (Decimal) (byte) b);
          case TypeCode.Int16:
            return (object) ((Decimal) a + (Decimal) (short) b);
          case TypeCode.UInt16:
            return (object) ((Decimal) a + (Decimal) (ushort) b);
          case TypeCode.Int32:
            return (object) ((Decimal) a + (Decimal) (int) b);
          case TypeCode.UInt32:
            return (object) ((Decimal) a + (Decimal) (uint) b);
          case TypeCode.Int64:
            return (object) ((Decimal) a + (Decimal) (long) b);
          case TypeCode.UInt64:
            return (object) ((Decimal) a + (Decimal) (ulong) b);
          case TypeCode.Single:
            return (object) ((Decimal) a + Convert.ToDecimal(b));
          case TypeCode.Double:
            return (object) ((Decimal) a + Convert.ToDecimal(b));
          case TypeCode.Decimal:
            return (object) ((Decimal) a + (Decimal) b);
        }
        break;
    }
    return (object) null;
  }

  public static object Divide(object a, object b)
  {
    if (a == null || b == null)
      return (object) null;
    a = Numbers.ConvertIfString(a);
    b = Numbers.ConvertIfString(b);
    if (a is double d1 && double.IsNaN(d1))
      return a;
    if (b is double d2 && double.IsNaN(d2))
      return b;
    TypeCode typeCode1 = ReflectionTools.GetTypeCode(a);
    TypeCode typeCode2 = ReflectionTools.GetTypeCode(b);
    switch (typeCode1)
    {
      case TypeCode.SByte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (sbyte) a / (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (sbyte) a / (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (sbyte) a / (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (sbyte) a / (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (sbyte) a / (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (sbyte) a / (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (sbyte) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (sbyte) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (sbyte) a / (Decimal) b);
        }
        break;
      case TypeCode.Byte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'byte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (byte) a / (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (byte) a / (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (byte) a / (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (byte) a / (int) b);
          case TypeCode.UInt32:
            return (object) ((uint) (byte) a / (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (byte) a / (long) b);
          case TypeCode.UInt64:
            return (object) ((ulong) (byte) a / (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (byte) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (byte) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (byte) a / (Decimal) b);
        }
        break;
      case TypeCode.Int16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (short) a / (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (short) a / (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (short) a / (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (short) a / (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (short) a / (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (short) a / (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (short) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (short) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (short) a / (Decimal) b);
        }
        break;
      case TypeCode.UInt16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ushort' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (ushort) a / (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (ushort) a / (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (ushort) a / (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (ushort) a / (int) b);
          case TypeCode.UInt32:
            return (object) ((uint) (ushort) a / (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (ushort) a / (long) b);
          case TypeCode.UInt64:
            return (object) ((ulong) (ushort) a / (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ushort) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ushort) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ushort) a / (Decimal) b);
        }
        break;
      case TypeCode.Int32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) a / (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) a / (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) a / (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) a / (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (int) a / (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (int) a / (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (int) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (int) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (int) a / (Decimal) b);
        }
        break;
      case TypeCode.UInt32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'uint' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) (uint) a / (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) (uint) a / (long) (short) b);
          case TypeCode.UInt16:
            return (object) ((uint) a / (uint) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) (uint) a / (long) (int) b);
          case TypeCode.UInt32:
            return (object) ((uint) a / (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (uint) a / (long) b);
          case TypeCode.UInt64:
            return (object) ((ulong) (uint) a / (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (uint) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (uint) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (uint) a / (Decimal) b);
        }
        break;
      case TypeCode.Int64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) a / (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) a / (long) (short) b);
          case TypeCode.UInt16:
            return (object) ((long) a / (long) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) a / (long) (int) b);
          case TypeCode.UInt32:
            return (object) ((long) a / (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) a / (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (long) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (long) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (long) a / (Decimal) b);
        }
        break;
      case TypeCode.UInt64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
          case TypeCode.SByte:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'sbyte'");
          case TypeCode.Int16:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'short'");
          case TypeCode.UInt16:
            return (object) ((ulong) a / (ulong) (ushort) b);
          case TypeCode.Int32:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'int'");
          case TypeCode.UInt32:
            return (object) ((ulong) a / (ulong) (uint) b);
          case TypeCode.Int64:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'long'");
          case TypeCode.UInt64:
            return (object) ((ulong) a / (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ulong) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ulong) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ulong) a / (Decimal) b);
        }
        break;
      case TypeCode.Single:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'float' and 'bool'");
          case TypeCode.SByte:
            return (object) (float) ((double) (float) a / (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) (float) ((double) (float) a / (double) (short) b);
          case TypeCode.UInt16:
            return (object) (float) ((double) (float) a / (double) (ushort) b);
          case TypeCode.Int32:
            return (object) (float) ((double) (float) a / (double) (int) b);
          case TypeCode.UInt32:
            return (object) (float) ((double) (float) a / (double) (uint) b);
          case TypeCode.Int64:
            return (object) (float) ((double) (float) a / (double) (long) b);
          case TypeCode.UInt64:
            return (object) (float) ((double) (float) a / (double) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (float) a / (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (float) a / (double) b);
          case TypeCode.Decimal:
            return (object) ((double) (float) a / (double) b);
        }
        break;
      case TypeCode.Double:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'double' and 'bool'");
          case TypeCode.SByte:
            return (object) ((double) a / (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((double) a / (double) (short) b);
          case TypeCode.UInt16:
            return (object) ((double) a / (double) (ushort) b);
          case TypeCode.Int32:
            return (object) ((double) a / (double) (int) b);
          case TypeCode.UInt32:
            return (object) ((double) a / (double) (uint) b);
          case TypeCode.Int64:
            return (object) ((double) a / (double) (long) b);
          case TypeCode.UInt64:
            return (object) ((double) a / (double) (ulong) b);
          case TypeCode.Single:
            return (object) ((double) a / (double) (float) b);
          case TypeCode.Double:
          case TypeCode.Decimal:
            return (object) ((double) a / (double) b);
        }
        break;
      case TypeCode.Decimal:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'bool'");
          case TypeCode.SByte:
            return (object) ((Decimal) a / (Decimal) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((Decimal) a / (Decimal) (short) b);
          case TypeCode.UInt16:
            return (object) ((Decimal) a / (Decimal) (ushort) b);
          case TypeCode.Int32:
            return (object) ((Decimal) a / (Decimal) (int) b);
          case TypeCode.UInt32:
            return (object) ((Decimal) a / (Decimal) (uint) b);
          case TypeCode.Int64:
            return (object) ((Decimal) a / (Decimal) (long) b);
          case TypeCode.UInt64:
            return (object) ((Decimal) a / (Decimal) (ulong) b);
          case TypeCode.Single:
          case TypeCode.Double:
          case TypeCode.Decimal:
            return (object) ((Decimal) a / (Decimal) b);
        }
        break;
    }
    return (object) null;
  }

  public static object Multiply(object a, object b)
  {
    if (a == null || b == null)
      return (object) null;
    a = Numbers.ConvertIfString(a);
    b = Numbers.ConvertIfString(b);
    if (a is double d1 && double.IsNaN(d1))
      return a;
    if (b is double d2 && double.IsNaN(d2))
      return b;
    TypeCode typeCode1 = ReflectionTools.GetTypeCode(a);
    TypeCode typeCode2 = ReflectionTools.GetTypeCode(b);
    switch (typeCode1)
    {
      case TypeCode.SByte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (sbyte) a * (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (sbyte) a * (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (sbyte) a * (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (sbyte) a * (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (sbyte) a * (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (sbyte) a * (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (sbyte) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (sbyte) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (sbyte) a * (Decimal) b);
        }
        break;
      case TypeCode.Byte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'byte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (byte) a * (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (byte) a * (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (byte) a * (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (byte) a * (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (byte) a * (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (byte) a * (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (byte) a * (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (byte) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (byte) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (byte) a * (Decimal) b);
        }
        break;
      case TypeCode.Int16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (short) a * (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (short) a * (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (short) a * (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (short) a * (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (short) a * (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (short) a * (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (short) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (short) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (short) a * (Decimal) b);
        }
        break;
      case TypeCode.UInt16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ushort' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (ushort) a * (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (ushort) a * (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (ushort) a * (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (ushort) a * (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (ushort) a * (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (ushort) a * (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (ushort) a * (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ushort) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ushort) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ushort) a * (Decimal) b);
        }
        break;
      case TypeCode.Int32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) a * (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) a * (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) a * (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) a * (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (int) a * (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (int) a * (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (int) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (int) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (int) a * (Decimal) b);
        }
        break;
      case TypeCode.UInt32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'uint' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) (uint) a * (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) (uint) a * (long) (short) b);
          case TypeCode.UInt16:
            return (object) (uint) ((int) (uint) a * (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) (uint) a * (long) (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (uint) a * (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (uint) a * (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (uint) a * (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (uint) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (uint) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (uint) a * (Decimal) b);
        }
        break;
      case TypeCode.Int64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) a * (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) a * (long) (short) b);
          case TypeCode.UInt16:
            return (object) ((long) a * (long) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) a * (long) (int) b);
          case TypeCode.UInt32:
            return (object) ((long) a * (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) a * (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (long) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (long) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (long) a * (Decimal) b);
        }
        break;
      case TypeCode.UInt64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'bool'");
          case TypeCode.SByte:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'sbyte'");
          case TypeCode.Int16:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'short'");
          case TypeCode.UInt16:
            return (object) (ulong) ((long) (ulong) a * (long) (ushort) b);
          case TypeCode.Int32:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'int'");
          case TypeCode.UInt32:
            return (object) (ulong) ((long) (ulong) a * (long) (uint) b);
          case TypeCode.Int64:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'long'");
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (ulong) a * (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ulong) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ulong) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ulong) a * (Decimal) b);
        }
        break;
      case TypeCode.Single:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'bool'");
          case TypeCode.SByte:
            return (object) (float) ((double) (float) a * (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) (float) ((double) (float) a * (double) (short) b);
          case TypeCode.UInt16:
            return (object) (float) ((double) (float) a * (double) (ushort) b);
          case TypeCode.Int32:
            return (object) (float) ((double) (float) a * (double) (int) b);
          case TypeCode.UInt32:
            return (object) (float) ((double) (float) a * (double) (uint) b);
          case TypeCode.Int64:
            return (object) (float) ((double) (float) a * (double) (long) b);
          case TypeCode.UInt64:
            return (object) (float) ((double) (float) a * (double) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (float) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (float) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((double) (float) a * (double) (Decimal) b);
        }
        break;
      case TypeCode.Double:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'bool'");
          case TypeCode.SByte:
            return (object) ((double) a * (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((double) a * (double) (short) b);
          case TypeCode.UInt16:
            return (object) ((double) a * (double) (ushort) b);
          case TypeCode.Int32:
            return (object) ((double) a * (double) (int) b);
          case TypeCode.UInt32:
            return (object) ((double) a * (double) (uint) b);
          case TypeCode.Int64:
            return (object) ((double) a * (double) (long) b);
          case TypeCode.UInt64:
            return (object) ((double) a * (double) (ulong) b);
          case TypeCode.Single:
            return (object) ((double) a * (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) a * (double) b);
          case TypeCode.Decimal:
            return (object) ((double) a * (double) (Decimal) b);
        }
        break;
      case TypeCode.Decimal:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'bool'");
          case TypeCode.SByte:
            return (object) ((Decimal) a * (Decimal) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((Decimal) a * (Decimal) (short) b);
          case TypeCode.UInt16:
            return (object) ((Decimal) a * (Decimal) (ushort) b);
          case TypeCode.Int32:
            return (object) ((Decimal) a * (Decimal) (int) b);
          case TypeCode.UInt32:
            return (object) ((Decimal) a * (Decimal) (uint) b);
          case TypeCode.Int64:
            return (object) ((Decimal) a * (Decimal) (long) b);
          case TypeCode.UInt64:
            return (object) ((Decimal) a * (Decimal) (ulong) b);
          case TypeCode.Single:
            return (object) ((Decimal) a * (Decimal) (float) b);
          case TypeCode.Double:
            return (object) ((Decimal) a * (Decimal) (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) a * (Decimal) b);
        }
        break;
    }
    return (object) null;
  }

  public static object Subtract(object a, object b)
  {
    if (a == null || b == null)
      return (object) null;
    a = Numbers.ConvertIfString(a);
    b = Numbers.ConvertIfString(b);
    if (a is double d1 && double.IsNaN(d1))
      return a;
    if (b is double d2 && double.IsNaN(d2))
      return b;
    TypeCode typeCode1 = ReflectionTools.GetTypeCode(a);
    TypeCode typeCode2 = ReflectionTools.GetTypeCode(b);
    switch (typeCode1)
    {
      case TypeCode.Boolean:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'bool'");
          case TypeCode.SByte:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Byte:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Int16:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.UInt16:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Int32:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.UInt32:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Int64:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Single:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Double:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
          case TypeCode.Decimal:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
        }
        break;
      case TypeCode.SByte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (sbyte) a - (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (sbyte) a - (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (sbyte) a - (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (sbyte) a - (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (sbyte) a - (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (sbyte) a - (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (sbyte) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (sbyte) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (sbyte) a - (Decimal) b);
        }
        break;
      case TypeCode.Byte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'byte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (byte) a - (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (byte) a - (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (byte) a - (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (byte) a - (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (byte) a - (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (byte) a - (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (byte) a - (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (byte) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (byte) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (byte) a - (Decimal) b);
        }
        break;
      case TypeCode.Int16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (short) a - (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (short) a - (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (short) a - (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (short) a - (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (short) a - (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (short) a - (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (short) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (short) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (short) a - (Decimal) b);
        }
        break;
      case TypeCode.UInt16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ushort' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (ushort) a - (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (ushort) a - (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (ushort) a - (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (ushort) a - (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (ushort) a - (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (ushort) a - (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (ushort) a - (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ushort) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ushort) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ushort) a - (Decimal) b);
        }
        break;
      case TypeCode.Int32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) a - (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) a - (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) a - (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) a - (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (int) a - (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (int) a - (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (int) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (int) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (int) a - (Decimal) b);
        }
        break;
      case TypeCode.UInt32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'uint' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) (uint) a - (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) (uint) a - (long) (short) b);
          case TypeCode.UInt16:
            return (object) (uint) ((int) (uint) a - (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) (uint) a - (long) (int) b);
          case TypeCode.UInt32:
            return (object) (uint) ((int) (uint) a - (int) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (uint) a - (long) b);
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (uint) a - (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (uint) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (uint) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (uint) a - (Decimal) b);
        }
        break;
      case TypeCode.Int64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) a - (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) a - (long) (short) b);
          case TypeCode.UInt16:
            return (object) ((long) a - (long) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) a - (long) (int) b);
          case TypeCode.UInt32:
            return (object) ((long) a - (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) a - (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (long) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (long) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (long) a - (Decimal) b);
        }
        break;
      case TypeCode.UInt64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
          case TypeCode.SByte:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'double'");
          case TypeCode.Int16:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'short'");
          case TypeCode.UInt16:
            return (object) (ulong) ((long) (ulong) a - (long) (ushort) b);
          case TypeCode.Int32:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'int'");
          case TypeCode.UInt32:
            return (object) (ulong) ((long) (ulong) a - (long) (uint) b);
          case TypeCode.Int64:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'long'");
          case TypeCode.UInt64:
            return (object) (ulong) ((long) (ulong) a - (long) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ulong) a - (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ulong) a - (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ulong) a - (Decimal) b);
        }
        break;
      case TypeCode.Single:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'bool'");
          case TypeCode.SByte:
            return (object) (float) ((double) (float) a - (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) (float) ((double) (float) a - (double) (short) b);
          case TypeCode.UInt16:
            return (object) (float) ((double) (float) a - (double) (ushort) b);
          case TypeCode.Int32:
            return (object) (float) ((double) (float) a - (double) (int) b);
          case TypeCode.UInt32:
            return (object) (float) ((double) (float) a - (double) (uint) b);
          case TypeCode.Int64:
            return (object) (float) ((double) (float) a - (double) (long) b);
          case TypeCode.UInt64:
            return (object) (float) ((double) (float) a - (double) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (float) a - (double) (float) b);
          case TypeCode.Double:
          case TypeCode.Decimal:
            return (object) ((double) (float) a - (double) b);
        }
        break;
      case TypeCode.Double:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'bool'");
          case TypeCode.SByte:
            return (object) ((double) a - (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((double) a - (double) (short) b);
          case TypeCode.UInt16:
            return (object) ((double) a - (double) (ushort) b);
          case TypeCode.Int32:
            return (object) ((double) a - (double) (int) b);
          case TypeCode.UInt32:
            return (object) ((double) a - (double) (uint) b);
          case TypeCode.Int64:
            return (object) ((double) a - (double) (long) b);
          case TypeCode.UInt64:
            return (object) ((double) a - (double) (ulong) b);
          case TypeCode.Single:
            return (object) ((double) a - (double) (float) b);
          case TypeCode.Double:
          case TypeCode.Decimal:
            return (object) ((double) a - (double) b);
        }
        break;
      case TypeCode.Decimal:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'bool'");
          case TypeCode.SByte:
            return (object) ((Decimal) a - (Decimal) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((Decimal) a - (Decimal) (short) b);
          case TypeCode.UInt16:
            return (object) ((Decimal) a - (Decimal) (ushort) b);
          case TypeCode.Int32:
            return (object) ((Decimal) a - (Decimal) (int) b);
          case TypeCode.UInt32:
            return (object) ((Decimal) a - (Decimal) (uint) b);
          case TypeCode.Int64:
            return (object) ((Decimal) a - (Decimal) (long) b);
          case TypeCode.UInt64:
            return (object) ((Decimal) a - (Decimal) (ulong) b);
          case TypeCode.Single:
          case TypeCode.Double:
          case TypeCode.Decimal:
            return (object) ((Decimal) a - (Decimal) b);
        }
        break;
    }
    return (object) null;
  }

  public static object Modulus(object a, object b)
  {
    if (a == null || b == null)
      return (object) null;
    a = Numbers.ConvertIfString(a);
    b = Numbers.ConvertIfString(b);
    if (a is double d1 && double.IsNaN(d1))
      return a;
    if (b is double d2 && double.IsNaN(d2))
      return b;
    TypeCode typeCode1 = ReflectionTools.GetTypeCode(a);
    TypeCode typeCode2 = ReflectionTools.GetTypeCode(b);
    switch (typeCode1)
    {
      case TypeCode.SByte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (sbyte) a % (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (sbyte) a % (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (sbyte) a % (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (sbyte) a % (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (sbyte) a % (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (sbyte) a % (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (sbyte) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (sbyte) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (sbyte) a % (Decimal) b);
        }
        break;
      case TypeCode.Byte:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'byte' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (byte) a % (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (byte) a % (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (byte) a % (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (byte) a % (int) b);
          case TypeCode.UInt32:
            return (object) ((uint) (byte) a % (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (byte) a % (long) b);
          case TypeCode.UInt64:
            return (object) ((ulong) (byte) a % (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (byte) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (byte) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (byte) a % (Decimal) b);
        }
        break;
      case TypeCode.Int16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (short) a % (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (short) a % (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (short) a % (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (short) a % (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (short) a % (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (short) a % (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (short) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (short) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (short) a % (Decimal) b);
        }
        break;
      case TypeCode.UInt16:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ushort' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) (ushort) a % (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) (ushort) a % (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) (ushort) a % (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) (ushort) a % (int) b);
          case TypeCode.UInt32:
            return (object) ((uint) (ushort) a % (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (ushort) a % (long) b);
          case TypeCode.UInt64:
            return (object) ((ulong) (ushort) a % (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ushort) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ushort) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ushort) a % (Decimal) b);
        }
        break;
      case TypeCode.Int32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'bool'");
          case TypeCode.SByte:
            return (object) ((int) a % (int) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((int) a % (int) (short) b);
          case TypeCode.UInt16:
            return (object) ((int) a % (int) (ushort) b);
          case TypeCode.Int32:
            return (object) ((int) a % (int) b);
          case TypeCode.UInt32:
            return (object) ((long) (int) a % (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (int) a % (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (int) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (int) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (int) a % (Decimal) b);
        }
        break;
      case TypeCode.UInt32:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'uint' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) (uint) a % (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) (uint) a % (long) (short) b);
          case TypeCode.UInt16:
            return (object) ((uint) a % (uint) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) (uint) a % (long) (int) b);
          case TypeCode.UInt32:
            return (object) ((uint) a % (uint) b);
          case TypeCode.Int64:
            return (object) ((long) (uint) a % (long) b);
          case TypeCode.UInt64:
            return (object) ((ulong) (uint) a % (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (uint) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (uint) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (uint) a % (Decimal) b);
        }
        break;
      case TypeCode.Int64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'bool'");
          case TypeCode.SByte:
            return (object) ((long) a % (long) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((long) a % (long) (short) b);
          case TypeCode.UInt16:
            return (object) ((long) a % (long) (ushort) b);
          case TypeCode.Int32:
            return (object) ((long) a % (long) (int) b);
          case TypeCode.UInt32:
            return (object) ((long) a % (long) (uint) b);
          case TypeCode.Int64:
            return (object) ((long) a % (long) b);
          case TypeCode.UInt64:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'ulong'");
          case TypeCode.Single:
            return (object) (float) ((double) (long) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (long) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (long) a % (Decimal) b);
        }
        break;
      case TypeCode.UInt64:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'bool'");
          case TypeCode.SByte:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'sbyte'");
          case TypeCode.Int16:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'short'");
          case TypeCode.UInt16:
            return (object) ((ulong) a % (ulong) (ushort) b);
          case TypeCode.Int32:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'int'");
          case TypeCode.UInt32:
            return (object) ((ulong) a % (ulong) (uint) b);
          case TypeCode.Int64:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'long'");
          case TypeCode.UInt64:
            return (object) ((ulong) a % (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (ulong) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (ulong) a % (double) b);
          case TypeCode.Decimal:
            return (object) ((Decimal) (ulong) a % (Decimal) b);
        }
        break;
      case TypeCode.Single:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'bool'");
          case TypeCode.SByte:
            return (object) (float) ((double) (float) a % (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) (float) ((double) (float) a % (double) (short) b);
          case TypeCode.UInt16:
            return (object) (float) ((double) (float) a % (double) (ushort) b);
          case TypeCode.Int32:
            return (object) (float) ((double) (float) a % (double) (int) b);
          case TypeCode.UInt32:
            return (object) (float) ((double) (float) a % (double) (uint) b);
          case TypeCode.Int64:
            return (object) (float) ((double) (float) a % (double) (long) b);
          case TypeCode.UInt64:
            return (object) (float) ((double) (float) a % (double) (ulong) b);
          case TypeCode.Single:
            return (object) (float) ((double) (float) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) (float) a % (double) b);
          case TypeCode.Decimal:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'decimal'");
        }
        break;
      case TypeCode.Double:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'bool'");
          case TypeCode.SByte:
            return (object) ((double) a % (double) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((double) a % (double) (short) b);
          case TypeCode.UInt16:
            return (object) ((double) a % (double) (ushort) b);
          case TypeCode.Int32:
            return (object) ((double) a % (double) (int) b);
          case TypeCode.UInt32:
            return (object) ((double) a % (double) (uint) b);
          case TypeCode.Int64:
            return (object) ((double) a % (double) (long) b);
          case TypeCode.UInt64:
            return (object) ((double) a % (double) (ulong) b);
          case TypeCode.Single:
            return (object) ((double) a % (double) (float) b);
          case TypeCode.Double:
            return (object) ((double) a % (double) b);
          case TypeCode.Decimal:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'decimal'");
        }
        break;
      case TypeCode.Decimal:
        switch (typeCode2)
        {
          case TypeCode.Boolean:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'bool'");
          case TypeCode.SByte:
            return (object) ((Decimal) a % (Decimal) (sbyte) b);
          case TypeCode.Int16:
            return (object) ((Decimal) a % (Decimal) (short) b);
          case TypeCode.UInt16:
            return (object) ((Decimal) a % (Decimal) (ushort) b);
          case TypeCode.Int32:
            return (object) ((Decimal) a % (Decimal) (int) b);
          case TypeCode.UInt32:
            return (object) ((Decimal) a % (Decimal) (uint) b);
          case TypeCode.Int64:
            return (object) ((Decimal) a % (Decimal) (long) b);
          case TypeCode.UInt64:
            return (object) ((Decimal) a % (Decimal) (ulong) b);
          case TypeCode.Single:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'float'");
          case TypeCode.Double:
            throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'decimal'");
          case TypeCode.Decimal:
            return (object) ((Decimal) a % (Decimal) b);
        }
        break;
    }
    return (object) null;
  }

  public static object Max(object a, object b)
  {
    a = Numbers.ConvertIfString(a);
    b = Numbers.ConvertIfString(b);
    if (a == null || b == null)
      return (object) null;
    switch (ReflectionTools.GetTypeCode(a))
    {
      case TypeCode.SByte:
        return (object) Math.Max((sbyte) a, Convert.ToSByte(b));
      case TypeCode.Byte:
        return (object) Math.Max((byte) a, Convert.ToByte(b));
      case TypeCode.Int16:
        return (object) Math.Max((short) a, Convert.ToInt16(b));
      case TypeCode.UInt16:
        return (object) Math.Max((ushort) a, Convert.ToUInt16(b));
      case TypeCode.Int32:
        return (object) Math.Max((int) a, Convert.ToInt32(b));
      case TypeCode.UInt32:
        return (object) Math.Max((uint) a, Convert.ToUInt32(b));
      case TypeCode.Int64:
        return (object) Math.Max((long) a, Convert.ToInt64(b));
      case TypeCode.UInt64:
        return (object) Math.Max((ulong) a, Convert.ToUInt64(b));
      case TypeCode.Single:
        return (object) Math.Max((float) a, Convert.ToSingle(b));
      case TypeCode.Double:
        return (object) Math.Max((double) a, Convert.ToDouble(b));
      case TypeCode.Decimal:
        return (object) Math.Max((Decimal) a, Convert.ToDecimal(b));
      default:
        return (object) null;
    }
  }

  public static object Min(object a, object b)
  {
    a = Numbers.ConvertIfString(a);
    b = Numbers.ConvertIfString(b);
    if (a == null && b == null)
      return (object) null;
    if (a == null)
      return b;
    if (b == null)
      return a;
    switch (ReflectionTools.GetTypeCode(a))
    {
      case TypeCode.SByte:
        return (object) Math.Min((sbyte) a, Convert.ToSByte(b));
      case TypeCode.Byte:
        return (object) Math.Min((byte) a, Convert.ToByte(b));
      case TypeCode.Int16:
        return (object) Math.Min((short) a, Convert.ToInt16(b));
      case TypeCode.UInt16:
        return (object) Math.Min((ushort) a, Convert.ToUInt16(b));
      case TypeCode.Int32:
        return (object) Math.Min((int) a, Convert.ToInt32(b));
      case TypeCode.UInt32:
        return (object) Math.Min((uint) a, Convert.ToUInt32(b));
      case TypeCode.Int64:
        return (object) Math.Min((long) a, Convert.ToInt64(b));
      case TypeCode.UInt64:
        return (object) Math.Min((ulong) a, Convert.ToUInt64(b));
      case TypeCode.Single:
        return (object) Math.Min((float) a, Convert.ToSingle(b));
      case TypeCode.Double:
        return (object) Math.Min((double) a, Convert.ToDouble(b));
      case TypeCode.Decimal:
        return (object) Math.Min((Decimal) a, Convert.ToDecimal(b));
      default:
        return (object) null;
    }
  }
}
