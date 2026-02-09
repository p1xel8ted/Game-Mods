// Decompiled with JetBrains decompiler
// Type: Expressive.ExtensionMethods
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Expressive;

public static class ExtensionMethods
{
  public static bool IsArithmeticOperator(this string source)
  {
    return string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "-", StringComparison.Ordinal) || string.Equals(source, "−", StringComparison.Ordinal) || string.Equals(source, "/", StringComparison.Ordinal) || string.Equals(source, "÷", StringComparison.Ordinal) || string.Equals(source, "*", StringComparison.Ordinal) || string.Equals(source, "×", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal);
  }

  public static bool IsNumeric(this string source)
  {
    return double.TryParse(source, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out double _);
  }

  public static T PeekOrDefault<T>(this Queue<T> queue)
  {
    return queue.Count <= 0 ? default (T) : queue.Peek();
  }

  public static string SubstringUpTo(this string source, int startIndex, char character)
  {
    if (startIndex == 0)
      return source.Substring(startIndex, source.IndexOf(character) + 1);
    string str = source.Substring(startIndex);
    return str.Substring(0, str.IndexOf(character) + 1);
  }
}
