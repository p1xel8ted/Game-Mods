// Decompiled with JetBrains decompiler
// Type: NotificationFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class NotificationFollower : NotificationBase
{
  [SerializeField]
  public SkeletonGraphic _spine;
  public NotificationCentre.NotificationType _type;
  public FollowerInfo _followerInfo;

  public override float _onScreenDuration => 6f;

  public override float _showHideDuration => 0.4f;

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

  public override void Localize()
  {
    if (!(bool) (Object) this._description || this._followerInfo == null)
      return;
    this._description.text = string.Format(LocalizationManager.GetTranslation(NotificationCentre.GetLocKey(this._type)), (object) $"<color=#FFD201>{this._followerInfo.Name}</color>");
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
      case NotificationFollower.Animation.Soaking:
        return "Avatars/avatar-soaked";
      case NotificationFollower.Animation.Freezing:
        return "Avatars/avatar-freezing";
      case NotificationFollower.Animation.Overheating:
        return "Avatars/avatar-overheated";
      case NotificationFollower.Animation.Drunk:
        return "Avatars/avatar-drunk";
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
    Soaking,
    Freezing,
    Overheating,
    Drunk,
  }
}
