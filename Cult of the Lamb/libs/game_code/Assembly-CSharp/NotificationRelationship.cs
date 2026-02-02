// Decompiled with JetBrains decompiler
// Type: NotificationRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class NotificationRelationship : NotificationBase
{
  [SerializeField]
  public SkeletonGraphic _spineA;
  [SerializeField]
  public SkeletonGraphic _spineB;
  public NotificationCentre.NotificationType _type;
  public FollowerInfo _followerInfoA;
  public FollowerInfo _followerInfoB;

  public override float _onScreenDuration => 3f;

  public override float _showHideDuration => 0.4f;

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

  public override void Localize()
  {
    this._description.text = string.Format(LocalizationManager.GetTranslation($"Notifications/Relationship/{this._type}"), (object) this._followerInfoA.Name, (object) this._followerInfoB.Name);
  }
}
