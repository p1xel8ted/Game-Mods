// Decompiled with JetBrains decompiler
// Type: RatBuffItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RatBuffItemGUI : MonoBehaviour
{
  public UILabel txt_header;
  public UILabel txt_descr;
  public UILabel txt_timer;
  public UI2DSprite buff_icon;
  public Item buff;
  public bool buff_is_null;
  public string buff_id;

  public void Init()
  {
    this.buff = (Item) null;
    this.buff_is_null = true;
  }

  public void Draw(Item rat_buff)
  {
    this.buff = rat_buff;
    this.buff_is_null = this.buff == null;
    this.buff_id = this.buff_is_null ? string.Empty : this.buff.id;
    this.txt_header.text = this.buff_is_null ? "" : this.buff.definition.GetItemName();
    this.txt_descr.text = this.buff_is_null ? "" : this.buff.definition.GetItemDescription(this.buff);
    this.buff_icon.sprite2D = this.buff_is_null ? (UnityEngine.Sprite) null : EasySpritesCollection.GetSprite(this.buff.definition.GetIcon());
  }

  public void Redraw()
  {
    if (this.buff_is_null)
      return;
    if (this.buff == null)
    {
      this.buff_is_null = true;
      if (string.IsNullOrEmpty(this.buff_id))
        return;
      this.Draw((Item) null);
      GUIElements.me.rat_cell_gui.Redraw();
    }
    else
    {
      if (this.buff.id != this.buff_id)
        this.Draw(this.buff);
      if (this.buff.definition.has_durability)
      {
        int num1 = (int) ((double) this.buff.durability / (double) this.buff.definition.durability_decrease);
        int num2 = num1 % 60;
        this.txt_timer.text = $"{(num1 / 60).ToString()}:{(num2 < 10 ? "0" : "")}{num2.ToString()}";
      }
      else
        this.txt_timer.text = string.Empty;
    }
  }

  public void Update() => this.Redraw();
}
