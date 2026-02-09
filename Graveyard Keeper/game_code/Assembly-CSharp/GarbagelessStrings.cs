// Decompiled with JetBrains decompiler
// Type: GarbagelessStrings
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Text;

#nullable disable
public static class GarbagelessStrings
{
  public static StringBuilder _sb = new StringBuilder();

  public static void IntToCharsWithLeadingZeros(
    int v,
    ref char[] c,
    int sign_chars,
    int start_pos)
  {
    int index1 = start_pos + sign_chars - 1;
    for (int index2 = sign_chars; index2 > 0; --index2)
    {
      c[index1] = (char) (48 /*0x30*/ + v % 10);
      v /= 10;
      --index1;
    }
  }

  public static void StringToChars(ref string s, ref char[] c)
  {
    int length = s.Length;
    for (int index = 0; index < length; ++index)
      c[index] = s[index];
    c[length] = char.MinValue;
  }

  public static string CharsToString(ref char[] chars)
  {
    GarbagelessStrings._sb.Length = 0;
    for (int index = 0; index < chars.Length; ++index)
    {
      char ch = chars[index];
      if (ch != char.MinValue)
        GarbagelessStrings._sb.Append(ch);
      else
        break;
    }
    return GarbagelessStrings._sb.ToString();
  }

  public static int GetHashCode(ref char[] chars)
  {
    int num1 = 5381;
    int num2 = num1;
    for (int index = 0; index < chars.Length && chars[index] != char.MinValue; index += 2)
    {
      num1 = (num1 << 5) + num1 ^ (int) chars[index];
      if (index != chars.Length - 1 && chars[index + 1] != char.MinValue)
        num2 = (num2 << 5) + num2 ^ (int) chars[index + 1];
      else
        break;
    }
    return num1 + num2 * 1566083941;
  }
}
