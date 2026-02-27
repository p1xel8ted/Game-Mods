// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.DeleteMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.MainMenu;

public class DeleteMenu : UISubmenuBase
{
  public System.Action OnBackButtonPressed;
  [Header("Delete Menu")]
  [SerializeField]
  public SaveSlotButtonBase[] _saveSlots;
  [SerializeField]
  public Button _backButton;
  [SerializeField]
  public ButtonHighlightController _buttonHighlight;
  [SerializeField]
  public LoadMenu _loadMenu;
  public int _slotMarkedForDeletion;

  public void Start()
  {
    this._backButton.onClick.AddListener(new UnityAction(this.OnBackButtonClicked));
  }

  public void SetupSlot(int slot) => this._saveSlots[slot].SetupSaveSlot(slot);

  public void SetupSlot(int slot, MetaData metaData)
  {
    this._saveSlots[slot].SetupSaveSlot(slot, metaData);
  }

  public override void OnShowStarted()
  {
    foreach (SaveSlotButtonBase saveSlot in this._saveSlots)
    {
      saveSlot.UpdateSaveSlot();
      saveSlot.OnSaveSlotPressed += new Action<int>(this.OnTryDeleteSaveSlot);
    }
  }

  public override void OnHideStarted()
  {
    foreach (SaveSlotButtonBase saveSlot in this._saveSlots)
      saveSlot.OnSaveSlotPressed -= new Action<int>(this.OnTryDeleteSaveSlot);
  }

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.UpdateButtonHighlight);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnNavigatorSelectionChanged);
  }

  public new void OnDisable()
  {
    if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance == (UnityEngine.Object) null)
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.UpdateButtonHighlight);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnNavigatorSelectionChanged);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.OnBackButtonClicked();
  }

  public void OnTryDeleteSaveSlot(int slotIndex)
  {
    this._slotMarkedForDeletion = slotIndex;
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.OnConfirm += new System.Action(this.ConfirmDeletion);
    confirmationWindow.OnCancel += new System.Action(this.CancelDeletion);
  }

  public void ConfirmDeletion()
  {
    SaveAndLoad.DeleteSaveSlot(this._slotMarkedForDeletion);
    if (SaveAndLoad.SaveExist(this._slotMarkedForDeletion + 10))
      this._saveSlots[this._slotMarkedForDeletion].SetupSaveSlot(this._slotMarkedForDeletion + 10, this._loadMenu.BaseGameSaveSlots[this._slotMarkedForDeletion].MetaData.Value);
    if (this._slotMarkedForDeletion >= 10)
      this._slotMarkedForDeletion -= 10;
    this._saveSlots[this._slotMarkedForDeletion].UpdateSaveSlot();
    foreach (SaveSlotButtonBase saveSlot in this._loadMenu.SaveSlots)
      saveSlot.UpdateSaveSlot();
    this.OnBackButtonClicked();
  }

  public void CancelDeletion()
  {
  }

  public void OnBackButtonClicked()
  {
    this._buttonHighlight.SetAsBlack();
    System.Action backButtonPressed = this.OnBackButtonPressed;
    if (backButtonPressed == null)
      return;
    backButtonPressed();
  }

  public void OnNavigatorSelectionChanged(Selectable current, Selectable previous)
  {
    this.UpdateButtonHighlight(current);
  }

  public void UpdateButtonHighlight(Selectable target)
  {
    if (!this._canvasGroup.interactable)
      return;
    if (((IEnumerable<SaveSlotButtonBase>) this._saveSlots).Any<SaveSlotButtonBase>((Func<SaveSlotButtonBase, bool>) (s => (UnityEngine.Object) s.Button == (UnityEngine.Object) target)))
      this._buttonHighlight.SetAsRed();
    else
      this._buttonHighlight.SetAsBlack();
  }
}
