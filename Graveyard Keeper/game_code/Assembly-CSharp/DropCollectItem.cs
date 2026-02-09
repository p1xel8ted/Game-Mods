// Decompiled with JetBrains decompiler
// Type: DropCollectItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DropCollectItem : MonoBehaviour
{
  public const float LIFETIME = 3.5f;
  public float time;
  public string item_id;
  public int n;
  public ItemDefinition _definition;
  public UILabel txt_num;
  public BaseItemCellGUI item_icon;
  public bool _is_tech_point;
  public bool _is_money;
  public UILabel txt_right;
  public bool show_counter_if_one;
  public bool show_counter_x;

  public void Redraw()
  {
    this._is_tech_point = TechDefinition.TECH_POINTS.Contains(this.item_id);
    this._is_money = this.item_id == "money";
    this.item_icon.gameObject.SetActive(true);
    this.txt_right.gameObject.SetActive(false);
    if (!this._is_tech_point && !this._is_money)
    {
      this._definition = GameBalance.me.GetData<ItemDefinition>(this.item_id);
      if (this._definition == null)
      {
        Debug.LogError((object) ("Item definition not found in balance: " + this.item_id), (Object) this);
        return;
      }
    }
    if (!this._is_money)
      this.item_icon.DrawIcon(this._definition.GetIcon());
    GJL.EnsureLabelHasCorrectFont(this.item_icon.item_name, false);
    if (this._is_tech_point)
      this.item_icon.item_name.text = GJL.L("tech_point_" + this.item_id);
    else if (this._is_money)
    {
      this.item_icon.gameObject.SetActive(false);
      this.txt_right.gameObject.SetActive(true);
      this.txt_right.text = Trading.FormatMoney((float) this.n / 100f, true, false);
      this.item_icon.item_name.text = GJL.L("Money") + "     ";
    }
    else
      this.item_icon.item_name.text = this._definition.GetItemName();
    if (this._is_money)
      return;
    this._definition.TryDrawQualityOrDisableGameObject(this.item_icon.quality_icon);
    if (!this.show_counter_if_one && this.n == 1)
      this.txt_num.text = "";
    else
      this.txt_num.text = (this.show_counter_x ? "x" : "") + this.n.ToString();
  }

  public void AddMoreItems(int amount)
  {
    this.time = 0.0f;
    this.n += amount;
    this.Redraw();
  }

  public void Draw(Item i)
  {
    this.item_id = i.id;
    this.n = i.value;
    this.Redraw();
    DropCollectGUI.RedrawGrid(this.transform);
  }

  public void Update()
  {
    this.time += Time.deltaTime;
    if ((double) this.time <= 3.5)
      return;
    DropCollectGUI.Despawn(this);
  }
}
