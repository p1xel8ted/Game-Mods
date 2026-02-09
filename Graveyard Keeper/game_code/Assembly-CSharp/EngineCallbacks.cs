// Decompiled with JetBrains decompiler
// Type: EngineCallbacks
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EngineCallbacks : LazyEngineCallbacks
{
  public override void OnCurrentItemChanged(ItemDefinition item, ItemDefinition prev_item)
  {
    if (item == null)
      item = ItemDefinition.none;
    if (prev_item != null && prev_item.is_placable)
      FloatingWorldGameObject.StopCurrentFloating();
    if (!item.is_placable)
      return;
    FloatingWorldGameObject floatingObject = FloatingWorldGameObject.CreateFloatingObject(Prefabs.me.test_place_obj_prefab);
    floatingObject.wobj.SetObject(item.id);
    floatingObject.UpdateObjSize();
    FloatingWorldGameObject.MoveCurrentFloatingObject((Vector2) MainGame.me.player.transform.localPosition, false, new Vector2?(MainGame.me.player_char.direction));
  }
}
