// Decompiled with JetBrains decompiler
// Type: RewiredGameplayInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using MMBiomeGeneration;
using MMTools;
using Rewired;
using src.Extensions;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RewiredGameplayInputSource : CategoryInputSource
{
  protected override int Category => 0;

  private bool OverlapsWithInteractionBinding(int action)
  {
    if (this._rewiredPlayer == null || !((Object) MonoSingleton<Indicator>.Instance != (Object) null) || !((Object) Interactor.CurrentInteraction != (Object) null))
      return false;
    ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(0, InputManager.General.GetLastActiveController().type);
    ActionElementMap actionElementMap1 = controllerMapForCategory.GetActionElementMap(action, Pole.Positive);
    if (actionElementMap1 == null)
      return false;
    List<int> intList = new List<int>() { 9 };
    if (MonoSingleton<Indicator>.Instance.HasSecondaryInteraction)
      intList.Add(68);
    if (MonoSingleton<Indicator>.Instance.HasThirdInteraction)
      intList.Add(67);
    if (MonoSingleton<Indicator>.Instance.HasFourthInteraction)
      intList.Add(66);
    foreach (int action1 in intList)
    {
      ActionElementMap actionElementMap2 = controllerMapForCategory.GetActionElementMap(action1, Pole.Positive);
      if (actionElementMap2 != null && actionElementMap2.elementIdentifierId == actionElementMap1.elementIdentifierId)
        return true;
    }
    return false;
  }

  public float GetHorizontalAxis() => this.GetAxis(1);

  public float GetVerticalAxis() => this.GetAxis(0);

  public bool GetPauseButtonDown() => this.GetButtonDown(17);

  public bool GetMenuButtonDown() => this.GetButtonDown(26);

  public bool GetMenuButtonHeld() => this.GetButtonHeld(26);

  public bool GetAttackButtonDown() => this.GetButtonDown(2);

  public bool GetAttackButtonHeld() => this.GetButtonHeld(2);

  public bool GetAttackButtonUp() => this.GetButtonUp(2);

  public bool GetDodgeButtonDown()
  {
    return !this.OverlapsWithInteractionBinding(16 /*0x10*/) && this.GetButtonDown(16 /*0x10*/) && !MonoSingleton<UIManager>.Instance.ForceBlockMenus && (double) Time.timeScale >= 0.25 && !MMTransition.IsPlaying && !GameManager.InMenu && UIMenuBase.ActiveMenus.Count == 0;
  }

  public bool GetDodgeButtonHeld()
  {
    return !this.OverlapsWithInteractionBinding(16 /*0x10*/) && this.GetButtonHeld(16 /*0x10*/);
  }

  public bool GetDodgeRollButtonDown() => this.GetButtonUp(16 /*0x10*/);

  public bool GetCurseButtonDown() => this.GetButtonDown(13);

  public bool GetCurseButtonHeld() => this.GetButtonHeld(13);

  public bool GetCurseButtonUp() => this.GetButtonUp(13);

  public bool GetInteractButtonDown()
  {
    return (!((Object) BiomeGenerator.Instance != (Object) null) || InputManager.General.GetLastActiveController() == null || InputManager.General.GetLastActiveController().type != ControllerType.Mouse) && this.GetButtonDown(9);
  }

  public bool GetInteractButtonHeld()
  {
    return (!((Object) BiomeGenerator.Instance != (Object) null) || InputManager.General.GetLastActiveController() == null || InputManager.General.GetLastActiveController().type != ControllerType.Mouse) && this.GetButtonHeld(9);
  }

  public bool GetInteractButtonUp()
  {
    return (!((Object) BiomeGenerator.Instance != (Object) null) || InputManager.General.GetLastActiveController().type != ControllerType.Mouse) && this.GetButtonUp(9);
  }

  public bool GetInteract2ButtonDown()
  {
    return (!((Object) BiomeGenerator.Instance != (Object) null) || InputManager.General.GetLastActiveController().type != ControllerType.Mouse) && this.GetButtonDown(68);
  }

  public bool GetInteract3ButtonHeld() => this.GetButtonHeld(67);

  public bool GetPlaceMoveUpgradeButtonDown() => this.GetButtonDown(69);

  public bool GetPlaceMoveUpgradeButtonHeld() => this.GetButtonHeld(69);

  public bool GetRemoveFlipButtonDown() => this.GetButtonDown(70);

  public bool GetRemoveFlipButtonHeld() => this.GetButtonHeld(70);

  public bool GetTrackQuestButtonDown() => this.GetButtonDown(31 /*0x1F*/);

  public bool GetReturnToBaseButtonHeld()
  {
    return this.GetButtonHeld(23) && !MonoSingleton<UIManager>.Instance.MenusBlocked;
  }

  public bool GetMeditateButtonDown()
  {
    return !this.OverlapsWithInteractionBinding(59) && !MonoSingleton<UIManager>.Instance.MenusBlocked && (!((Object) PlayerFarming.Instance != (Object) null) || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.CustomAnimation) && this.GetButtonDown(59);
  }

  public bool GetMeditateButtonHeld() => this.GetButtonHeld(59);

  public bool GetAdvanceDialogueButtonDown() => this.GetButtonDown(64 /*0x40*/);

  public bool GetBleatButtonDown() => this.GetButtonDown(58);

  public bool GetBleatButtonHeld() => this.GetButtonHeld(58);

  public bool GetBleatButtonUp() => this.GetButtonUp(58);

  public bool GetCancelFishingButtonDown() => this.GetButtonDown(73);
}
