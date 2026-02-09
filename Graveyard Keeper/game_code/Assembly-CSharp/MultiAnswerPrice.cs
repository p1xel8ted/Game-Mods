// Decompiled with JetBrains decompiler
// Type: MultiAnswerPrice
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MultiAnswerPrice : MonoBehaviour
{
  public UI2DSprite back;
  public UI2DSprite price_icon;
  public UIWidget price_widget;
  public UILabel price_label_n;
  public UI2DSprite price_quality_icon;
  public UIWidget price_lock_available;
  public UIWidget price_lock_locked;
  public UnityEngine.Sprite default_back_spr;
  public UnityEngine.Sprite second_back_spr;

  public void SetBack(bool default_back)
  {
    this.back.sprite2D = default_back ? this.default_back_spr : this.second_back_spr;
  }
}
