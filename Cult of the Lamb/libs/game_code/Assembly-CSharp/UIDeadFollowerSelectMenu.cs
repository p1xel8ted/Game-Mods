// Decompiled with JetBrains decompiler
// Type: UIDeadFollowerSelectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class UIDeadFollowerSelectMenu : UIFollowerSelectBase<DeadFollowerInformationBox>
{
  [SerializeField]
  public TMP_Text capacity;

  public override bool AllowsVoting => false;

  public void Show(
    int amount,
    int maxCapacity,
    List<FollowerSelectEntry> followerSelectEntries,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    this.Show(followerSelectEntries, instant, hideOnSelection, cancellable, hasSelection);
    this.capacity.text = ScriptLocalization.UI_CryptMenu.OccupiedSlots;
    this.capacity.text = string.Format(this.capacity.text, (object) amount, (object) maxCapacity);
    if (amount != 0)
      return;
    this._controlPrompts.HideAcceptButton();
  }

  public override DeadFollowerInformationBox PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.DeadFollowerInformationBox;
  }
}
