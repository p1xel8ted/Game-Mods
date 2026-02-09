// Decompiled with JetBrains decompiler
// Type: Prefabs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using SmartPools;
using UnityEngine;

#nullable disable
public class Prefabs : MonoBehaviour
{
  public static Prefabs _instance;
  public DropResGameObject drop_res_game_object;
  public WorldGameObject test_place_obj_prefab;
  public GameObject movement_and_seeker_setup;
  public GameObject dock_point;
  public GameObject debug_teleport;
  public GameObject player_prefab;
  public static WorldGameObject _wgo_prefab;
  public static ProjectileObject _projectile_prefab;
  public static WGOMark _mark_prefab;
  public static BaseItemCellGUI _item_cell;
  public TechPointsDrop _tech_points_drop;

  public static Prefabs me
  {
    get => Prefabs._instance ?? (Prefabs._instance = Object.FindObjectOfType<Prefabs>());
  }

  public void Start()
  {
    Prefabs._instance = this;
    if ((Object) this.drop_res_game_object == (Object) null)
      this.drop_res_game_object = this.GetComponentInChildren<DropResGameObject>(true);
    this.drop_res_game_object.Deactivate<DropResGameObject>();
    this.test_place_obj_prefab.Deactivate<WorldGameObject>();
    this.movement_and_seeker_setup.SetActive(false);
    if ((bool) (Object) this.dock_point)
      this.dock_point.Deactivate();
    if ((bool) (Object) this.debug_teleport)
      this.debug_teleport.Deactivate();
    SmartPooler.CreatePool<WorldGameObject>(Prefabs.wgo_prefab, 10000).ConfigurePool(40, false);
  }

  public static WorldGameObject wgo_prefab
  {
    get
    {
      if ((Object) Prefabs._wgo_prefab == (Object) null)
      {
        GameObject gameObject = Resources.Load<GameObject>("wgo prefab");
        if ((Object) gameObject == (Object) null)
        {
          Debug.LogError((object) "cannot load wgo prefab");
          return (WorldGameObject) null;
        }
        gameObject.SetActive(false);
        Prefabs._wgo_prefab = gameObject.GetComponent<WorldGameObject>();
      }
      return Prefabs._wgo_prefab;
    }
  }

  public static ProjectileObject projectile_prefab
  {
    get
    {
      if ((Object) Prefabs._projectile_prefab == (Object) null)
      {
        GameObject gameObject = Resources.Load<GameObject>("projectile prefab");
        if ((Object) gameObject == (Object) null)
        {
          Debug.LogError((object) "cannot load wgo prefab");
          return (ProjectileObject) null;
        }
        gameObject.SetActive(false);
        Prefabs._projectile_prefab = gameObject.GetComponent<ProjectileObject>();
      }
      return Prefabs._projectile_prefab;
    }
  }

  public static WGOMark mark_prefab
  {
    get
    {
      if ((Object) Prefabs._mark_prefab == (Object) null)
      {
        Prefabs._mark_prefab = Resources.Load<WGOMark>("wgo mark");
        if ((Object) Prefabs._mark_prefab == (Object) null)
        {
          Debug.LogError((object) "cannot load wgo mark prefab");
          return (WGOMark) null;
        }
      }
      return Prefabs._mark_prefab;
    }
  }

  public static BaseItemCellGUI item_cell
  {
    get
    {
      if ((Object) Prefabs._item_cell == (Object) null)
      {
        GameObject gameObject = Resources.Load<GameObject>("Base Item Cell");
        if ((Object) gameObject == (Object) null)
        {
          Debug.LogError((object) "cannot load item cell prefab");
          return (BaseItemCellGUI) null;
        }
        gameObject.SetActive(false);
        Prefabs._item_cell = gameObject.GetComponent<BaseItemCellGUI>();
      }
      return Prefabs._item_cell;
    }
  }

  public TechPointsDrop tech_points_drop
  {
    get
    {
      if ((Object) this._tech_points_drop == (Object) null)
      {
        this._tech_points_drop = Resources.Load<TechPointsDrop>("tech points drop");
        if ((Object) this._tech_points_drop == (Object) null)
        {
          Debug.LogError((object) "cannot load tech_points_drop prefab");
          return (TechPointsDrop) null;
        }
      }
      return this._tech_points_drop;
    }
  }
}
