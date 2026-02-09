// Decompiled with JetBrains decompiler
// Type: CharacterSkin
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Text;

#nullable disable
[Serializable]
public class CharacterSkin
{
  public int weapon = 1;
  public int head = 1;
  public int body = 1;
  public int legs = 1;
  public static StringBuilder _sb = new StringBuilder();

  public string ReplaceSpriteName(string s)
  {
    if (s.Length < 7)
      return s;
    string str = s.Substring(4, 3);
    int num = -1;
    switch (str)
    {
      case "wpn":
        num = this.weapon;
        break;
    }
    if (num == -1)
      return s;
    if (num == 0)
      return "transparent_2x2";
    CharacterSkin._sb.Length = 0;
    CharacterSkin._sb.Append(num.ToString("000"));
    CharacterSkin._sb.Append(s.Substring(3));
    return CharacterSkin._sb.ToString();
  }
}
