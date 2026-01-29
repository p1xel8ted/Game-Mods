// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationFaith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class HistoricalNotificationFaith : HistoricalNotificationBase<FinalizedFaithNotification>
{
  [SerializeField]
  public Image _faithIcon;
  [SerializeField]
  public TextMeshProUGUI _faithDeltaText;
  [SerializeField]
  public GameObject _followerSpineContainer;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [Header("Icons")]
  [SerializeField]
  public Sprite faithDoubleUp;
  [SerializeField]
  public Sprite faithUp;
  [SerializeField]
  public Sprite faithDown;
  [SerializeField]
  public Sprite faithDoubleDown;

  public override void ConfigureImpl(FinalizedFaithNotification finalizedNotification)
  {
    float faithDelta = finalizedNotification.FaithDelta;
    if ((double) faithDelta <= -10.0)
    {
      this._faithIcon.sprite = this.faithDoubleDown;
      this._faithDeltaText.text = LocalizeIntegration.ReverseText(faithDelta.ToString()).Bold().Colour(StaticColors.RedColor);
    }
    else if ((double) faithDelta < 0.0)
    {
      this._faithIcon.sprite = this.faithDown;
      this._faithDeltaText.text = LocalizeIntegration.ReverseText(faithDelta.ToString()).Bold().Colour(StaticColors.RedColor);
    }
    else if ((double) faithDelta >= 10.0)
    {
      this._faithIcon.sprite = this.faithDoubleUp;
      this._faithDeltaText.text = LocalizeIntegration.ReverseText(faithDelta.ToString()).Bold().Colour(StaticColors.GreenColor);
    }
    else if ((double) faithDelta > 0.0)
    {
      this._faithIcon.sprite = this.faithUp;
      this._faithDeltaText.text = LocalizeIntegration.ReverseText(faithDelta.ToString()).Bold().Colour(StaticColors.GreenColor);
    }
    else
    {
      this._faithIcon.gameObject.SetActive(false);
      this._faithDeltaText.gameObject.SetActive(false);
    }
    this._followerSpineContainer.SetActive(finalizedNotification.followerInfoSnapshot != null);
    if (!this._followerSpineContainer.activeSelf)
      return;
    if (finalizedNotification.followerInfoSnapshot != null)
      this._followerSpine.ConfigureFollowerSkin(finalizedNotification.followerInfoSnapshot);
    if ((double) faithDelta > 0.0)
      this._followerSpine.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(NotificationFollower.Animation.Normal), true);
    else
      this._followerSpine.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(NotificationFollower.Animation.Sad), true);
  }

  public override string GetLocalizedDescription(FinalizedFaithNotification finalizedNotification)
  {
    return finalizedNotification.followerInfoSnapshot != null && finalizedNotification.LocalisedParameters.Length == 0 ? string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object) finalizedNotification.followerInfoSnapshot.Name) : base.GetLocalizedDescription(finalizedNotification);
  }
}
