// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.SaveSlotButton_Load
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.UINavigator;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI.MainMenu;

public class SaveSlotButton_Load : SaveSlotButtonBase
{
  [SerializeField]
  public SaveSlotButton_BaseGame _baseGameSaveButton;
  [SerializeField]
  public MMButton _mmButton;

  public MMButton MMButton => this._mmButton;

  public override void Awake()
  {
    base.Awake();
    this._baseGameSaveButton.gameObject.SetActive(false);
    this._baseGameSaveButton.MMButton.Interactable = false;
    this._mmButton.OnSelected += new System.Action(this.OnSelected);
    this._mmButton.OnDeselected += new System.Action(this.OnDeselected);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this._mmButton.OnSelected -= new System.Action(this.OnSelected);
    this._mmButton.OnDeselected -= new System.Action(this.OnDeselected);
  }

  public void SetupBaseGameSaveSlot(int index, global::MetaData metaData)
  {
    this._baseGameSaveButton.SetupSaveSlot(index, metaData);
  }

  public void OnSelected()
  {
    if (this._baseGameSaveButton.gameObject.activeSelf || !this._baseGameSaveButton.MetaData.HasValue)
      return;
    this._baseGameSaveButton.MMButton.Interactable = false;
    this._baseGameSaveButton.transform.DOKill();
    this._baseGameSaveButton.gameObject.SetActive(true);
    this._baseGameSaveButton.transform.localPosition = this.transform.localPosition;
    this._baseGameSaveButton.transform.DOLocalMove(this.transform.localPosition + Vector3.right * 250f, 0.2f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this._baseGameSaveButton.MMButton.Interactable = true));
  }

  public void OnDeselected()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null) || MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == this.MMButton || MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == this._baseGameSaveButton.MMButton)
      return;
    this._baseGameSaveButton.MMButton.Interactable = false;
    this._baseGameSaveButton.gameObject.SetActive(false);
  }

  public override void UpdateSaveSlot()
  {
    if (!this._metaData.HasValue && this._baseGameSaveButton.MetaData.HasValue)
    {
      this._metaData = this._baseGameSaveButton.MetaData;
      this._saveIndex = this._baseGameSaveButton.SaveIndex;
      this._baseGameSaveButton.ClearMeta();
    }
    base.UpdateSaveSlot();
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override void LocalizeOccupied()
  {
    if (!this._metaData.HasValue)
      return;
    this._text.text = this._metaData.Value.ToString();
  }

  public override void LocalizeEmpty() => this._text.text = ScriptLocalization.UI_MainMenu.NewSave;

  public override void SetupOccupiedSlot()
  {
    if (!this._metaData.HasValue)
      return;
    this._completionBadge.SetActive(this._metaData.Value.GameBeaten && !this._metaData.Value.SandboxBeaten);
    this._sandboxCompletionBadge.SetActive(this._metaData.Value.SandboxBeaten);
    this._button.interactable = true;
  }

  public override void SetupEmptySlot()
  {
    this._button.interactable = true;
    this._completionBadge.SetActive(false);
    this._sandboxCompletionBadge.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003COnSelected\u003Eb__7_0()
  {
    this._baseGameSaveButton.MMButton.Interactable = true;
  }
}
