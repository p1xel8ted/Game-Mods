// Decompiled with JetBrains decompiler
// Type: Expressive.Expressions.BinaryExpressionType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Expressive.Expressions;

public enum BinaryExpressionType
{
  Unknown,
  And,
  Or,
  NotEqual,
  LessThanOrEqual,
  GreaterThanOrEqual,
  LessThan,
  GreaterThan,
  Equal,
  Subtract,
  Add,
  Modulus,
  Divide,
  Multiply,
  BitwiseOr,
  BitwiseAnd,
  BitwiseXOr,
  LeftShift,
  RightShift,
  NullCoalescing,
}
