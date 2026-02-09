// Decompiled with JetBrains decompiler
// Type: Inventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class Inventory
{
  public Item _data;
  public string _name;
  public string _preset;
  public bool _name_is_set;
  public bool _is_player;
  public string _obj_id;
  public bool is_locked;
  public Inventory.VendorTierInfo vendor_tier_info;

  public Item data => this._data;

  public int size => (int) this.data.GetParam("inventory_size");

  public string preset => this._preset;

  public string name
  {
    get
    {
      if (!this._name_is_set)
      {
        this._name = !this._is_player ? this._obj_id + "_inventory" : "player_inventory";
        this._name_is_set = true;
      }
      return GJL.L(this._name);
    }
  }

  public Inventory(Item data, string name = "", string preset = "")
  {
    this._data = data;
    this._name = name;
    this._preset = preset;
    this._name_is_set = true;
    this._obj_id = string.Empty;
  }

  public Inventory(WorldGameObject obj)
  {
    this._data = obj.data;
    this._name_is_set = false;
    this._obj_id = obj.obj_id;
    this._is_player = obj.is_player;
    this._preset = obj.is_player ? string.Empty : obj.obj_def.inventory_preset;
  }

  public void ClearName()
  {
    this._name = string.Empty;
    this._name_is_set = true;
  }

  public bool IsTavernPalette() => this._obj_id == "tavern_cellar_rack";

  public class VendorTierInfo
  {
    public int tier_1;
    public int tier_2;
    public float progress;
    public bool progressbar_visible;
  }
}
