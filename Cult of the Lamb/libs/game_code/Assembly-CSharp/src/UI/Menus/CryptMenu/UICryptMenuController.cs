// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.CryptMenu.UICryptMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus.CryptMenu;

public class UICryptMenuController : UIFollowerSelectBase<DeadFollowerInformationBox>
{
  [SerializeField]
  public TMP_Text _descriptionText;
  [SerializeField]
  public TMP_Text _occupiedText;
  public int _slotLimit;

  public override bool AllowsVoting => false;

  public void Configure(Interaction_Crypt crypt)
  {
    this._slotLimit = crypt.structureBrain.DEAD_BODY_SLOTS;
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this._descriptionText.text = ScriptLocalization.UI_CryptMenu.Description;
    this._descriptionText.text = string.Format(this._descriptionText.text, (object) LocalizeIntegration.ReverseText(this._slotLimit.ToString()));
    this._occupiedText.text = ScriptLocalization.UI_CryptMenu.OccupiedSlots;
    this._occupiedText.text = !LocalizeIntegration.IsArabic() ? string.Format(this._occupiedText.text, (object) this._followerInfoBoxes.Count, (object) this._slotLimit) : string.Format(this._occupiedText.text, (object) LocalizeIntegration.ReverseText(this._followerInfoBoxes.Count.ToString()), (object) LocalizeIntegration.ReverseText(this._slotLimit.ToString()));
    if (this._followerInfoBoxes.Count != 0)
      return;
    this._controlPrompts.HideAcceptButton();
  }

  public override DeadFollowerInformationBox PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.DeadFollowerInformationBox;
  }
}
