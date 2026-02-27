// Decompiled with JetBrains decompiler
// Type: ControlUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using Steamworks;
using Unify;
using UnityEngine;

#nullable disable
public class ControlUtilities
{
  public static InputType GetCurrentInputType(Controller controller = null)
  {
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.PS4:
        return InputType.DualShock4;
      case UnifyManager.Platform.Switch:
        return controller != null && controller.name.Contains("Pro Controller") ? InputType.SwitchProController : InputType.SwitchJoyConsDocked;
      case UnifyManager.Platform.PS5:
        return InputType.DualSense;
      case UnifyManager.Platform.GameCoreConsole:
        return InputType.XboxSeries;
      default:
        if (controller == null || controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse)
          return InputType.Keyboard;
        if (SettingsManager.Settings.Game.GamepadPrompts != 0)
          return ControlUtilities.InputTypeFromControlPromptSetting();
        InputType currentInputType = ControlUtilities.InputTypeFromSteamInputType(GeneralInputSource.GetSteamInputType());
        Debug.Log((object) $"Steam informs us the controller is a {currentInputType}".Colour(Color.cyan));
        if (currentInputType != InputType.Undefined)
          return currentInputType;
        if (controller.name.Contains("Sony"))
        {
          if (controller.name.Contains("DualSense"))
            return InputType.DualSense;
          return controller.name.Contains("DualShock 4") ? InputType.DualShock4 : InputType.Undefined;
        }
        if (controller.name.Contains("Xbox"))
          return InputType.XboxSeries;
        if (controller.name.Contains("Nintendo"))
          return InputType.SwitchJoyConsDocked;
        return controller.name.Contains("Pro Controller") ? InputType.SwitchProController : InputType.XboxSeries;
    }
  }

  public static Platform GetPlatformFromInputType(InputType inputType)
  {
    switch (inputType)
    {
      case InputType.Undefined:
      case InputType.Keyboard:
        return Platform.PC;
      case InputType.DualShock4:
        return Platform.PS4;
      case InputType.DualSense:
        return Platform.PS5;
      case InputType.Xbox360:
      case InputType.XboxOne:
      case InputType.XboxSeries:
        return Platform.XboxSeries;
      case InputType.SwitchJoyConsDetached:
      case InputType.SwitchJoyConsDocked:
      case InputType.SwitchHandheld:
      case InputType.SwitchProController:
        return Platform.Switch;
      default:
        return Platform.PC;
    }
  }

  public static Platform GetPlatformFromUnifyPlatform()
  {
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.None:
      case UnifyManager.Platform.Standalone:
        return Platform.PC;
      case UnifyManager.Platform.XboxOne:
      case UnifyManager.Platform.GameCore:
      case UnifyManager.Platform.GameCoreConsole:
        return Platform.XboxSeries;
      case UnifyManager.Platform.PS4:
        return Platform.PS4;
      case UnifyManager.Platform.Switch:
        return Platform.Switch;
      case UnifyManager.Platform.PS5:
        return Platform.PS5;
      default:
        return Platform.PC;
    }
  }

  public static InputType GetCurrentInputTypeFromUnifyPlatform()
  {
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.PS4:
        return InputType.DualShock4;
      case UnifyManager.Platform.Switch:
        return InputType.SwitchJoyConsDocked;
      case UnifyManager.Platform.PS5:
        return InputType.DualSense;
      default:
        return InputType.Xbox360;
    }
  }

  private static InputType InputTypeFromControlPromptSetting()
  {
    switch (SettingsManager.Settings.Game.GamepadPrompts)
    {
      case 1:
        return InputType.XboxSeries;
      case 2:
        return InputType.DualShock4;
      case 3:
        return InputType.DualSense;
      case 4:
        return InputType.SwitchProController;
      default:
        return InputType.Undefined;
    }
  }

  public static InputType InputTypeFromSteamInputType(ESteamInputType steamInputType)
  {
    switch (steamInputType)
    {
      case ESteamInputType.k_ESteamInputType_XBox360Controller:
        return InputType.Xbox360;
      case ESteamInputType.k_ESteamInputType_XBoxOneController:
        return InputType.XboxOne;
      case ESteamInputType.k_ESteamInputType_PS4Controller:
        return InputType.DualShock4;
      case ESteamInputType.k_ESteamInputType_SwitchJoyConPair:
        return InputType.SwitchJoyConsDocked;
      case ESteamInputType.k_ESteamInputType_SwitchProController:
        return InputType.SwitchProController;
      default:
        return InputType.Undefined;
    }
  }
}
