// Decompiled with JetBrains decompiler
// Type: ButtonTipsStr
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class ButtonTipsStr : MonoBehaviour
{
  public UILabel label;

  public void Start()
  {
    if (!((Object) this.label == (Object) null))
      return;
    this.label = this.GetComponent<UILabel>();
  }

  public void Clear() => this.label.text = "";

  public void Print(params GameKeyTip[] tips)
  {
    this.Print(((IEnumerable<GameKeyTip>) tips).ToList<GameKeyTip>());
  }

  public void Print(string separator, params GameKeyTip[] tips)
  {
    this.Print(((IEnumerable<GameKeyTip>) tips).ToList<GameKeyTip>(), separator);
  }

  public void Print(List<GameKeyTip> tips, string separator = "   ")
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < tips.Count; ++index)
    {
      string str = tips[index].ToString();
      stringBuilder.Append(str);
      if (str.Length > 0 && index < tips.Count - 1)
        stringBuilder.Append(separator);
    }
    if (!((Object) this.label != (Object) null))
      return;
    this.label.text = stringBuilder.ToString();
  }

  public void Print(GameKeyTip tip) => this.label.text = tip.ToString();

  public void PrintClose(bool gamepad_only = true)
  {
  }

  public void PrintBack(bool gamepad_only = true)
  {
  }
}
