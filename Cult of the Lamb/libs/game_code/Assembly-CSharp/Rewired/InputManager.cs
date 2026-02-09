// Decompiled with JetBrains decompiler
// Type: Rewired.InputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
namespace Rewired;

[AddComponentMenu("Rewired/Input Manager")]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class InputManager : InputManager_Base
{
  public bool ignoreRecompile;

  public override void OnInitialized() => this.SubscribeEvents();

  public override void OnDeinitialized() => this.UnsubscribeEvents();

  public override void DetectPlatform()
  {
    this.scriptingBackend = ScriptingBackend.Mono;
    this.scriptingAPILevel = ScriptingAPILevel.Net20;
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
    this.scriptingBackend = ScriptingBackend.Mono;
    this.scriptingAPILevel = ScriptingAPILevel.Net46;
  }

  public override void CheckRecompile()
  {
  }

  public override IExternalTools GetExternalTools() => (IExternalTools) new ExternalTools();

  public bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
  {
    return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
  }

  public void SubscribeEvents()
  {
    this.UnsubscribeEvents();
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
  }

  public void UnsubscribeEvents()
  {
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
  }

  public void OnSceneLoaded(Scene scene, LoadSceneMode mode) => this.OnSceneLoaded();
}
