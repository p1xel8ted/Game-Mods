// Decompiled with JetBrains decompiler
// Type: NotificationHelpHinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationHelpHinder : NotificationTwitch
{
  public static NotificationHelpHinder Instance;
  [SerializeField]
  public Image bar;

  public override float _onScreenDuration
  {
    get
    {
      return (float) (TwitchHelpHinder.VotingPhaseDuration + TwitchHelpHinder.VotingPhaseDuration) + 3f;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    NotificationHelpHinder.Instance = this;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    NotificationHelpHinder.Instance = (NotificationHelpHinder) null;
  }

  public void Update()
  {
    if (!TwitchHelpHinder.Active)
      return;
    this.UpdateBar(1f - TwitchHelpHinder.Timer / (float) (TwitchHelpHinder.VotingPhaseDuration + TwitchHelpHinder.VotingPhaseDuration));
    if ((double) this.bar.fillAmount > 0.0)
      return;
    NotificationHelpHinder.CloseNotification();
  }

  public void UpdateBar(float norm) => this.bar.fillAmount = norm;

  public static void CloseNotification()
  {
    if (!(bool) (Object) NotificationHelpHinder.Instance)
      return;
    NotificationHelpHinder.Instance.Hide();
    NotificationHelpHinder.Instance = (NotificationHelpHinder) null;
  }
}
