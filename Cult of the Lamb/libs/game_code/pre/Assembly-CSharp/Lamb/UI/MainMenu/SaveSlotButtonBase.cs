// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.SaveSlotButtonBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.MainMenu;

public abstract class SaveSlotButtonBase : MonoBehaviour
{
  public Action<int> OnSaveSlotPressed;
  [SerializeField]
  protected Button _button;
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected GameObject _completionBadge;
  private int _saveIndex;
  private bool _occupied;
  protected global::MetaData? _metaData;
  protected COTLDataReadWriter<global::MetaData> _metaDataReader = new COTLDataReadWriter<global::MetaData>();

  public Button Button => this._button;

  public global::MetaData? MetaData => this._metaData;

  public bool Occupied => this._occupied;

  private void Awake()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public void SetupSaveSlot(int index)
  {
    this._saveIndex = index;
    this.UpdateSaveSlot();
    this._button.onClick.RemoveAllListeners();
    this._button.onClick.AddListener(new UnityAction(this.OnSaveSlotButtonClicked));
  }

  public void SetupSaveSlot(int index, global::MetaData metaData)
  {
    SaveAndLoad.OnSaveSlotDeleted += new Action<int>(this.OnSaveSlotDeleted);
    this._metaData = new global::MetaData?(metaData);
    this.SetupSaveSlot(index);
  }

  public void UpdateSaveSlot()
  {
    if (this._metaData.HasValue)
    {
      this.SetupOccupiedSlot();
      this.LocalizeOccupied();
      this._occupied = true;
    }
    else
    {
      this.SetupEmptySlot();
      this.LocalizeEmpty();
      this._occupied = false;
    }
  }

  private void Localize()
  {
    if (this._occupied)
      this.LocalizeOccupied();
    else
      this.LocalizeEmpty();
  }

  protected abstract void LocalizeOccupied();

  protected abstract void LocalizeEmpty();

  private void OnSaveSlotButtonClicked()
  {
    Action<int> onSaveSlotPressed = this.OnSaveSlotPressed;
    if (onSaveSlotPressed == null)
      return;
    onSaveSlotPressed(this._saveIndex);
  }

  private void OnDestroy()
  {
    SaveAndLoad.OnSaveSlotDeleted -= new Action<int>(this.OnSaveSlotDeleted);
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  private void OnSaveSlotDeleted(int slot)
  {
    if (slot != this._saveIndex)
      return;
    this._metaData = new global::MetaData?();
    this._occupied = false;
    this.SetupEmptySlot();
    this.LocalizeEmpty();
  }

  protected abstract void SetupOccupiedSlot();

  protected abstract void SetupEmptySlot();
}
