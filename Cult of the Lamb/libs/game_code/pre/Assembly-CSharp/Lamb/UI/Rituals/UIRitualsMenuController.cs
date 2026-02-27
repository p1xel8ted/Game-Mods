// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.UIRitualsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Rituals;

public class UIRitualsMenuController : UIMenuBase
{
  public Action<UpgradeSystem.Type> OnRitualSelected;
  [Header("Rituals Menu")]
  [SerializeField]
  private RitualInfoCardController _infoCardController;
  [SerializeField]
  private RitualItem _ritualItemTemplate;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  private RectTransform _ritualsContent;
  [Header("Special Rituals")]
  [SerializeField]
  private GameObject _specialRitualsContainer;
  [SerializeField]
  private RitualItem _specialRitualItem;
  private List<RitualItem> _ritualItems = new List<RitualItem>();
  private bool _didCancel;
  private UpgradeSystem.Type _showRitual = UpgradeSystem.Type.Count;

  public void Show(UpgradeSystem.Type showRitual, bool instant = false)
  {
    this._showRitual = showRitual;
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    this._specialRitualsContainer.SetActive(false);
    UIManager.PlayAudio("event:/ui/open_menu");
    foreach (UpgradeSystem.Type secondaryRitual in UpgradeSystem.SecondaryRituals)
      this.AddRitual(secondaryRitual);
    UpgradeSystem.Type[] secondaryRitualPairs = UpgradeSystem.SecondaryRitualPairs;
    for (int index = 0; index < secondaryRitualPairs.Length; index += 2)
    {
      if (CheatConsole.UnlockAllRituals)
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
      if (UpgradeSystem.GetUnlocked(specialRitual))
      {
        this.ConfigureItem(this._specialRitualItem, specialRitual);
        this._specialRitualsContainer.SetActive(true);
        break;
      }
    }
    if (this._specialRitualItem.gameObject.activeInHierarchy)
      this.OverrideDefault((Selectable) this._specialRitualItem.Button);
    else
      this.OverrideDefault((Selectable) this._ritualItems[0].Button);
    this.ActivateNavigation();
  }

  private void ConfigureItem(RitualItem ritualItem, UpgradeSystem.Type ritual)
  {
    ritualItem.Configure(ritual);
    ritualItem.OnRitualItemSelected += new Action<UpgradeSystem.Type>(this.RitualItemSelected);
    this._ritualItems.Add(ritualItem);
  }

  private void AddRitual(UpgradeSystem.Type ritual)
  {
    if (ritual == UpgradeSystem.Type.Ritual_Blank)
      return;
    this.ConfigureItem(this._ritualItemTemplate.Instantiate<RitualItem>((Transform) this._ritualsContent), ritual);
  }

  protected override IEnumerator DoShowAnimation()
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
      target.ForceLockedState();
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      ritualsMenuController.SetActiveStateForMenu(false);
      // ISSUE: reference to a compiler-generated method
      yield return (object) ritualsMenuController.\u003C\u003En__0();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      yield return (object) target.DoUnlock();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      ritualsMenuController._infoCardController.ShowCardWithParam(target.RitualType);
      yield return (object) new WaitForSecondsRealtime(0.1f);
      ritualsMenuController._controlPrompts.ShowAcceptButton();
      while (!InputManager.UI.GetAcceptButtonDown())
        yield return (object) null;
      ritualsMenuController._controlPrompts.HideAcceptButton();
      ritualsMenuController.Hide();
      target = (RitualItem) null;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      yield return (object) ritualsMenuController.\u003C\u003En__0();
    }
  }

  private void RitualItemSelected(UpgradeSystem.Type ritual)
  {
    Action<UpgradeSystem.Type> onRitualSelected = this.OnRitualSelected;
    if (onRitualSelected != null)
      onRitualSelected(ritual);
    this.Hide();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this._didCancel = true;
    this.Hide();
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
