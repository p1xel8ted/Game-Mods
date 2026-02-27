// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNodeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UpgradeTreeNodeInfoCard : UIInfoCardBase<UpgradeTreeNode>
{
  [SerializeField]
  private TextMeshProUGUI _nodeNameText;

  public override void Configure(UpgradeTreeNode node)
  {
    this._nodeNameText.text = UpgradeSystem.GetLocalizedName(node.Upgrade);
    Vector2 anchoredPosition = node.RectTransform.anchoredPosition;
    anchoredPosition.y -= (float) ((double) node.RectTransform.rect.height * (double) node.RectTransform.localScale.y * 0.5);
    anchoredPosition.y -= 20f;
    this.RectTransform.anchoredPosition = anchoredPosition;
  }

  protected override void DoShow(bool instant)
  {
    this.CanvasGroup.DOKill();
    if (instant)
      this.CanvasGroup.alpha = 1f;
    else
      this.CanvasGroup.DOFade(1f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  protected override void DoHide(bool instant)
  {
    this.CanvasGroup.DOKill();
    if (instant)
      this.CanvasGroup.alpha = 0.0f;
    else
      this.CanvasGroup.DOFade(0.0f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }
}
