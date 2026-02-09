// Decompiled with JetBrains decompiler
// Type: EventDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class EventDefinition
{
  public string spawn_wgo;
  public string drop_item;
  public string anim_trigger;
  public GameRes set_game_res = new GameRes();
  public string event_to_fire;
  public string sound;
  public bool anim_driven;

  public System.Action Invoke(
    Vector2 pos,
    Animator animator,
    System.Action dlg,
    WorldGameObject target_wgo,
    out bool need_to_wait_animation_finish)
  {
    need_to_wait_animation_finish = false;
    if ((UnityEngine.Object) animator != (UnityEngine.Object) null && animator.isInitialized)
    {
      animator.SetTrigger(this.anim_trigger);
      need_to_wait_animation_finish = true;
    }
    System.Action action = (System.Action) (() =>
    {
      if (dlg != null)
        dlg();
      if (!string.IsNullOrEmpty(this.spawn_wgo))
      {
        Transform parent = MainGame.me.dungeon_root.dungeon_is_loaded_now ? MainGame.me.dungeon_root.transform : MainGame.me.world_root;
        if (this.spawn_wgo.StartsWith("*"))
        {
          WorldMap.SpawnWSO(parent, this.spawn_wgo.Substring(1), new Vector2?(pos));
          Debug.Log((object) $"Spawned WSO \"{this.spawn_wgo}\" at pos {pos.ToString()}");
        }
        else
        {
          WorldMap.SpawnWGO(parent, this.spawn_wgo, new Vector3?((Vector3) pos));
          Debug.Log((object) $"Spawned WGO \"{this.spawn_wgo}\" at pos {pos.ToString()}");
        }
      }
      if (!string.IsNullOrEmpty(this.drop_item))
        DropResGameObject.Drop((Vector3) pos, new Item(this.drop_item, 1), MainGame.me.world_root);
      if (!((UnityEngine.Object) target_wgo != (UnityEngine.Object) null))
        return;
      target_wgo.SetParam(this.set_game_res);
      if (string.IsNullOrEmpty(this.event_to_fire))
        return;
      target_wgo.FireEvent(this.event_to_fire);
    });
    if (this.anim_driven)
      return action;
    action();
    return (System.Action) null;
  }
}
