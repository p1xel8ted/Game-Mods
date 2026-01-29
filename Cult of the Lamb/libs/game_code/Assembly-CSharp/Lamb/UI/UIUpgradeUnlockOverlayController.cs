// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradeUnlockOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIUpgradeUnlockOverlayController : UIUpgradeUnlockOverlayControllerBase
{
  [SerializeField]
  public TextMeshProUGUI _requirementsText;

  public override void OnShowStarted()
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

  public override bool IsAvailable() => UpgradeSystem.AbilityPoints > 0;
}
