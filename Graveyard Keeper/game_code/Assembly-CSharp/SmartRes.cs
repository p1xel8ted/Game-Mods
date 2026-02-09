// Decompiled with JetBrains decompiler
// Type: SmartRes
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartRes
{
  public SmartRes.ResType res_type;
  public Item item;
  [SerializeField]
  public GameResAtom _res;
  public WorldGameObject _linked_wgo;

  public void FillVisualData(
    out string icon_name,
    out int n,
    out string quality_icon,
    WorldGameObject linked_wgo = null)
  {
    quality_icon = icon_name = "";
    n = 0;
    this._linked_wgo = linked_wgo;
    switch (this.res_type)
    {
      case SmartRes.ResType.Empty:
        icon_name = "";
        n = 0;
        break;
      case SmartRes.ResType.Item:
        ItemDefinition data = GameBalance.me.GetData<ItemDefinition>(this.item.id);
        icon_name = data == null ? "i_" + this.item.id : data.GetIcon();
        if (data != null)
          quality_icon = data.GetQualityIconName();
        n = this.item.value;
        break;
      case SmartRes.ResType.GameRes:
        icon_name = "res_" + this._res.type;
        n = Mathf.RoundToInt(this._res.value);
        if (this._res.type == "_rel")
          icon_name = (n > 0 ? ":+(positive)" : ":-(negative)") + Mathf.Abs(n).ToString();
        if (this._res.type == "money")
          icon_name = ":+" + Trading.FormatMoney(this._res.value, use_spaces: false);
        if (!(this._res.type == "r") && !(this._res.type == "b") && !(this._res.type == "g"))
          break;
        icon_name = $":+({this._res.type}){this._res.value.ToString()}";
        break;
    }
  }

  public GameResAtom res
  {
    get
    {
      return this._res.type == "_rel" && (UnityEngine.Object) this._linked_wgo != (UnityEngine.Object) null ? new GameResAtom("_rel_" + (string.IsNullOrEmpty(this._linked_wgo.obj_def.npc_alias) ? this._linked_wgo.obj_id : this._linked_wgo.obj_def.npc_alias), this._res.value) : this._res;
    }
    set => this._res = value;
  }

  public override string ToString()
  {
    return $"[SmartRes {this.res_type}, item={this.item}, res={this._res}]";
  }

  public enum ResType
  {
    Empty,
    Item,
    GameRes,
  }
}
