// Decompiled with JetBrains decompiler
// Type: BuffIcon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BuffIcon : MonoBehaviour
{
  public UI2DSprite icon;
  public UILabel txt_timer;
  public bool show_timer;
  [NonSerialized]
  public PlayerBuff linked_buff;

  public void Draw(PlayerBuff buff)
  {
    this.linked_buff = buff;
    this.icon.sprite2D = EasySpritesCollection.GetSprite(buff.definition.GetIconName());
    this.show_timer = !this.linked_buff.definition.do_not_show_timer;
    this.Redraw();
  }

  public void Redraw()
  {
    if (this.show_timer)
    {
      string timerText = this.linked_buff.GetTimerText();
      if (!(timerText != this.txt_timer.text))
        return;
      this.txt_timer.text = timerText;
    }
    else
      this.txt_timer.text = string.Empty;
  }
}
