// Decompiled with JetBrains decompiler
// Type: I2.Loc.RTLFixerTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace I2.Loc;

internal class RTLFixerTool
{
  internal static bool showTashkeel = true;
  internal static bool useHinduNumbers;

  internal static string RemoveTashkeel(string str, out List<TashkeelLocation> tashkeelLocation)
  {
    tashkeelLocation = new List<TashkeelLocation>();
    char[] charArray = str.ToCharArray();
    int num = 0;
    for (int position = 0; position < charArray.Length; ++position)
    {
      if (charArray[position] == 'ً')
      {
        tashkeelLocation.Add(new TashkeelLocation('ً', position));
        ++num;
      }
      else if (charArray[position] == 'ٌ')
      {
        tashkeelLocation.Add(new TashkeelLocation('ٌ', position));
        ++num;
      }
      else if (charArray[position] == 'ٍ')
      {
        tashkeelLocation.Add(new TashkeelLocation('ٍ', position));
        ++num;
      }
      else if (charArray[position] == 'َ')
      {
        if (num > 0 && tashkeelLocation[num - 1].tashkeel == 'ّ')
        {
          tashkeelLocation[num - 1].tashkeel = 'ﱠ';
        }
        else
        {
          tashkeelLocation.Add(new TashkeelLocation('َ', position));
          ++num;
        }
      }
      else if (charArray[position] == 'ُ')
      {
        if (num > 0 && tashkeelLocation[num - 1].tashkeel == 'ّ')
        {
          tashkeelLocation[num - 1].tashkeel = 'ﱡ';
        }
        else
        {
          tashkeelLocation.Add(new TashkeelLocation('ُ', position));
          ++num;
        }
      }
      else if (charArray[position] == 'ِ')
      {
        if (num > 0 && tashkeelLocation[num - 1].tashkeel == 'ّ')
        {
          tashkeelLocation[num - 1].tashkeel = 'ﱢ';
        }
        else
        {
          tashkeelLocation.Add(new TashkeelLocation('ِ', position));
          ++num;
        }
      }
      else if (charArray[position] == 'ّ')
      {
        if (num > 0)
        {
          if (tashkeelLocation[num - 1].tashkeel == 'َ')
          {
            tashkeelLocation[num - 1].tashkeel = 'ﱠ';
            continue;
          }
          if (tashkeelLocation[num - 1].tashkeel == 'ُ')
          {
            tashkeelLocation[num - 1].tashkeel = 'ﱡ';
            continue;
          }
          if (tashkeelLocation[num - 1].tashkeel == 'ِ')
          {
            tashkeelLocation[num - 1].tashkeel = 'ﱢ';
            continue;
          }
        }
        tashkeelLocation.Add(new TashkeelLocation('ّ', position));
        ++num;
      }
      else if (charArray[position] == 'ْ')
      {
        tashkeelLocation.Add(new TashkeelLocation('ْ', position));
        ++num;
      }
      else if (charArray[position] == 'ٓ')
      {
        tashkeelLocation.Add(new TashkeelLocation('ٓ', position));
        ++num;
      }
    }
    string[] strArray = str.Split('ً', 'ٌ', 'ٍ', 'َ', 'ُ', 'ِ', 'ّ', 'ْ', 'ٓ', 'ﱠ', 'ﱡ', 'ﱢ');
    str = "";
    foreach (string str1 in strArray)
      str += str1;
    return str;
  }

  internal static char[] ReturnTashkeel(char[] letters, List<TashkeelLocation> tashkeelLocation)
  {
    char[] chArray = new char[letters.Length + tashkeelLocation.Count];
    int index1 = 0;
    for (int index2 = 0; index2 < letters.Length; ++index2)
    {
      chArray[index1] = letters[index2];
      ++index1;
      foreach (TashkeelLocation tashkeelLocation1 in tashkeelLocation)
      {
        if (tashkeelLocation1.position == index1)
        {
          chArray[index1] = tashkeelLocation1.tashkeel;
          ++index1;
        }
      }
    }
    return chArray;
  }

  internal static string FixLine(string str)
  {
    string str1 = "";
    List<TashkeelLocation> tashkeelLocation;
    string str2 = RTLFixerTool.RemoveTashkeel(str, out tashkeelLocation);
    char[] charArray = str2.ToCharArray();
    char[] letters = str2.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
      charArray[index] = (char) ArabicTable.ArabicMapper.Convert((int) charArray[index]);
    for (int index = 0; index < charArray.Length; ++index)
    {
      bool flag = false;
      if (charArray[index] == 'ﻝ' && index < charArray.Length - 1)
      {
        if (charArray[index + 1] == 'ﺇ')
        {
          charArray[index] = 'ﻷ';
          letters[index + 1] = char.MaxValue;
          flag = true;
        }
        else if (charArray[index + 1] == 'ﺍ')
        {
          charArray[index] = 'ﻹ';
          letters[index + 1] = char.MaxValue;
          flag = true;
        }
        else if (charArray[index + 1] == 'ﺃ')
        {
          charArray[index] = 'ﻵ';
          letters[index + 1] = char.MaxValue;
          flag = true;
        }
        else if (charArray[index + 1] == 'ﺁ')
        {
          charArray[index] = 'ﻳ';
          letters[index + 1] = char.MaxValue;
          flag = true;
        }
      }
      if (!RTLFixerTool.IsIgnoredCharacter(charArray[index]))
      {
        if (RTLFixerTool.IsMiddleLetter(charArray, index))
          letters[index] = (char) ((uint) charArray[index] + 3U);
        else if (RTLFixerTool.IsFinishingLetter(charArray, index))
          letters[index] = (char) ((uint) charArray[index] + 1U);
        else if (RTLFixerTool.IsLeadingLetter(charArray, index))
          letters[index] = (char) ((uint) charArray[index] + 2U);
      }
      str1 = $"{str1}{Convert.ToString((int) charArray[index], 16 /*0x10*/)} ";
      if (flag)
        ++index;
      if (RTLFixerTool.useHinduNumbers)
      {
        if (charArray[index] == '0')
          letters[index] = '٠';
        else if (charArray[index] == '1')
          letters[index] = '١';
        else if (charArray[index] == '2')
          letters[index] = '٢';
        else if (charArray[index] == '3')
          letters[index] = '٣';
        else if (charArray[index] == '4')
          letters[index] = '٤';
        else if (charArray[index] == '5')
          letters[index] = '٥';
        else if (charArray[index] == '6')
          letters[index] = '٦';
        else if (charArray[index] == '7')
          letters[index] = '٧';
        else if (charArray[index] == '8')
          letters[index] = '٨';
        else if (charArray[index] == '9')
          letters[index] = '٩';
      }
    }
    if (RTLFixerTool.showTashkeel)
      letters = RTLFixerTool.ReturnTashkeel(letters, tashkeelLocation);
    List<char> charList1 = new List<char>();
    List<char> charList2 = new List<char>();
    for (int index1 = letters.Length - 1; index1 >= 0; --index1)
    {
      if (char.IsPunctuation(letters[index1]) && index1 > 0 && index1 < letters.Length - 1 && (char.IsPunctuation(letters[index1 - 1]) || char.IsPunctuation(letters[index1 + 1])))
      {
        if (letters[index1] == '(')
          charList1.Add(')');
        else if (letters[index1] == ')')
          charList1.Add('(');
        else if (letters[index1] == '<')
          charList1.Add('>');
        else if (letters[index1] == '>')
          charList1.Add('<');
        else if (letters[index1] == '[')
          charList1.Add(']');
        else if (letters[index1] == ']')
          charList1.Add('[');
        else if (letters[index1] != char.MaxValue)
          charList1.Add(letters[index1]);
      }
      else if (letters[index1] == ' ' && index1 > 0 && index1 < letters.Length - 1 && (char.IsLower(letters[index1 - 1]) || char.IsUpper(letters[index1 - 1]) || char.IsNumber(letters[index1 - 1])) && (char.IsLower(letters[index1 + 1]) || char.IsUpper(letters[index1 + 1]) || char.IsNumber(letters[index1 + 1])))
        charList2.Add(letters[index1]);
      else if (char.IsNumber(letters[index1]) || char.IsLower(letters[index1]) || char.IsUpper(letters[index1]) || char.IsSymbol(letters[index1]) || char.IsPunctuation(letters[index1]))
      {
        if (letters[index1] == '(')
          charList2.Add(')');
        else if (letters[index1] == ')')
          charList2.Add('(');
        else if (letters[index1] == '<')
          charList2.Add('>');
        else if (letters[index1] == '>')
          charList2.Add('<');
        else if (letters[index1] == '[')
          charList1.Add(']');
        else if (letters[index1] == ']')
          charList1.Add('[');
        else
          charList2.Add(letters[index1]);
      }
      else if (letters[index1] >= '\uD800' && letters[index1] <= '\uDBFF' || letters[index1] >= '\uDC00' && letters[index1] <= '\uDFFF')
      {
        charList2.Add(letters[index1]);
      }
      else
      {
        if (charList2.Count > 0)
        {
          for (int index2 = 0; index2 < charList2.Count; ++index2)
            charList1.Add(charList2[charList2.Count - 1 - index2]);
          charList2.Clear();
        }
        if (letters[index1] != char.MaxValue)
          charList1.Add(letters[index1]);
      }
    }
    if (charList2.Count > 0)
    {
      for (int index = 0; index < charList2.Count; ++index)
        charList1.Add(charList2[charList2.Count - 1 - index]);
      charList2.Clear();
    }
    char[] chArray = new char[charList1.Count];
    for (int index = 0; index < chArray.Length; ++index)
      chArray[index] = charList1[index];
    str = new string(chArray);
    return str;
  }

  internal static bool IsIgnoredCharacter(char ch)
  {
    int num1 = char.IsPunctuation(ch) ? 1 : 0;
    bool flag1 = char.IsNumber(ch);
    bool flag2 = char.IsLower(ch);
    bool flag3 = char.IsUpper(ch);
    bool flag4 = char.IsSymbol(ch);
    bool flag5 = ch == 'ﭖ' || ch == 'ﭺ' || ch == 'ﮊ' || ch == 'ﮒ' || ch == 'ﮎ';
    bool flag6 = ((ch > '\uFEFF' ? 0 : (ch >= 'ﹰ' ? 1 : 0)) | (flag5 ? 1 : 0)) != 0 || ch == 'ﯼ';
    int num2 = flag1 ? 1 : 0;
    return (num1 | num2 | (flag2 ? 1 : 0) | (flag3 ? 1 : 0) | (flag4 ? 1 : 0)) != 0 || !flag6 || ch == 'a' || ch == '>' || ch == '<' || ch == '؛';
  }

  internal static bool IsLeadingLetter(char[] letters, int index)
  {
    int num1 = index == 0 || letters[index - 1] == ' ' || letters[index - 1] == '*' || letters[index - 1] == 'A' || char.IsPunctuation(letters[index - 1]) || letters[index - 1] == '>' || letters[index - 1] == '<' || letters[index - 1] == 'ﺍ' || letters[index - 1] == 'ﺩ' || letters[index - 1] == 'ﺫ' || letters[index - 1] == 'ﺭ' || letters[index - 1] == 'ﺯ' || letters[index - 1] == 'ﮊ' || letters[index - 1] == 'ﻭ' || letters[index - 1] == 'ﺁ' || letters[index - 1] == 'ﺃ' || letters[index - 1] == 'ﺇ' ? 1 : (letters[index - 1] == 'ﺅ' ? 1 : 0);
    bool flag1 = letters[index] != ' ' && letters[index] != 'ﺩ' && letters[index] != 'ﺫ' && letters[index] != 'ﺭ' && letters[index] != 'ﺯ' && letters[index] != 'ﮊ' && letters[index] != 'ﺍ' && letters[index] != 'ﺃ' && letters[index] != 'ﺇ' && letters[index] != 'ﺁ' && letters[index] != 'ﺅ' && letters[index] != 'ﻭ' && letters[index] != 'ﺀ';
    bool flag2 = index < letters.Length - 1 && letters[index + 1] != ' ' && !char.IsPunctuation(letters[index + 1]) && !char.IsNumber(letters[index + 1]) && !char.IsSymbol(letters[index + 1]) && !char.IsLower(letters[index + 1]) && !char.IsUpper(letters[index + 1]) && letters[index + 1] != 'ﺀ';
    int num2 = flag1 ? 1 : 0;
    return (num1 & num2 & (flag2 ? 1 : 0)) != 0;
  }

  internal static bool IsFinishingLetter(char[] letters, int index)
  {
    return ((index == 0 ? 0 : (letters[index - 1] == ' ' || letters[index - 1] == 'ﺩ' || letters[index - 1] == 'ﺫ' || letters[index - 1] == 'ﺭ' || letters[index - 1] == 'ﺯ' || letters[index - 1] == 'ﮊ' || letters[index - 1] == 'ﻭ' || letters[index - 1] == 'ﺍ' || letters[index - 1] == 'ﺁ' || letters[index - 1] == 'ﺃ' || letters[index - 1] == 'ﺇ' || letters[index - 1] == 'ﺅ' || letters[index - 1] == 'ﺀ' || char.IsPunctuation(letters[index - 1]) || letters[index - 1] == '>' ? 0 : (letters[index - 1] != '<' ? 1 : 0))) & (letters[index] == ' ' ? (false ? 1 : 0) : (letters[index] != 'ﺀ' ? 1 : 0))) != 0;
  }

  internal static bool IsMiddleLetter(char[] letters, int index)
  {
    bool flag1 = index != 0 && letters[index] != 'ﺍ' && letters[index] != 'ﺩ' && letters[index] != 'ﺫ' && letters[index] != 'ﺭ' && letters[index] != 'ﺯ' && letters[index] != 'ﮊ' && letters[index] != 'ﻭ' && letters[index] != 'ﺁ' && letters[index] != 'ﺃ' && letters[index] != 'ﺇ' && letters[index] != 'ﺅ' && letters[index] != 'ﺀ';
    bool flag2 = index != 0 && letters[index - 1] != 'ﺍ' && letters[index - 1] != 'ﺩ' && letters[index - 1] != 'ﺫ' && letters[index - 1] != 'ﺭ' && letters[index - 1] != 'ﺯ' && letters[index - 1] != 'ﮊ' && letters[index - 1] != 'ﻭ' && letters[index - 1] != 'ﺁ' && letters[index - 1] != 'ﺃ' && letters[index - 1] != 'ﺇ' && letters[index - 1] != 'ﺅ' && letters[index - 1] != 'ﺀ' && !char.IsPunctuation(letters[index - 1]) && letters[index - 1] != '>' && letters[index - 1] != '<' && letters[index - 1] != ' ' && letters[index - 1] != '*';
    if (((index >= letters.Length - 1 ? 0 : (letters[index + 1] == ' ' || letters[index + 1] == '\r' || letters[index + 1] == 'ﺀ' || char.IsNumber(letters[index + 1]) || char.IsSymbol(letters[index + 1]) ? 0 : (!char.IsPunctuation(letters[index + 1]) ? 1 : 0))) & (flag2 ? 1 : 0) & (flag1 ? 1 : 0)) == 0)
      return false;
    try
    {
      return !char.IsPunctuation(letters[index + 1]);
    }
    catch
    {
      return false;
    }
  }
}
