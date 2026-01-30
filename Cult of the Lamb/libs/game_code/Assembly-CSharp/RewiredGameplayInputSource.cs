// Decompiled with JetBrains decompiler
// Type: RewiredGameplayInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Rewired;
using src.Extensions;
using System.Collections.Generic;
using Unify.Input;
using UnityEngine;

#nullable disable
public class RewiredGameplayInputSource : CategoryInputSource
{
  public override int Category => 0;

  public static int[] AllBindings
  {
    get
    {
      return new int[21]
      {
        1,
        0,
        64 /*0x40*/,
        2,
        16 /*0x10*/,
        13,
        71,
        93,
        9,
        68,
        67,
        66,
        73,
        69,
        70,
        59,
        23,
        58,
        31 /*0x1F*/,
        17,
        26
      };
    }
  }

  public bool OverlapsWithInteractionBinding(int action, PlayerFarming playerFarming)
  {
    if (this._rewiredPlayer == null || !((Object) playerFarming != (Object) null) || !((Object) playerFarming.indicator != (Object) null) || !((Object) playerFarming.interactor.CurrentInteraction != (Object) null))
      return false;
    List<int> intList = new List<int>();
    if (playerFarming.interactor.CurrentInteraction.Interactable)
      intList.Add(9);
    if (playerFarming.indicator.HasSecondaryInteraction)
      intList.Add(68);
    if (playerFarming.indicator.HasThirdInteraction)
      intList.Add(67);
    if (playerFarming.indicator.HasFourthInteraction)
      intList.Add(66);
    return intList.Count > 0 && this.OverlapsWithBinding(playerFarming, action, intList.ToArray());
  }

  public bool OverlapsWithBinding(
    PlayerFarming playerFarming,
    int action,
    params int[] overlapCheck)
  {
    Controller activeController = InputManager.General.GetLastActiveController(playerFarming);
    if (activeController == null)
      return false;
    ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(0, activeController.type, playerFarming);
    if (controllerMapForCategory == null)
      return false;
    ActionElementMap actionElementMap1 = controllerMapForCategory.GetActionElementMap(action, Pole.Positive);
    if (actionElementMap1 == null)
      return false;
    foreach (int action1 in overlapCheck)
    {
      ActionElementMap actionElementMap2 = controllerMapForCategory.GetActionElementMap(action1, Pole.Positive);
      if (actionElementMap2 != null && actionElementMap2.elementIdentifierId == actionElementMap1.elementIdentifierId)
        return true;
    }
    return false;
  }

  public float GetHorizontalAxis(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    float horizontalAxis = this.GetAxis(1, playerFarming);
    if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.InvertMovement)
      horizontalAxis = -horizontalAxis;
    return horizontalAxis;
  }

  public float GetVerticalAxis(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    float verticalAxis = this.GetAxis(0, playerFarming);
    if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.InvertMovement)
      verticalAxis = -verticalAxis;
    return verticalAxis;
  }

  public float GetHorizontalSecondaryAxis(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    float horizontalSecondaryAxis = this.GetAxis(96 /*0x60*/, playerFarming);
    if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.InvertMovement)
      horizontalSecondaryAxis = -horizontalSecondaryAxis;
    return horizontalSecondaryAxis;
  }

  public float GetVerticalSecondaryAxis(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    float verticalSecondaryAxis = this.GetAxis(97, playerFarming);
    if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.InvertMovement)
      verticalSecondaryAxis = -verticalSecondaryAxis;
    return verticalSecondaryAxis;
  }

  public bool GetPauseButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(17, playerFarming);
  }

  public bool GetMenuButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(26, playerFarming);
  }

  public bool GetMenuButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(26, playerFarming);
  }

  public bool GetAttackButtonDown(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    if (!this.GetButtonDown(2, playerFarming))
      return false;
    if (!this.OverlapsWithInteractionBinding(2, playerFarming))
      return true;
    switch (playerFarming.interactor.CurrentInteraction)
    {
      case Interaction_EntranceShrine _:
      case Interaction_EntrySignPost _:
      case Interaction_Woodcutting _:
      case Interaction_Berries _:
      case Interaction_Tailor _:
      case Interaction_TempleAltar _:
      case Interaction_SimpleConversation _:
        return true;
      default:
        return false;
    }
  }

  public bool GetAttackButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(2, playerFarming);
  }

  public bool GetAttackButtonUp(PlayerFarming playerFarming = null)
  {
    return this.GetButtonUp(2, playerFarming);
  }

  public bool GetDodgeButtonDown(PlayerFarming playerFarming = null)
  {
    return !this.OverlapsWithInteractionBinding(16 /*0x10*/, playerFarming) && this.GetButtonDown(16 /*0x10*/, playerFarming);
  }

  public bool GetDodgeButtonHeld(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    return !this.OverlapsWithInteractionBinding(16 /*0x10*/, playerFarming) && this.GetButtonHeld(16 /*0x10*/, playerFarming);
  }

  public bool GetDodgeRollButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonUp(16 /*0x10*/, playerFarming);
  }

  public bool GetCurseButtonDown(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    return !this.OverlapsWithInteractionBinding(13, playerFarming) && this.GetButtonDown(13, playerFarming);
  }

  public bool GetCurseButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(13, playerFarming);
  }

  public bool GetCurseButtonUp(PlayerFarming playerFarming = null)
  {
    return this.GetButtonUp(13, playerFarming);
  }

  public bool GetInteractButtonDown(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    return this.GetButtonDown(9, playerFarming);
  }

  public bool GetInteractButtonHeld(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    return this.GetButtonHeld(9, playerFarming);
  }

  public bool GetInteractButtonUp(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    return this.GetButtonUp(9, playerFarming);
  }

  public bool GetInteract2ButtonDown(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    return this.GetButtonDown(68, playerFarming);
  }

  public bool GetInteract2ButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(68, playerFarming);
  }

  public bool GetInteract3ButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(67, playerFarming);
  }

  public bool GetInteract3ButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(67, playerFarming);
  }

  public bool GetInteract4ButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(66, playerFarming);
  }

  public bool GetPlaceMoveUpgradeButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(69, playerFarming);
  }

  public bool GetPlaceMoveUpgradeButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(69, playerFarming);
  }

  public bool GetRemoveFlipButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(70, playerFarming);
  }

  public bool GetRemoveFlipButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(70, playerFarming);
  }

  public bool GetTrackQuestButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(31 /*0x1F*/, playerFarming);
  }

  public bool GetTrackQuestButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(31 /*0x1F*/, playerFarming);
  }

  public bool GetReturnToBaseButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(23, playerFarming) && !MonoSingleton<UIManager>.Instance.MenusBlocked;
  }

  public bool GetAbilityButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(23, playerFarming);
  }

  public bool GetMeditateButtonDown(PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    return !this.OverlapsWithInteractionBinding(59, playerFarming) && !MonoSingleton<UIManager>.Instance.MenusBlocked && (!((Object) playerFarming != (Object) null) || playerFarming.state.CURRENT_STATE != StateMachine.State.CustomAnimation) && this.GetButtonDown(59, playerFarming);
  }

  public bool GetMeditateButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(59, playerFarming);
  }

  public bool GetAdvanceDialogueButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(64 /*0x40*/, playerFarming);
  }

  public bool GetBleatButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(58, playerFarming);
  }

  public bool GetBleatButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(58, playerFarming);
  }

  public bool GetBleatButtonUp(PlayerFarming playerFarming = null)
  {
    return this.GetButtonUp(58, playerFarming);
  }

  public bool GetRelicButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(71, playerFarming);
  }

  public bool GetCancelFishingButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(73, playerFarming);
  }

  public bool GetRelicButtonDown(PlayerFarming playerFarming = null)
  {
    return !MonoSingleton<UIManager>.Instance.MenusBlocked && this.GetButtonDown(71, playerFarming);
  }

  public bool GetFleeceAbilityButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(93, playerFarming);
  }

  public bool GetFleeceAbilityButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(93, playerFarming);
  }

  public bool GetHeavyAttackButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(94, playerFarming);
  }

  public bool GetHeavyAttackButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(94, playerFarming);
  }

  public bool GetAnyButtonDownExcludingMouse(PlayerFarming playerFarming)
  {
    if ((Object) playerFarming == (Object) null)
    {
      for (int playerNo = 0; playerNo < 4; ++playerNo)
      {
        Player player = RewiredInputManager.GetPlayer(playerNo);
        if (player != null && player.GetAnyButtonDown())
          return true;
      }
      return false;
    }
    return (bool) (Object) playerFarming && playerFarming.rewiredPlayer != null && playerFarming.rewiredPlayer.GetAnyButtonDown();
  }
}
