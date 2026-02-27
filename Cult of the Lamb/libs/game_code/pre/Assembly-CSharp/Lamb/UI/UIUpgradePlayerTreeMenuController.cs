// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradePlayerTreeMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIUpgradePlayerTreeMenuController : 
  UIUpgradeTreeMenuBase<UIPlayerUpgradeUnlockOverlayController>
{
  protected override Selectable GetDefaultSelectable()
  {
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      if (treeNode.Upgrade == DataManager.Instance.MostRecentPlayerTreeUpgrade)
        return (Selectable) treeNode.Button;
    }
    return (Selectable) null;
  }

  protected override int UpgradePoints() => UpgradeSystem.DisciplePoints;

  protected override string GetPointsText() => "";

  protected override void DoUnlock(UpgradeSystem.Type upgrade)
  {
    UpgradeSystem.UnlockAbility(upgrade);
    DataManager.Instance.MostRecentPlayerTreeUpgrade = upgrade;
    DataManager.Instance.LastDaySincePlayerUpgrade = TimeManager.CurrentDay;
    this._didUpgraded = true;
  }

  protected override void DoRelease()
  {
  }

  protected override UpgradeTreeNode.TreeTier TreeTier()
  {
    return DataManager.Instance.CurrentPlayerUpgradeTreeTier;
  }

  protected override void UpdateTier(UpgradeTreeNode.TreeTier tier)
  {
    DataManager.Instance.CurrentPlayerUpgradeTreeTier = tier;
  }

  public override void OnCancelButtonInput()
  {
  }

  public override void Awake()
  {
    this.disableBackPrompt = this.gameObject.transform.Find("UpgradeTreeMenuContainer/Control Prompts/Back Prompt").gameObject;
    this.disableBackPrompt.SetActive(false);
    base.Awake();
  }
}
