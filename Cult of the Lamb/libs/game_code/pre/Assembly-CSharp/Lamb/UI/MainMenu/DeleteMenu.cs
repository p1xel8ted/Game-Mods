// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.DeleteMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private SaveSlotButtonBase[] _saveSlots;
  [SerializeField]
  private Button _backButton;
  [SerializeField]
  private ButtonHighlightController _buttonHighlight;
  private int _slotMarkedForDeletion;

  public void Start()
  {
    this._backButton.onClick.AddListener(new UnityAction(this.OnBackButtonClicked));
  }

  public void SetupSlot(int slot) => this._saveSlots[slot].SetupSaveSlot(slot);

  public void SetupSlot(int slot, MetaData metaData)
  {
    this._saveSlots[slot].SetupSaveSlot(slot, metaData);
  }

  protected override void OnShowStarted()
  {
    foreach (SaveSlotButtonBase saveSlot in this._saveSlots)
      saveSlot.OnSaveSlotPressed += new Action<int>(this.OnTryDeleteSaveSlot);
  }

  protected override void OnHideStarted()
  {
    foreach (SaveSlotButtonBase saveSlot in this._saveSlots)
      saveSlot.OnSaveSlotPressed -= new Action<int>(this.OnTryDeleteSaveSlot);
  }

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.UpdateButtonHighlight);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnNavigatorSelectionChanged);
  }

  public void OnDisable()
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

  private void OnTryDeleteSaveSlot(int slotIndex)
  {
    this._slotMarkedForDeletion = slotIndex;
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.OnConfirm += new System.Action(this.ConfirmDeletion);
    confirmationWindow.OnCancel += new System.Action(this.CancelDeletion);
  }

  private void ConfirmDeletion()
  {
    SaveAndLoad.DeleteSaveSlot(this._slotMarkedForDeletion);
    this._saveSlots[this._slotMarkedForDeletion].UpdateSaveSlot();
    this.OnBackButtonClicked();
  }

  private void CancelDeletion()
  {
  }

  private void OnBackButtonClicked()
  {
    this._buttonHighlight.SetAsBlack();
    System.Action backButtonPressed = this.OnBackButtonPressed;
    if (backButtonPressed == null)
      return;
    backButtonPressed();
  }

  private void OnNavigatorSelectionChanged(Selectable current, Selectable previous)
  {
    this.UpdateButtonHighlight(current);
  }

  private void UpdateButtonHighlight(Selectable target)
  {
    if (!this._canvasGroup.interactable)
      return;
    if (((IEnumerable<SaveSlotButtonBase>) this._saveSlots).Any<SaveSlotButtonBase>((Func<SaveSlotButtonBase, bool>) (s => (UnityEngine.Object) s.Button == (UnityEngine.Object) target)))
      this._buttonHighlight.SetAsRed();
    else
      this._buttonHighlight.SetAsBlack();
  }
}
