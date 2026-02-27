// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Unify;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class SettingsTabNavigatorBase : MMTabNavigatorBase<SettingsTab>
{
  [SerializeField]
  private SettingsTab _graphicsTab;

  private void Awake()
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
