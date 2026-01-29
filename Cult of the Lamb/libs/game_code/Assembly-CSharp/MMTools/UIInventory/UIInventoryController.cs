// Decompiled with JetBrains decompiler
// Type: MMTools.UIInventory.UIInventoryController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;
using Unify.Input;
using UnityEngine;

#nullable disable
namespace MMTools.UIInventory;

public class UIInventoryController : BaseMonoBehaviour
{
  public System.Action Callback;
  [HideInInspector]
  public float PauseTimeSpeed = 0.005f;
  public UIInventoryList ItemsList;
  public Player _RewiredController;
  public UIInventoryItem CurrentSelection;
  public UIInventoryItem CurrentHighlighted;
  public MMTools.UIInventory.UIInventoryListSelector CurrentSelectionGroup;
  public MMTools.UIInventory.UIInventoryListSelector[] UIInventoryListSelector;

  public void Start()
  {
    GameManager.SetTimeScale(this.PauseTimeSpeed);
    this.StartUIInventoryController();
  }

  [HideInInspector]
  public Player RewiredController
  {
    get
    {
      if (this._RewiredController == null)
        this._RewiredController = RewiredInputManager.MainPlayer;
      return this._RewiredController;
    }
  }

  public virtual void StartUIInventoryController()
  {
    InventoryItem inventoryItem1 = new InventoryItem();
    inventoryItem1.Init(14, 1);
    this.UpdateCurrentSelection(inventoryItem1);
    this.UpdateCurrentHighlighted(inventoryItem1);
    List<InventoryItem> ItemsToPopulate = new List<InventoryItem>();
    InventoryItem inventoryItem2 = new InventoryItem();
    inventoryItem2.Init(14, 1);
    ItemsToPopulate.Add(inventoryItem2);
    InventoryItem inventoryItem3 = new InventoryItem();
    inventoryItem3.Init(9, 1);
    ItemsToPopulate.Add(inventoryItem3);
    InventoryItem inventoryItem4 = new InventoryItem();
    inventoryItem4.Init(11, 1);
    ItemsToPopulate.Add(inventoryItem4);
    this.ItemsList.PopulateList(ItemsToPopulate);
    this.SelectionManagementStart(0);
  }

  public void UpdateCurrentSelection(InventoryItem item)
  {
    if (!((UnityEngine.Object) this.CurrentSelection != (UnityEngine.Object) null))
      return;
    this.CurrentSelection.Init(item);
  }

  public void UpdateCurrentHighlighted(InventoryItem item)
  {
    if (!((UnityEngine.Object) this.CurrentHighlighted != (UnityEngine.Object) null))
      return;
    this.CurrentHighlighted.Init(item);
  }

  public void SelectionManagementStart(int Selection)
  {
    this.CurrentSelectionGroup = this.UIInventoryListSelector[0];
    this.CurrentSelectionGroup.SetActive(Selection);
    this.CurrentSelectionGroup.OnMove += new MMTools.UIInventory.UIInventoryListSelector.ListSelectorAction(this.OnMove);
    this.CurrentSelectionGroup.OnSelect += new MMTools.UIInventory.UIInventoryListSelector.ListSelectorAction(this.OnSelect);
  }

  public virtual void OnMove(InventoryItem Item) => this.CurrentHighlighted.Init(Item);

  public virtual void OnSelect(InventoryItem Item) => this.CurrentSelection.Init(Item);

  public void Update() => this.UpdateUIInventoryController();

  public virtual void UpdateUIInventoryController()
  {
    if (this.UIInventoryListSelector.Length == 0)
      return;
    if ((UnityEngine.Object) this.CurrentSelectionGroup != (UnityEngine.Object) null)
      this.CurrentSelectionGroup.DoUpdate(this.RewiredController);
    if (!((UnityEngine.Object) this.CurrentSelectionGroup == (UnityEngine.Object) this.UIInventoryListSelector[0]) || !InputManager.UI.GetCancelButtonDown())
      return;
    this.Close();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void GetSelectorGroups()
  {
    this.UIInventoryListSelector = this.GetComponentsInChildren<MMTools.UIInventory.UIInventoryListSelector>();
  }

  public virtual void Close()
  {
    if (this.Callback != null)
      this.Callback();
    GameManager.SetTimeScale(1f);
  }
}
