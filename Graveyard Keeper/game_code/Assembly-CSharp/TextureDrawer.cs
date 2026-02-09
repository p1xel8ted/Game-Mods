// Decompiled with JetBrains decompiler
// Type: TextureDrawer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class TextureDrawer : MonoBehaviour
{
  public const int TEXTURE_SIZE_IN_BITS = 8;
  public const int TEXTURE_SIZE = 256 /*0x0100*/;
  public const float DEAD_MOBS_PERCENT_FOR_DUNGEON_COMPLETING = 90f;
  public int random_seed = -1;
  public int mobs_level = 1;
  public Tileset tileset;
  public MeshRenderer mesh_renderer;
  public List<WorldGameObject> spawned_mobs;
  public IntVector2 enter_point = new IntVector2(128 /*0x80*/, 128 /*0x80*/);
  public GameObject enter_to_dunge;
  public List<DungeonRoom> main_walker_rooms = new List<DungeonRoom>();
  [HideInInspector]
  public int main_walker_thickness = 3;
  [HideInInspector]
  public int main_walker_step_length = 6;
  [HideInInspector]
  public int main_walker_min_path = 128 /*0x80*/;
  [HideInInspector]
  public int main_walker_max_path = 256 /*0x0100*/;
  [HideInInspector]
  public int main_walker_max_steps_between_rooms = 3;
  [HideInInspector]
  public int sub_walkers_count = 3;
  [HideInInspector]
  public int room_borders = 2;
  public DungeonPreset cur_dungeon_preset;
  public Dungeon cur_dungeon;
  public SavedDungeon cur_saved_dungeon;
  public DungeonWalker.ActionChances main_walker_chances = new DungeonWalker.ActionChances();
  public bool dungeon_is_loaded_now;
  public DungeonStatistics statistics = new DungeonStatistics();

  public void DrawTexture(DungeonPreset dungeon_preset = null)
  {
    if ((Object) this.mesh_renderer == (Object) null)
    {
      Debug.LogError((object) "Mesh renderer is null!");
    }
    else
    {
      Texture2D texture2D = this.mesh_renderer.GetComponent<Texture2D>();
      if ((Object) texture2D == (Object) null || texture2D.width != 256 /*0x0100*/)
      {
        texture2D = new Texture2D(256 /*0x0100*/, 256 /*0x0100*/, TextureFormat.RGBA32, false);
        texture2D.filterMode = FilterMode.Point;
      }
      Color[] colors = new Color[65536 /*0x010000*/];
      this.DestroyTiles();
      if ((Object) dungeon_preset == (Object) null)
      {
        Debug.LogError((object) "Can not generate dungeon: dungeon preset is null!");
      }
      else
      {
        this.cur_dungeon_preset = dungeon_preset;
        this.tileset = this.cur_dungeon_preset.GetTileset();
        this.cur_saved_dungeon = !Application.isPlaying ? new SavedDungeon() : MainGame.me.save.dungeons.GetSavedDungeon(this.cur_dungeon_preset.dungeon_level);
        if (this.cur_saved_dungeon == null)
        {
          Debug.LogError((object) "Dungeon save is null!");
        }
        else
        {
          if (this.cur_saved_dungeon.seed == -1)
            this.cur_saved_dungeon.seed = !Application.isPlaying ? (this.random_seed == -1 ? Random.Range(0, 10000) : this.random_seed) : MainGame.me.save.dungeons.GetDungeonSeed(this.cur_dungeon_preset.dungeon_level);
          if (this.statistics == null)
            this.statistics = new DungeonStatistics();
          this.statistics.SetDefault();
          float realtimeSinceStartup = Time.realtimeSinceStartup;
          Dungeon dungeon = this.cur_dungeon_preset.GenerateDungeon(this.cur_saved_dungeon, true);
          Debug.Log((object) ("#dgen# Dungeon generation time = " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString()));
          Debug.Log((object) this.statistics.ToString());
          int[,] matrix = dungeon.GetMatrix();
          for (int index1 = 0; index1 < 256 /*0x0100*/; ++index1)
          {
            for (int index2 = 0; index2 < 256 /*0x0100*/; ++index2)
            {
              Color color = (index1 + index2) % 2 == 1 ? Color.gray : Color.black;
              if (matrix[index1, index2] == 1)
                color = Color.green;
              else if (matrix[index1, index2] == 2)
                color = Color.blue;
              colors[index1 + index2 * 256 /*0x0100*/] = color;
            }
          }
          this.ArrangeTiles(matrix);
          List<MobSpawner> mob_spawners;
          this.PlaceRooms(dungeon, out mob_spawners, this.cur_saved_dungeon.objects);
          this.spawned_mobs = this.ActivateMobSpawners(mob_spawners, this.cur_dungeon_preset.dungeon_level, this.cur_saved_dungeon.objects);
          this.InitAllOptimizedColliders();
          this.cur_dungeon = dungeon;
          texture2D.SetPixels(colors);
          texture2D.Apply();
          if (!this.dungeon_is_loaded_now)
            GJTimer.AddTimer(0.1f, new GJTimer.VoidDelegate(this.ImportGDPoints));
          this.mesh_renderer.material.mainTexture = (Texture) texture2D;
          this.dungeon_is_loaded_now = true;
          foreach (ChunkedGameObject componentsInChild in this.gameObject.GetComponentsInChildren<ChunkedGameObject>(true))
            componentsInChild.is_temp = true;
        }
      }
    }
  }

  public void ArrangeTiles(int[,] dunge_matrix)
  {
    if ((Object) this.tileset == (Object) null)
      return;
    Debug.Log((object) "#dgen# Started arranging tiles.");
    for (int x = 1; x < (int) byte.MaxValue; ++x)
    {
      for (int y = 1; y < (int) byte.MaxValue; ++y)
      {
        if (dunge_matrix[x, y] == 1)
        {
          bool need_mirror = false;
          WorldSimpleObject original = (WorldSimpleObject) null;
          if (dunge_matrix[x - 1, y] != 0 && dunge_matrix[x + 1, y] != 0 && dunge_matrix[x, y - 1] != 0 && dunge_matrix[x, y + 1] != 0)
          {
            if (dunge_matrix[x + 1, y + 1] == 0)
              original = this.tileset.GetTilePrefab(Tileset.WallType.CornerInDownLeft, out need_mirror);
            else if (dunge_matrix[x + 1, y - 1] == 0)
              original = this.tileset.GetTilePrefab(Tileset.WallType.CornerInUpLeft, out need_mirror);
            else if (dunge_matrix[x - 1, y + 1] == 0)
              original = this.tileset.GetTilePrefab(Tileset.WallType.CornerInDownRight, out need_mirror);
            else if (dunge_matrix[x - 1, y - 1] == 0)
              original = this.tileset.GetTilePrefab(Tileset.WallType.CornerInUpRight, out need_mirror);
          }
          else if (dunge_matrix[x - 1, y] != 0 && dunge_matrix[x + 1, y] != 0 && dunge_matrix[x, y - 1] != 0 && dunge_matrix[x, y + 1] == 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.Up, out need_mirror);
          else if (dunge_matrix[x - 1, y] != 0 && dunge_matrix[x + 1, y] != 0 && dunge_matrix[x, y - 1] == 0 && dunge_matrix[x, y + 1] != 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.Down, out need_mirror);
          else if (dunge_matrix[x - 1, y] == 0 && dunge_matrix[x + 1, y] != 0 && dunge_matrix[x, y - 1] != 0 && dunge_matrix[x, y + 1] != 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.Left, out need_mirror);
          else if (dunge_matrix[x - 1, y] != 0 && dunge_matrix[x + 1, y] == 0 && dunge_matrix[x, y - 1] != 0 && dunge_matrix[x, y + 1] != 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.Right, out need_mirror);
          else if (dunge_matrix[x - 1, y] == 0 && dunge_matrix[x + 1, y] != 0 && dunge_matrix[x, y - 1] != 0 && dunge_matrix[x, y + 1] == 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.CornerOutUpLeft, out need_mirror);
          else if (dunge_matrix[x - 1, y] != 0 && dunge_matrix[x + 1, y] == 0 && dunge_matrix[x, y - 1] != 0 && dunge_matrix[x, y + 1] == 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.CornerOutUpRight, out need_mirror);
          else if (dunge_matrix[x - 1, y] == 0 && dunge_matrix[x + 1, y] != 0 && dunge_matrix[x, y - 1] == 0 && dunge_matrix[x, y + 1] != 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.CornerOutDownLeft, out need_mirror);
          else if (dunge_matrix[x - 1, y] != 0 && dunge_matrix[x + 1, y] == 0 && dunge_matrix[x, y - 1] == 0 && dunge_matrix[x, y + 1] != 0)
            original = this.tileset.GetTilePrefab(Tileset.WallType.CornerOutDownRight, out need_mirror);
          if ((Object) original != (Object) null)
          {
            WorldSimpleObject worldSimpleObject = Object.Instantiate<WorldSimpleObject>(original, this.gameObject.transform, false);
            if ((Object) worldSimpleObject == (Object) null)
            {
              Debug.LogError((object) ("Can not Instantiate tile: " + original.name));
              continue;
            }
            worldSimpleObject.transform.localPosition = (Vector3) new Vector2((float) x, (float) y);
            if (need_mirror)
            {
              Vector3 localScale = worldSimpleObject.transform.localScale;
              localScale.x *= -1f;
              worldSimpleObject.transform.localScale = localScale;
            }
          }
          Object.Instantiate<WorldSimpleObject>(this.tileset.GetTilePrefab(Tileset.WallType.Floor, out bool _), this.gameObject.transform, false).transform.localPosition = (Vector3) new Vector2((float) x, (float) y);
        }
      }
    }
  }

  public void PlaceRooms(
    Dungeon generated_dungeon,
    out List<MobSpawner> mob_spawners,
    List<SavedDungeonObject> saved_objs)
  {
    mob_spawners = new List<MobSpawner>();
    if ((Object) this.tileset == (Object) null)
      return;
    mob_spawners = new List<MobSpawner>();
    foreach (DungeonRoom placedRoom in generated_dungeon.main_walker.placed_rooms)
    {
      List<MobSpawner> spawners;
      placedRoom.room_interior.DrawRoom(this.gameObject.transform, placedRoom.coords.ToVector2(), placedRoom.enters_coords, generated_dungeon.main_walker.real_thickness, this.tileset, out spawners, saved_objs, generated_dungeon.main_walker.placed_rooms.IndexOf(placedRoom) == 0);
      mob_spawners.AddRange((IEnumerable<MobSpawner>) spawners);
    }
    foreach (DungeonWalker subWalker in generated_dungeon.sub_walkers)
    {
      foreach (DungeonRoom placedRoom in subWalker.placed_rooms)
      {
        List<MobSpawner> spawners;
        placedRoom.room_interior.DrawRoom(this.gameObject.transform, placedRoom.coords.ToVector2(), placedRoom.enters_coords, subWalker.real_thickness, this.tileset, out spawners, saved_objs, subWalker.placed_rooms.IndexOf(placedRoom) == 0);
        mob_spawners.AddRange((IEnumerable<MobSpawner>) spawners);
      }
    }
  }

  public void DestroyTiles()
  {
    foreach (Component componentsInChild in this.gameObject.GetComponentsInChildren<WorldGameObject>(true))
      componentsInChild.gameObject.Destroy();
    foreach (Component componentsInChild in this.gameObject.GetComponentsInChildren<WorldSimpleObject>(true))
      componentsInChild.gameObject.Destroy();
    foreach (DropResGameObject componentsInChild in this.gameObject.GetComponentsInChildren<DropResGameObject>(true))
    {
      WorldMap.OnDropItemRemoved(componentsInChild.res);
      componentsInChild.is_collected = true;
      componentsInChild.DestroyLinkedHint();
    }
    if (this.dungeon_is_loaded_now)
      this.ExportGDPoints();
    this.dungeon_is_loaded_now = false;
    this.cur_dungeon_preset = (DungeonPreset) null;
    this.cur_saved_dungeon = (SavedDungeon) null;
    GJTimer.AddTimer(0.05f, new GJTimer.VoidDelegate(ChunkManager.RemovePedingTempObjects));
  }

  public List<WorldGameObject> ActivateMobSpawners(
    List<MobSpawner> mob_spawners,
    int dungeon_level,
    List<SavedDungeonObject> saved_objs = null)
  {
    if (saved_objs == null)
      saved_objs = new List<SavedDungeonObject>();
    List<WorldGameObject> worldGameObjectList = new List<WorldGameObject>();
    foreach (MobSpawner mobSpawner in mob_spawners)
    {
      mobSpawner.ActivateSpawner(dungeon_level, saved_objs);
      worldGameObjectList.AddRange((IEnumerable<WorldGameObject>) mobSpawner.spawned_mobs);
    }
    return worldGameObjectList;
  }

  public bool TrySaveDungeon()
  {
    if ((Object) this.cur_dungeon_preset == (Object) null)
    {
      Debug.LogError((object) "Current dungeon preset is null!");
      return false;
    }
    if (this.cur_saved_dungeon == null)
    {
      Debug.LogError((object) ("cur_saved_dungeon is null! Loading cur_saved_dungeon #" + this.cur_dungeon_preset.dungeon_level.ToString()));
      this.cur_saved_dungeon = MainGame.me.save.dungeons.GetSavedDungeon(this.cur_dungeon_preset.dungeon_level);
    }
    if (this.cur_saved_dungeon == null)
    {
      Debug.LogError((object) "Saved dungeon is null!");
      return false;
    }
    this.cur_saved_dungeon.dungeon_preset_name = this.cur_dungeon_preset.name;
    if (this.cur_saved_dungeon.seed == -1)
      this.cur_saved_dungeon.seed = MainGame.me.save.dungeons.GetDungeonSeed(this.cur_dungeon_preset.dungeon_level);
    this.cur_saved_dungeon.objects = new List<SavedDungeonObject>();
    WorldGameObject[] componentsInChildren = this.GetComponentsInChildren<WorldGameObject>(true);
    List<BaseCharacterComponent> characterComponentList = new List<BaseCharacterComponent>();
    foreach (WorldGameObject worldGameObject in componentsInChildren)
    {
      if (!((Object) worldGameObject == (Object) null) && !(worldGameObject.obj_id == "0") && !(worldGameObject.obj_id == "empty") && worldGameObject.obj_def != null && worldGameObject.obj_def.type != ObjectDefinition.ObjType.Default && worldGameObject.components.character.enabled)
        characterComponentList.Add(worldGameObject.components.character);
    }
    if (characterComponentList.Count == 0)
    {
      Debug.Log((object) "Not found any alive mob in dungeon.");
    }
    else
    {
      this.cur_saved_dungeon.objects = new List<SavedDungeonObject>();
      foreach (BaseCharacterComponent characterComponent in characterComponentList)
        this.cur_saved_dungeon.objects.Add(new SavedDungeonObject()
        {
          name = characterComponent.wgo.name,
          local_position = characterComponent.spawner_coords,
          type = SavedDungeonObject.SavedDungeonObjectType.Mob,
          mob_is_alive = true
        });
    }
    foreach (WorldGameObject worldGameObject in componentsInChildren)
    {
      if (!worldGameObject.components.character.enabled)
        this.cur_saved_dungeon.objects.Add(new SavedDungeonObject()
        {
          name = worldGameObject.obj_id,
          local_position = (Vector2) worldGameObject.transform.localPosition,
          type = SavedDungeonObject.SavedDungeonObjectType.WGO
        });
    }
    this.cur_saved_dungeon.is_empty = false;
    this.UpdateDungeonState();
    this.spawned_mobs = (List<WorldGameObject>) null;
    return true;
  }

  public void InitAllOptimizedColliders()
  {
    OptimizedCollider2D[] componentsInChildren = this.gameObject.GetComponentsInChildren<OptimizedCollider2D>(true);
    if (componentsInChildren == null || componentsInChildren.Length == 0)
      return;
    foreach (OptimizedCollider2D optimizedCollider2D in componentsInChildren)
      optimizedCollider2D.Init();
  }

  public float GetDeadMobsPercent()
  {
    if (this.spawned_mobs == null || this.spawned_mobs.Count == 0)
      return 100f;
    int num = 0;
    foreach (WorldGameObject spawnedMob in this.spawned_mobs)
    {
      if ((Object) spawnedMob == (Object) null)
        ++num;
      else if (spawnedMob.is_dead)
        ++num;
    }
    return (float) ((double) num / (double) this.spawned_mobs.Count * 100.0);
  }

  public void UpdateDungeonState()
  {
    if (!this.dungeon_is_loaded_now || !Application.isPlaying || (Object) this.cur_dungeon_preset == (Object) null || this.cur_saved_dungeon == null || this.cur_saved_dungeon.is_completed || (double) this.GetDeadMobsPercent() <= 90.0)
      return;
    this.cur_saved_dungeon.is_completed = true;
    Stats.DesignEvent($"Dungeon:{this.cur_dungeon_preset.dungeon_level.ToString()}:Complete");
    MainGame.me.save.quests.CheckKeyQuests("dungeon_completed_" + this.cur_dungeon_preset.dungeon_level.ToString());
  }

  public void ImportGDPoints()
  {
    List<GDPoint> list = ((IEnumerable<GDPoint>) MainGame.me.dungeon_root.GetComponentsInChildren<GDPoint>(true)).ToList<GDPoint>();
    if (list == null || list.Count <= 0)
      return;
    Debug.Log((object) ("Import GDPoints on dungeon, count: " + list.Count.ToString()));
    WorldMap.ImportGDPointsOnLoadedScene(list);
  }

  public void ExportGDPoints()
  {
    List<GDPoint> list = ((IEnumerable<GDPoint>) MainGame.me.dungeon_root.GetComponentsInChildren<GDPoint>(true)).ToList<GDPoint>();
    if (list == null || list.Count <= 0)
      return;
    Debug.Log((object) ("Export GDPoints on dungeon, count: " + list.Count.ToString()));
    WorldMap.ExportGDPointsOnUnloadedScene(list);
  }
}
