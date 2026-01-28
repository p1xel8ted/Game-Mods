// Decompiled with JetBrains decompiler
// Type: UI_SpellSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools.UIInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UI_SpellSelect : UIInventoryController
{
  public PlayerSpells playerSpells;
  public bool Closing;

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

  public IEnumerator CloseRoutine()
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
