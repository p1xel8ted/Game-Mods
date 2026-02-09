// Decompiled with JetBrains decompiler
// Type: DropsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DropsList : MonoBehaviour
{
  public static DropsList _instance;
  public List<DropResGameObject> drops = new List<DropResGameObject>();
  public bool _initialized;

  public static DropsList me
  {
    get
    {
      return DropsList._instance ?? (DropsList._instance = Object.FindObjectOfType<DropsList>() ?? new GameObject("~DropsList").AddComponent<DropsList>());
    }
  }

  public bool Add(DropResGameObject drop)
  {
    if (this.drops.Contains(drop))
      return false;
    this.drops.Add(drop);
    return true;
  }

  public void Update()
  {
    for (int index = 0; index < this.drops.Count; ++index)
    {
      this.drops[index].UpdateMe();
      if (this.drops[index].is_collected)
      {
        Object.Destroy((Object) this.drops[index].gameObject);
        this.drops.RemoveAt(index);
        --index;
      }
    }
  }

  public void FixedUpdate()
  {
    float fixedDeltaTime = Time.fixedDeltaTime;
    foreach (DropResGameObject drop in this.drops)
      drop.FixedUpdateMe(fixedDeltaTime);
  }

  public void CheckDrops(WorldGameObject target_obj)
  {
    Vector3 position = target_obj.transform.position;
    foreach (DropResGameObject drop in this.drops)
    {
      if (!drop.has_target)
        drop.ProcessDropCollectorRangeCheck(target_obj, position);
    }
  }

  public void SetHighlighted(DropResGameObject drop)
  {
    if ((Object) drop == (Object) null)
    {
      foreach (DropResGameObject drop1 in this.drops)
        drop1.SetInteractionHilight(false);
    }
    else
    {
      foreach (DropResGameObject drop2 in this.drops)
        drop2.SetInteractionHilight((Object) drop2 == (Object) drop);
    }
  }

  public void RemoveAllDropsFromTheScene()
  {
    for (int index = 0; index < this.drops.Count; ++index)
      Object.Destroy((Object) this.drops[index].gameObject);
    this.drops.Clear();
  }

  public void ToGameSave(GameSave save)
  {
    save.drops = new List<GameSave.SavedDropItem>();
    foreach (DropResGameObject drop in this.drops)
      save.drops.Add(new GameSave.SavedDropItem()
      {
        pos = drop.transform.position,
        res = drop.res,
        zone_id = drop.zone_id
      });
  }

  public void FromGameSave(GameSave save)
  {
    if (save?.drops == null)
      return;
    foreach (GameSave.SavedDropItem drop in save.drops)
    {
      DropResGameObject dropResGameObject = DropResGameObject.Drop(drop.pos, drop.res, MainGame.me.world_root, Direction.IgnoreDirection, force_stacked_drop: true);
      if ((Object) dropResGameObject == (Object) null)
        Debug.LogError((object) ("Couldn't drop: " + drop.res?.ToString()));
      else
        dropResGameObject.zone_id = drop.zone_id;
    }
  }
}
