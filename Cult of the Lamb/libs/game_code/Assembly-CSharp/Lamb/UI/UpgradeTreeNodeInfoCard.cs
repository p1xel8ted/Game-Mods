// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNodeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public TextMeshProUGUI _nodeNameText;

  public override void Configure(UpgradeTreeNode node)
  {
    this._nodeNameText.text = UpgradeSystem.GetLocalizedName(node.Upgrade);
    Vector2 anchoredPosition = node.RectTransform.anchoredPosition;
    anchoredPosition.y -= (float) ((double) node.RectTransform.rect.height * (double) node.RectTransform.localScale.y * 0.5);
    anchoredPosition.y -= 20f;
    this.RectTransform.anchoredPosition = anchoredPosition;
  }

  public override void DoShow(bool instant)
  {
    this.CanvasGroup.DOKill();
    if (instant)
      this.CanvasGroup.alpha = 1f;
    else
      this.CanvasGroup.DOFade(1f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public override void DoHide(bool instant)
  {
    this.CanvasGroup.DOKill();
    if (instant)
      this.CanvasGroup.alpha = 0.0f;
    else
      this.CanvasGroup.DOFade(0.0f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }
}
