// Decompiled with JetBrains decompiler
// Type: src.UI.UIConfirmationCountdownWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI;

public class UIConfirmationCountdownWindow : UIMenuConfirmationWindow
{
  public string _cachedBody;

  public void Configure(string header, string body, int seconds)
  {
    this._cachedBody = body;
    this._headerText.text = header;
    this._bodyText.text = string.Format(this._cachedBody, (object) seconds);
    this.StartCoroutine((IEnumerator) this.DoCountdown(seconds));
  }

  public IEnumerator DoCountdown(int seconds)
  {
    UIConfirmationCountdownWindow confirmationCountdownWindow = this;
    while (seconds > 0)
    {
      yield return (object) new WaitForSecondsRealtime(1f);
      --seconds;
      confirmationCountdownWindow._bodyText.text = string.Format(confirmationCountdownWindow._cachedBody, (object) seconds);
    }
    confirmationCountdownWindow.OnCancelClicked();
  }
}
