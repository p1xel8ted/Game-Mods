// Decompiled with JetBrains decompiler
// Type: NotificationFaith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationFaith : NotificationBase
{
  [SerializeField]
  public TextMeshProUGUI _faithDeltaText;
  [SerializeField]
  public Image _faithIcon;
  [SerializeField]
  public SkeletonGraphic _spine;
  [Header(" Icons")]
  [SerializeField]
  public Sprite faithDoubleUp;
  [SerializeField]
  public Sprite faithUp;
  [SerializeField]
  public Sprite faithDown;
  [SerializeField]
  public Sprite faithDoubleDown;
  public float _faithDelta;
  public string _locKey;
  public FollowerInfo _followerInfo;
  public bool _includeName;
  public string[] _extraText;

  public override float _onScreenDuration => 6f;

  public override float _showHideDuration => 0.4f;

  public string LocKey => this._locKey;

  public void ResetState()
  {
    this._faithIcon.gameObject.SetActive(true);
    this._faithDeltaText.gameObject.SetActive(true);
  }

  public void Configure(
    string locKey,
    float faithDelta,
    FollowerInfo followerInfo,
    bool includeName = true,
    NotificationBase.Flair flair = NotificationBase.Flair.None,
    bool showAdditionSubtractionSymbols = false,
    params string[] args)
  {
    this.ResetState();
    this._faithDelta = faithDelta;
    this._locKey = locKey;
    this._followerInfo = followerInfo;
    this._includeName = includeName;
    this._extraText = args;
    int num1 = locKey.Equals("Notifications/GainedTrait") ? 1 : 0;
    NotificationFollower.Animation followerAnimation = (double) faithDelta > 0.0 ? NotificationFollower.Animation.Normal : NotificationFollower.Animation.Sad;
    if (FollowerBrainStats.BrainWashed)
      faithDelta = 0.0f;
    this.UpdateDelta(faithDelta, showAdditionSubtractionSymbols, false);
    if (num1 != 0)
    {
      this._faithIcon.gameObject.SetActive(false);
      this._faithDeltaText.gameObject.SetActive(false);
    }
    if (this._followerInfo != null)
    {
      this._spine.ConfigureFollowerSkin(this._followerInfo);
      this._spine.AnimationState.SetAnimation(0, NotificationFollower.GetAnimation(followerAnimation), true);
    }
    float num2 = Mathf.Sign(faithDelta);
    if ((double) num2 != -1.0)
    {
      if ((double) num2 == 1.0)
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

  public override void Localize()
  {
    string format = this._extraText.Length == 0 ? LocalizationManager.GetTranslation(this._locKey) : string.Format(LocalizationManager.GetTranslation(this._locKey), (object[]) this._extraText);
    if (this._followerInfo != null)
    {
      if (this._includeName)
        this._description.text = string.Format(format, (object) this._followerInfo.Name);
      else
        this._description.text = format;
      this._spine.gameObject.SetActive(true);
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

  public void UpdateDelta(float delta, bool showAdditionSubtractionSymbols, bool append)
  {
    this.ResetState();
    if (append)
    {
      this._faithDelta += delta;
      delta = this._faithDelta;
    }
    if ((double) delta <= -10.0)
    {
      this._faithIcon.sprite = this.faithDoubleDown;
      this._faithDeltaText.text = LocalizeIntegration.ReverseText(delta.ToString()).Bold().Colour(StaticColors.RedColor);
    }
    else if ((double) delta < 0.0)
    {
      this._faithIcon.sprite = this.faithDown;
      this._faithDeltaText.text = LocalizeIntegration.ReverseText(delta.ToString()).Bold().Colour(StaticColors.RedColor);
    }
    else if ((double) delta >= 10.0)
    {
      this._faithIcon.sprite = this.faithDoubleUp;
      if (LocalizationManager.CurrentLanguage == "Arabic")
        this._faithDeltaText.text = (LocalizeIntegration.ReverseText(delta.ToString()) + (showAdditionSubtractionSymbols ? "+" : "")).Bold().Colour(StaticColors.GreenColor);
      else
        this._faithDeltaText.text = ((showAdditionSubtractionSymbols ? "+" : "") + delta.ToString()).Bold().Colour(StaticColors.GreenColor);
    }
    else if ((double) delta > 0.0)
    {
      this._faithIcon.sprite = this.faithUp;
      if (LocalizationManager.CurrentLanguage == "Arabic")
        this._faithDeltaText.text = (LocalizeIntegration.ReverseText(delta.ToString()) + (showAdditionSubtractionSymbols ? "+" : "")).Bold().Colour(StaticColors.GreenColor);
      else
        this._faithDeltaText.text = ((showAdditionSubtractionSymbols ? "+" : "") + delta.ToString()).Bold().Colour(StaticColors.GreenColor);
    }
    else
    {
      this._faithIcon.gameObject.SetActive(false);
      this._faithDeltaText.gameObject.SetActive(false);
    }
  }
}
