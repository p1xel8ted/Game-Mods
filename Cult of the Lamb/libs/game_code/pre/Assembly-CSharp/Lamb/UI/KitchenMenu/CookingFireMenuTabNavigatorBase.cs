// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KitchenMenu.CookingFireMenuTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
namespace Lamb.UI.KitchenMenu;

public class CookingFireMenuTabNavigatorBase : MMTabNavigatorBase<CooklingFireMenuTab>
{
  private static int _recentTab;

  public override void ShowDefault()
  {
    this._defaultTabIndex = CookingFireMenuTabNavigatorBase._recentTab;
    base.ShowDefault();
  }

  protected override void PerformTransitionTo(CooklingFireMenuTab from, CooklingFireMenuTab to)
  {
    CookingFireMenuTabNavigatorBase._recentTab = this._tabs.IndexOf<CooklingFireMenuTab>(to);
    from.Menu.Hide();
    to.Menu.Show();
    int num1 = this._tabs.IndexOf<CooklingFireMenuTab>(to);
    int num2 = this._tabs.IndexOf<CooklingFireMenuTab>(from);
    RectTransform component1 = to.Menu.GetComponent<RectTransform>();
    RectTransform component2 = from.Menu.GetComponent<RectTransform>();
    if (num1 > num2)
    {
      component1.DOKill();
      component1.anchoredPosition = new Vector2(50f, 0.0f);
      component1.DOAnchorPos(Vector2.zero, 0.1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      component2.DOKill();
      component2.anchoredPosition = Vector2.zero;
      component2.DOAnchorPos(new Vector2(-50f, 0.0f), 0.1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    }
    else
    {
      if (num2 <= num1)
        return;
      component1.DOKill();
      component1.anchoredPosition = new Vector2(-50f, 0.0f);
      component1.DOAnchorPos(Vector2.zero, 0.1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      component2.DOKill();
      component2.anchoredPosition = Vector2.zero;
      component2.DOAnchorPos(new Vector2(50f, 0.0f), 0.1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    }
  }
}
