// Decompiled with JetBrains decompiler
// Type: SaveGameFixer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class SaveGameFixer
{
  public static bool _need_reset_donkey_flag;

  public static void UnstuckWGO(WorldGameObject wgo)
  {
    if (!wgo.obj_def.IsNPC() || wgo.components.character.movement_state != MovementComponent.MovementState.None || wgo.GetParamInt("on_the_way_now") != 1)
      return;
    wgo.SetParam("on_the_way_now", 0.0f);
    if (wgo.obj_id.Contains("donkey"))
    {
      SaveGameFixer._need_reset_donkey_flag = true;
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("default_destroy_point");
      wgo.transform.position = gdPointByGdTag.transform.position;
      wgo.RefreshPositionCache();
      wgo.OnCameToGDPoint(gdPointByGdTag);
    }
    Debug.Log((object) ("Fixing stuck " + wgo.name), (Object) wgo);
  }

  public static void OnAfterAllInits()
  {
    if (!SaveGameFixer._need_reset_donkey_flag)
      return;
    SaveGameFixer._need_reset_donkey_flag = false;
    MainGame.me.player.SetParam("donkey_on_scene", 0.0f);
  }
}
