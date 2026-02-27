// Decompiled with JetBrains decompiler
// Type: NotificationFaith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationFaith : NotificationBase
{
  [SerializeField]
  private TextMeshProUGUI _faithDeltaText;
  [SerializeField]
  private Image _faithIcon;
  [SerializeField]
  private SkeletonGraphic _spine;
  [Header(" Icons")]
  [SerializeField]
  private Sprite faithDoubleUp;
  [SerializeField]
  private Sprite faithUp;
  [SerializeField]
  private Sprite faithDown;
  [SerializeField]
  private Sprite faithDoubleDown;
  private string _locKey;
  private FollowerInfo _followerInfo;
  private bool _includeName;
  private string[] _extraText;

  protected override float _onScreenDuration => 6f;

  protected override float _showHideDuration => 0.4f;

  public void Configure(
    string locKey,
    float faithDelta,
    FollowerInfo followerInfo,
    bool includeName = true,
    NotificationBase.Flair flair = NotificationBase.Flair.None,
    params string[] args)
  {
    this._locKey = locKey;
    this._followerInfo = followerInfo;
    this._includeName = includeName;
    this._extraText = args;
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
    if (this._followerInfo != null)
    {
      this._spine.ConfigureFollowerSkin(this._followerInfo);
      this._spine.AnimationState.SetAnimation(0, "Avatars/avatar-normal", true);
    }
    float num1 = Mathf.Sign(faithDelta);
    float num2 = -1f;
    if (!num2.Equals(num1))
    {
      num2 = 1f;
      if (num2.Equals(num1))
      {
        if ((double) Mathf.Abs(faithDelta) > 14.0)
          AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up_big");
        else
          AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up");
      }
    }
    else if ((double) Mathf.Abs(faithDelta) > 14.0)
      AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_down_big");
    else
      AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_down");
    this.Configure(flair);
  }

  protected override void Localize()
  {
    string format = this._extraText.Length == 0 ? LocalizationManager.GetTranslation(this._locKey) : string.Format(LocalizationManager.GetTranslation(this._locKey), (object[]) this._extraText);
    if (this._followerInfo != null)
    {
      if (this._includeName)
        this._description.text = string.Format(format, (object) this._followerInfo.Name);
      else
        this._description.text = format;
    }
    else if (format.Contains("{0}"))
    {
      this._description.text = string.Format(format, (object) ScriptLocalization.Inventory.FOLLOWERS);
    }
    else
    {
      this._description.text = format;
      this._spine.gameObject.SetActive(false);
    }
  }
}
