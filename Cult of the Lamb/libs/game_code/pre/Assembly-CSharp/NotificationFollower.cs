// Decompiled with JetBrains decompiler
// Type: NotificationFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class NotificationFollower : NotificationBase
{
  [SerializeField]
  private SkeletonGraphic _spine;
  private NotificationCentre.NotificationType _type;
  private FollowerInfo _followerInfo;

  protected override float _onScreenDuration => 6f;

  protected override float _showHideDuration => 0.4f;

  public void Configure(
    NotificationCentre.NotificationType type,
    FollowerInfo followerInfo,
    NotificationFollower.Animation followerAnimation,
    NotificationBase.Flair flair = NotificationBase.Flair.None)
  {
    this._type = type;
    this._followerInfo = followerInfo;
    this.Configure(flair);
    if (this._followerInfo == null)
      return;
    this._spine.ConfigureFollowerSkin(this._followerInfo);
    this._spine.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(followerAnimation), true);
  }

  protected override void Localize()
  {
    if (!(bool) (Object) this._description || this._followerInfo == null)
      return;
    this._description.text = string.Format(LocalizationManager.GetTranslation(NotificationCentre.GetLocKey(this._type)), (object) this._followerInfo.Name);
  }

  public static string GetAnimation(NotificationFollower.Animation followerAnimation)
  {
    switch (followerAnimation)
    {
      case NotificationFollower.Animation.Angry:
        return "Avatars/avatar-angry";
      case NotificationFollower.Animation.Happy:
        return "Avatars/avatar-happy";
      case NotificationFollower.Animation.Normal:
        return "Avatars/avatar-normal";
      case NotificationFollower.Animation.Sad:
        return "Avatars/avatar-sad";
      case NotificationFollower.Animation.Sick:
        return "Avatars/avatar-sick";
      case NotificationFollower.Animation.Tired:
        return "Avatars/avatar-tired";
      case NotificationFollower.Animation.Unhappy:
        return "Avatars/avatar-unhappy";
      case NotificationFollower.Animation.VeryAngry:
        return "Avatars/avatar-veryangry";
      case NotificationFollower.Animation.Dead:
        return "Avatars/avatar-dead";
      case NotificationFollower.Animation.Dissenting:
        return "Avatars/avatar-dissenter2";
      default:
        return "";
    }
  }

  public enum Animation
  {
    Angry,
    Happy,
    Normal,
    Sad,
    Sick,
    Tired,
    Unhappy,
    VeryAngry,
    Dead,
    Dissenting,
  }
}
