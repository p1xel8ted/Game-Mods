// Decompiled with JetBrains decompiler
// Type: MixerInteractiveDialog
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MixerInteractiveDialog : MonoBehaviour
{
  public Text shortCodeElement;
  public Canvas _dialogCanvas;

  public void Start() => this._dialogCanvas = this.GetComponent<Canvas>();

  public void Update()
  {
    if (!((Object) this._dialogCanvas != (Object) null) || !this._dialogCanvas.enabled || !Input.GetButton("Cancel"))
      return;
    this.Hide();
  }

  public void Show(string shortCode)
  {
    this.RefreshShortCode(shortCode);
    if (!((Object) this._dialogCanvas != (Object) null))
      return;
    this._dialogCanvas.enabled = true;
  }

  public void Hide()
  {
    if (!((Object) this._dialogCanvas != (Object) null))
      return;
    this._dialogCanvas.enabled = false;
  }

  public void RefreshShortCode(string shortCode) => this.shortCodeElement.text = shortCode;
}
