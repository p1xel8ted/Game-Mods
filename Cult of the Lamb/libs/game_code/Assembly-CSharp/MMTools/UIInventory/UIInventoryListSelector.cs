// Decompiled with JetBrains decompiler
// Type: MMTools.UIInventory.UIInventoryListSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace MMTools.UIInventory;

public class UIInventoryListSelector : BaseMonoBehaviour
{
  public UIInventoryListSelector.ListSelectorAction OnMove;
  public UIInventoryListSelector.ListSelectorAction OnSelect;
  public UIInventoryList List;
  public Image Selector;
  public float selectionDelay;
  public Vector3 SelectorTargetPosition;
  public int _CurrentSelection;

  public int CURRENT_SELECTION
  {
    get => this._CurrentSelection;
    set
    {
      this.selectionDelay = 0.25f;
      this._CurrentSelection = value;
      if (this.List.Items.Count <= 0)
        return;
      while (this._CurrentSelection > this.List.Items.Count - 1)
        this._CurrentSelection -= this.List.Items.Count;
      while (this._CurrentSelection < 0)
        this._CurrentSelection += this.List.Items.Count;
      this.SelectorTargetPosition = this.List.Items[this.CURRENT_SELECTION].rectTransform.position;
      UIInventoryListSelector.ListSelectorAction onMove = this.OnMove;
      if (onMove == null)
        return;
      onMove(this.List.Items[this.CURRENT_SELECTION].Item);
    }
  }

  public void OnEnable()
  {
    this.Selector.enabled = false;
    this.List = this.GetComponent<UIInventoryList>();
  }

  public void SetActive(int Selection)
  {
    this.CURRENT_SELECTION = Selection;
    this.Selector.enabled = true;
    this.Selector.transform.position = this.SelectorTargetPosition;
  }

  public void Update()
  {
    this.Selector.transform.position = Vector3.Lerp(this.Selector.transform.position, this.SelectorTargetPosition, 35f * Time.unscaledDeltaTime);
  }

  public void DoUpdate(Player RewiredController)
  {
    this.selectionDelay -= Time.unscaledDeltaTime;
    if ((double) InputManager.UI.GetHorizontalAxis() > 0.30000001192092896 && (double) this.selectionDelay < 0.0)
      ++this.CURRENT_SELECTION;
    if ((double) InputManager.UI.GetHorizontalAxis() < -0.30000001192092896 && (double) this.selectionDelay < 0.0)
      --this.CURRENT_SELECTION;
    if ((double) InputManager.UI.GetVerticalAxis() > 0.30000001192092896 && (double) this.selectionDelay < 0.0)
      this.CURRENT_SELECTION -= this.List.GridSize.x;
    if ((double) InputManager.UI.GetVerticalAxis() < -0.30000001192092896 && (double) this.selectionDelay < 0.0)
      this.CURRENT_SELECTION += this.List.GridSize.x;
    if ((double) InputManager.UI.GetVerticalAxis() >= -0.30000001192092896 && (double) InputManager.UI.GetVerticalAxis() <= 0.30000001192092896 && (double) InputManager.Gameplay.GetHorizontalAxis() < 0.30000001192092896 && (double) InputManager.Gameplay.GetHorizontalAxis() > -0.30000001192092896)
      this.selectionDelay = 0.0f;
    if (!InputManager.UI.GetAcceptButtonDown() || this.List.Items[this.CURRENT_SELECTION].Item == null || this.List.Items[this.CURRENT_SELECTION].Item.type == 0)
      return;
    UIInventoryListSelector.ListSelectorAction onSelect = this.OnSelect;
    if (onSelect == null)
      return;
    onSelect(this.List.Items[this.CURRENT_SELECTION].Item);
  }

  public delegate void ListSelectorAction(InventoryItem Item);
}
