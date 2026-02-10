// Decompiled with JetBrains decompiler
// Type: StickerItemSpeechBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UINavigator;
using System;
using UnityEngine;

#nullable disable
public class StickerItemSpeechBubble : StickerItem
{
  [SerializeField]
  public MMInputField inputField;

  public override void OnStickerPlaced()
  {
    base.OnStickerPlaced();
    this._editOverlay.CancelSticker();
    this._editOverlay.DisableAllInputs();
    this.inputField.OnStartedEditing += new System.Action(this.OnStartedEditing);
    this.inputField.OnEndedEditing += new Action<string>(this.OnEndedEditing);
    this.inputField.gameObject.SetActive(true);
    this.inputField.Interactable = true;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.inputField);
    this.inputField.TryPerformConfirmAction();
  }

  public void OnStartedEditing()
  {
  }

  public void OnEndedEditing(string s)
  {
    this.inputField.text = s;
    this.inputField.Interactable = false;
    this.inputField.OnStartedEditing -= new System.Action(this.OnStartedEditing);
    this.inputField.OnEndedEditing -= new Action<string>(this.OnEndedEditing);
    this._editOverlay.EnableAllInputs();
    this._editOverlay.MoveToSticker(this.StickerData);
  }

  public override void Flip()
  {
    base.Flip();
    Vector3 localScale = this.inputField.transform.localScale;
    localScale.x *= -1f;
    this.inputField.transform.localScale = localScale;
  }
}
