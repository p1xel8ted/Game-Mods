// Decompiled with JetBrains decompiler
// Type: PerkBuffItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PerkBuffItemGUI : MonoBehaviour
{
  public UIWidget widget;
  public GameObject go_perk;
  public GameObject go_buff;
  public UILabel txt_header;
  public UILabel txt_descr;
  public UILabel txt_timer;
  public UI2DSprite icon_perk;
  public UI2DSprite icon_buff;
  public bool _is_buff;
  [NonSerialized]
  public PlayerBuff linked_buff;
  public int _default_hgt;

  public void Draw(PlayerBuff buff)
  {
    this.go_buff.SetActive(true);
    this.go_perk.SetActive(false);
    this._is_buff = true;
    this.linked_buff = buff;
    this.txt_header.text = buff.definition.GetLocalizedName();
    this.txt_descr.text = buff.definition.GetDescriptionIfExists();
    this.icon_buff.sprite2D = EasySpritesCollection.GetSprite(buff.definition.GetIconName());
    this.RecalculateDescriptionHeight();
    this.GetComponentInChildren<SimpleUITable>().Reposition();
    this.Redraw();
  }

  public void Draw(PerkDefinition perk)
  {
    this.go_buff.SetActive(false);
    this.go_perk.SetActive(true);
    this._is_buff = false;
    this.linked_buff = (PlayerBuff) null;
    this.txt_header.text = GJL.L(perk.id);
    this.txt_descr.text = perk.GetDescriptionIfExists();
    this.icon_perk.sprite2D = EasySpritesCollection.GetSprite(perk.GetIcon());
    this.RecalculateDescriptionHeight();
    this.GetComponentInChildren<SimpleUITable>().Reposition();
    this.Redraw();
  }

  public void RecalculateDescriptionHeight()
  {
    this.txt_descr.ProcessText();
    int num = Mathf.RoundToInt(this.txt_descr.localSize.y - 20f);
    if (num <= 0)
      return;
    if (this._default_hgt == 0)
      this._default_hgt = this.widget.height;
    this.widget.height = this._default_hgt + num;
    this.icon_perk.transform.localPosition = new Vector3(this.icon_perk.transform.localPosition.x, (float) (num / 2));
  }

  public void Redraw()
  {
    string str = this.txt_timer.text;
    if (this._is_buff)
      str = !this.linked_buff.definition.do_not_show_timer ? this.linked_buff.GetTimerText() : string.Empty;
    if (!(str != this.txt_timer.text))
      return;
    this.txt_timer.text = str;
  }

  public void Update() => this.Redraw();
}
