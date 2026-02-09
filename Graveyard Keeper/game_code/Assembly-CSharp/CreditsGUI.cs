// Decompiled with JetBrains decompiler
// Type: CreditsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

#nullable disable
public class CreditsGUI : BaseGUI
{
  public GameObject obj;
  public UIWidget first_line;
  public UIWidget last_line;
  public GameObject black;
  public float _y;
  public float _y0;
  public bool _scrolling;
  public float scroll_speed = 0.1f;

  public override bool OnPressedBack()
  {
    GUIElements.me.main_menu.OnBackFromCredits();
    return true;
  }

  public override void Open()
  {
    this.Open(false);
    UIPanel uipanel = this.gameObject.GetComponent<UIPanel>();
    uipanel.alpha = 0.0f;
    DOTween.To((DOSetter<float>) (alpha => uipanel.alpha = alpha), 0.0f, 1f, 1f);
    this._y0 = (float) (-(double) this.first_line.transform.localPosition.y - (double) Screen.height / 4.0 - 10.0);
    this._y = (float) -((double) this.first_line.transform.localPosition.y - (double) Screen.height / 12.0);
    this.obj.transform.localPosition = new Vector3(0.0f, this._y);
    this._scrolling = true;
    this.black.SetActive(false);
    this.obj.transform.localPosition = Vector3.zero;
  }

  public void OpenScrolling()
  {
    this.Open(false);
    this._y0 = this._y = (float) Mathf.RoundToInt((float) (-(double) this.first_line.transform.localPosition.y - (double) Screen.height / 4.0 - 10.0));
    this.obj.transform.localPosition = new Vector3(0.0f, this._y);
    this._scrolling = true;
    this.black.SetActive(true);
    GUIElements.me.ingame_menu.SetControllsActive(false);
  }

  public override void Update()
  {
    base.Update();
    if (!this._scrolling)
      return;
    this._y += Time.deltaTime * this.scroll_speed;
    this.obj.transform.localPosition = new Vector3(0.0f, this._y);
    if ((double) this.obj.transform.localPosition.y - (double) Mathf.Abs(this.last_line.transform.localPosition.y) <= (double) Screen.height / 4.0 + 10.0)
      return;
    this._y = this._y0;
  }

  public override void Hide(bool play_hide_sound = true)
  {
    base.Hide(play_hide_sound);
    if (!this._scrolling)
      return;
    GUIElements.me.ingame_menu.ReturnToMainMenu();
  }
}
