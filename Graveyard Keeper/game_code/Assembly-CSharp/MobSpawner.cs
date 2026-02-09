// Decompiled with JetBrains decompiler
// Type: MobSpawner
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class MobSpawner : WorldSimpleObject
{
  public string spawner_id = string.Empty;
  public float chance_to_spawn = 1f;
  public List<WorldGameObject> spawned_mobs = new List<WorldGameObject>();
  public string custom_tag = "";
  public bool _delayed_mobs_list_deserialize;
  public List<long> _delayed_list;

  public void ActivateSpawner(int dungeon_level, List<SavedDungeonObject> saved_objs)
  {
    bool flag = false;
    if (saved_objs != null && saved_objs.Count > 0)
    {
      Vector2 localPosition = (Vector2) this.transform.localPosition;
      foreach (SavedDungeonObject savedObj in saved_objs)
      {
        if (savedObj.type == SavedDungeonObject.SavedDungeonObjectType.Mob)
        {
          Vector2 vector2 = savedObj.local_position - localPosition;
          if ((double) Mathf.Abs(vector2.x) < 0.1 && (double) Mathf.Abs(vector2.y) < 0.1)
          {
            if (savedObj.mob_is_alive)
              flag = true;
            Debug.Log((object) $"#dgen# Mob Spawner {this.name}[{this.spawner_id}] is {(flag ? "" : "NOT ")}activated because of saved_objs");
            break;
          }
        }
      }
    }
    else
    {
      float num = UnityEngine.Random.value;
      if ((double) num <= (double) this.chance_to_spawn)
      {
        Debug.Log((object) $"[{num.ToString()}<={this.chance_to_spawn.ToString()}]");
        flag = true;
      }
      else
        Debug.Log((object) $"[{num.ToString()}>{this.chance_to_spawn.ToString()}]");
    }
    if (flag)
    {
      SpawnerDefinition spawnerDefinition = (SpawnerDefinition) null;
      int num = 0;
      while (dungeon_level - num >= 1)
      {
        spawnerDefinition = GameBalance.me.GetDataOrNull<SpawnerDefinition>($"{this.spawner_id}_{(dungeon_level - num).ToString()}");
        ++num;
        if (spawnerDefinition != null)
          break;
      }
      if (spawnerDefinition == null)
      {
        Debug.LogError((object) $"Can not find data for: [{this.spawner_id}_{dungeon_level.ToString()}]");
      }
      else
      {
        SpawnerDefinition.MobDefinition mobToSpawn = spawnerDefinition.GetMobToSpawn();
        if (mobToSpawn == null)
        {
          Debug.Log((object) "Mob to spawn is null!");
        }
        else
        {
          Debug.Log((object) ("Spawning mob: " + mobToSpawn.mob_name));
          this.SpawnMob(mobToSpawn);
        }
      }
    }
    else
      Debug.Log((object) "NOT Spawning mob");
  }

  public void ActivateSpawner()
  {
    SpawnerDefinition dataOrNull = GameBalance.me.GetDataOrNull<SpawnerDefinition>(this.spawner_id);
    if (dataOrNull == null)
    {
      Debug.LogError((object) $"Can not find data for: [{this.spawner_id}]");
    }
    else
    {
      SpawnerDefinition.MobDefinition mobToSpawn = dataOrNull.GetMobToSpawn();
      if (mobToSpawn == null)
      {
        Debug.Log((object) "Mob to spawn is null!");
      }
      else
      {
        Debug.Log((object) ("Spawning mob: " + mobToSpawn.mob_name));
        this.SpawnMob(mobToSpawn);
      }
    }
  }

  public void SpawnMob(SpawnerDefinition.MobDefinition mob_to_spawn)
  {
    if (mob_to_spawn.mobs_count == 0)
      return;
    int num = 0;
    do
    {
      Vector2 vector2 = (Vector2) this.transform.localPosition + UnityEngine.Random.insideUnitCircle * 0.01f;
      WorldGameObject wgo = WorldMap.SpawnWGO(this.transform.parent, mob_to_spawn.mob_name);
      wgo.transform.localPosition = (Vector3) vector2;
      BaseCharacterComponent character = wgo.components.character;
      character.spawner_coords = (Vector2) this.transform.localPosition;
      character.spawner = this;
      WorldMap.OnUsedSpawner(this);
      this.spawned_mobs.Add(wgo);
      if (!string.IsNullOrEmpty(mob_to_spawn.craft_name))
        GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => wgo.TryStartCraft(mob_to_spawn.craft_name)));
      ++num;
    }
    while (num < mob_to_spawn.mobs_count);
  }

  public MobSpawner.SerializableSpawner ToSerializable()
  {
    if (this.unique_id == -1L)
      this.unique_id = UniqueID.GetUniqueID();
    List<long> longList = new List<long>();
    foreach (WorldGameObject spawnedMob in this.spawned_mobs)
    {
      if (spawnedMob.unique_id == -1L)
        Debug.LogError((object) "Mob doesn't have a unique ID (has to be created before spawner serialize)", (UnityEngine.Object) spawnedMob);
      else
        longList.Add(spawnedMob.unique_id);
    }
    return new MobSpawner.SerializableSpawner()
    {
      chance_to_spawn = this.chance_to_spawn,
      unique_id = this.unique_id,
      custom_tag = this.custom_tag,
      spawner_id = this.spawner_id,
      mobs = longList,
      coords = new IntVector2((Vector2) this.transform.position)
    };
  }

  public void FromSerializable(MobSpawner.SerializableSpawner data)
  {
    this.chance_to_spawn = data.chance_to_spawn;
    this.unique_id = data.unique_id;
    this.custom_tag = data.custom_tag;
    this.spawner_id = data.spawner_id;
    this.spawned_mobs.Clear();
    this._delayed_mobs_list_deserialize = true;
    this._delayed_list = data.mobs;
  }

  public void DelayedDeserialize()
  {
    if (!this._delayed_mobs_list_deserialize)
      return;
    this.spawned_mobs.Clear();
    foreach (long delayed in this._delayed_list)
    {
      WorldGameObject objectByUniqueId = WorldMap.GetWorldGameObjectByUniqueId(delayed);
      if ((UnityEngine.Object) objectByUniqueId != (UnityEngine.Object) null)
        this.spawned_mobs.Add(objectByUniqueId);
    }
    this._delayed_mobs_list_deserialize = false;
    this._delayed_list = (List<long>) null;
  }

  [Serializable]
  public struct SerializableSpawner
  {
    public long unique_id;
    public string custom_tag;
    public string spawner_id;
    public float chance_to_spawn;
    public List<long> mobs;
    public IntVector2 coords;
  }
}
