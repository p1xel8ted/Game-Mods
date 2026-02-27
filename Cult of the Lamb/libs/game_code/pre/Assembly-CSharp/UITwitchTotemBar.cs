// Decompiled with JetBrains decompiler
// Type: UITwitchTotemBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UITwitchTotemBar : MonoBehaviour
{
  [SerializeField]
  private SpriteXPBar bar;
  private int currentContributions;

  private void OnEnable()
  {
    if (TwitchAuthentication.IsAuthenticated && !TwitchTotem.Deactivated)
      this.bar.Show(true);
    else
      this.gameObject.SetActive(false);
  }

  private void Awake()
  {
    TwitchTotem.TotemUpdated += new TwitchTotem.TotemResponse(this.TwitchTotem_TotemUpdated);
    TwitchAuthentication.OnAuthenticated += new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    TwitchAuthentication.OnLoggedOut += new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnLoggedOut);
    this.TwitchTotem_TotemUpdated(TwitchTotem.CurrentContributions);
  }

  private void OnDestroy()
  {
    TwitchTotem.TotemUpdated -= new TwitchTotem.TotemResponse(this.TwitchTotem_TotemUpdated);
    TwitchAuthentication.OnAuthenticated -= new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    TwitchAuthentication.OnLoggedOut -= new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnLoggedOut);
  }

  private void TwitchAuthentication_OnAuthenticated()
  {
    if (TwitchTotem.Deactivated)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.gameObject.SetActive(true);
      this.bar.Show(true);
      this.TwitchTotem_TotemUpdated(TwitchTotem.CurrentContributions);
    }
  }

  private void TwitchAuthentication_OnLoggedOut()
  {
    this.gameObject.SetActive(false);
    this.bar.Hide(true);
  }

  private void TwitchTotem_TotemUpdated(int contributions)
  {
    contributions -= 10 * TwitchTotem.TwitchTotemsCompleted;
    if (contributions != this.currentContributions)
      this.bar.UpdateBar(Mathf.Clamp01((float) contributions / 10f));
    this.currentContributions = contributions;
  }
}
