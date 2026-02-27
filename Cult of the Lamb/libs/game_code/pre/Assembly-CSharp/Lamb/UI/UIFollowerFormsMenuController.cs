// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFollowerFormsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using src.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIFollowerFormsMenuController : UIMenuBase
{
  [Header("Follower Forms menu")]
  [SerializeField]
  private IndoctrinationFormItem _formItemTemplate;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [Header("Content")]
  [SerializeField]
  private RectTransform _miscContent;
  [SerializeField]
  private RectTransform _specialEventsContent;
  [SerializeField]
  private RectTransform _dlcContent;
  [SerializeField]
  private RectTransform _darkwoodContent;
  [SerializeField]
  private RectTransform _anuraContent;
  [SerializeField]
  private RectTransform _anchorDeepContent;
  [SerializeField]
  private RectTransform _silkCradleContent;
  [Header("Counts")]
  [SerializeField]
  private TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  private TextMeshProUGUI _specialEventsUnlocked;
  [SerializeField]
  private TextMeshProUGUI _dlcUnlocked;
  [SerializeField]
  private TextMeshProUGUI _darkwoodUnlocked;
  [SerializeField]
  private TextMeshProUGUI _anuraUnlocked;
  [SerializeField]
  private TextMeshProUGUI _anchorDeepUnlocked;
  [SerializeField]
  private TextMeshProUGUI _silkCradleUnlocked;
  [Header("Headers")]
  [SerializeField]
  private GameObject _dlcHeader;
  [SerializeField]
  private GameObject _specialEventsHeader;
  private List<IndoctrinationFormItem> _formItems = new List<IndoctrinationFormItem>();

  protected override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/followers/appearance_menu_appear");
    this._scrollRect.normalizedPosition = Vector2.one;
    this._scrollRect.enabled = false;
    if (this._formItems.Count == 0)
    {
      this._scrollRect.enabled = false;
      bool flag1 = false;
      foreach (string dlcSkin in DataManager.Instance.DLCSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(dlcSkin))
        {
          flag1 = true;
          break;
        }
      }
      this._dlcContent.gameObject.SetActive(flag1);
      this._dlcHeader.SetActive(flag1);
      bool flag2 = false;
      foreach (string specialEventSkin in DataManager.Instance.SpecialEventSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(specialEventSkin))
        {
          flag2 = true;
          break;
        }
      }
      this._specialEventsContent.gameObject.SetActive(flag2);
      this._specialEventsHeader.SetActive(flag2);
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Other), this._miscContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.SpecialEvents), this._specialEventsContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon1), this._darkwoodContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon2), this._anuraContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon3), this._anchorDeepContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon4), this._silkCradleContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.DLC), this._dlcContent));
      this.SetUnlockedText(this._miscUnlocked, WorshipperData.DropLocation.Other);
      this.SetUnlockedText(this._specialEventsUnlocked, WorshipperData.DropLocation.SpecialEvents);
      this.SetUnlockedText(this._darkwoodUnlocked, WorshipperData.DropLocation.Dungeon1);
      this.SetUnlockedText(this._anuraUnlocked, WorshipperData.DropLocation.Dungeon2);
      this.SetUnlockedText(this._anchorDeepUnlocked, WorshipperData.DropLocation.Dungeon3);
      this.SetUnlockedText(this._silkCradleUnlocked, WorshipperData.DropLocation.Dungeon4);
      this.SetUnlockedText(this._dlcUnlocked, WorshipperData.DropLocation.DLC);
      foreach (IndoctrinationFormItem formItem in this._formItems)
      {
        formItem.SetAsDefault();
        formItem.Button.OnConfirmDenied = (System.Action) null;
        formItem.Button.Confirmable = false;
        formItem.Button._confirmDeniedSFX = "";
      }
    }
    this._scrollRect.enabled = true;
    this.OverrideDefaultOnce((Selectable) this._formItems[0].Button);
    this.ActivateNavigation();
  }

  private List<IndoctrinationFormItem> Populate(
    List<WorshipperData.SkinAndData> types,
    RectTransform contentContainer)
  {
    List<IndoctrinationFormItem> indoctrinationFormItemList = new List<IndoctrinationFormItem>();
    foreach (WorshipperData.SkinAndData type in types)
    {
      IndoctrinationFormItem indoctrinationFormItem = this._formItemTemplate.Instantiate<IndoctrinationFormItem>((Transform) contentContainer);
      indoctrinationFormItem.Configure(type);
      indoctrinationFormItem.Button.Confirmable = false;
      this._formItems.Add(indoctrinationFormItem);
    }
    return indoctrinationFormItemList;
  }

  private void SetUnlockedText(TextMeshProUGUI target, WorshipperData.DropLocation dropLocation)
  {
    int num = 0;
    List<WorshipperData.SkinAndData> skinsFromLocation = WorshipperData.Instance.GetSkinsFromLocation(dropLocation);
    foreach (WorshipperData.SkinAndData skinAndData in skinsFromLocation)
    {
      if (DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
        ++num;
    }
    target.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) $"{num}/{skinsFromLocation.Count}");
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideStarted()
  {
    this._scrollRect.enabled = false;
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_close");
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
