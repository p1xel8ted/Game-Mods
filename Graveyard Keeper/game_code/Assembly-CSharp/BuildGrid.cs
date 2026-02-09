// Decompiled with JetBrains decompiler
// Type: BuildGrid
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BuildGrid : MonoBehaviour
{
  public static Dictionary<long, bool> _has_build_colliders = new Dictionary<long, bool>();
  public Texture2D _tx;
  public Material[] _mats;
  public const int TEXTURE_SIZE = 1024 /*0x0400*/;
  public const int BUILD_GRID_DIVIDER = 3;
  public const int SCAN_AREA_SIZE = 100;
  public const int HALF_TEXTURE_SIZE = 512 /*0x0200*/;
  public const int BUILD_GRID_SIZE = 32 /*0x20*/;
  public const int HALF_BUILD_GRID_SIZE = 16 /*0x10*/;
  public static Vector2 BUILD_GRID_VECTOR2 = new Vector2(32f, 32f);
  public static Vector2 GRID_CHECK_BOX_SIZE = new Vector2(32f, 32f) * 0.9f;
  public static Color[] _colors = new Color[1048576 /*0x100000*/];
  public static Collider2D[] _colliders = new Collider2D[100];
  public static BuildGrid _me = (BuildGrid) null;
  public static bool _build_grid_shown = false;
  public const float C_TOTEM_MOD = -0.01f;
  public string _custom_sub_zone = "";
  public List<int> _drawn_totem_radius_cells = new List<int>();
  public BuildingSubZoneConfiguration _current_sub_zone_configuration;

  public static BuildingSubZoneConfiguration current_sub_zone_configuration
  {
    get => BuildGrid.me._current_sub_zone_configuration;
  }

  [ContextMenu("Refresh grid")]
  public void RefreshGrid()
  {
    Debug.Log((object) nameof (RefreshGrid));
    this._current_sub_zone_configuration = (BuildingSubZoneConfiguration) null;
    this._drawn_totem_radius_cells.Clear();
    this.CacheMaterials();
    if ((Object) this._tx == (Object) null)
    {
      Texture2D texture2D = new Texture2D(1024 /*0x0400*/, 1024 /*0x0400*/, TextureFormat.Alpha8, false);
      texture2D.filterMode = FilterMode.Point;
      this._tx = texture2D;
    }
    Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    string curBuildZoneId = MainGame.me.build_mode_logics.cur_build_zone_id;
    Vector2 position = (Vector2) this.transform.position;
    bool flag1 = !string.IsNullOrEmpty(this._custom_sub_zone);
    for (int index1 = -100; index1 < 100; ++index1)
    {
      int num1 = index1 * 32 /*0x20*/;
      for (int index2 = -100; index2 < 100; ++index2)
      {
        int num2 = index2 * 32 /*0x20*/;
        int num3 = 1024 /*0x0400*/ - (index1 + 512 /*0x0200*/);
        int num4 = 1024 /*0x0400*/ - (index2 + 512 /*0x0200*/);
        Vector2 vector2 = new Vector2((float) (num1 - 16 /*0x10*/), (float) (num2 - 16 /*0x10*/)) + position;
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(vector2, BuildGrid.GRID_CHECK_BOX_SIZE, 0.0f, 524288 /*0x080000*/);
        bool flag2 = false;
        foreach (Collider2D collider2D in collider2DArray)
        {
          if ((Object) collider2D != (Object) null)
          {
            WorldZone component1 = collider2D.GetComponent<WorldZone>();
            if (flag1)
            {
              if ((Object) component1 == (Object) null)
              {
                WorldSubZone component2 = collider2D.GetComponent<WorldSubZone>();
                if (!((Object) component2 == (Object) null) && !(component2.sub_zone_id != this._custom_sub_zone))
                {
                  flag2 = true;
                  this._current_sub_zone_configuration = component2.buildingSubZoneConfiguration;
                }
                else
                  continue;
              }
              else
                continue;
            }
            else if (!((Object) component1 == (Object) null))
              flag2 = component1.id == curBuildZoneId && component1.IsAvailableForBuild();
            else
              continue;
            if (flag2)
              break;
          }
        }
        BuildGrid.me.transform.position = BuildGrid.me.transform.position with
        {
          z = (Object) this._current_sub_zone_configuration == (Object) null ? 1900f : this._current_sub_zone_configuration.z_build_grid_sorting
        };
        bool flag3 = BuildGrid.IsCellBusy(vector2);
        color.a = flag2 ? (flag3 ? 1f : 0.5f) : 0.0f;
        BuildGrid._colors[num3 + num4 * 1024 /*0x0400*/] = color;
      }
    }
    this.DrawExistingTotemRadiuses();
    this.ApplyColors();
    this.Update();
  }

  public void DrawExistingTotemRadiuses()
  {
    WorldZone zoneById = WorldZone.GetZoneByID(MainGame.me.build_mode_logics.cur_build_zone_id);
    if ((Object) zoneById == (Object) null)
      return;
    foreach (WorldGameObject zoneWgO in zoneById.GetZoneWGOs())
    {
      if (zoneWgO.obj_def.IsTotem())
        this.RedrawTotemRadius(zoneWgO, is_floating_object: false);
    }
  }

  public void RedrawTotemRadius(
    WorldGameObject wobj,
    Vector2 center_offset = default (Vector2),
    bool is_floating_object = true)
  {
    if ((Object) wobj == (Object) null || !wobj.obj_def.IsTotem())
      return;
    if (is_floating_object)
      this.ClearPreviousTotemRadius(false);
    Vector2 vector2 = (Vector2) wobj.transform.position + center_offset * 96f;
    int num1 = 1024 /*0x0400*/ - Mathf.RoundToInt((float) (((double) vector2.x + 16.0 - (double) this.transform.position.x) / 32.0 + 512.0));
    int num2 = 1024 /*0x0400*/ - Mathf.RoundToInt((float) (((double) vector2.y + 16.0 - (double) this.transform.position.y) / 32.0 + 512.0));
    float num3 = wobj.obj_def.totem_radius * wobj.obj_def.totem_radius;
    for (int index1 = -100; index1 < 100; ++index1)
    {
      for (int index2 = -100; index2 < 100; ++index2)
      {
        int num4 = 1024 /*0x0400*/ - (index1 + 512 /*0x0200*/);
        int num5 = 1024 /*0x0400*/ - (index2 + 512 /*0x0200*/);
        if ((double) ((num4 - num1) * (num4 - num1) + (num5 - num2) * (num5 - num2)) <= (double) num3)
          this.ColorizeTotemCell(num4 + num5 * 1024 /*0x0400*/, false, is_floating_object);
      }
    }
    if (!is_floating_object)
      return;
    this.ApplyColors();
  }

  public void ColorizeTotemCell(int pos, bool apply_colors, bool is_floating_object = true)
  {
    if (is_floating_object && !this._drawn_totem_radius_cells.Contains(pos))
      this._drawn_totem_radius_cells.Add(pos);
    BuildGrid._colors[pos].a += -0.01f;
    if (!(is_floating_object & apply_colors))
      return;
    this.ApplyColors();
  }

  public void ClearPreviousTotemRadius(bool apply_colors)
  {
    foreach (int drawnTotemRadiusCell in this._drawn_totem_radius_cells)
      BuildGrid._colors[drawnTotemRadiusCell].a -= -0.01f;
    this._drawn_totem_radius_cells.Clear();
    if (!apply_colors)
      return;
    this.ApplyColors();
  }

  public void ApplyColors()
  {
    this._tx.SetPixels(BuildGrid._colors);
    this._tx.Apply();
    foreach (Material mat in this._mats)
    {
      mat.SetTexture("_MainTex3", (Texture) this._tx);
      mat.SetFloat("_Scale", 300f);
      mat.SetFloat("_Shift", 362f);
    }
  }

  public void CacheMaterials()
  {
    if (this._mats != null)
      return;
    MeshRenderer[] componentsInChildren = this.GetComponentsInChildren<MeshRenderer>();
    this._mats = new Material[componentsInChildren.Length];
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      this._mats[index] = new Material(componentsInChildren[index].sharedMaterial);
      componentsInChildren[index].material = this._mats[index];
    }
  }

  public void Show(bool show = true)
  {
    if (show)
    {
      BuildGrid._has_build_colliders.Clear();
      this.RefreshGrid();
    }
    this.gameObject.SetActive(show);
  }

  public static BuildGrid me => MainGame.me.build_grid;

  public static void ReshowBuildGrid()
  {
    BuildGrid.ShowBuildGrid(true, true, BuildGrid.me._custom_sub_zone);
    GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => BuildGrid.ShowBuildGrid(true, true, BuildGrid.me._custom_sub_zone)));
  }

  public static void ShowBuildGrid(bool show, bool ignore_repeating = false, string custom_sub_zone = "")
  {
    if (!ignore_repeating && BuildGrid._build_grid_shown == show && custom_sub_zone == BuildGrid.me._custom_sub_zone)
      return;
    Debug.Log((object) $"ShowBuildGrid {show.ToString()}, current state = {BuildGrid._build_grid_shown.ToString()}");
    BuildGrid._build_grid_shown = show;
    BuildGrid.me._custom_sub_zone = show ? custom_sub_zone : "";
    BuildGrid.me.Show(show);
  }

  public void MoveBuildGridTo(Vector2 pos)
  {
    BuildGrid.me.transform.position = BuildGrid.me.transform.position with
    {
      x = (float) ((double) Mathf.Round(pos.x / 96f) * 96.0 + 48.0),
      y = (float) ((double) Mathf.Round(pos.y / 96f) * 96.0 + 48.0),
      z = 1900f
    };
  }

  public static bool IsCellBusy(Vector2 pos, List<Collider2D> target_colliders = null)
  {
    foreach (Collider2D collider in Physics2D.OverlapBoxAll(pos, BuildGrid.GRID_CHECK_BOX_SIZE, 0.0f, 8389121))
    {
      if (!BuildGrid.SkipCollider(collider))
      {
        if (target_colliders != null)
        {
          if (target_colliders.Contains(collider))
            return true;
        }
        else if (!((Object) collider.GetComponentInParent<FloatingWorldGameObject>() != (Object) null))
          return true;
      }
    }
    return false;
  }

  public static bool SkipCollider(Collider2D collider, WorldGameObject owner = null)
  {
    if ((Object) owner == (Object) null)
    {
      owner = collider.GetComponentInParent<WorldGameObject>();
      if ((Object) owner == (Object) null)
        return false;
    }
    if (BuildGrid.WGOHasBuildCollider(owner))
      return collider.gameObject.layer != 23;
    return collider.gameObject.layer != 0 && collider.gameObject.layer != 9;
  }

  public static bool WGOHasBuildCollider(WorldGameObject wgo)
  {
    if ((Object) wgo == (Object) null)
      return false;
    long uniqueId = wgo.unique_id;
    if (BuildGrid._has_build_colliders.ContainsKey(uniqueId))
      return BuildGrid._has_build_colliders[uniqueId];
    BuildGrid._has_build_colliders.Add(uniqueId, false);
    foreach (Component componentsInChild in wgo.GetComponentsInChildren<Collider2D>())
    {
      if (componentsInChild.gameObject.layer == 23)
      {
        BuildGrid._has_build_colliders[uniqueId] = true;
        break;
      }
    }
    return BuildGrid._has_build_colliders[uniqueId];
  }

  public void Update()
  {
    if (!this.gameObject.activeInHierarchy || !MainGame.game_started)
      return;
    this.CacheMaterials();
    Color ambientLight = RenderSettings.ambientLight with
    {
      a = RenderSettings.ambientIntensity
    };
    foreach (Material mat in this._mats)
      mat.SetColor("_AmbientColor", ambientLight);
  }

  public static string GetCurrentSubZoneID() => BuildGrid.me._custom_sub_zone;
}
