// Decompiled with JetBrains decompiler
// Type: DialogGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DialogGUI : BaseGUI
{
  public DialogButtonsGUI _dialog_buttons;
  public UILabel label_1;
  public UILabel label_2;
  public UILabel label_3;
  public UILabel stars_1;
  public UILabel stars_2;
  public bool _run_second_option_by_pressed_back;
  public UITable table;
  public GameObject item_container;
  public GameObject button_separator;
  public GameObject ingredients_go;
  public BaseItemCellGUI[] _ingredients;

  public override void Init()
  {
    this._dialog_buttons = this.GetComponentInChildren<DialogButtonsGUI>(true);
    if ((Object) this._dialog_buttons != (Object) null)
      this._dialog_buttons.Init();
    if ((Object) this.ingredients_go != (Object) null)
      this._ingredients = this.ingredients_go.GetComponentsInChildren<BaseItemCellGUI>(true);
    this.Deactivate<DialogGUI>();
    base.Init();
  }

  public void OpenYesNo(
    string text,
    GJCommons.VoidDelegate delegate_1,
    GJCommons.VoidDelegate delegate_2 = null,
    GJCommons.VoidDelegate on_hide = null)
  {
    this.Open(text, GJL.L("yes"), delegate_1, GJL.L("no"), delegate_2, on_hide);
  }

  public void OpenOK(
    string text,
    GJCommons.VoidDelegate on_hide = null,
    string text2 = "",
    bool separate_with_stars = false,
    string text3 = "")
  {
    this.Open(text, GJL.L("ok"), (GJCommons.VoidDelegate) null, on_hide: on_hide, text2: text2, separate_with_stars: separate_with_stars, text3: text3);
  }

  public void Open(
    string text,
    string option_1,
    GJCommons.VoidDelegate delegate_1,
    string option_2 = null,
    GJCommons.VoidDelegate delegate_2 = null,
    GJCommons.VoidDelegate on_hide = null,
    GameKey key_1 = GameKey.Select,
    GameKey key_2 = GameKey.Back,
    string text2 = "",
    bool separate_with_stars = false,
    string text3 = "")
  {
    this.OpenDialog(text, option_1, delegate_1, option_2, delegate_2, on_hide, key_1, key_2, text2, separate_with_stars: separate_with_stars, text3: text3);
  }

  public void OpenDialog(
    string text,
    string option_1,
    GJCommons.VoidDelegate delegate_1,
    string option_2 = null,
    GJCommons.VoidDelegate delegate_2 = null,
    GJCommons.VoidDelegate on_hide = null,
    GameKey key_1 = GameKey.Select,
    GameKey key_2 = GameKey.Back,
    string text2 = "",
    Item item = null,
    bool separate_with_stars = false,
    string text3 = "")
  {
    this.Open();
    if ((Object) this.label_1 != (Object) null)
      this.label_1.text = LocalizedLabel.ColorizeTags(GJL.L(text), LocalizedLabel.TextColor.Tutorial);
    if ((Object) this.label_2 != (Object) null)
    {
      this.label_2.text = GJL.L(text2);
      this.label_2.gameObject.SetActive(!string.IsNullOrEmpty(text2));
    }
    if ((Object) this.label_3 != (Object) null)
    {
      this.label_3.text = GJL.L(text3);
      this.label_3.gameObject.SetActive(!string.IsNullOrEmpty(text3));
    }
    if ((Object) this.stars_1 != (Object) null)
      this.stars_1.gameObject.SetActive(separate_with_stars && !string.IsNullOrEmpty(text2));
    if ((Object) this.stars_2 != (Object) null)
      this.stars_2.gameObject.SetActive(separate_with_stars && !string.IsNullOrEmpty(text3));
    if ((Object) this.button_separator != (Object) null)
      this.button_separator.SetActive(!string.IsNullOrEmpty(text2));
    if ((Object) this.ingredients_go != (Object) null)
      this.ingredients_go.SetActive(false);
    if ((Object) this.item_container != (Object) null)
    {
      this.item_container.SetActive(item != null);
      if (item != null)
      {
        if (item.inventory != null && item.inventory.Count > 0)
        {
          this.ingredients_go.SetActive(true);
          this.item_container.SetActive(false);
          BaseItemCellGUI.DrawIngredients(this._ingredients, item.inventory, MainGame.me.player.GetMultiInventory(sortWGOS: true));
          this.ingredients_go.GetComponentInChildren<UITableOrGrid>().Reposition();
        }
        else
          this.item_container.GetComponent<BaseItemCellGUI>().DrawIngredient(item, MainGame.me.player.GetMultiInventory(sortWGOS: true));
      }
    }
    if ((Object) this._dialog_buttons != (Object) null)
      this._dialog_buttons.Set(option_1, (GJCommons.VoidDelegate) (() =>
      {
        this.Hide();
        on_hide.TryInvoke();
        delegate_1.TryInvoke();
      }), option_2, (GJCommons.VoidDelegate) (() =>
      {
        if (!this.is_shown)
          return;
        this.Hide();
        on_hide.TryInvoke();
        delegate_2.TryInvoke();
      }), key_1: key_1, key_2: key_2);
    if ((Object) this.table != (Object) null)
      this.table.Reposition();
    this.UpdateAllAnchors();
    foreach (DialogButtonGUI componentsInChild in this.gameObject.GetComponentsInChildren<DialogButtonGUI>(true))
      componentsInChild.RestoreHeight();
  }
}
