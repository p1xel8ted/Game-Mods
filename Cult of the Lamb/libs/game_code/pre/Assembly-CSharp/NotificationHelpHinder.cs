// Decompiled with JetBrains decompiler
// Type: NotificationHelpHinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationHelpHinder : NotificationTwitch
{
  public static NotificationHelpHinder Instance;
  [SerializeField]
  private Image bar;

  protected override float _onScreenDuration
  {
    get
    {
      return (float) ((double) TwitchHelpHinder.VotingPhaseDuration + (double) TwitchHelpHinder.ChoicePhaseDuration + 3.0);
    }
  }

  private void Awake() => NotificationHelpHinder.Instance = this;

  protected override void OnDestroy()
  {
    base.OnDestroy();
    NotificationHelpHinder.Instance = (NotificationHelpHinder) null;
  }

  private void Update()
  {
    if (!TwitchHelpHinder.Active)
      return;
    this.UpdateBar(1f - TwitchHelpHinder.Timer / (TwitchHelpHinder.VotingPhaseDuration + TwitchHelpHinder.ChoicePhaseDuration));
    if ((double) this.bar.fillAmount > 0.0)
      return;
    NotificationHelpHinder.CloseNotification();
  }

  private void UpdateBar(float norm) => this.bar.fillAmount = norm;

  public static void CloseNotification()
  {
    if (!(bool) (Object) NotificationHelpHinder.Instance)
      return;
    NotificationHelpHinder.Instance.Hide();
    NotificationHelpHinder.Instance = (NotificationHelpHinder) null;
  }
}
