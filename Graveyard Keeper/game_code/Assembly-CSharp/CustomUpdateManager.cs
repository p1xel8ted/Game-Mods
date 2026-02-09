// Decompiled with JetBrains decompiler
// Type: CustomUpdateManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CustomUpdateManager : MonoBehaviour
{
  public static List<WorldGameObject> wgos = new List<WorldGameObject>();
  public static List<ICustomUpdateMonoBehaviour> updates = new List<ICustomUpdateMonoBehaviour>();
  public static CustomUpdateManager me = (CustomUpdateManager) null;

  public static void Init()
  {
    CustomUpdateManager.me = SingletonGameObjects.FindOrCreate<CustomUpdateManager>();
  }

  public void Update()
  {
    if (!MainGame.game_started || MainGame.paused)
    {
      if (!GUIElements.me.game_gui.is_shown)
        return;
      if (LazyInput.GetKeyDown(GameKey.Inventory))
        GUIElements.me.game_gui.OpenOrSelectTab(GameGUI.TabType.Inventory);
      if (LazyInput.GetKeyDown(GameKey.KnownNPCs))
        GUIElements.me.game_gui.OpenOrSelectTab(GameGUI.TabType.NPCs);
      if (LazyInput.GetKeyDown(GameKey.Techs))
        GUIElements.me.game_gui.OpenOrSelectTab(GameGUI.TabType.Techs);
      if (!LazyInput.GetKeyDown(GameKey.Map))
        return;
      GUIElements.me.game_gui.OpenOrSelectTab(GameGUI.TabType.Map);
    }
    else
    {
      for (int index = 0; index < CustomUpdateManager.wgos.Count; ++index)
        CustomUpdateManager.wgos[index].CustomUpdate();
      for (int index = 0; index < CustomUpdateManager.updates.Count; ++index)
        CustomUpdateManager.updates[index].CustomUpdate();
      CraftComponent.UpdateAllCrafts(Time.deltaTime);
      ItemsDurabilityManager.EveryFrameUpdate(Time.deltaTime);
      BuffsLogics.UpdateEveryFrame();
    }
  }

  public void FixedUpdate()
  {
    if (!MainGame.game_started || MainGame.paused)
      return;
    for (int index = 0; index < CustomUpdateManager.wgos.Count; ++index)
      CustomUpdateManager.wgos[index].CustomFixedUpdate();
  }

  public void LateUpdate()
  {
    if (!MainGame.game_started)
      return;
    for (int index = 0; index < CustomUpdateManager.wgos.Count; ++index)
      CustomUpdateManager.wgos[index].CustomLateUpdate();
  }
}
