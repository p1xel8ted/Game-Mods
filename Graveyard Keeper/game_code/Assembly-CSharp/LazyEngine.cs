// Decompiled with JetBrains decompiler
// Type: LazyEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class LazyEngine
{
  public static LazyEngineCallbacks _callbacks = (LazyEngineCallbacks) null;
  public static string _cur_item = "";
  public static Transform _world_root;
  public const int LAYER_INTERACTION_COLLIDERS = 8;

  public static LazyEngineCallbacks callbacks => LazyEngine._callbacks;

  public static Transform world_root => LazyEngine._world_root;

  public static void Init(Transform world_root, LazyEngineCallbacks custom_callbacks)
  {
    LazyEngine._callbacks = custom_callbacks;
    LazyEngine._world_root = world_root;
  }

  public static void _OnItemChanged(ItemDefinition item)
  {
    string id = item == null ? "" : item.id;
    if (LazyEngine._cur_item == id)
      return;
    ItemDefinition dataOrNull = LazyEngine._cur_item == "" ? (ItemDefinition) null : GameBalance.me.GetDataOrNull<ItemDefinition>(LazyEngine._cur_item);
    LazyEngine._cur_item = id;
    LazyEngine.callbacks.OnCurrentItemChanged(item, dataOrNull);
  }

  public static void CancelCurrentItem() => LazyEngine._cur_item = "";
}
