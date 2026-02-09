// Decompiled with JetBrains decompiler
// Type: FishingDistanceChoosingBarItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FishingDistanceChoosingBarItemGUI : MonoBehaviour
{
  public UI2DSprite icon;
  public UILabel label;

  public void SetFishItem(string sprite_name, int chance)
  {
    this.icon.sprite2D = EasySpritesCollection.GetSprite(sprite_name);
    this.label.text = chance.ToString() + "%";
  }
}
