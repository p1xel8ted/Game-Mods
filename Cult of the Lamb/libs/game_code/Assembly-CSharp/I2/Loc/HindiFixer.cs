// Decompiled with JetBrains decompiler
// Type: I2.Loc.HindiFixer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace I2.Loc;

public class HindiFixer
{
  public static string Fix(string text)
  {
    char[] charArray = text.ToCharArray();
    bool flag = false;
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] == 'ि' && !char.IsWhiteSpace(charArray[index - 1]) && charArray[index - 1] != char.MinValue)
      {
        charArray[index] = charArray[index - 1];
        charArray[index - 1] = 'ि';
        flag = true;
      }
      if (index != charArray.Length - 1)
      {
        if (charArray[index] == 'इ' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ऌ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
        if (charArray[index] == 'ृ' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ॄ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
        if (charArray[index] == 'ँ' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ॐ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
        if (charArray[index] == 'ऋ' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ॠ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
        if (charArray[index] == 'ई' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ॡ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
        if (charArray[index] == 'ि' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ॢ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
        if (charArray[index] == 'ी' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ॣ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
        if (charArray[index] == '।' && charArray[index + 1] == '़')
        {
          charArray[index] = 'ऽ';
          charArray[index + 1] = char.MinValue;
          flag = true;
        }
      }
    }
    if (!flag)
      return text;
    string str = new string(((IEnumerable<char>) charArray).Where<char>((Func<char, bool>) (x => x > char.MinValue)).ToArray<char>());
    if (str == text)
      return str;
    text = str;
    return text;
  }
}
