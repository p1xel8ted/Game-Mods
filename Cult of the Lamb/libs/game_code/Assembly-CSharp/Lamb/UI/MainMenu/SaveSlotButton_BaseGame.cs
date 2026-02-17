// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.SaveSlotButton_BaseGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI.MainMenu;

public class SaveSlotButton_BaseGame : SaveSlotButtonBase
{
  [SerializeField]
  public SaveSlotButton_Load parentSlot;
  [SerializeField]
  public MMButton _mmButton;

  public MMButton MMButton => this._mmButton;

  public override void Awake()
  {
    base.Awake();
    this._mmButton.OnSelected += new System.Action(this.OnSelected);
    this._mmButton.OnDeselected += new System.Action(this.OnDeselected);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this._mmButton.OnSelected -= new System.Action(this.OnSelected);
    this._mmButton.OnDeselected -= new System.Action(this.OnDeselected);
  }

  public void OnSelected()
  {
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void OnDeselected() => this.parentSlot.OnDeselected();

  public override void LocalizeOccupied()
  {
    if (!this._metaData.HasValue)
      return;
    this._text.text = this._metaData.Value.ToString();
  }

  public override void LocalizeEmpty()
  {
  }

  public override void SetupOccupiedSlot()
  {
    if (!this._metaData.HasValue)
      return;
    this._button.interactable = true;
    this.parentSlot.UpdateSaveSlot();
  }

  public override void SetupEmptySlot()
  {
  }

  public void ClearMeta()
  {
    this._metaData = new global::MetaData?();
    this.UpdateSaveSlot();
  }
}
