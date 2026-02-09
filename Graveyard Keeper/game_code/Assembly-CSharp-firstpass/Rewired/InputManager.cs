// Decompiled with JetBrains decompiler
// Type: Rewired.InputManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using System.ComponentModel;
using System.Text.RegularExpressions;

#nullable disable
namespace Rewired;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class InputManager : InputManager_Base
{
  public override void DetectPlatform()
  {
    this.editorPlatform = EditorPlatform.None;
    this.platform = Platform.Unknown;
    this.webplayerPlatform = WebplayerPlatform.None;
    this.isEditor = false;
    if (UnityEngine.SystemInfo.deviceName == null)
    {
      string empty1 = string.Empty;
    }
    if (UnityEngine.SystemInfo.deviceModel == null)
    {
      string empty2 = string.Empty;
    }
    this.platform = Platform.Windows;
  }

  public override void CheckRecompile()
  {
  }

  public override IExternalTools GetExternalTools() => (IExternalTools) new ExternalTools();

  public bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
  {
    return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
  }
}
