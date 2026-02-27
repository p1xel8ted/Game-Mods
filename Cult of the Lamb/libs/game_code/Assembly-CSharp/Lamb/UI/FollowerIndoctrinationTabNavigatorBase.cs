// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerIndoctrinationTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class FollowerIndoctrinationTabNavigatorBase : MMTabNavigatorBase<FollowerIndoctrinationTab>
{
  [SerializeField]
  public RectTransform _indicatorTransform;
  [SerializeField]
  public Image _indicatorImage;

  public override void PerformTransitionTo(
    FollowerIndoctrinationTab from,
    FollowerIndoctrinationTab to)
  {
    base.PerformTransitionTo(from, to);
    this._indicatorImage.sprite = to.CategorySprite;
    this._indicatorTransform.DOKill();
    this._indicatorTransform.DOAnchorPosX(to.RectTransform.anchoredPosition.x, 0.2f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
  }
}
