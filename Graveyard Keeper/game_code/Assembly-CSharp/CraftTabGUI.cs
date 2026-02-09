// Decompiled with JetBrains decompiler
// Type: CraftTabGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CraftTabGUI : MonoBehaviour
{
  public string tab_id;
  public Action<string> _on_clicked_delegate;
  public UIChangingSprite tab_back;
  public UIButton button;
  public Color clr_normal;
  public Color clr_selected;
  public UI2DSprite spr_icon;

  public void OnTabClicked()
  {
    if (this._on_clicked_delegate == null)
      return;
    this._on_clicked_delegate(this.tab_id);
  }

  public void Draw(WorldGameObject craftery_wgo, string tab_id, Action<string> on_clicked_delegate)
  {
    this.tab_id = tab_id;
    if (string.IsNullOrEmpty(tab_id))
      tab_id = "other";
    string sprite_name = "tab_" + tab_id;
    if (this.tab_id.StartsWith("?"))
      sprite_name = "tab_custom_" + tab_id.Substring(1);
    this.spr_icon.sprite2D = EasySpritesCollection.GetSprite(sprite_name);
    if ((UnityEngine.Object) this.spr_icon.sprite2D == (UnityEngine.Object) null)
      this.spr_icon.sprite2D = EasySpritesCollection.GetSprite("tab_other");
    this._on_clicked_delegate = on_clicked_delegate;
  }

  public void SetSelectedState(bool is_selected)
  {
    this.spr_icon.color = is_selected ? this.clr_selected : this.clr_normal;
    this.tab_back.ChangeSprite(is_selected ? 0 : 1);
    this.button.normalSprite2D = this.tab_back.GetComponent<UI2DSprite>().sprite2D;
  }
}
