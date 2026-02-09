// Decompiled with JetBrains decompiler
// Type: LocalizedLabel
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LocalizedLabel : MonoBehaviour
{
  public string token = "";
  public LocalizedLabel.TextColor text_color;
  public string _lng_token = "";
  public UILabel _label;
  public bool _initialized;
  [Space(15f)]
  public string prefix = "";
  public GamePadButton gamepad_button_prefix;
  [Space(15f)]
  public List<GameKey> replace_params_with_keys = new List<GameKey>();
  public int _remembered_line_spacing;
  [Space(10f)]
  public bool modify_line_spacing_for_gamepad;
  public int gamepad_line_spacing = -2;

  public static string ColorizeTags(string s, LocalizedLabel.TextColor color)
  {
    string str = "FFFFFF";
    switch (color)
    {
      case LocalizedLabel.TextColor.Tutorial:
      case LocalizedLabel.TextColor.Task:
        str = "FFBD00";
        break;
      case LocalizedLabel.TextColor.SpeechBubble:
        str = "bb5c1c";
        break;
    }
    if (s.Contains("<") && s.Contains(">"))
      s = s.Replace("<", $"[c][{str}]").Replace(">", "[-][/c]");
    return s;
  }

  public void Localize()
  {
    if (!this._initialized)
    {
      this._label = this.GetComponent<UILabel>();
      this._remembered_line_spacing = this._label.spacingY;
      this._initialized = true;
      this._lng_token = this.token;
      if (string.IsNullOrEmpty(this._lng_token))
        this._lng_token = this._label.text;
    }
    if (string.IsNullOrEmpty(this._lng_token))
    {
      Debug.LogError((object) "LocalizedLabel token is empty", (Object) this.gameObject);
    }
    else
    {
      string str = this.prefix + PlatformSpecific.GetGamepadButtonSymbol(this.gamepad_button_prefix) + LocalizedLabel.ColorizeTags(GJL.L(this._lng_token), this.text_color);
      int num = 0;
      foreach (GameKey replaceParamsWithKey in this.replace_params_with_keys)
      {
        ++num;
        str = str.Replace("%" + num.ToString(), GameKeyTip.GetIcon(replaceParamsWithKey));
      }
      this._label.text = str;
      if (this.modify_line_spacing_for_gamepad)
        this._label.spacingY = LazyInput.gamepad_active ? this.gamepad_line_spacing : this._remembered_line_spacing;
      GJL.EnsureLabelHasCorrectFont(this._label);
    }
  }

  public enum TextColor
  {
    Default,
    Tutorial,
    SpeechBubble,
    Task,
  }
}
