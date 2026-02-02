// Decompiled with JetBrains decompiler
// Type: ControlMappings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Plawius;
using Rewired;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
[CreateAssetMenu(fileName = "Control Mapping", menuName = "Massive Monster/Control Mapping", order = 1)]
public class ControlMappings : ScriptableObject
{
  public const string kPCFontPath = "Assets/Plawius-ControllerIcons/Fonts/PC-Filled SDF.asset";
  public const string kSwitchFontPath = "Assets/Plawius-ControllerIcons/Fonts/Switch-Filled SDF.asset";
  public const string kPlaystationFontPath = "Assets/Plawius-ControllerIcons/Fonts/PS4-Filled SDF.asset";
  public const string kPS5FontPath = "Assets/Plawius-ControllerIcons/Fonts/PS5-Filled SDF.asset";
  public const string kXboxFontPath = "Assets/Plawius-ControllerIcons/Fonts/Xbox-Filled SDF.asset";
  [Header("Fonts")]
  [SerializeField]
  public ControllerIcons _fontController;
  public static TMP_FontAsset _PCFontAsset;
  public static TMP_FontAsset _switchFontAsset;
  public static TMP_FontAsset _ps4FontAsset;
  public static TMP_FontAsset _ps5FontAsset;
  public static TMP_FontAsset _xboxFontAsset;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void LoadPlatformFonts() => ControlMappings.DoLoadPlatformFonts();

  public static async void DoLoadPlatformFonts()
  {
    ControlMappings._PCFontAsset = await ControlMappings.LoadFontAsset("Assets/Plawius-ControllerIcons/Fonts/PC-Filled SDF.asset");
    ControlMappings._switchFontAsset = await ControlMappings.LoadFontAsset("Assets/Plawius-ControllerIcons/Fonts/Switch-Filled SDF.asset");
    ControlMappings._ps4FontAsset = await ControlMappings.LoadFontAsset("Assets/Plawius-ControllerIcons/Fonts/PS4-Filled SDF.asset");
    ControlMappings._ps5FontAsset = await ControlMappings.LoadFontAsset("Assets/Plawius-ControllerIcons/Fonts/PS5-Filled SDF.asset");
    ControlMappings._xboxFontAsset = await ControlMappings.LoadFontAsset("Assets/Plawius-ControllerIcons/Fonts/Xbox-Filled SDF.asset");
  }

  public static async Task<TMP_FontAsset> LoadFontAsset(string path)
  {
    Debug.Log((object) ("Load font asset at path: " + path).Colour(Color.yellow));
    AsyncOperationHandle<TMP_FontAsset> asyncOperation = Addressables.LoadAssetAsync<TMP_FontAsset>((object) path);
    TMP_FontAsset task = await asyncOperation.Task;
    if ((Object) asyncOperation.Result != (Object) null)
      return asyncOperation.Result;
    Debug.Log((object) "Font failed to load!".Colour(Color.red));
    return (TMP_FontAsset) null;
  }

  public TMP_FontAsset GetFontForPlatform(Platform platform)
  {
    switch (platform)
    {
      case Platform.Undefined:
      case Platform.PC:
        return ControlMappings._PCFontAsset;
      case Platform.PS4:
        return ControlMappings._ps4FontAsset;
      case Platform.PS5:
        return ControlMappings._ps5FontAsset;
      case Platform.XboxOne:
      case Platform.XboxSeries:
        return ControlMappings._xboxFontAsset;
      case Platform.Switch:
        return ControlMappings._switchFontAsset;
      default:
        return (TMP_FontAsset) null;
    }
  }

  public static string GetControllerCodeFromID(int id)
  {
    switch (id)
    {
      case 3:
      case 24:
        return "\uE90F";
      case 4:
        return "\uE900";
      case 5:
        return "\uE901";
      case 7:
        return ControlUtilities.GetCurrentInputType(InputManager.General.GetLastActiveController()) == InputType.SwitchProController ? "\uE903" : "\uE902";
      case 8:
        return ControlUtilities.GetCurrentInputType(InputManager.General.GetLastActiveController()) == InputType.SwitchProController ? "\uE902" : "\uE903";
      case 10:
        return "\uE915";
      case 11:
        return "\uE916";
      case 12:
        return "\uE917";
      case 13:
        return "\uE918";
      case 14:
        return "\uE919";
      case 15:
        return "\uE91A";
      case 16 /*0x10*/:
        return "\uE919";
      case 17:
        return "\uE909";
      case 18:
        return "\uE914";
      case 19:
        return "\uE90B";
      case 20:
        return "\uE90C";
      case 21:
        return "\uE90D";
      case 22:
        return "\uE90E";
      case 23:
        return "\uE904";
      default:
        return id.ToString();
    }
  }

  public static string GetKeyboardCode(KeyboardKeyCode keyCode, out bool isSpecialCharacter)
  {
    isSpecialCharacter = true;
    switch (keyCode)
    {
      case KeyboardKeyCode.Backspace:
        return "\uE901";
      case KeyboardKeyCode.Tab:
        return "\uE903";
      case KeyboardKeyCode.Return:
      case KeyboardKeyCode.KeypadEnter:
        return "\uE900";
      case KeyboardKeyCode.Escape:
        return "\uE91A";
      case KeyboardKeyCode.Space:
        return "\uE921";
      case KeyboardKeyCode.LeftBracket:
        return "\uE915";
      case KeyboardKeyCode.RightBracket:
        return "\uE917";
      case KeyboardKeyCode.UpArrow:
        return "\uE90B";
      case KeyboardKeyCode.DownArrow:
        return "\uE90D";
      case KeyboardKeyCode.RightArrow:
        return "\uE90C";
      case KeyboardKeyCode.LeftArrow:
        return "\uE90E";
      case KeyboardKeyCode.RightShift:
      case KeyboardKeyCode.LeftShift:
        return "\uE902";
      case KeyboardKeyCode.RightControl:
      case KeyboardKeyCode.LeftControl:
        return "\uE916";
      case KeyboardKeyCode.RightAlt:
      case KeyboardKeyCode.LeftAlt:
        return "\uE918";
      default:
        isSpecialCharacter = false;
        if (keyCode <= KeyboardKeyCode.BackQuote)
        {
          if (keyCode == KeyboardKeyCode.Pause)
            return "Pause";
          switch (keyCode - 34)
          {
            case KeyboardKeyCode.None:
              return "\"";
            case (KeyboardKeyCode) 1:
            case (KeyboardKeyCode) 2:
            case (KeyboardKeyCode) 3:
            case (KeyboardKeyCode) 4:
            case (KeyboardKeyCode) 6:
            case (KeyboardKeyCode) 7:
            case KeyboardKeyCode.Backspace:
            case KeyboardKeyCode.Tab:
            case (KeyboardKeyCode) 26:
              goto label_55;
            case (KeyboardKeyCode) 5:
              return "'";
            case (KeyboardKeyCode) 10:
              return ",";
            case (KeyboardKeyCode) 11:
              goto label_43;
            case KeyboardKeyCode.Clear:
              goto label_45;
            case KeyboardKeyCode.Return:
              goto label_38;
            case (KeyboardKeyCode) 14:
              break;
            case (KeyboardKeyCode) 15:
              goto label_28;
            case (KeyboardKeyCode) 16 /*0x10*/:
              goto label_29;
            case (KeyboardKeyCode) 17:
              goto label_30;
            case (KeyboardKeyCode) 18:
              goto label_31;
            case KeyboardKeyCode.Pause:
              goto label_32;
            case (KeyboardKeyCode) 20:
              goto label_33;
            case (KeyboardKeyCode) 21:
              goto label_34;
            case (KeyboardKeyCode) 22:
              goto label_35;
            case (KeyboardKeyCode) 23:
              goto label_36;
            case (KeyboardKeyCode) 24:
              return ":";
            case (KeyboardKeyCode) 25:
              return ";";
            case KeyboardKeyCode.Escape:
              goto label_41;
            default:
              switch (keyCode - 91)
              {
                case KeyboardKeyCode.None:
                  return "[";
                case (KeyboardKeyCode) 1:
                  return "\\";
                case (KeyboardKeyCode) 2:
                  return "]";
                case (KeyboardKeyCode) 5:
                  return "`";
                default:
                  goto label_55;
              }
          }
        }
        else if (keyCode <= KeyboardKeyCode.Numlock)
        {
          switch (keyCode - 256 /*0x0100*/)
          {
            case KeyboardKeyCode.None:
              break;
            case (KeyboardKeyCode) 1:
              goto label_28;
            case (KeyboardKeyCode) 2:
              goto label_29;
            case (KeyboardKeyCode) 3:
              goto label_30;
            case (KeyboardKeyCode) 4:
              goto label_31;
            case (KeyboardKeyCode) 5:
              goto label_32;
            case (KeyboardKeyCode) 6:
              goto label_33;
            case (KeyboardKeyCode) 7:
              goto label_34;
            case KeyboardKeyCode.Backspace:
              goto label_35;
            case KeyboardKeyCode.Tab:
              goto label_36;
            case (KeyboardKeyCode) 10:
              goto label_45;
            case (KeyboardKeyCode) 11:
              goto label_38;
            case KeyboardKeyCode.Clear:
              return "*";
            case KeyboardKeyCode.Return:
              goto label_43;
            case (KeyboardKeyCode) 14:
              return "+";
            case (KeyboardKeyCode) 15:
            case (KeyboardKeyCode) 17:
            case (KeyboardKeyCode) 18:
            case KeyboardKeyCode.Pause:
            case (KeyboardKeyCode) 20:
            case (KeyboardKeyCode) 21:
            case (KeyboardKeyCode) 22:
            case (KeyboardKeyCode) 23:
              goto label_55;
            case (KeyboardKeyCode) 16 /*0x10*/:
              goto label_41;
            case (KeyboardKeyCode) 24:
              return "PgUp";
            case (KeyboardKeyCode) 25:
              return "PgDn";
            default:
              if (keyCode == KeyboardKeyCode.Numlock)
                return "NumLk";
              goto label_55;
          }
        }
        else
        {
          if (keyCode == KeyboardKeyCode.ScrollLock)
            return "ScrLk";
          if (keyCode == KeyboardKeyCode.Print)
            return "PrtSc";
          goto label_55;
        }
        return "0";
label_28:
        return "1";
label_29:
        return "2";
label_30:
        return "3";
label_31:
        return "4";
label_32:
        return "5";
label_33:
        return "6";
label_34:
        return "7";
label_35:
        return "8";
label_36:
        return "9";
label_38:
        return "/";
label_41:
        return "=";
label_43:
        return "-";
label_45:
        return ".";
label_55:
        return keyCode.ToString();
    }
  }

  public static string GetMouseCode(MouseInputElement mouseInputElement, Pole axisContribution)
  {
    switch (mouseInputElement)
    {
      case MouseInputElement.AxisX:
        return axisContribution == Pole.Positive ? "\uE911" : "\uE913";
      case MouseInputElement.AxisY:
        return axisContribution == Pole.Positive ? "\uE910" : "\uE912";
      case MouseInputElement.Axis3:
        return axisContribution == Pole.Positive ? "\uE91D" : "\uE91F";
      case MouseInputElement.Button0:
        return "\uE909";
      case MouseInputElement.Button1:
        return "\uE914";
      case MouseInputElement.Button2:
        return "\uE919";
      case MouseInputElement.Button3:
        return "\uE920";
      case MouseInputElement.Button4:
        return "\uE91E";
      default:
        return mouseInputElement.ToString();
    }
  }

  public static string LocForAction(int action)
  {
    switch (action)
    {
      case 0:
        return "Move Vertical";
      case 1:
        return "Move Horizontal";
      case 2:
        return ScriptLocalization.UI_Settings_Controls.Attack;
      case 9:
        return ScriptLocalization.UI_Settings_Controls.Interact;
      case 13:
        return ScriptLocalization.UI_Settings_Controls.Shoot;
      case 16 /*0x10*/:
        return ScriptLocalization.UI_Settings_Controls.Dodge;
      case 17:
        return ScriptLocalization.UI_Settings_Controls.Pause;
      case 23:
        return ScriptLocalization.Interactions.ReturnToBase;
      case 26:
        return ScriptLocalization.UI_Settings_Controls.Menu;
      case 31 /*0x1F*/:
        return ScriptLocalization.UI_PauseScreen_Quests.TrackQuest;
      case 34:
        return "UI Vertical";
      case 35:
        return "UI Horizontal";
      case 38:
        return ScriptLocalization.UI_Generic.Accept;
      case 39:
        return ScriptLocalization.Interactions.Cancel;
      case 43:
        return ScriptLocalization.UI_Settings_Controls_UI.TabLeft;
      case 44:
        return ScriptLocalization.UI_Settings_Controls_UI.TabRight;
      case 48 /*0x30*/:
        return ScriptLocalization.UI_Generic.ApplyChanges;
      case 49:
        return ScriptLocalization.UI_Generic.ResetAll;
      case 56:
        return ScriptLocalization.Interactions.Cook;
      case 58:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.Bleat;
      case 59:
        return ScriptLocalization.UI_Settings_Controls.Meditate;
      case 60:
        return ScriptLocalization.UI_Settings_Controls.ResetBinding;
      case 64 /*0x40*/:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.AdvanceDialogue;
      case 65:
        return ScriptLocalization.UI_Settings_Controls_Bindings.Unbind;
      case 66:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.Interact4;
      case 67:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.Interact3;
      case 68:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.Interact2;
      case 69:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.PlaceMoveUpgrade;
      case 70:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.RemoveFlip;
      case 71:
        return ScriptLocalization.UI_Settings_Controls.UseRelic;
      case 73:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.CancelFishing;
      case 75:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.OpenGalleryLocateFolder;
      case 76:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.TakePhoto;
      case 77:
        return "Camera Height";
      case 80 /*0x50*/:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.DeletePhoto;
      case 83:
        return "Camera Focus";
      case 84:
        return "Camera Tilt";
      case 85:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.PlaceSticker;
      case 86:
        return "Rotate Sticker";
      case 87:
        return "Scale Sticker";
      case 88:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.FlipSticker;
      case 89:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.UndoSticker;
      case 90:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.SavePhoto;
      case 91:
        return ScriptLocalization.UI_Settings_Controls_PhotoMode.ClearStickers;
      case 93:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.FleeceAbility;
      case 94:
        return ScriptLocalization.UI_Settings_Controls_Gameplay.HeavyAttack;
      default:
        return string.Empty;
    }
  }
}
