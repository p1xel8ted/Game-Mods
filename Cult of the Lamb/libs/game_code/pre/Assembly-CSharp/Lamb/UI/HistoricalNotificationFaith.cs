// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationFaith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Image _faithIcon;
  [SerializeField]
  private TextMeshProUGUI _faithDeltaText;
  [SerializeField]
  private GameObject _followerSpineContainer;
  [SerializeField]
  private SkeletonGraphic _followerSpine;
  [Header("Icons")]
  [SerializeField]
  private Sprite faithDoubleUp;
  [SerializeField]
  private Sprite faithUp;
  [SerializeField]
  private Sprite faithDown;
  [SerializeField]
  private Sprite faithDoubleDown;

  protected override void ConfigureImpl(FinalizedFaithNotification finalizedNotification)
  {
    float faithDelta = finalizedNotification.FaithDelta;
    if ((double) faithDelta <= -10.0)
    {
      this._faithIcon.sprite = this.faithDoubleDown;
      this._faithDeltaText.text = faithDelta.ToString().Bold().Colour(StaticColors.RedColor);
    }
    else if ((double) faithDelta < 0.0)
    {
      this._faithIcon.sprite = this.faithDown;
      this._faithDeltaText.text = faithDelta.ToString().Bold().Colour(StaticColors.RedColor);
    }
    else if ((double) faithDelta >= 10.0)
    {
      this._faithIcon.sprite = this.faithDoubleUp;
      this._faithDeltaText.text = faithDelta.ToString().Bold().Colour(StaticColors.GreenColor);
    }
    else if ((double) faithDelta > 0.0)
    {
      this._faithIcon.sprite = this.faithUp;
      this._faithDeltaText.text = faithDelta.ToString().Bold().Colour(StaticColors.GreenColor);
    }
    else
    {
      this._faithIcon.gameObject.SetActive(false);
      this._faithDeltaText.gameObject.SetActive(false);
    }
    this._followerSpineContainer.SetActive(finalizedNotification.followerInfoSnapshot != null);
    if (finalizedNotification.followerInfoSnapshot == null)
      return;
    this._followerSpine.ConfigureFollower(finalizedNotification.followerInfoSnapshot);
  }

  protected override string GetLocalizedDescription(FinalizedFaithNotification finalizedNotification)
  {
    return finalizedNotification.followerInfoSnapshot != null ? string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object) finalizedNotification.followerInfoSnapshot.Name) : base.GetLocalizedDescription(finalizedNotification);
  }
}
