// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.SaveSlotButtonBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Button _button;
  [SerializeField]
  public TextMeshProUGUI _text;
  [SerializeField]
  public GameObject _completionBadge;
  [SerializeField]
  public GameObject _sandboxCompletionBadge;
  public int _saveIndex;
  public bool _occupied;
  public global::MetaData? _metaData;
  public COTLDataReadWriter<global::MetaData> _metaDataReader = new COTLDataReadWriter<global::MetaData>();

  public Button Button => this._button;

  public global::MetaData? MetaData => this._metaData;

  public bool Occupied => this._occupied;

  public int SaveIndex => this._saveIndex;

  public virtual void Awake()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public virtual void SetupSaveSlot(int index)
  {
    this._saveIndex = index;
    this.UpdateSaveSlot();
    this._button.onClick.RemoveAllListeners();
    this._button.onClick.AddListener(new UnityAction(this.OnSaveSlotButtonClicked));
  }

  public void SetupSaveSlot(int index, global::MetaData metaData)
  {
    SaveAndLoad.OnSaveSlotDeleted -= new Action<int>(this.OnSaveSlotDeleted);
    SaveAndLoad.OnSaveSlotDeleted += new Action<int>(this.OnSaveSlotDeleted);
    this._metaData = new global::MetaData?(metaData);
    this.SetupSaveSlot(index);
  }

  public virtual void UpdateSaveSlot()
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

  public void Localize()
  {
    if (this._occupied)
      this.LocalizeOccupied();
    else
      this.LocalizeEmpty();
  }

  public abstract void LocalizeOccupied();

  public abstract void LocalizeEmpty();

  public void OnSaveSlotButtonClicked()
  {
    Action<int> onSaveSlotPressed = this.OnSaveSlotPressed;
    if (onSaveSlotPressed == null)
      return;
    onSaveSlotPressed(this._saveIndex);
  }

  public virtual void OnDestroy()
  {
    SaveAndLoad.OnSaveSlotDeleted -= new Action<int>(this.OnSaveSlotDeleted);
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public void OnSaveSlotDeleted(int slot)
  {
    if (slot != this._saveIndex)
      return;
    this._metaData = new global::MetaData?();
    this._occupied = false;
    this.SetupEmptySlot();
    this.LocalizeEmpty();
  }

  public abstract void SetupOccupiedSlot();

  public abstract void SetupEmptySlot();
}
