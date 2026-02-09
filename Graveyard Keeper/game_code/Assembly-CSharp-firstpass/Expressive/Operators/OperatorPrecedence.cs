// Decompiled with JetBrains decompiler
// Type: Expressive.Operators.OperatorPrecedence
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Expressive.Operators;

public enum OperatorPrecedence
{
  Minimum = 0,
  Or = 1,
  And = 2,
  Equal = 3,
  NotEqual = 4,
  LessThan = 5,
  GreaterThan = 6,
  LessThanOrEqual = 7,
  GreaterThanOrEqual = 8,
  Not = 9,
  BitwiseOr = 10, // 0x0000000A
  BitwiseXOr = 11, // 0x0000000B
  BitwiseAnd = 12, // 0x0000000C
  LeftShift = 13, // 0x0000000D
  RightShift = 14, // 0x0000000E
  Add = 15, // 0x0000000F
  Subtract = 16, // 0x00000010
  Multiply = 17, // 0x00000011
  Modulus = 18, // 0x00000012
  Divide = 19, // 0x00000013
  Conditional = 20, // 0x00000014
  NullCoalescing = 20, // 0x00000014
  UnaryPlus = 21, // 0x00000015
  UnaryMinus = 22, // 0x00000016
  ParenthesisOpen = 23, // 0x00000017
  ParenthesisClose = 24, // 0x00000018
}
