// Decompiled with JetBrains decompiler
// Type: UI_SpellSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools.UIInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UI_SpellSelect : UIInventoryController
{
  public PlayerSpells playerSpells;
  private bool Closing;

  public static void Play(System.Action Callback, PlayerSpells playerSpells)
  {
    UI_SpellSelect component = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("MMUIInventory/UI Spell Select"), GlobalCanvasReference.Instance).GetComponent<UI_SpellSelect>();
    component.playerSpells = playerSpells;
    component.Callback = Callback;
  }

  public override void StartUIInventoryController()
  {
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    List<InventoryItem> ItemsToPopulate = new List<InventoryItem>();
    int Selection = 0;
    this.ItemsList.PopulateList(ItemsToPopulate);
    this.SelectionManagementStart(Selection);
  }

  public override void UpdateUIInventoryController()
  {
    if (this.Closing)
      return;
    base.UpdateUIInventoryController();
  }

  private IEnumerator CloseRoutine()
  {
    UI_SpellSelect uiSpellSelect = this;
    uiSpellSelect.Closing = true;
    float Timer = 0.0f;
    while ((double) (Timer += Time.unscaledDeltaTime) < 0.5)
      yield return (object) null;
    if (uiSpellSelect.Callback != null)
      uiSpellSelect.Callback();
    uiSpellSelect.Close();
    UnityEngine.Object.Destroy((UnityEngine.Object) uiSpellSelect.gameObject);
  }
}
