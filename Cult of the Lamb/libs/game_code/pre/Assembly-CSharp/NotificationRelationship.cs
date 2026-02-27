// Decompiled with JetBrains decompiler
// Type: NotificationRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class NotificationRelationship : NotificationBase
{
  [SerializeField]
  private SkeletonGraphic _spineA;
  [SerializeField]
  private SkeletonGraphic _spineB;
  private NotificationCentre.NotificationType _type;
  private FollowerInfo _followerInfoA;
  private FollowerInfo _followerInfoB;

  protected override float _onScreenDuration => 3f;

  protected override float _showHideDuration => 0.4f;

  public void Configure(
    NotificationCentre.NotificationType type,
    FollowerInfo followerInfoA,
    FollowerInfo followerInfoB,
    NotificationFollower.Animation followerAnimationA,
    NotificationFollower.Animation followerAnimationB,
    NotificationBase.Flair flair = NotificationBase.Flair.None)
  {
    this._type = type;
    this._followerInfoA = followerInfoA;
    this._followerInfoB = followerInfoB;
    this._spineA.ConfigureFollowerSkin(this._followerInfoA);
    this._spineA.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(followerAnimationA), true);
    this._spineB.ConfigureFollowerSkin(this._followerInfoB);
    this._spineB.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(followerAnimationB), true);
    this.Configure(flair);
  }

  protected override void Localize()
  {
    this._description.text = string.Format(LocalizationManager.GetTranslation($"Notifications/Relationship/{this._type}"), (object) this._followerInfoA.Name, (object) this._followerInfoB.Name);
  }
}
