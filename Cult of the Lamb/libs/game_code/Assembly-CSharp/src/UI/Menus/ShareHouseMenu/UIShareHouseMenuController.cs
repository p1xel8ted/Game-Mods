// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.ShareHouseMenu.UIShareHouseMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace src.UI.Menus.ShareHouseMenu;

public class UIShareHouseMenuController : UIMenuBase
{
  [SerializeField]
  public ShareHouseItem[] _followerSelectItems;
  [SerializeField]
  public TMP_Text _occupiedText;
  [SerializeField]
  public TMP_Text _buttonPrompt;
  [SerializeField]
  public TMP_Text _topDescriptionText;
  public Interaction_Bed _interactionBed;
  public StructuresData _structuresData;

  public void Show(Interaction_Bed interactionBed, bool instant = false)
  {
    this._interactionBed = interactionBed;
    this._structuresData = this._interactionBed.Structure.Structure_Info;
    this.OnShareHouseItemSelected(this._followerSelectItems[0]);
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    for (int index = 0; index < this._followerSelectItems.Length; ++index)
    {
      ShareHouseItem shareHouseItem = this._followerSelectItems[index];
      if (this._structuresData.MultipleFollowerIDs.Count >= index + 1)
        shareHouseItem.Configure(FollowerInfo.GetInfoByID(this._structuresData.MultipleFollowerIDs[index]));
      else
        shareHouseItem.ConfigureVacant();
      shareHouseItem.OnShareHouseItemSelected += new Action<ShareHouseItem>(this.OnShareHouseItemSelected);
      shareHouseItem.Button.onClick.AddListener((UnityAction) (() => this.OnFollowerSelected(shareHouseItem)));
    }
    this.UpdateOccupiedText();
    this.UpdateTopDescriptionText();
  }

  public void OnShareHouseItemSelected(ShareHouseItem shareHouseItem)
  {
    if (shareHouseItem.FollowerInfo != null)
      this._buttonPrompt.text = ScriptLocalization.UI_ShareHouseMenu.ReassignBed;
    else
      this._buttonPrompt.text = ScriptLocalization.UI_ShareHouseMenu.AssignBed;
  }

  public void OnFollowerSelected(ShareHouseItem shareHouseItem)
  {
    UIFollowerSelectMenuController menu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    menu.VotingType = TwitchVoting.VotingType.BED;
    menu.Show(this._interactionBed.GetFollowerSelectEntries(), false, UpgradeSystem.Type.Count, true, true, true, false, true);
    this.PushInstance<UIFollowerSelectMenuController>(menu);
    UIFollowerSelectMenuController selectMenuController = menu;
    selectMenuController.OnFollowerSelected = selectMenuController.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      shareHouseItem.Configure(followerInfo);
      this._interactionBed.OnFollowerChosenForConversion(followerInfo);
      this.UpdateOccupiedText();
    });
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public void UpdateOccupiedText()
  {
    int num = 0;
    foreach (FollowerSelectItem followerSelectItem in this._followerSelectItems)
    {
      if (followerSelectItem.FollowerInfo != null)
        ++num;
    }
    this._occupiedText.text = string.Format(ScriptLocalization.UI_ShareHouseMenu.OccupiedSlots, (object) num, (object) 3);
  }

  public void UpdateTopDescriptionText()
  {
    if (!((UnityEngine.Object) this._topDescriptionText != (UnityEngine.Object) null))
      return;
    this._topDescriptionText.text = string.Format(ScriptLocalization.UI_ShareHouseMenu.Body, (object) 3);
  }
}
