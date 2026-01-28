// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.WeatherInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Rituals;

public class WeatherInfoCard : UIInfoCardBase<SeasonsManager.WeatherEvent>
{
  [Header("Copy")]
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public Image _icon;
  [Header("Winter")]
  [SerializeField]
  public TextMeshProUGUI _warmthText;
  [SerializeField]
  public GameObject _warmthContainer;
  [SerializeField]
  public BarController _warmthBar;
  [Header("Sprite")]
  [SerializeField]
  public Sprite blizzardSprite;

  public override void Configure(SeasonsManager.WeatherEvent weather)
  {
    this._headerText.text = SeasonsManager.GetWeatherLocalisedTitle(weather);
    this._descriptionText.text = SeasonsManager.GetWeatherLocalisedDescription(weather);
    this._icon.sprite = this.blizzardSprite;
    float f = -50f;
    if ((double) f != 0.0)
    {
      this._warmthContainer.SetActive((double) f != 0.0);
      Color colour = (double) f > 0.0 ? StaticColors.GreenColor : StaticColors.RedColor;
      this._warmthText.text = (((double) f > 0.0 ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">") + Mathf.Abs(f).ToString()).Colour(colour);
      if ((double) f < 0.0)
        this._warmthBar.SetBarSizeForInfo(WarmthBar.WarmthNormalized, WarmthBar.WarmthNormalized + f / WarmthBar.MAX_WARMTH, FollowerBrainStats.LockedWarmth);
      else
        this._warmthBar.SetBarSizeForInfo(WarmthBar.WarmthNormalized + f / WarmthBar.MAX_WARMTH, WarmthBar.WarmthNormalized, FollowerBrainStats.LockedWarmth);
    }
    else
    {
      if (!((Object) this._warmthContainer != (Object) null))
        return;
      this._warmthContainer.gameObject.SetActive(false);
    }
  }
}
