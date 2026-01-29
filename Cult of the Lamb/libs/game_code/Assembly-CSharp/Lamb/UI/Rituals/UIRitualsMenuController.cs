// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.UIRitualsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Rituals;

public class UIRitualsMenuController : UIMenuBase
{
  public Action<UpgradeSystem.Type> OnRitualSelected;
  [Header("Rituals Menu")]
  [SerializeField]
  public RitualInfoCardController _infoCardController;
  [SerializeField]
  public RitualItem _ritualItemTemplate;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public RectTransform _ritualsContent;
  [SerializeField]
  public GameObject _dlcRitualsTitle;
  [SerializeField]
  public RectTransform _dlcRitualsContent;
  [SerializeField]
  public RectTransform _dlcRitualsDivider;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public GameObject _twitchFreeRitual;
  [Header("Special Rituals")]
  [SerializeField]
  public GameObject _specialRitualsHeader;
  [SerializeField]
  public GameObject _specialRitualsContainer;
  [SerializeField]
  public RitualItem _bloodmoonRitualItem;
  [SerializeField]
  public RitualItem _midwinterRitualItem;
  public List<RitualItem> _ritualItems = new List<RitualItem>();
  public bool _didCancel;
  public bool _forceDoRitual;
  public UpgradeSystem.Type _showRitual = UpgradeSystem.Type.Count;

  public void Show(UpgradeSystem.Type showRitual, bool forceDoRitual = false, bool instant = false)
  {
    this._showRitual = showRitual;
    this._forceDoRitual = forceDoRitual;
    this.Show(instant);
    bool flag = false;
    foreach (UpgradeSystem.Type unlockedUpgrade in UpgradeSystem.UnlockedUpgrades)
    {
      if (Ritual.MAJOR_DLC_RITUALS.Contains(unlockedUpgrade))
        flag = true;
    }
    this._dlcRitualsContent.gameObject.SetActive(DataManager.Instance.MAJOR_DLC & flag);
    this._dlcRitualsDivider.gameObject.SetActive(DataManager.Instance.MAJOR_DLC & flag);
    this._dlcRitualsTitle.gameObject.SetActive(DataManager.Instance.MAJOR_DLC & flag);
  }

  public override void OnShowStarted()
  {
    this._specialRitualsContainer.SetActive(false);
    this._specialRitualsHeader.SetActive(false);
    this._twitchFreeRitual.SetActive(DataManager.Instance.NextRitualFree);
    UIManager.PlayAudio("event:/ui/open_menu");
    this._midwinterRitualItem.gameObject.SetActive(false);
    this._bloodmoonRitualItem.gameObject.SetActive(false);
    if (DataManager.Instance.LongNightActive && TimeManager.IsNight)
    {
      if (!DataManager.Instance.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ritual_Midwinter))
        DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_Midwinter);
      this._midwinterRitualItem.gameObject.SetActive(true);
      this.ConfigureItem(this._midwinterRitualItem, UpgradeSystem.Type.Ritual_Midwinter);
      this._specialRitualsContainer.SetActive(true);
      this._specialRitualsHeader.SetActive(true);
      this.OverrideDefault((Selectable) this._midwinterRitualItem.Button);
      this.ActivateNavigation();
      this._infoCardController.Card1?.Show(UpgradeSystem.Type.Ritual_Midwinter, false);
    }
    foreach (UpgradeSystem.Type secondaryRitual in UpgradeSystem.SecondaryRituals)
      this.AddRitual(secondaryRitual);
    UpgradeSystem.Type[] secondaryRitualPairs = UpgradeSystem.SecondaryRitualPairs;
    for (int index = 0; index < secondaryRitualPairs.Length; index += 2)
    {
      if (CheatConsole.UnlockAllRituals || DataManager.Instance.OnboardedCrystalDoctrine)
      {
        this.AddRitual(secondaryRitualPairs[index]);
        this.AddRitual(secondaryRitualPairs[index + 1]);
      }
      else if (UpgradeSystem.GetUnlocked(secondaryRitualPairs[index]))
        this.AddRitual(secondaryRitualPairs[index]);
      else
        this.AddRitual(secondaryRitualPairs[index + 1]);
    }
    foreach (UpgradeSystem.Type singleRitual in UpgradeSystem.SingleRituals)
    {
      if (UpgradeSystem.GetUnlocked(singleRitual) && !UpgradeSystem.IsSpecialRitual(singleRitual))
        this.AddRitual(singleRitual);
    }
    foreach (UpgradeSystem.Type specialRitual in UpgradeSystem.SpecialRituals)
    {
      if (UpgradeSystem.GetUnlocked(specialRitual) && specialRitual == UpgradeSystem.Type.Ritual_Halloween)
      {
        this._bloodmoonRitualItem.gameObject.SetActive(true);
        this.ConfigureItem(this._bloodmoonRitualItem, specialRitual);
        this._specialRitualsContainer.SetActive(true);
        this._specialRitualsHeader.SetActive(true);
        break;
      }
    }
    if (this._bloodmoonRitualItem.gameObject.activeInHierarchy)
      this.OverrideDefault((Selectable) this._bloodmoonRitualItem.Button);
    else
      this.OverrideDefault((Selectable) this._ritualItems[0].Button);
    this.ActivateNavigation();
  }

  public void ConfigureItem(RitualItem ritualItem, UpgradeSystem.Type ritual)
  {
    ritualItem.Configure(ritual);
    ritualItem.OnRitualItemSelected += new Action<UpgradeSystem.Type>(this.RitualItemSelected);
    ritualItem.Button.OnConfirmDenied += new System.Action(this._infoCardController.ShakeLimitedTimeText);
    this._ritualItems.Add(ritualItem);
  }

  public void AddRitual(UpgradeSystem.Type ritual)
  {
    switch (ritual)
    {
      case UpgradeSystem.Type.Ritual_Blank:
        return;
      case UpgradeSystem.Type.Ritual_Snowman:
        if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Snowman))
          return;
        break;
    }
    this.ConfigureItem(this._ritualItemTemplate.Instantiate<RitualItem>(Ritual.MAJOR_DLC_RITUALS.Contains(ritual) ? (Transform) this._dlcRitualsContent : (Transform) this._ritualsContent), ritual);
  }

  public override IEnumerator DoShowAnimation()
  {
    UIRitualsMenuController ritualsMenuController = this;
    if (ritualsMenuController._showRitual != UpgradeSystem.Type.Count)
    {
      ritualsMenuController._controlPrompts.HideAcceptButton();
      ritualsMenuController._controlPrompts.HideCancelButton();
      RitualItem target = (RitualItem) null;
      foreach (RitualItem ritualItem in ritualsMenuController._ritualItems)
      {
        if (ritualItem.RitualType == ritualsMenuController._showRitual)
          target = ritualItem;
        else if (!ritualItem.Locked)
          ritualItem.ForceIncognitoState();
      }
      if (ritualsMenuController._forceDoRitual)
      {
        target.RemoveCost();
        ritualsMenuController._infoCardController.Card1.RemoveCost();
        ritualsMenuController._infoCardController.Card2.RemoveCost();
      }
      target.ForceLockedState();
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      ritualsMenuController.SetActiveStateForMenu(false);
      yield return (object) ritualsMenuController.\u003C\u003En__0();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      yield return (object) ritualsMenuController._scrollRect.DoScrollTo(target.RectTransform);
      yield return (object) target.DoUnlock();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      ritualsMenuController._infoCardController.ShowCardWithParam(target.RitualType);
      yield return (object) new WaitForSecondsRealtime(0.1f);
      ritualsMenuController._controlPrompts.ShowAcceptButton();
      if (ritualsMenuController._forceDoRitual)
      {
        ritualsMenuController.OverrideDefault((Selectable) target.Button);
        ritualsMenuController.SetActiveStateForMenu(true);
        foreach (RitualItem ritualItem in ritualsMenuController._ritualItems)
        {
          if (ritualItem.RitualType == ritualsMenuController._showRitual)
            ritualItem.Free = true;
          else
            ritualItem.Button.Confirmable = false;
        }
      }
      else
      {
        while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          yield return (object) null;
        ritualsMenuController._controlPrompts.HideAcceptButton();
        ritualsMenuController.Hide();
        target = (RitualItem) null;
      }
    }
    else
    {
      yield return (object) ritualsMenuController.\u003C\u003En__0();
      if (DataManager.Instance.LongNightActive && TimeManager.IsNight)
        ritualsMenuController._infoCardController.Card1?.Show(UpgradeSystem.Type.Ritual_Midwinter, false);
    }
  }

  public void RitualItemSelected(UpgradeSystem.Type ritual)
  {
    Action<UpgradeSystem.Type> onRitualSelected = this.OnRitualSelected;
    if (onRitualSelected != null)
      onRitualSelected(ritual);
    this.Hide();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this._forceDoRitual)
      return;
    this._didCancel = true;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
