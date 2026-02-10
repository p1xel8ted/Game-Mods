// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SacrificeFollowerInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SacrificeFollowerInfoCard : UIInfoCardBase<FollowerInfo>
{
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI SermonXPText;
  [SerializeField]
  public TextMeshProUGUI DeltaText;
  [SerializeField]
  public Image ProgressBar;
  [SerializeField]
  public Image InstantBar;
  [SerializeField]
  public GameObject GreenGlow;

  public override void Configure(FollowerInfo config)
  {
    this._headerText.text = ScriptLocalization.Interactions.Sacrifice;
    float xpBySermon = DoctrineUpgradeSystem.GetXPBySermon(SermonCategory.PlayerUpgrade);
    float xpTargetBySermon = DoctrineUpgradeSystem.GetXPTargetBySermon(SermonCategory.PlayerUpgrade);
    float num1 = xpTargetBySermon * ((float) RitualSacrifice.GetDevotionGain(config.XPLevel) / 100f);
    TextMeshProUGUI sermonXpText = this.SermonXPText;
    int num2 = Mathf.RoundToInt(xpBySermon * 10f);
    string str1 = num2.ToString();
    num2 = Mathf.RoundToInt(xpTargetBySermon * 10f);
    string str2 = num2.ToString();
    string str3 = $"{str1}/{str2}";
    sermonXpText.text = str3;
    this.DeltaText.text = $"(+{Mathf.Ceil(num1 * 10f).ToString()})";
    this.ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(xpBySermon / xpTargetBySermon, 0.0f, 1f), 1f);
    this.InstantBar.transform.localScale = this.ProgressBar.transform.localScale;
    this.InstantBar.transform.DOKill();
    this.InstantBar.transform.DOScale(new Vector3(Mathf.Clamp((xpBySermon + num1) / xpTargetBySermon, 0.0f, 1f), 1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.GreenGlow.SetActive((double) xpBySermon + (double) num1 > (double) xpTargetBySermon);
    if (!this.GreenGlow.activeSelf)
      return;
    this.GreenGlow.transform.DOKill();
    this.GreenGlow.transform.localScale = Vector3.one * 1.3f;
    this.GreenGlow.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
  }
}
