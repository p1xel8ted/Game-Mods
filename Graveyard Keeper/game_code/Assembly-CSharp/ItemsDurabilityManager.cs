// Decompiled with JetBrains decompiler
// Type: ItemsDurabilityManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ItemsDurabilityManager : MonoBehaviour
{
  public static List<WorldGameObject> _objs = (List<WorldGameObject>) null;
  public static List<Item> _drop_items = new List<Item>();
  public static bool _inited = false;
  public static float _delta_time = 0.0f;
  public static int _frames = 0;
  public bool _coroutine_is_active;
  public float _time_since_last_coroutine_start;
  public const int FRAMES_PERIOD = 10;
  public const int BREAK_WGOS_CYCLE_INTO_FRAMES = 4;
  public static ItemsDurabilityManager _me = (ItemsDurabilityManager) null;

  public static void Init(List<WorldGameObject> wgos_list, List<Item> drops_list)
  {
    if ((Object) ItemsDurabilityManager._me == (Object) null)
    {
      GameObject target = new GameObject("Items Durability Manager");
      ItemsDurabilityManager._me = target.AddComponent<ItemsDurabilityManager>();
      Object.DontDestroyOnLoad((Object) target);
    }
    ItemsDurabilityManager._objs = wgos_list;
    ItemsDurabilityManager._drop_items = drops_list;
    ItemsDurabilityManager._inited = true;
    ItemsDurabilityManager._delta_time = 0.0f;
    ItemsDurabilityManager._frames = 0;
  }

  public static void Stop() => ItemsDurabilityManager._inited = false;

  public void DoRecalc(float delta_time)
  {
    if (!ItemsDurabilityManager._inited)
      return;
    Item obj = (Item) null;
    if ((Object) MainGame.me.player != (Object) null)
      obj = MainGame.me.player.components.character.GetOverheadItem();
    obj?.UpdateDurability(delta_time);
    foreach (Item dropItem in ItemsDurabilityManager._drop_items)
      dropItem.UpdateDurability(delta_time);
    if (!this._coroutine_is_active)
    {
      if ((double) this._time_since_last_coroutine_start == 0.0)
        this._time_since_last_coroutine_start = delta_time;
      this.StartCoroutine(this.DoRecalcStep(this._time_since_last_coroutine_start));
      this._time_since_last_coroutine_start = 0.0f;
    }
    else
      this._time_since_last_coroutine_start += delta_time;
    if (!((Object) MainGame.me?.player != (Object) null))
      return;
    foreach (GameResAtom atom in MainGame.me.player.data.GetParams().ToAtomList())
    {
      if (atom.type.StartsWith("_cooldown_"))
        MainGame.me.player.data.SubFromParams(atom.type, delta_time);
    }
  }

  public IEnumerator DoRecalcStep(float delta_time)
  {
    this._coroutine_is_active = true;
    int cur_part = 0;
    int part_len = ItemsDurabilityManager._objs.Count / 4;
    for (int i = 0; i <= ItemsDurabilityManager._objs.Count; ++i)
    {
      if (part_len > 0)
      {
        int num = Mathf.FloorToInt((float) i / (float) part_len);
        if (num > cur_part)
        {
          cur_part = num;
          yield return (object) new WaitForEndOfFrame();
        }
      }
      if (i <= ItemsDurabilityManager._objs.Count)
      {
        WorldGameObject player;
        float parent_modificator;
        if (i == ItemsDurabilityManager._objs.Count)
        {
          player = MainGame.me.player;
          parent_modificator = 1f;
        }
        else
        {
          player = ItemsDurabilityManager._objs[i];
          if (!player.is_removed && player.data.inventory.Count != 0)
            parent_modificator = player.obj_def.durability_modificator;
          else
            continue;
        }
        foreach (Item obj in player.data.inventory)
          obj.UpdateDurability(delta_time, parent_modificator);
      }
      else
        break;
    }
    this._coroutine_is_active = false;
  }

  public static void EveryFrameUpdate(float delta_time)
  {
    if (!ItemsDurabilityManager._inited)
      return;
    ++ItemsDurabilityManager._frames;
    ItemsDurabilityManager._delta_time += delta_time;
    if (ItemsDurabilityManager._frames < 10)
      return;
    ItemsDurabilityManager._me.DoRecalc(ItemsDurabilityManager._delta_time);
    ItemsDurabilityManager._frames = 0;
    ItemsDurabilityManager._delta_time = 0.0f;
  }
}
