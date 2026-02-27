// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradeTreeMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIUpgradeTreeMenuController : UIUpgradeTreeMenuBase<UIUpgradeUnlockOverlayController>
{
  [SerializeField]
  private Image _divineInspirationBackground;

  protected override Selectable GetDefaultSelectable()
  {
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      if (treeNode.Upgrade == DataManager.Instance.MostRecentTreeUpgrade)
        return (Selectable) treeNode.Button;
    }
    return (Selectable) null;
  }

  protected override int UpgradePoints() => UpgradeSystem.AbilityPoints;

  protected override string GetPointsText() => UpgradeSystem.AbilityPoints.ToString();

  protected override void UpdatePointsText()
  {
    base.UpdatePointsText();
    if (this.UpgradePoints() > 0)
      return;
    this._divineInspirationBackground.color = "#FD1D03".ColourFromHex();
    this._pointsText.color = StaticColors.OffWhiteColor;
  }

  protected override void DoRelease()
  {
  }

  protected override void DoUnlock(UpgradeSystem.Type upgrade)
  {
    UpgradeSystem.UnlockAbility(upgrade);
    --UpgradeSystem.AbilityPoints;
    DataManager.Instance.MostRecentTreeUpgrade = upgrade;
  }

  protected override UpgradeTreeNode.TreeTier TreeTier()
  {
    return DataManager.Instance.CurrentUpgradeTreeTier;
  }

  protected override void UpdateTier(UpgradeTreeNode.TreeTier tier)
  {
    DataManager.Instance.CurrentUpgradeTreeTier = tier;
  }

  protected override void OnUnlockAnimationCompleted()
  {
    base.OnUnlockAnimationCompleted();
    this._cursor.LockPosition = false;
  }
}
