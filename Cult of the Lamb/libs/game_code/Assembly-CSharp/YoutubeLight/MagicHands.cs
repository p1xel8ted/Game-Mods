// Decompiled with JetBrains decompiler
// Type: YoutubeLight.MagicHands
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace YoutubeLight;

public static class MagicHands
{
  public static string ApplyOperation(string cipher, string op)
  {
    switch (op[0])
    {
      case 'r':
        return new string(((IEnumerable<char>) cipher.ToCharArray()).Reverse<char>().ToArray<char>());
      case 's':
        int opIndex1 = MagicHands.GetOpIndex(op);
        return cipher.Substring(opIndex1);
      case 'w':
        int opIndex2 = MagicHands.GetOpIndex(op);
        return MagicHands.SwapFirstChar(cipher, opIndex2);
      default:
        throw new NotImplementedException("Couldn't find cipher operation.");
    }
  }

  public static string DecipherWithOperations(string cipher, string operations)
  {
    return ((IEnumerable<string>) operations.Split(new string[1]
    {
      " "
    }, StringSplitOptions.RemoveEmptyEntries)).Aggregate<string, string>(cipher, new Func<string, string, string>(MagicHands.ApplyOperation));
  }

  public static string GetFunctionFromLine(string currentLine)
  {
    return new Regex("\\w+\\.(?<functionID>\\w+)\\(").Match(currentLine).Groups["functionID"].Value;
  }

  public static int GetOpIndex(string op) => int.Parse(new Regex(".(\\d+)").Match(op).Result("$1"));

  public static string SwapFirstChar(string cipher, int index)
  {
    return new StringBuilder(cipher)
    {
      [0] = cipher[index],
      [index] = cipher[0]
    }.ToString();
  }
}
