// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SacrificeFollowerInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI SermonXPText;
  [SerializeField]
  private TextMeshProUGUI DeltaText;
  [SerializeField]
  private Image ProgressBar;
  [SerializeField]
  private Image InstantBar;
  [SerializeField]
  private GameObject GreenGlow;

  public override void Configure(FollowerInfo config)
  {
    this._headerText.text = ScriptLocalization.Interactions.Sermon;
    float xpBySermon = DoctrineUpgradeSystem.GetXPBySermon(SermonCategory.PlayerUpgrade);
    float xpTargetBySermon = DoctrineUpgradeSystem.GetXPTargetBySermon(SermonCategory.PlayerUpgrade);
    float num = xpTargetBySermon * ((float) RitualSacrifice.GetDevotionGain(config.XPLevel) / 100f);
    this.SermonXPText.text = $"{(object) Mathf.RoundToInt(xpBySermon * 10f)}/{(object) Mathf.RoundToInt(xpTargetBySermon * 10f)}";
    this.DeltaText.text = $"(+{(object) Mathf.Ceil(num * 10f)})";
    this.ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(xpBySermon / xpTargetBySermon, 0.0f, 1f), 1f);
    this.InstantBar.transform.localScale = this.ProgressBar.transform.localScale;
    this.InstantBar.transform.DOKill();
    this.InstantBar.transform.DOScale(new Vector3(Mathf.Clamp((xpBySermon + num) / xpTargetBySermon, 0.0f, 1f), 1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.GreenGlow.SetActive((double) xpBySermon + (double) num > (double) xpTargetBySermon);
    if (!this.GreenGlow.activeSelf)
      return;
    this.GreenGlow.transform.DOKill();
    this.GreenGlow.transform.localScale = Vector3.one * 1.3f;
    this.GreenGlow.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
  }
}
