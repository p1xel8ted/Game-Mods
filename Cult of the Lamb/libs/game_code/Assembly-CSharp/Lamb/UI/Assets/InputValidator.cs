// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.InputValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "InputValidator", menuName = "Massive Monster/InputValidator", order = 100)]
[Serializable]
public class InputValidator : TMP_InputValidator
{
  public int MaxCharacters = 14;

  public override char Validate(ref string text, ref int pos, char ch)
  {
    if (text.Length >= this.MaxCharacters)
      return ch;
    string pattern = "[^\\p{Cc}\\p{Cn}\\p{Cs}]";
    if (!Regex.Match(ch.ToString(), pattern, RegexOptions.IgnoreCase).Success)
      return char.MinValue;
    ++pos;
    text += ch.ToString();
    return ch;
  }
}
