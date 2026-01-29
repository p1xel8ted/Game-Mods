// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unify;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class SettingsTabNavigatorBase : MMTabNavigatorBase<SettingsTab>
{
  [SerializeField]
  public SettingsTab _graphicsTab;

  public void Awake()
  {
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.None:
        break;
      case UnifyManager.Platform.Standalone:
        break;
      default:
        this._graphicsTab.gameObject.SetActive(false);
        break;
    }
  }
}
