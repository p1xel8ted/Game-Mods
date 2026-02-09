// Decompiled with JetBrains decompiler
// Type: UITwitchTotemBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UITwitchTotemBar : MonoBehaviour
{
  [SerializeField]
  public SpriteXPBar bar;
  public int currentContributions;

  public void OnEnable()
  {
    if (TwitchAuthentication.IsAuthenticated && !TwitchTotem.Deactivated)
      this.bar.Show(true);
    else
      this.gameObject.SetActive(false);
  }

  public void Awake()
  {
    TwitchTotem.TotemUpdated += new TwitchTotem.TotemResponse(this.TwitchTotem_TotemUpdated);
    TwitchAuthentication.OnAuthenticated += new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    TwitchAuthentication.OnLoggedOut += new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnLoggedOut);
    this.TwitchTotem_TotemUpdated(TwitchTotem.Contributions);
  }

  public void OnDestroy()
  {
    TwitchTotem.TotemUpdated -= new TwitchTotem.TotemResponse(this.TwitchTotem_TotemUpdated);
    TwitchAuthentication.OnAuthenticated -= new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    TwitchAuthentication.OnLoggedOut -= new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnLoggedOut);
  }

  public void TwitchAuthentication_OnAuthenticated()
  {
    if (TwitchTotem.Deactivated)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.gameObject.SetActive(true);
      this.bar.Show(true);
      this.TwitchTotem_TotemUpdated(TwitchTotem.Contributions);
    }
  }

  public void TwitchAuthentication_OnLoggedOut()
  {
    this.gameObject.SetActive(false);
    this.bar.Hide(true);
  }

  public void TwitchTotem_TotemUpdated(int contributions)
  {
    if (contributions != this.currentContributions)
      this.bar.UpdateBar(Mathf.Clamp01((float) contributions / 10f));
    this.currentContributions = contributions;
  }
}
