// Decompiled with JetBrains decompiler
// Type: TextWindowGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TextWindowGUI : BaseGUI
{
  public UILabel text;
  public GJCommons.VoidDelegate _on_closed;

  public void Open(string txt_id, GJCommons.VoidDelegate on_closed = null)
  {
    Debug.Log((object) ("Open text window " + txt_id));
    this.text.text = GJL.L(txt_id);
    this._on_closed = on_closed;
    this.Open();
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select("ok"));
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override bool OnPressedSelect()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    Debug.Log((object) nameof (OnClosePressed));
    base.OnClosePressed();
    if (this._on_closed == null)
      return;
    GJCommons.VoidDelegate onClosed = this._on_closed;
    this._on_closed = (GJCommons.VoidDelegate) null;
    if (onClosed == null)
      return;
    onClosed();
  }
}
