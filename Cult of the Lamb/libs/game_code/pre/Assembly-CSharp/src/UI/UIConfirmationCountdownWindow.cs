// Decompiled with JetBrains decompiler
// Type: src.UI.UIConfirmationCountdownWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI;

public class UIConfirmationCountdownWindow : UIMenuConfirmationWindow
{
  private string _cachedBody;

  public void Configure(string header, string body, int seconds)
  {
    this._cachedBody = body;
    this._headerText.text = header;
    this._bodyText.text = string.Format(this._cachedBody, (object) seconds);
    this.StartCoroutine((IEnumerator) this.DoCountdown(seconds));
  }

  private IEnumerator DoCountdown(int seconds)
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
