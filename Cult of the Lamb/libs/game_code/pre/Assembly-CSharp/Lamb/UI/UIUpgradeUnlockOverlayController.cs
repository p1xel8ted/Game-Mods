// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradeUnlockOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIUpgradeUnlockOverlayController : UIUpgradeUnlockOverlayControllerBase
{
  [SerializeField]
  private TextMeshProUGUI _requirementsText;

  protected override void OnShowStarted()
  {
    if (this._node.State != UpgradeTreeNode.NodeState.Unlocked && !this.IsAvailable())
    {
      this._requirementsText.gameObject.SetActive(true);
      this._requirementsText.text = string.Format(ScriptLocalization.UI_UpgradeTree.Requires, (object) string.Join(" ", "1x", ScriptLocalization.UI.AbilityPoints)).Replace(":", string.Empty);
    }
    else
      this._requirementsText.gameObject.SetActive(false);
    base.OnShowStarted();
  }

  protected override bool IsAvailable() => UpgradeSystem.AbilityPoints > 0;
}
